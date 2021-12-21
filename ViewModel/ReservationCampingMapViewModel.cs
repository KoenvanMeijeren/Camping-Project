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

        public const string 
            SelectAll = "Alle",
            ColorAvailable = "#FF68C948",
            ColorFilteredOut = "#4D4D4D",
            ColorReserved = "#C1272D";

        private readonly ObservableCollection<string> _accommodations;

        private DateTime _checkOutDate, _checkInDate;
        private string _minNightPrice, _maxNightPrice, _selectedAccommodation, _guests;
        
        #endregion
        
        #region Properties
        
        public Dictionary<int, CampingMapItemViewModel> CampingFields { get; private set; }
        
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
                this.FilterFields();

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
                this.FilterFields();

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
                this.FilterFields();

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
                this.FilterFields();
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
                this.FilterFields();
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

                if (this._checkOutDate < this.CheckInDate)
                {
                    this.CheckInDate = this._checkOutDate.AddDays(-1);
                }
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

                this.FilterFields();
            }
        }
        
        #endregion

        #region Events

        public static event EventHandler<ReservationDurationEventArgs> ReserveEvent;

        #endregion
        
        #region View construction

        public ReservationCampingMapViewModel()
        {
            this.Accommodations = new ObservableCollection<string>();

            this.FillCampingFields();
            this.SetFields();
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

        private void ManageCampingPlaceViewModelOnCampingPlacesUpdated(object sender, EventArgs e)
        {
            this.SetFields();
        }

        private void ReservationCampingGuestViewModelOnReservationConfirmedEvent(object sender, ReservationEventArgs e)
        {
            this.SetFields();
        }

        private void FillCampingFields()
        {
            this.CampingFields = new Dictionary<int, CampingMapItemViewModel>
            {
                {1, this.CampingField1 = new CampingMapItemViewModel(1, ColorAvailable, null)},
                {2, this.CampingField2 = new CampingMapItemViewModel(2, ColorAvailable, null)},
                {3, this.CampingField3 = new CampingMapItemViewModel(3, ColorAvailable, null)},
                {4, this.CampingField4 = new CampingMapItemViewModel(4, ColorAvailable, null)},
                {5, this.CampingField5 = new CampingMapItemViewModel(5, ColorAvailable, null)},
                {6, this.CampingField6 = new CampingMapItemViewModel(6, ColorAvailable, null)},
                {7, this.CampingField7 = new CampingMapItemViewModel(7, ColorAvailable, null)},
                {8, this.CampingField8 = new CampingMapItemViewModel(8, ColorAvailable, null)},
                {9, this.CampingField9 = new CampingMapItemViewModel(9, ColorAvailable, null)},
                {10, this.CampingField10 = new CampingMapItemViewModel(10, ColorAvailable, null)},
                {11, this.CampingField11 = new CampingMapItemViewModel(11, ColorAvailable, null)},
                {12, this.CampingField12 = new CampingMapItemViewModel(12, ColorAvailable, null)},
                {13, this.CampingField13 = new CampingMapItemViewModel(13, ColorAvailable, null)},
                {14, this.CampingField14 = new CampingMapItemViewModel(14, ColorAvailable, null)},
                {15, this.CampingField15 = new CampingMapItemViewModel(15, ColorAvailable, null)},
                {16, this.CampingField16 = new CampingMapItemViewModel(16, ColorAvailable, null)},
                {17, this.CampingField17 = new CampingMapItemViewModel(17, ColorAvailable, null)},
                {18, this.CampingField18 = new CampingMapItemViewModel(18, ColorAvailable, null)},
                {19, this.CampingField19 = new CampingMapItemViewModel(19, ColorAvailable, null)},
                {20, this.CampingField20 = new CampingMapItemViewModel(20, ColorAvailable, null)},
                {21, this.CampingField21 = new CampingMapItemViewModel(21, ColorAvailable, null)},
                {22, this.CampingField22 = new CampingMapItemViewModel(22, ColorAvailable, null)},
                {23, this.CampingField23 = new CampingMapItemViewModel(23, ColorAvailable, null)},
                {24, this.CampingField24 = new CampingMapItemViewModel(24, ColorAvailable, null)},
                {25, this.CampingField25 = new CampingMapItemViewModel(25, ColorAvailable, null)},
                {26, this.CampingField26 = new CampingMapItemViewModel(26, ColorAvailable, null)},
                {27, this.CampingField27 = new CampingMapItemViewModel(27, ColorAvailable, null)},
                {28, this.CampingField28 = new CampingMapItemViewModel(28, ColorAvailable, null)},
                {29, this.CampingField29 = new CampingMapItemViewModel(29, ColorAvailable, null)},
                {30, this.CampingField30 = new CampingMapItemViewModel(30, ColorAvailable, null)},
                {31, this.CampingField31 = new CampingMapItemViewModel(31, ColorAvailable, null)},
                {32, this.CampingField32 = new CampingMapItemViewModel(32, ColorAvailable, null)},
                {33, this.CampingField33 = new CampingMapItemViewModel(33, ColorAvailable, null)},
                {34, this.CampingField34 = new CampingMapItemViewModel(34, ColorAvailable, null)}
            };
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

        private void SetFields()
        {
            CampingPlace emptyCampingPlace = new CampingPlace();

            foreach (CampingMapItemViewModel campingField in this.CampingFields.Values)
            {
                var campingPlace = emptyCampingPlace.SelectByPlaceNumber(campingField.LocationNumber);
                if (campingPlace != null)
                {
                    campingField.CampingPlace = campingPlace;
                }
            }
            
            this.FilterFields();
        }

        private void FilterFields()
        {
            if (this.CampingFields == null)
            {
                return;
            }

            bool CampingPlaceFilter(CampingPlace campingPlace) =>
                (this._selectedAccommodation != null && (this._selectedAccommodation.Equals(SelectAll) || campingPlace.Type.Accommodation.Name.Equals(this._selectedAccommodation)))
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

        private CampingMapItemViewModel GetSelectedCampingField(string selectedImage)
        {
            CampingMapItemViewModel selectedCampingField = selectedImage switch
            {
                "CampingFieldImage1" => this.CampingField1,
                "CampingFieldImage2" => this.CampingField2,
                "CampingFieldImage3" => this.CampingField3,
                "CampingFieldImage4" => this.CampingField4,
                "CampingFieldImage5" => this.CampingField5,
                "CampingFieldImage6" => this.CampingField6,
                "CampingFieldImage7" => this.CampingField7,
                "CampingFieldImage8" => this.CampingField8,
                "CampingFieldImage9" => this.CampingField9,
                "CampingFieldImage10" => this.CampingField10,
                "CampingFieldImage11" => this.CampingField11,
                "CampingFieldImage12" => this.CampingField12,
                "CampingFieldImage13" => this.CampingField13,
                "CampingFieldImage14" => this.CampingField14,
                "CampingFieldImage15" => this.CampingField15,
                "CampingFieldImage16" => this.CampingField16,
                "CampingFieldImage17" => this.CampingField17,
                "CampingFieldImage18" => this.CampingField18,
                "CampingFieldImage19" => this.CampingField19,
                "CampingFieldImage20" => this.CampingField20,
                "CampingFieldImage21" => this.CampingField21,
                "CampingFieldImage22" => this.CampingField22,
                "CampingFieldImage23" => this.CampingField23,
                "CampingFieldImage24" => this.CampingField24,
                "CampingFieldImage25" => this.CampingField25,
                "CampingFieldImage26" => this.CampingField26,
                "CampingFieldImage27" => this.CampingField27,
                "CampingFieldImage28" => this.CampingField28,
                "CampingFieldImage29" => this.CampingField29,
                "CampingFieldImage30" => this.CampingField30,
                "CampingFieldImage31" => this.CampingField31,
                "CampingFieldImage32" => this.CampingField32,
                "CampingFieldImage33" => this.CampingField33,
                "CampingFieldImage34" => this.CampingField34,
                _ => null
            };
            return selectedCampingField;
        }

        #endregion

        #region Input

        private void ResetInput()
        {
            this.SelectedAccommodation = SelectAll;
            this.CheckInDate = DateTime.Today;
            this.CheckOutDate = DateTime.Today.AddDays(1);
            this.MinNightPrice = "";
            this.MaxNightPrice = "";
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
