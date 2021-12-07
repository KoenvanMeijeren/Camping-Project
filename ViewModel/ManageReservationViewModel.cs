using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Model;

namespace ViewModel
{
    public class ManageReservationViewModel : ObservableObject
    {
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

    /*    private void ExecuteReverseReservationmanagement()
        {
           
        }
        private bool CanExecuteReverseReservationmanagement()
        {
            return true;
        }
        public ICommand BackToDashboard => new RelayCommand(ExecuteReverseReservationmanagement, CanExecuteReverseReservationmanagement);*/


        private void ExecuteUpdateReservation()
        {
            var result = MessageBox.Show("Weet u zeker dat u de reservering wil aanpassen?", "Reservering bijwerken", System.Windows.MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                ReservationDuration updatedReservationDuraton = new ReservationDuration(this._reservation.Duration.Id.ToString(), this.CheckInDate.ToString(), this.CheckOutDate.ToString());
                Reservation updatedReservationObject =new Reservation(_reservation.Id.ToString(), this.NumberOfPeople, this.CampingCustomer, this.SelectedCampingPlaceObject, updatedReservationDuraton);
                
                //in 1x moet het gecommict worden
                    
                bool succesfullyUpdated = updatedReservationObject.Update(this.NumberOfPeople, this.CampingCustomer, this.SelectedCampingPlaceObject, updatedReservationDuraton);                
                bool durationsuccesfullyupdated = updatedReservationDuraton.Update(this.CheckInDate.ToString(), this.CheckOutDate.ToString());
                
                if (succesfullyUpdated && durationsuccesfullyupdated)
                {
                    MessageBox.Show("Reservering is aangepast!", "Reservering is bijgewerkt", System.Windows.MessageBoxButton.OK);
                }
            }            
        }



        private bool CanExecuteUpdateReservation()
        {
            return true;
        }
        public ICommand UpdateReservation => new RelayCommand(ExecuteUpdateReservation, CanExecuteUpdateReservation);
        #endregion
    }
}
