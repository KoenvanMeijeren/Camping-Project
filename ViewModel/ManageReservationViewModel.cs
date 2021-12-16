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
        
        public static event EventHandler<ReservationEventArgs> FromReservationBackToDashboardEvent;
        public static event EventHandler<ReservationEventArgs> UpdateReservationCollection;
        
        private readonly Reservation _reservationModel = new Reservation();
        private readonly CampingPlace _campingPlaceModel = new CampingPlace();

        private Reservation _reservation;
        private CampingCustomer _campingCustomer;
        
        private ObservableCollection<CampingPlace> _campingPlaces;
        private CampingPlace _selectedCampingPlace;
        
        private DateTime _checkInDate, _checkOutDate;
        private string _pageTitle, _numberOfPeople;
        
        #endregion
        
        #region Properties
        public string PageTitle 
        {
            get => _pageTitle;
            private set
            {
                if (Equals(value, this._pageTitle))
                {
                    return;
                }

                this._pageTitle = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }
    
        public string NumberOfPeople
        {
            get => _numberOfPeople;
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

        public ObservableCollection<CampingPlace> CampingPlaces
        {
            get => this._campingPlaces;
            private set
            {
                if (Equals(value, this._campingPlaces))
                {
                    return;
                }

                this._campingPlaces = value;
            }

        }

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
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
                
                this.CheckOutDate = this._checkInDate.AddDays(daysDifference);
                
                this.SetAvailableCampingPlaces(this.GetCampingPlaces());
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
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
                
                this.SetAvailableCampingPlaces(this.GetCampingPlaces());
            }
        }

        #endregion

        #region View construction

        public ManageReservationViewModel()
        {
            this.CampingPlaces = new ObservableCollection<CampingPlace>();
            this.SetAvailableCampingPlaces(this.GetCampingPlaces());

            ReservationCollectionViewModel.ManageReservationEvent += this.OnManageReservationEvent;
        }

        private void SetAvailableCampingPlaces(IEnumerable<CampingPlace> campingPlaces)
        {
            var selectedCampingPlace = this.SelectedCampingPlace;
            if(selectedCampingPlace == null){
                return;
            }
            
            this.CampingPlaces.Clear();

            this.CampingPlaces.Add(selectedCampingPlace);
            foreach (var campingPlace in campingPlaces)
            {
                //don't insert selected campingplace
                if(campingPlace.Location != selectedCampingPlace.Location)
                {
                    this.CampingPlaces.Add(campingPlace);
                }
            }

            this.SelectedCampingPlace = selectedCampingPlace;
        }

        private void OnManageReservationEvent(object sender, ReservationEventArgs args)
        {
            if (args.Reservation == null)
            {
                return;
            }
            
            this._reservation = args.Reservation;
            this._campingCustomer = this._reservation.CampingCustomer;
            
            this.NumberOfPeople = this._reservation.NumberOfPeople.ToString();
            this.SelectedCampingPlace = this._reservation.CampingPlace;
            this.CheckInDate = this._reservation.CheckInDatetime;
            this.CheckOutDate = this._reservation.CheckOutDatetime;
            this.PageTitle = "Reservering " + this._reservation.Id + " bijwerken";
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

            ExecuteGoToDashBoard();
        }
        private bool CanExecuteUpdateReservation()
        {
            return true;
        }
        public ICommand UpdateReservation => new RelayCommand(ExecuteUpdateReservation, CanExecuteUpdateReservation);
        

        /// <summary>
        /// This method fires event to go to (an updated) the dashboard page.
        /// </summary>
        private void ExecuteGoToDashBoard()
        {
            UpdateReservationCollection?.Invoke(this, new ReservationEventArgs(this._reservation));
            FromReservationBackToDashboardEvent?.Invoke(this, new ReservationEventArgs(_reservation));
        }

        public ICommand GoBackToDashboard => new RelayCommand(ExecuteGoToDashBoard);


        /// <summary>
        /// Deletes the reservation in de Reservationtable, reservationdurationtable and reservationcampingGuesttable
        /// </summary>
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
            MessageBox.Show(context, caption, MessageBoxButton.OK);
            
            this.ExecuteGoToDashBoard();
        }

        private bool CanExecuteDeleteReservation()
        {
            //Is it possible to check this execution?
            return true;
        }
        public ICommand DeleteReservation => new RelayCommand(ExecuteDeleteReservation, CanExecuteDeleteReservation);

        #endregion

        #region Database interaction

        private IEnumerable<CampingPlace> GetCampingPlaces()
        {
            return this.ToFilteredOnReservedCampingPlaces(SelectCampingPlaces());
        }

        public virtual IEnumerable<CampingPlace> SelectCampingPlaces()
        {
            return this._campingPlaceModel.Select();
        }

        private IEnumerable<CampingPlace> ToFilteredOnReservedCampingPlaces(IEnumerable<CampingPlace> viewData)
        {
            var reservations = this.GetReservationModel();

            foreach (Reservation reservation in reservations)
            {
                if (reservation.CheckInDatetime.Date < this.CheckOutDate.Date && this.CheckInDate.Date < reservation.CheckOutDatetime.Date)
                {
                    viewData = viewData.Where(campingPlace => campingPlace.Id != reservation.CampingPlace.Id).ToList();
                }
            }

            return viewData;
        }

        public virtual IEnumerable<Reservation> GetReservationModel()
        {
            Reservation reservationModel = new Reservation();
            return reservationModel.Select();
        }


        #endregion

    }
}
