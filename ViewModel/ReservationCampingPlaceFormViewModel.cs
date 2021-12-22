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
using ViewModel.EventArguments;

namespace ViewModel
{
    public class ReservationCampingPlaceFormViewModel : ObservableObject
    {
        #region Fields
        private readonly CampingPlace _campingPlaceModel = new CampingPlace();
        private readonly Accommodation _accommodationModel = new Accommodation();
        private readonly Reservation _reservationModel = new Reservation();
        
        public const string SelectAll = "Alle";

        private ObservableCollection<CampingPlace> _campingPlaces;
        private CampingPlace _selectedCampingPlace;
        
        private readonly ObservableCollection<string> _accommodations;

        private DateTime _checkOutDate, _checkInDate;
        private string _minNightPrice, _maxNightPrice, _selectedAccommodation, _guests;

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
                this.SetOverview();

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
                this.SetOverview();

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
                this.SetOverview();

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
                this.SetOverview();
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

                if (daysDifference > 0)
                {
                    this.CheckOutDate = this._checkInDate.AddDays(daysDifference);
                }
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
                this.SetOverview();
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

                if (this._checkOutDate < this.CheckInDate)
                {
                    this.CheckInDate = this._checkOutDate.AddDays(-1);
                }
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
        
        public ObservableCollection<string> Accommodations
        {
            get => this._accommodations;
            private init
            {
                if (Equals(value, this._accommodations))
                {
                    return;
                }
                
                this._accommodations = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string SelectedAccommodation
        {
            get => this._selectedAccommodation;
            set
            {
                if (Equals(value, this._selectedAccommodation))
                {
                    return;
                }

                this._selectedAccommodation = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
                
                this.SetOverview();
            }
        }

        #endregion

        #region View construction
        
        public ReservationCampingPlaceFormViewModel()
        {
            this.CampingPlaces = new ObservableCollection<CampingPlace>();
            this.Accommodations = new ObservableCollection<string>();
            
            this.SetAccommodations();
            this.SelectedAccommodation = SelectAll;
            this.CheckInDate = DateTime.Today;
            this.CheckOutDate = DateTime.Today.AddDays(1);
            
            ReservationCampingGuestViewModel.ReservationConfirmedEvent += this.ReservationCampingGuestViewModelOnReservationConfirmedEvent;
            ManageCampingMapViewModel.CampingPlacesUpdated += this.ManageCampingPlaceViewModelOnCampingPlacesUpdated;
            ManageAccommodationViewModel.AccommodationsUpdated += this.ManageAccommodationViewModelOnAccommodationsUpdated;
        }

        private void ManageAccommodationViewModelOnAccommodationsUpdated(object sender, EventArgs e)
        {
            this.SetAccommodations();
            this.SelectedAccommodation = SelectAll;
        }

        private void ManageCampingPlaceViewModelOnCampingPlacesUpdated(object sender, UpdateCampingPlaceEventArgs e)
        {
            if (e.Inserted)
            {
                this.CampingPlaces.Add(e.CampingPlace);
                return;
            }

            if (e.Removed)
            {
                this.CampingPlaces.Remove(e.CampingPlace);
                return;
            }

            this.CampingPlaces.Remove(e.CampingPlace);
            this.CampingPlaces.Add(e.CampingPlace);
        }

        private void ReservationCampingGuestViewModelOnReservationConfirmedEvent(object sender, ReservationEventArgs e)
        {
            this.SetOverview();
        }

        private void SetAccommodations()
        {
            this.Accommodations.Clear();

            this.Accommodations.Add(SelectAll);
            foreach (var accommodation in this.GetAccommodations())
            {
                this.Accommodations.Add(accommodation.Name);
            }
        }

        private void SetOverview()
        {
            if (this.CampingPlaces == null)
            {
                return;
            }
            
            // Removes all current camping places.
            this.CampingPlaces.Clear();

            bool CampingPlaceFilter(CampingPlace campingPlace) => 
                (this._selectedAccommodation != null && (this._selectedAccommodation.Equals(SelectAll) || campingPlace.Type.Accommodation.Name.Equals(this._selectedAccommodation))) 
                && (!int.TryParse(this.MinNightPrice, out int min) || campingPlace.TotalPrice >= min) 
                && (!int.TryParse(this.MaxNightPrice, out int max) || campingPlace.TotalPrice <= max) 
                && (!int.TryParse(this.Guests, out int guests) || campingPlace.Type.GuestLimit >= guests);

            var campingPlaceItems = this.GetCampingPlaces().Where(CampingPlaceFilter);
            foreach (CampingPlace item in campingPlaceItems)
            {
                this.CampingPlaces.Add(item);
            }
        }
        
        #endregion

        #region Input

        private void ResetInput()
        {
            this.SelectedAccommodation = SelectAll;
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
            // Removes reserved camping places from the list.
            foreach (Reservation reservation in this.GetReservations())
            {
                if (reservation.CheckInDatetime.Date <= checkOutDate.Date && checkInDate.Date <= reservation.CheckOutDatetime.Date)
                {
                    campingPlaceList = campingPlaceList.Where(campingPlace => campingPlace.Id != reservation.CampingPlace.Id).ToList();
                }
            }

            return campingPlaceList;
        }

        public virtual IEnumerable<Reservation> GetReservations()
        {
            return this._reservationModel.Select();
        }
        
        public virtual IEnumerable<Accommodation> GetAccommodations()
        {
            return this._accommodationModel.Select();
        }

        #endregion    
    }
}

