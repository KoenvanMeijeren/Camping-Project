using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using SystemCore;
using ViewModel.EventArguments;

namespace ViewModel
{
    public class ReservationCampingMapViewModel : CampingMapViewModelBase
    {
        #region Fields
        private readonly CampingPlace _campingPlaceModel = new CampingPlace();
        private readonly Accommodation _accommodationModel = new Accommodation();
        private readonly Reservation _reservationModel = new Reservation();

        private const string 
            SelectAll = "Alle",
            ColorAvailable = "#FF68C948",
            ColorFilteredOut = "#4D4D4D",
            ColorReserved = "#C1272D";

        private readonly ObservableCollection<string> _accommodations;

        private DateTime _checkOutDate, _checkInDate;
        private string _minNightPrice, _maxNightPrice, _selectedAccommodation, _guests;
        
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
                this.FilterOverview();
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
                this.FilterOverview();
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
                this.FilterOverview();
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

                if (daysDifference > 0)
                {
                    this._checkOutDate = this._checkInDate.AddDays(daysDifference);
                }
                
                this.FilterOverview();
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

                if (this._checkOutDate < this.CheckInDate)
                {
                    this._checkInDate = this._checkOutDate.AddDays(-1);
                }
                
                this.FilterOverview();
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
                this.FilterOverview();
            }
        }
        
        #endregion

        #region Events

        public static event EventHandler<ReservationDurationEventArgs> ReserveEvent;

        #endregion
        
        #region View construction

        public ReservationCampingMapViewModel()
        {
            this._accommodations = new ObservableCollection<string>();

            this.InitializeInternalCampingFields();
            this.InitializeAccommodations();
            this._checkInDate = DateTime.Today;
            this._checkOutDate = DateTime.Today.AddDays(1);
            this._selectedAccommodation = SelectAll;
            this.InitializeOverview();

            ReservationPaymentViewModel.ReservationConfirmedEvent += this.ReservationCampingGuestViewModelOnReservationConfirmedEvent;
            ManageCampingMapViewModel.CampingPlacesUpdated += this.ManageCampingPlaceViewModelOnCampingPlacesUpdated;
            ManageAccommodationViewModel.AccommodationStringsUpdated += this.ManageAccommodationViewModelOnAccommodationsUpdated;
            ManageReservationViewModel.ReservationUpdated += this.ManageReservationViewModelOnReservationUpdated;
            ManageCampingPlaceTypeViewModel.CampingPlaceTypesUpdated += this.ManageCampingPlaceTypeViewModelOnCampingPlaceTypesUpdated;
        }

        private void ManageCampingPlaceTypeViewModelOnCampingPlaceTypesUpdated(object sender, UpdateModelEventArgs<CampingPlaceType> e)
        {
            this.InitializeOverview();
        }

        private void ManageReservationViewModelOnReservationUpdated(object sender, UpdateModelEventArgs<Reservation> e)
        {
            this.InitializeOverview();
        }

        private void ManageAccommodationViewModelOnAccommodationsUpdated(object sender, UpdateModelEventArgs<Accommodation> e)
        {
            if (e.Inserted)
            {
                this.Accommodations.Add(e.Model.ToString());
            }
            else if (e.Removed)
            {
                this.Accommodations.Remove(e.Model.ToString());
            }

            this._selectedAccommodation = SelectAll;
            
            // This method calls the on property changed event.
            this.FilterOverview();
        }

        private void ManageCampingPlaceViewModelOnCampingPlacesUpdated(object sender, UpdateModelEventArgs<CampingPlace> e)
        {
            var campingField = this.CampingFields[e.Model.Number];
            if (campingField == null)
            {
                return;
            }
            
            if (e.Removed)
            {
                campingField.CampingPlace = null;
                this.InitializeOverview();
                return;
            }
            
            campingField.CampingPlace = e.Model;
            this.InitializeOverview();
        }

        private void ReservationCampingGuestViewModelOnReservationConfirmedEvent(object sender, UpdateModelEventArgs<Reservation> e)
        {
            this.InitializeOverview();
        }

        /// <summary>
        /// Sets the available accommodations. Calling this method should be avoided, because this is a heavy method.
        /// </summary>
        private void InitializeAccommodations()
        {
            this.Accommodations.Clear();

            this.Accommodations.Add(SelectAll);
            foreach (var accommodation in this.GetAccommodations())
            {
                this.Accommodations.Add(accommodation.Name);
            }

            this.SelectedAccommodation = SelectAll;
        }

        /// <summary>
        /// Sets the available camping places. Calling this method should be avoided, because this is a heavy method.
        /// </summary>
        private void InitializeOverview()
        {
            this.SetCampingPlacesToFields();
            
            this.FilterOverview();
        }

        protected override void SetCampingPlacesToFields()
        {
            if (this.CampingFields == null || !this.CampingFields.Any())
            {
                this.InitializeInternalCampingFields();
            }
            
            foreach (CampingMapItemViewModel campingField in this.CampingFields.Values)
            {
                var campingPlace = this.GetCampingPlaceByNumber(campingField);
                if (campingPlace == null)
                {
                    continue;
                }
                
                campingField.CampingPlace = campingPlace;
            }
        }

        /// <summary>
        /// Filters the overview and triggers the on property changed event after filtering.
        /// </summary>
        private void FilterOverview()
        {
            if (this.CampingFields == null)
            {
                return;
            }

            bool CampingPlaceFilter(CampingPlace campingPlace) =>
                (this.SelectedAccommodation != null && (this.SelectedAccommodation.Equals(SelectAll) || campingPlace.Type.Accommodation.Name.Equals(this.SelectedAccommodation)))
                && (!int.TryParse(this.MinNightPrice, out int min) || campingPlace.TotalPrice >= min)
                && (!int.TryParse(this.MaxNightPrice, out int max) || campingPlace.TotalPrice <= max)
                && (!int.TryParse(this.Guests, out int guests) || campingPlace.Type.GuestLimit >= guests);

            foreach (CampingMapItemViewModel campingField in CampingFields.Values)
            {
                if (campingField.CampingPlace != null && CampingPlaceFilter(campingField.CampingPlace))
                {
                    campingField.BackgroundColor = ColorAvailable;
                } 
                else
                {
                    campingField.BackgroundColor = ColorFilteredOut;
                }
            }
            
            this.FilterOnReserved();
            this.OnPropertyChanged(new PropertyChangedEventArgs(null));
        }

        public void StartReservation(string selectedImage)
        {
            CampingMapItemViewModel selectedCampingField = this.GetSelectedCampingField(selectedImage);

            if (selectedCampingField == null || selectedCampingField.BackgroundColor != ColorAvailable)
            {
                return;
            }

            ReserveEvent?.Invoke(this, new ReservationDurationEventArgs(selectedCampingField.CampingPlace, this.CheckInDate, this.CheckOutDate));
            this.ResetInput();
        }

        #endregion

        #region Input

        private void ResetInput()
        {
            this._selectedAccommodation = SelectAll;
            this._checkInDate = DateTime.Today;
            this._checkOutDate = DateTime.Today.AddDays(1);
            this._minNightPrice = "";
            this._maxNightPrice = "";
            this._guests = "";
            
            this.OnPropertyChanged(new PropertyChangedEventArgs(null));
        }

        #endregion

        #region Database interaction

        private void FilterOnReserved()
        {
            // Removes reserved camping places from the list.
            foreach (Reservation reservation in this.GetReservations())
            {
                if (reservation.CheckInDatetime.Date > CheckOutDate.Date || CheckInDate.Date > reservation.CheckOutDatetime.Date)
                {
                    continue;
                }
                
                foreach (CampingMapItemViewModel campingField in CampingFields.Values)
                {
                    if (campingField.CampingPlace != null && campingField.CampingPlace.Id == reservation.CampingPlace.Id && campingField.BackgroundColor == ColorAvailable)
                    {
                        campingField.BackgroundColor = ColorReserved;
                    }
                }
            }
        }

        public virtual IEnumerable<Reservation> GetReservations()
        {
            return this._reservationModel.Select();
        }

        public virtual IEnumerable<Accommodation> GetAccommodations()
        {
            return this._accommodationModel.Select();
        }

        protected override CampingPlace GetCampingPlaceByNumber(CampingMapItemViewModel campingField)
        {
            return this._campingPlaceModel.SelectByPlaceNumber(campingField.LocationNumber);
        }
        
        #endregion    
    }
}
