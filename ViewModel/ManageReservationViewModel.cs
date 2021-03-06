using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Model;
using SystemCore;
using ViewModel.EventArguments;

namespace ViewModel
{
    public class ManageReservationViewModel : ObservableObject
    {
        #region Fields

        private readonly Reservation _reservationModel = new Reservation();
        private readonly CampingPlace _campingPlaceModel = new CampingPlace();

        private Reservation _reservation;
        private CampingCustomer _campingCustomer;

        private CampingPlace _selectedCampingPlace;
        
        private DateTime _checkInDate, _checkOutDate;
        private string _numberOfPeople;
        private string _location;
        private string _totalPrice;

        private ObservableCollection<CampingGuest> _campingGuests;

        #endregion

        #region Properties
        public string PageTitle { get; private set; }

        public string TotalPrice {
            get => this._totalPrice;
            set
            {
                if (Equals(value, this._totalPrice))
                {
                    return;
                }

                this._totalPrice = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string Location {
            get => this._location;
            set
            {
                if (Equals(value, this._location))
                {
                    return;
                }

                this._location = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string NumberOfPeople
        {
            get => this._numberOfPeople;
            set
            {
                if (Equals(value, this._numberOfPeople))
                {
                    return;
                }

                this._numberOfPeople = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public ObservableCollection<CampingPlace> CampingPlaces { get; private init; }

        public CampingPlace SelectedCampingPlace
        {
            get => this._selectedCampingPlace;
            set
            {
                if (Equals(value, this._selectedCampingPlace))
                {
                    return;
                }

                this._selectedCampingPlace = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public DateTime CheckInDate
        {
            get => this._checkInDate;
            set
            {
                if (Equals(value, this._checkInDate))
                {
                    return;
                }
                
                int daysDifference = this._checkOutDate.Subtract(this._checkInDate).Days;
                
                this._checkInDate = value;
                this._checkOutDate = this._checkInDate.AddDays(daysDifference);
                
                // This method calls the on property changed event.
                this.InitializeAvailableCampingPlaces();
            }
        }

        public DateTime CheckOutDate
        {
            get => this._checkOutDate;
            set
            {
                if (Equals(value, this._checkOutDate))
                {
                    return;
                }
                
                this._checkOutDate = value;
                
                // This method calls the on property changed event.
                this.InitializeAvailableCampingPlaces();
            }
        }

        public ObservableCollection<CampingGuest> CampingGuests
        {
            get => this._campingGuests;
            private set
            {
                if (Equals(value, this._campingGuests))
                {
                    return;
                }

                this._campingGuests = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        #endregion

        #region Events

        public static event EventHandler FromReservationBackToDashboardEvent;
        public static event EventHandler<UpdateModelEventArgs<Reservation>> ReservationUpdated;

        #endregion

        #region View construction

        public ManageReservationViewModel()
        {
            this.CampingPlaces = new ObservableCollection<CampingPlace>();
            this.CampingGuests = new ObservableCollection<CampingGuest>();
            this.InitializeAvailableCampingPlaces();

            ReservationCollectionViewModel.ManageReservationEvent += this.OnManageReservationEvent;
            ManageCampingMapViewModel.CampingPlacesUpdated += this.ManageCampingPlaceViewModelOnCampingPlacesUpdated;
        }

        private void ManageCampingPlaceViewModelOnCampingPlacesUpdated(object sender, UpdateModelEventArgs<CampingPlace> e)
        {
            e.UpdateCollection(this.CampingPlaces);
        }

        /// <summary>
        /// Sets the available camping places. Calling this method should be avoided, because this is a heavy method.
        /// </summary>
        private void InitializeAvailableCampingPlaces()
        {
            var selectedCampingPlace = this.SelectedCampingPlace;
            
            this.CampingPlaces.Clear();

            this.CampingPlaces.Add(selectedCampingPlace);
            foreach (var campingPlace in this.GetFilteredCampingPlaces())
            {
                this.CampingPlaces.Add(campingPlace);
            }

            this._selectedCampingPlace = selectedCampingPlace;
            this.OnPropertyChanged(new PropertyChangedEventArgs(null));
        }

        private void OnManageReservationEvent(object sender, ReservationEventArgs args)
        {

            if (args.Reservation == null)
            {
                return;
            }


            this._reservation = args.Reservation;
            this._campingCustomer = this._reservation.CampingCustomer;

            this.CampingGuests.Clear();
            foreach (var reservationCampingGuest in this._reservation.CampingGuests)
            {
                this._campingGuests.Add(reservationCampingGuest.CampingGuest);
            }

            this.PageTitle = "Reservering " + this._reservation.Id + " bijwerken";
            this._numberOfPeople = this._reservation.NumberOfPeople.ToString();
            this._selectedCampingPlace = this._reservation.CampingPlace;
            this._location = this._reservation.CampingPlace.Location;
            this._totalPrice = this._reservation.TotalPriceString;

            this._checkInDate = this._reservation.CheckInDatetime;
            
            // This triggers the on property changed event.
            this.CheckOutDate = this._reservation.CheckOutDatetime;

        }
        
        #endregion

        #region Command
        
        /// <summary>
        /// This method updates the selected reservation.
        /// </summary>
        private void ExecuteUpdateReservation()
        {
            var result = MessageBox.Show("Weet u zeker dat u de reservering wil aanpassen?", "Reservering bijwerken", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            Reservation reservation = new Reservation(this._reservation.Id.ToString(), this.NumberOfPeople, this._campingCustomer, this.SelectedCampingPlace, this._reservation.ReservationDeleted, this._reservation.ReservationPaid, this._reservation.ReservationRestitutionPaid, null, this._checkInDate.ToString(CultureInfo.InvariantCulture), this._checkOutDate.ToString(CultureInfo.InvariantCulture));

            bool reservationUpdated = reservation.Update();

            string context = "Reservering is aangepast!";
            string caption = "Reservering is bijgwerkt";
            if (!reservationUpdated)
            {
                context = "Reservering is door omstandigheden niet volledig aangepast";
                caption = "Reservering is mogelijk bijgewerkt";
            }
            
            MessageBox.Show(context, caption, MessageBoxButton.OK);

            ManageReservationViewModel.ReservationUpdated?.Invoke(this, new UpdateModelEventArgs<Reservation>(reservation, false, false));
            
            this.GoBackToDashboard.Execute(null);
        }
        
        public ICommand UpdateReservation => new RelayCommand(ExecuteUpdateReservation);

        /// <summary>
        /// This method fires event to go to (an updated) the dashboard page.
        /// </summary>
        private void ExecuteGoToDashBoard()
        {
            ManageReservationViewModel.FromReservationBackToDashboardEvent?.Invoke(this, EventArgs.Empty);
        }

        public ICommand GoBackToDashboard => new RelayCommand(ExecuteGoToDashBoard);

        private void ExecuteDeleteReservation()
        {
            var result = MessageBox.Show($"Weet u zeker dat u reservering {this._reservation.Id.ToString()} wil verwijderen?", "Reservering verwijderen", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            bool campingGuestsDeleted = true;
            if (this._reservation.CampingGuests.Count > 0)
            {
                foreach (var reservationCampingGuest in this._reservation.CampingGuests)
                {
                    bool campingGuestDeleted = reservationCampingGuest.Delete();

                    //checks if any camping guest hasn't been deleted
                    if (!campingGuestDeleted)
                    {
                        campingGuestsDeleted = false;
                    }
                }
            }

            bool reservationDeleted = this._reservation.Delete();

            string context = "Reservering is verwijderd!";
            string caption = "Succesvol verwijderd";

            if (!reservationDeleted || !campingGuestsDeleted)
            {
                context = "Reservering is door omstandigheden niet volledig verwijderd";
                caption = "Reservering is mogelijk geheel verwijderd";
            }
            
            ManageReservationViewModel.ReservationUpdated?.Invoke(this, new UpdateModelEventArgs<Reservation>(this._reservation, false, true));
            MessageBox.Show(context, caption, MessageBoxButton.OK);
            
            this.ExecuteGoToDashBoard();
        }

        public ICommand DeleteReservation => new RelayCommand(ExecuteDeleteReservation);

        #endregion

        #region Database interaction

        public IEnumerable<CampingPlace> GetFilteredCampingPlaces()
        {
            return this.ToFilteredOnReservedCampingPlaces(this.GetCampingPlaces());
        }

        public virtual IEnumerable<CampingPlace> GetCampingPlaces()
        {
            return this._campingPlaceModel.Select();
        }

        private IEnumerable<CampingPlace> ToFilteredOnReservedCampingPlaces(IEnumerable<CampingPlace> viewData)
        {
            var reservations = this.GetReservations();

            foreach (Reservation reservation in reservations)
            {
                if (reservation.CheckInDatetime.Date < this.CheckOutDate.Date && this.CheckInDate.Date < reservation.CheckOutDatetime.Date)
                {
                    viewData = viewData.Where(campingPlace => campingPlace.Id != reservation.CampingPlace.Id).ToList();
                }
            }

            return viewData;
        }

        public virtual IEnumerable<Reservation> GetReservations()
        {
            return this._reservationModel.Select();
        }


        #endregion

    }
}
