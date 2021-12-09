using Microsoft.Toolkit.Mvvm.ComponentModel;
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

namespace ViewModel
{
    public class ReservationCustomerOverviewViewModel : ObservableObject
    {
        // TODO: Change this to session variable
        private int _customerID = 107;
        public List<Reservation> Reservations { get; private set; } = new List<Reservation>();

        private ObservableCollection<Reservation> _reservationsCollection;
        private Reservation _selectedReservation;
        private ObservableCollection<CampingGuest> _campingGuestCollection;

        #region Properties
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
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

                this.DisplayNewCustomerGuestData(this._selectedReservation);
                this.DisplayNewReservationInfoData(this._selectedReservation);
            }
        }

        public ObservableCollection<CampingGuest> CampingGuestCollection
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

        public ReservationCustomerOverviewViewModel()
        {
            Reservation reservationModel = new Reservation();
            this.Reservations = reservationModel.GetCustomersReservations(this._customerID);
            this.ReservationsCollection = new ObservableCollection<Reservation>(this.Reservations);
            this.SelectedReservation = this.Reservations.First();
        }

        /// <summary>
        /// Replaces the values of the reservation info in the reservation overview
        /// </summary>
        /// <param name="reservation">Reservation object of the selected reservation</param>
        private void DisplayNewReservationInfoData(Reservation reservation)
        {
            this.InfoStartDate = reservation.Duration.CheckInDate;
            this.InfoEndDate = reservation.Duration.CheckOutDate;
            this.InfoAmountOfGuests = reservation.NumberOfPeople.ToString();
            this.InfoAccommodationType = reservation.CampingPlace.Type.Accommodation.Name;
            this.InfoSurface = reservation.CampingPlace.Surface.ToString(CultureInfo.InvariantCulture);
            this.InfoLocation = reservation.CampingPlace.Location;
            this.InfoTotalPrice = reservation.TotalPrice.ToString(CultureInfo.InvariantCulture);
        }

        private void DisplayNewCustomerGuestData(Reservation reservation)
        {
            List<CampingGuest> campingGuestList = new List<CampingGuest>();

            CampingGuest gast1 = new CampingGuest("test", "test", "2000-19-19");
            CampingGuest gast2 = new CampingGuest("test", "test", "2000-19-13");
            CampingGuest gast3 = new CampingGuest("test", "test", "2000-19-14");
            campingGuestList.Add(gast1);
            campingGuestList.Add(gast2);
            campingGuestList.Add(gast3);

            this.CampingGuestCollection = new ObservableCollection<CampingGuest>(campingGuestList);
        }
    }
}