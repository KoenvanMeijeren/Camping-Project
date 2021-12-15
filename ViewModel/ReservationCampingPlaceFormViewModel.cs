using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;

namespace ViewModel
{
    public class ReservationCampingPlaceFormViewModel : ObservableObject
    {
        #region Fields
        private readonly CampingPlace _campingPlaceModel = new CampingPlace();
        
        public const string SelectAll = "Alle";

        private ObservableCollection<CampingPlace> _campingPlaces;
        
        private readonly ObservableCollection<string> _campingPlaceTypes;
        private CampingPlace _selectedCampingPlace;

        private DateTime _checkOutDate, _checkInDate;
        private string _minNightPrice, _maxNightPrice, _selectedCampingPlaceType, _guests;

        public static event EventHandler<ReservationDurationEventArgs> ReserveEvent;
        
        #endregion
        
        #region Properties
        public string MinNightPrice
        {
            get => this._minNightPrice;
            set
            {
                if (Equals(value, this._minNightPrice))
                {
                    return;
                }

                this._minNightPrice = value;
                this.SetOverview(this.GetCampingPlaces());

                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string Guests
        {
            get => this._guests;
            set
            {
                if (Equals(value, this._guests))
                {
                    return;
                }

                this._guests = value;
                this.SetOverview(this.GetCampingPlaces());

                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }
        
        public string MaxNightPrice
        {
            get => this._maxNightPrice;
            set
            {
                if (Equals(value, this._maxNightPrice))
                {
                    return;
                }

                this._maxNightPrice = value;
                this.SetOverview(this.GetCampingPlaces());

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
                this.SetOverview(this.GetCampingPlaces());
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
                
                this.CheckOutDate = this._checkInDate.AddDays(daysDifference);
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
                this.SetOverview(this.GetCampingPlaces());
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
        
        public ObservableCollection<string> CampingPlaceTypes
        {
            get => this._campingPlaceTypes;
            private init
            {
                if (Equals(value, this._campingPlaceTypes))
                {
                    return;
                }
                
                this._campingPlaceTypes = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string SelectedCampingPlaceType
        {
            get => this._selectedCampingPlaceType;
            set
            {
                if (Equals(value, this._selectedCampingPlaceType))
                {
                    return;
                }

                this._selectedCampingPlaceType = value;
                this.SetOverview(this.GetCampingPlaces());
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        #endregion

        #region View construction
        
        public ReservationCampingPlaceFormViewModel()
        {
            this.CampingPlaces = new ObservableCollection<CampingPlace>();
            this.CampingPlaceTypes = new ObservableCollection<string> {
                SelectAll
            };

            //Loop through rows in Accommodation table
            foreach (var accommodationDatabaseRow in new Accommodation().Select())
            {
                this.CampingPlaceTypes.Add(accommodationDatabaseRow.Name);
            }
            
            this.SelectedCampingPlaceType = SelectAll;
            this.CheckInDate = DateTime.Today;
            this.CheckOutDate = DateTime.Today.AddDays(1);
            
            ReservationCampingGuestViewModel.ReservationConfirmedEvent += this.ReservationCampingGuestViewModelOnReservationConfirmedEvent;
        }

        private void ReservationCampingGuestViewModelOnReservationConfirmedEvent(object? sender, ReservationEventArgs e)
        {
            this.SetOverview(this.GetCampingPlaces());
        }

        private void SetOverview(IEnumerable<CampingPlace> campingPlaceItems)
        {
            if (this.CampingPlaces == null)
            {
                return;
            }
            
            // Removes all current camping places.
            this.CampingPlaces.Clear();

            bool CampingPlaceFilter(CampingPlace campingPlace) => 
                (this._selectedCampingPlaceType.Equals(SelectAll) || campingPlace.Type.Accommodation.Name.Equals(this._selectedCampingPlaceType)) 
                && (!int.TryParse(this.MinNightPrice, out int min) || campingPlace.TotalPrice >= min) 
                && (!int.TryParse(this.MaxNightPrice, out int max) || campingPlace.TotalPrice <= max) 
                && (!int.TryParse(this.Guests, out int guests) || campingPlace.Type.GuestLimit >= guests);

            campingPlaceItems = campingPlaceItems.Where(CampingPlaceFilter);
            foreach (CampingPlace item in campingPlaceItems)
            {
                this.CampingPlaces.Add(item);
            }
        }
        
        #endregion

        #region Input

        private void ResetInput()
        {
            this.SelectedCampingPlaceType = SelectAll;
            this.SelectedCampingPlace = null;
            this.CheckInDate = DateTime.Today;
            this.CheckOutDate = DateTime.Today.AddDays(1);
            this.MinNightPrice = "";
            this.MaxNightPrice = "";
        }

        #endregion
        
        #region Commands
        private void ExecuteStartReservation()
        {
            ReserveEvent?.Invoke(this, new ReservationDurationEventArgs(this.SelectedCampingPlace, this.CheckInDate, this.CheckOutDate));
            this.ResetInput();
        }

        private bool CanExecuteStartReservation()
        {
            return this.SelectedCampingPlace != null;
        }

        public ICommand StartReservation => new RelayCommand(ExecuteStartReservation, CanExecuteStartReservation);

        #endregion
        
        #region Database interaction
        
        public virtual IEnumerable<CampingPlace> GetCampingPlaces()
        {
            return this.ToFilteredOnReservedCampingPlaces(this._campingPlaceModel.Select(), this.CheckInDate, this.CheckOutDate);
        }

        public virtual IEnumerable<CampingPlace> ToFilteredOnReservedCampingPlaces(IEnumerable<CampingPlace> campingPlaceList, DateTime checkInDate, DateTime checkOutDate)
        {
            var reservations = this.GetReservationModel();

            // Removes reserved camping places from the list.
            foreach (Reservation reservation in reservations)
            {
                if (reservation.CheckInDatetime.Date <= checkOutDate.Date && checkInDate.Date <= reservation.CheckOutDatetime.Date)
                {
                    campingPlaceList = campingPlaceList.Where(campingPlace => campingPlace.Id != reservation.CampingPlace.Id).ToList();
                }
            }

            return campingPlaceList;
        }

        public virtual IEnumerable<Reservation> GetReservationModel()
        {
            Reservation reservationModel = new Reservation();
            return reservationModel.Select();
        }

        #endregion    
    }
}

