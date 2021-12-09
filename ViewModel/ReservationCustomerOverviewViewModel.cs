using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ViewModel
{
    public class ReservationCustomerOverviewViewModel : ObservableObject
    {
        // TODO: Change this to session variable
        private int _customerID = 107;
        public List<Reservation> Reservations { get; private set; } = new List<Reservation>();

        private ObservableCollection<Reservation> _reservationsCollection;
        private Reservation _selectedReservation;
        private ObservableCollection<ReservationCampingGuest> _campingGuestCollection;

        #region Properties
        private string _infoID = "ID: ";
        public string InfoID
        {
            get => _infoID;
            set => this._infoID = "ID: " + value;
        }

        private string _infoStartDate = "Begindatum: ";
        public string InfoStartDate
        {
            get => _infoStartDate;
            set => this._infoStartDate = "Begindatum: " + value;
        }

        private string _infoEndDate = "Einddatum: ";
        public string InfoEndDate
        {
            get => _infoEndDate;
            set => this._infoEndDate = "Einddatum: " + value;
        }

        private string _infoAmountOfGuests = "Aantal personen: ";
        public string InfoAmountOfGuests
        {
            get => _infoAmountOfGuests;
            set => this._infoAmountOfGuests = "Aantal personen: " + value;
        }

        private string _infoAccommodationType = "Type: ";
        public string InfoAccommodationType
        {
            get => _infoAccommodationType;
            set => this._infoAccommodationType = "Type: " + value;
        }

        private string _infoSurface = "Oppervlakte: ";
        public string InfoSurface
        {
            get => _infoSurface;
            set => this._infoSurface = "Oppervlakte: " + value + "m2";
        }

        private string _infoLocation = "Locatie: ";
        public string InfoLocation
        {
            get => _infoLocation;
            set => this._infoLocation = "Locatie: " + value;
        }

        private string _infoTotalPrice = "Totaalprijs: ";
        public string InfoTotalPrice
        {
            get => _infoTotalPrice;
            set => this._infoTotalPrice = "Totaalprijs: €" + value;
        }

        public ObservableCollection<Reservation> ReservationsCollection
        {
            get => this._reservationsCollection;
            set
            {
                if (Equals(value, this._reservationsCollection))
                {
                    return;
                }
                
                this._reservationsCollection = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }
        public Reservation SelectedReservation
        {
            get => this._selectedReservation;
            set
            {
                if (Equals(value, this._selectedReservation))
                {
                    return;
                }

                this._selectedReservation = value;
                this.DisplayNewCustomerGuestData(value);
                this.DisplayNewReservationInfoData(value);
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

            }
        }

        public ObservableCollection<ReservationCampingGuest> CampingGuestCollection
        {
            get => this._campingGuestCollection;
            set
            {
                if (Equals(value, this._campingGuestCollection))
                {
                    return;
                }

                this._campingGuestCollection = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }
        #endregion

        #region Overview
        public ReservationCustomerOverviewViewModel()
        {
            Reservation reservationModel = new Reservation();
            this.Reservations = reservationModel.GetCustomersReservations(this._customerID);

            // Removes already deleted from list
            this.Reservations.RemoveAll(item => item.ReservationDeleted == ReservationColumnStatus.True);
            this.ReservationsCollection = new ObservableCollection<Reservation>(this.Reservations);

            // Check if there are reservations for customer
            if (ReservationsCollection.Count > 0) 
            {
                this.SelectedReservation = this.Reservations.First();
               /* DE-ACTIVATE THIS BUTTON IN CASE NO RESERVATIONs this.DeleteReservationButton.IsEnabled = false; */
            }
        }

        /// <summary>
        /// Replaces the values of the reservation info in the reservation overview
        /// </summary>
        /// <param name="reservation">Reservation object of the selected reservation</param>
        private void DisplayNewReservationInfoData(Reservation reservation)
        {
            if (reservation != null)
            {
                this.InfoID = reservation.Id.ToString();
                this.InfoStartDate = reservation.Duration.CheckInDate;
                this.InfoEndDate = reservation.Duration.CheckOutDate;
                // Amount of guests + customer
                this.InfoAmountOfGuests = (reservation.CampingGuests.Count + 1).ToString();
                this.InfoAccommodationType = reservation.CampingPlace.Type.Accommodation.Name;
                this.InfoSurface = reservation.CampingPlace.Surface.ToString(CultureInfo.InvariantCulture);
                this.InfoLocation = reservation.CampingPlace.Location;
                this.InfoTotalPrice = reservation.TotalPrice.ToString(CultureInfo.InvariantCulture);
            } else
            {
                this.InfoID = "";
                this.InfoStartDate = "";
                this.InfoEndDate = "";
                this.InfoAmountOfGuests = "";
                this.InfoAccommodationType = "";
                this.InfoSurface = "";
                this.InfoLocation = "";
                this.InfoTotalPrice = "";
            }
        }

        private void DisplayNewCustomerGuestData(Reservation reservation)
        {
            this.CampingGuestCollection = (reservation != null) ? new ObservableCollection<ReservationCampingGuest>(reservation.CampingGuests) : new ObservableCollection<ReservationCampingGuest>();
        }
        #endregion

        #region Delete reservation
        /// <summary>
        /// Check if user can delete the reservation
        /// </summary>
        /// <returns>Boolean value of result</returns>
        private bool CanExecuteDeleteReservation()
        {
            return this._selectedReservation != null;
        }
        public ICommand DeleteReservation => new RelayCommand(ExecuteDeleteReservation, CanExecuteDeleteReservation);

        /// <summary>
        /// Executes the reservation delete process
        /// </summary>
        private void ExecuteDeleteReservation()
        {
            // Check if reservation has passed
            if (DateTime.Today >= Convert.ToDateTime(_selectedReservation.Duration.CheckInDate))
            {
                MessageBox.Show("Reserveringen van vandaag of eerder kunnen niet meer worden verwijderd.", "Reservering verwijderen ", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            
            bool checkIfReservationIsWithinOneWeek = DateTime.Today.AddMonths(+1) >= Convert.ToDateTime(_selectedReservation.Duration.CheckInDate);
            string restitionValue = checkIfReservationIsWithinOneWeek ? (this._selectedReservation.TotalPrice / 2).ToString(CultureInfo.InvariantCulture) : this._selectedReservation.TotalPrice.ToString(CultureInfo.InvariantCulture);

            // Confirmation box
            MessageBoxResult messageBoxResult = MessageBox.Show($"Weet je zeker dat je de reservering wil annuleren? Het restitutiebedrag bedraagt: €{restitionValue},-", "Reservering verwijderen", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                // Checks if update was succesful
                if (_selectedReservation.Update(_selectedReservation.CampingGuests.Count.ToString(), _selectedReservation.CampingCustomer, _selectedReservation.CampingPlace, _selectedReservation.Duration, ReservationColumnStatus.True, ReservationColumnStatus.False, ReservationColumnStatus.False))
                {
                    this.Reservations.Remove(_selectedReservation);
                    this.ReservationsCollection.Remove(_selectedReservation);

                    MessageBox.Show($"Reservering geannuleerd. Het restitutiebedrag van €{restitionValue},- wordt binnen vijf werkdagen op uw rekening gestort.", "Restitutie", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Check if there are still reservations left
                    if (this.Reservations.Count > 0)
                    {
                        this.SelectedReservation = this.Reservations.First();
                    }
                }
            }
        }
        #endregion
    }
}