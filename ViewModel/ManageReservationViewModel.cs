using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
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
        private const string SelectAll = "Alle";
        private Reservation _reservation;
        private string _numberOfPeople;
        private CampingCustomer CampingCustomer;
        private ObservableCollection<string> _campingPlaces;
        private CampingPlace SelectedCampingPlaceObject { get; set; }
        private string _selectedCampingPlace;
        private DateTime _checkInDate;
        private DateTime _checkOutDate;


        #region properties
  
    
        public string NumberOfPeople
        {
            get
            {
                return _numberOfPeople;
            }
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

        public ObservableCollection<string> CampingPlaces
        {
            get
            {
                return this._campingPlaces;
            }
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

        public string SelectedCampingPlace
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
                GetSelectedCampingPlaceObject(_selectedCampingPlace);
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

                this._checkInDate = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
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
            }
        }

        #endregion

        public ManageReservationViewModel()
        {
            this.CampingPlaces = new ObservableCollection<string>();

            ReservationCollectionViewModel.ManageReservationEvent += this.OnManageReservationEvent;
            
            foreach (var campingPlace in new CampingPlace().Select())
            {
                this.CampingPlaces.Add(campingPlace.GetLocation());
            }
        }

        private void GetSelectedCampingPlaceObject(string campingPlaceInput)
        {
            foreach (var campingPlace in new CampingPlace().Select())
            {
                if (campingPlace.GetLocation().Equals(campingPlaceInput))
                {
                    this.SelectedCampingPlaceObject = campingPlace;
                }
            }
        }

        private void OnManageReservationEvent(object sender, ReservationEventArgs args)
        {
            if(args.Reservation is Reservation r){
                this._reservation = r;
                this.NumberOfPeople = r.NumberOfPeople.ToString();
                this.SelectedCampingPlace = r.CampingPlace.GetLocation();
                this.SelectedCampingPlaceObject = r.CampingPlace;
                this.CheckInDate = r.Duration.CheckInDatetime;
                this.CheckOutDate = r.Duration.CheckOutDatetime;
                this.CampingCustomer = r.CampingCustomer;
            }
          
        }

        #region Command
        private bool CanExecuteUpdateReservation()
        {
            return true;
        }
        public ICommand UpdateReservation => new RelayCommand(ExecuteUpdateReservation, CanExecuteUpdateReservation);
        #endregion


        private void ExecuteUpdateReservation()
        {
            var result = MessageBox.Show("Weet u zeker dat u de reservering wil aanpassen?", "Reservering bijwerken", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            ReservationDuration updatedReservationDuraton = new ReservationDuration(this._reservation.Duration.Id.ToString(), this.CheckInDate.ToString(), this.CheckOutDate.ToString());
            Reservation updatedReservationObject = new Reservation(_reservation.Id.ToString(), this.NumberOfPeople, this.CampingCustomer, this.SelectedCampingPlaceObject, updatedReservationDuraton);

            bool succesfullyUpdated = updatedReservationObject.Update(this.NumberOfPeople, this.CampingCustomer, this.SelectedCampingPlaceObject, updatedReservationDuraton);
            bool durationsuccesfullyupdated = updatedReservationDuraton.Update(this.CheckInDate.ToString(), this.CheckOutDate.ToString());

            string context = "Reservering is aangepast!";
            string caption = "Reservering is bijgwerkt";
            if (succesfullyUpdated || durationsuccesfullyupdated)
            {
                context = "Reservering is door vage omstandigheden niet goed aangepast";
                caption = "Reservering is mogelijk bijgewerkt";
            }
            MessageBox.Show(context, caption, System.Windows.MessageBoxButton.OK);

        }


        private void ExecuteGoToDashBoard()
        {
            FromReservationBackToDashboardEvent?.Invoke(this, new ReservationEventArgs(_reservation));
        }

        private bool CanExecuteGoToDashboard()
        {
            //Is it possible to check this execution?
            return true;
        }
        public ICommand GoBackToDashboard => new RelayCommand(ExecuteGoToDashBoard, CanExecuteGoToDashboard);



        private void ExecuteDeleteReservation()
        {
            var result = MessageBox.Show("Weet u zeker dat u deze reservering wil verwijderen?", "Reservering verwijderen", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            ReservationDuration deletedReservationDuraton = new ReservationDuration(this._reservation.Duration.Id.ToString(), this.CheckInDate.ToString(), this.CheckOutDate.ToString());
            Reservation deletedReservationObject = new Reservation(_reservation.Id.ToString(), this.NumberOfPeople, this.CampingCustomer, this.SelectedCampingPlaceObject, deletedReservationDuraton);

            bool succesfullDeleted = deletedReservationDuraton.Delete();
            bool durationsuccesfullydeleted = deletedReservationObject.Delete();

            string context = "Reservering is verwijderd!";
            string caption = "Succesvol verwijderd";
            if (succesfullDeleted || durationsuccesfullydeleted)
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

    }
}
