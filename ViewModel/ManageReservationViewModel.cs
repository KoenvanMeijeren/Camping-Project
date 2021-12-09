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

namespace ViewModel
{
    public class ManageReservationViewModel : ObservableObject
    {
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


        #region properties
        public string PageTitle 
        {
            get => _pageTitle;
            set
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
            set
            {
                if (Equals(value, this._campingPlaces))
                {
                    return;
                }

                this._campingPlaces = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
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
                
                this.SetAvailableCampingPlaces();
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
                
                this.SetAvailableCampingPlaces();
            }
        }

        #endregion

        public ManageReservationViewModel()
        {
            this.CampingPlaces = new ObservableCollection<CampingPlace>(this._campingPlaceModel.Select());

            ReservationCollectionViewModel.ManageReservationEvent += this.OnManageReservationEvent;
        }

        private void SetAvailableCampingPlaces()
        {
            var selectedCampingPlace = this.SelectedCampingPlace;
            
            this.CampingPlaces.Clear();

            this.CampingPlaces.Add(selectedCampingPlace);
            foreach (var campingPlace in this.ToFilteredOnReservedCampingPlaces(this._campingPlaceModel.Select()))
            {
                this.CampingPlaces.Add(campingPlace);
            }

            this.SelectedCampingPlace = selectedCampingPlace;
        }

        private IEnumerable<CampingPlace> ToFilteredOnReservedCampingPlaces(IEnumerable<CampingPlace> viewData)
        {
            var reservations = this._reservationModel.Select();
            foreach (Reservation reservation in reservations)
            {
                ReservationDuration reservationDuration = reservation.Duration;
                if (reservationDuration.CheckInDatetime.Date < this.CheckOutDate.Date && this.CheckInDate.Date < reservationDuration.CheckOutDatetime.Date)
                {
                    viewData = viewData.Where(campingPlace => campingPlace.Id != reservation.CampingPlace.Id).ToList();
                }
            }

            return viewData;
        }

        private void OnManageReservationEvent(object sender, ReservationEventArgs args)
        {
            if(args.Reservation == null)
            {
                return;
            }
            
            this._reservation = args.Reservation;
            this._campingCustomer = this._reservation.CampingCustomer;
            
            this.NumberOfPeople = this._reservation.NumberOfPeople.ToString();
            this.SelectedCampingPlace = this._reservation.CampingPlace;
            this.CheckInDate = this._reservation.Duration.CheckInDatetime;
            this.CheckOutDate = this._reservation.Duration.CheckOutDatetime;
            this.PageTitle = "Reservering " + this._reservation.Id + " bijwerken";

            this.SetAvailableCampingPlaces();
        }

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

            ReservationDuration updatedReservationDuraton = new ReservationDuration(this._reservation.Duration.Id.ToString(), this.CheckInDate.ToString(CultureInfo.InvariantCulture), this.CheckOutDate.ToString(CultureInfo.InvariantCulture));
            Reservation updatedReservationObject = new Reservation(_reservation.Id.ToString(), this.NumberOfPeople, this._campingCustomer, this.SelectedCampingPlace, updatedReservationDuraton);

            bool succesfullyUpdated = updatedReservationObject.Update(this.NumberOfPeople, this._campingCustomer, this.SelectedCampingPlace, updatedReservationDuraton);
            bool durationsuccesfullyupdated = updatedReservationDuraton.Update(this.CheckInDate.ToString(CultureInfo.InvariantCulture), this.CheckOutDate.ToString(CultureInfo.InvariantCulture));

            string context = "Reservering is aangepast!";
            string caption = "Reservering is bijgwerkt";
            if (!succesfullyUpdated || !durationsuccesfullyupdated)
            {
                context = "Reservering is door vage omstandigheden niet goed aangepast";
                caption = "Reservering is mogelijk bijgewerkt";
            }
            
            MessageBox.Show(context, caption, System.Windows.MessageBoxButton.OK);
            //update page
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

        private bool CanExecuteGoToDashboard()
        {
            //Is it possible to check this execution?
            return true;
        }
        public ICommand GoBackToDashboard => new RelayCommand(ExecuteGoToDashBoard, CanExecuteGoToDashboard);


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

            ReservationDuration deletedReservationDuraton = new ReservationDuration(this._reservation.Duration.Id.ToString(), this.CheckInDate.ToString(CultureInfo.InvariantCulture), this.CheckOutDate.ToString(CultureInfo.InvariantCulture));
            Reservation deletedReservationObject = new Reservation(_reservation.Id.ToString(), this.NumberOfPeople, this._campingCustomer, this.SelectedCampingPlace, deletedReservationDuraton);
            ReservationCampingGuest deletedCampingGuest = new ReservationCampingGuest();

            //CAMPINGGUEST ISN'T DELETED
            var campingGuestSuccesfullyDeleted = deletedCampingGuest.DeleteReservationCampingGuestConnection(_reservation.Id);
            bool durationSuccesfullydeleted = deletedReservationObject.Delete();
            bool succesfullDeleted = deletedReservationDuraton.Delete();

            string context = "Reservering is verwijderd!";
            string caption = "Succesvol verwijderd";

            if (!succesfullDeleted || !durationSuccesfullydeleted || !campingGuestSuccesfullyDeleted)
            {
                context = "Reservering is door vage omstandigheden niet goed verwijderd";
                caption = "Reservering is mogelijk geheel verwijderd";
            }
            MessageBox.Show(context, caption, System.Windows.MessageBoxButton.OK);
        }

        private bool CanExecuteDeleteReservation()
        {
            //Is it possible to check this execution?
            return true;
        }
        public ICommand DeleteReservation => new RelayCommand(ExecuteDeleteReservation, CanExecuteDeleteReservation);

        #endregion

    }
}
