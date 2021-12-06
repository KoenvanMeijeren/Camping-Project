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
using System.Windows.Input;

namespace ViewModel
{
    public class CampingPlacesCollectionViewModel : ObservableObject
    {
        #region Fields
        private readonly CampingPlace _campingPlaceModel = new CampingPlace();
        
        private const string SelectAll = "Alle";

        private readonly ObservableCollection<string> _campingPlaceTypes;
        private string _selectedCampingPlaceType;
        
        private CampingPlace _selectedCampingPlace;
        
        private ObservableCollection<CampingPlace> _campingPlaces;
        public static event EventHandler<ReservationEventArgs> ReserveEvent;
        
        private DateTime _checkOutDate;
        private DateTime _checkInDate;
        private string _minNightPrice;
        private string _maxNightPrice;

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
                this.SetOverview(GetCampingPlaces());

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
                this.SetOverview(GetCampingPlaces());

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

                this._checkInDate = value;
                this.SetOverview(GetCampingPlaces());
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
                this.SetOverview(GetCampingPlaces());
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

        public string SelectedPlaceType
        {
            get => this._selectedCampingPlaceType;
            set
            {
                if (Equals(value, this._selectedCampingPlaceType))
                {
                    return;
                }

                this._selectedCampingPlaceType = value;
                this.SetOverview(GetCampingPlaces());
                
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        #endregion

        #region View construction
        
        public CampingPlacesCollectionViewModel()
        {
            this.CampingPlaceTypes = new ObservableCollection<string>();
            this.CampingPlaceTypes.Add("Alle");
            this.CampingPlaceTypes.Add("Bungalow");
            this.CampingPlaceTypes.Add("Camper");
            this.CampingPlaceTypes.Add("Caravan");
            this.CampingPlaceTypes.Add("Chalet");
            this.CampingPlaceTypes.Add("Tent");
            
            this.CampingPlaces = new ObservableCollection<CampingPlace>(this.GetCampingPlaces());
            
            this.SelectedPlaceType = SelectAll;
            this.CheckInDate = DateTime.Today;
            this.CheckOutDate = DateTime.Today.AddDays(1);
        }

        private void SetOverview(IEnumerable<CampingPlace> campingPlaceItems)
        {
            // Removes all current camping places.
            this.CampingPlaces.Clear();

            var selectedCampingPlaceType = this.SelectedPlaceType;

            //var campingPlaceItems  = this.GetCampingPlaces();
            //campingPlaceItems = this.ToFilteredOnReservedCampingPlaces(campingPlaceItems, CheckInDate, CheckOutDate);

            if (!selectedCampingPlaceType.Equals(SelectAll))
            {
                campingPlaceItems = campingPlaceItems.Where(campingPlace => campingPlace.Type.Accommodation.Name.Equals(selectedCampingPlaceType)).ToList();
            }


            if (int.TryParse(this.MinNightPrice, out int min))
            {
                campingPlaceItems = campingPlaceItems.Where(campingPlace => campingPlace.TotalPrice >= min).ToList();
            }

            if (int.TryParse(this.MaxNightPrice, out int max))
            {
                campingPlaceItems = campingPlaceItems.Where(campingPlace => campingPlace.TotalPrice <= max).ToList();
            }

            // Sets the camping places on the screen.
            foreach (CampingPlace item in campingPlaceItems)
            {
                this.CampingPlaces.Add(item);
            }
        }
        
        #endregion

        #region Commands
        private void ExecuteStartReservation()
        {
            ReserveEvent?.Invoke(this, new ReservationEventArgs(this.SelectedCampingPlace, this.CheckInDate, this.CheckOutDate));
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
            return this._campingPlaceModel.Select();
        }

        public virtual IEnumerable<CampingPlace> ToFilteredOnReservedCampingPlaces(IEnumerable<CampingPlace> viewData, DateTime checkInDate, DateTime checkOutDate)
        {
            Reservation reservationModel = new Reservation();

            var reservations = reservationModel.Select();
            foreach (Reservation reservation in reservations)
            {
                ReservationDuration reservationDuration = reservation.Duration;
                if (reservationDuration.CheckInDatetime.Date < checkOutDate.Date && checkInDate.Date < reservationDuration.CheckOutDatetime.Date)
                {
                    viewData = viewData.Where(campingPlace => campingPlace.Id != reservation.CampingPlace.Id).ToList();
                }
            }

            return viewData;
        }

        #endregion
        
    }
}

