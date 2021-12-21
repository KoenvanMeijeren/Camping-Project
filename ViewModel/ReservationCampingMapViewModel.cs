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
    public class ReservationCampingMapViewModel : ObservableObject
    {
        private Dictionary<int, CampingField> CampingFields;

        #region Fields
        private readonly CampingPlace _campingPlaceModel = new CampingPlace();
        private readonly Accommodation _accommodationModel = new Accommodation();
        private readonly Reservation _reservationModel = new Reservation();

        public const string SelectAll = "Alle";

        public const string ColorAvailable = "#FF68C948";
        public const string ColorFilteredOut = "#4D4D4D";
        public const string ColorReserved = "#C1272D";

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

        #region CampingField Fields

        private CampingField
        _campingField1,
        _campingField2,
        _campingField3,
        _campingField4,
        _campingField5,
        _campingField6,
        _campingField7,
        _campingField8,
        _campingField9,
        _campingField10,
        _campingField11,
        _campingField12,
        _campingField13,
        _campingField14,
        _campingField15,
        _campingField16,
        _campingField17,
        _campingField18,
        _campingField19,
        _campingField20,
        _campingField21,
        _campingField22,
        _campingField23,
        _campingField24,
        _campingField25,
        _campingField26,
        _campingField27,
        _campingField28,
        _campingField29,
        _campingField30,
        _campingField31,
        _campingField32,
        _campingField33,
        _campingField34;
        #endregion

        #region CampingField Properties
        public CampingField CampingField1
        {
            get => this._campingField1;
            set
            {
                this._campingField1 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField2
        {
            get => this._campingField2;
            set
            {
                this._campingField2 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField3
        {
            get => this._campingField3;
            set
            {
                this._campingField3 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField4
        {
            get => this._campingField4;
            set
            {
                this._campingField4 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField5
        {
            get => this._campingField5;
            set
            {
                this._campingField5 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField6
        {
            get => this._campingField6;
            set
            {
                this._campingField6 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField7
        {
            get => this._campingField7;
            set
            {
                this._campingField7 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField8
        {
            get => this._campingField8;
            set
            {
                this._campingField8 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField9
        {
            get => this._campingField9;
            set
            {
                this._campingField9 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField10
        {
            get => this._campingField10;
            set
            {
                this._campingField10 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField11
        {
            get => this._campingField11;
            set
            {
                this._campingField11 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField12
        {
            get => this._campingField12;
            set
            {
                this._campingField12 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField13
        {
            get => this._campingField13;
            set
            {
                this._campingField13 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField14
        {
            get => this._campingField14;
            set
            {
                this._campingField14 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField15
        {
            get => this._campingField15;
            set
            {
                this._campingField15 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField16
        {
            get => this._campingField16;
            set
            {
                this._campingField16 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField17
        {
            get => this._campingField17;
            set
            {
                this._campingField17 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField18
        {
            get => this._campingField18;
            set
            {
                this._campingField18 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField19
        {
            get => this._campingField19;
            set
            {
                this._campingField19 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField20
        {
            get => this._campingField20;
            set
            {
                this._campingField20 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField21
        {
            get => this._campingField21;
            set
            {
                this._campingField21 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField22
        {
            get => this._campingField22;
            set
            {
                this._campingField22 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField23
        {
            get => this._campingField23;
            set
            {
                this._campingField23 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField24
        {
            get => this._campingField24;
            set
            {
                this._campingField24 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField25
        {
            get => this._campingField25;
            set
            {
                this._campingField25 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField26
        {
            get => this._campingField26;
            set
            {
                this._campingField26 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField27
        {
            get => this._campingField27;
            set
            {
                this._campingField27 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField28
        {
            get => this._campingField28;
            set
            {
                this._campingField28 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField29
        {
            get => this._campingField29;
            set
            {
                this._campingField29 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField30
        {
            get => this._campingField30;
            set
            {
                this._campingField30 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField31
        {
            get => this._campingField31;
            set
            {
                this._campingField31 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField32
        {
            get => this._campingField32;
            set
            {
                this._campingField32 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField33
        {
            get => this._campingField33;
            set
            {
                this._campingField33 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField34
        {
            get => this._campingField34;
            set
            {
                this._campingField34 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }
        #endregion

        #region Graphics

        private const string
            ComponentsFolder = "/MapComponents/",
            AddButtonImage = "CampingFieldImage-Add.png",
            BungalowImage = "CampingFieldImage-Bungalow.png",
            CamperImage = "CampingFieldImage-Camper.png",
            CaravanImage = "CampingFieldImage-Caravan.png",
            ChaletImage = "CampingFieldImage-Chalet.png",
            TentImage = "CampingFieldImage-Tent.png",
            UnknownImage = "CampingFieldImage-Unknown.png";

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
            ManageCampingPlaceViewModel.CampingPlacesUpdated += this.ManageCampingPlaceViewModelOnCampingPlacesUpdated;
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
            this.CampingFields = new Dictionary<int, CampingField>
            {
                {1, this.CampingField1 = new CampingField(1, ColorAvailable, null)},
                {2, this.CampingField2 = new CampingField(2, ColorAvailable, null)},
                {3, this.CampingField3 = new CampingField(3, ColorAvailable, null)},
                {4, this.CampingField4 = new CampingField(4, ColorAvailable, null)},
                {5, this.CampingField5 = new CampingField(5, ColorAvailable, null)},
                {6, this.CampingField6 = new CampingField(6, ColorAvailable, null)},
                {7, this.CampingField7 = new CampingField(7, ColorAvailable, null)},
                {8, this.CampingField8 = new CampingField(8, ColorAvailable, null)},
                {9, this.CampingField9 = new CampingField(9, ColorAvailable, null)},
                {10, this.CampingField10 = new CampingField(10, ColorAvailable, null)},
                {11, this.CampingField11 = new CampingField(11, ColorAvailable, null)},
                {12, this.CampingField12 = new CampingField(12, ColorAvailable, null)},
                {13, this.CampingField13 = new CampingField(13, ColorAvailable, null)},
                {14, this.CampingField14 = new CampingField(14, ColorAvailable, null)},
                {15, this.CampingField15 = new CampingField(15, ColorAvailable, null)},
                {16, this.CampingField16 = new CampingField(16, ColorAvailable, null)},
                {17, this.CampingField17 = new CampingField(17, ColorAvailable, null)},
                {18, this.CampingField18 = new CampingField(18, ColorAvailable, null)},
                {19, this.CampingField19 = new CampingField(19, ColorAvailable, null)},
                {20, this.CampingField20 = new CampingField(20, ColorAvailable, null)},
                {21, this.CampingField21 = new CampingField(21, ColorAvailable, null)},
                {22, this.CampingField22 = new CampingField(22, ColorAvailable, null)},
                {23, this.CampingField23 = new CampingField(23, ColorAvailable, null)},
                {24, this.CampingField24 = new CampingField(24, ColorAvailable, null)},
                {25, this.CampingField25 = new CampingField(25, ColorAvailable, null)},
                {26, this.CampingField26 = new CampingField(26, ColorAvailable, null)},
                {27, this.CampingField27 = new CampingField(27, ColorAvailable, null)},
                {28, this.CampingField28 = new CampingField(28, ColorAvailable, null)},
                {29, this.CampingField29 = new CampingField(29, ColorAvailable, null)},
                {30, this.CampingField30 = new CampingField(30, ColorAvailable, null)},
                {31, this.CampingField31 = new CampingField(31, ColorAvailable, null)},
                {32, this.CampingField32 = new CampingField(32, ColorAvailable, null)},
                {33, this.CampingField33 = new CampingField(33, ColorAvailable, null)},
                {34, this.CampingField34 = new CampingField(34, ColorAvailable, null)}
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

            foreach (CampingField campingField in this.CampingFields.Values)
            {
                var campingPlace = emptyCampingPlace.SelectByPlaceNumber(campingField.LocationNumber);
                if (campingPlace != null)
                {
                    campingField.ImageResource = ComponentsFolder + GetCampingFieldImage(campingPlace.Type.Accommodation);
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

            foreach (CampingField campingField in CampingFields.Values)
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
            CampingField selectedCampingField = this.GetSelectedCampingField(selectedImage);

            if (selectedCampingField == null || selectedCampingField.BackgroundColor != ColorAvailable)
            {
                return;
            }

            ReserveEvent?.Invoke(this, new ReservationDurationEventArgs(selectedCampingField.CampingPlace, this.CheckInDate, this.CheckOutDate));
            this.ResetInput();
        }

        public CampingField GetSelectedCampingField(string selectedImage)
        {
            CampingField selectedCampingField = selectedImage switch
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


            private static string GetCampingFieldImage(Accommodation accommodation)
        {
            return accommodation.Type switch
            {
                AccommodationTypes.Bungalow => BungalowImage,
                AccommodationTypes.Camper => CamperImage,
                AccommodationTypes.Caravan => CaravanImage,
                AccommodationTypes.Chalet => ChaletImage,
                AccommodationTypes.Tent => TentImage,
                AccommodationTypes.Unknown => UnknownImage,
                _ => UnknownImage
            };
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

        public virtual void FilterOnReserved()
        {
            // Removes reserved camping places from the list.
            foreach (Reservation reservation in this.GetReservations())
            {
                if (reservation.CheckInDatetime.Date <= CheckOutDate.Date && CheckInDate.Date <= reservation.CheckOutDatetime.Date)
                {
                    foreach (CampingField campingField in CampingFields.Values)
                    {
                        if (campingField.CampingPlace != null && campingField.CampingPlace.Id == reservation.CampingPlace.Id && campingField.BackgroundColor == ColorAvailable)
                        {
                            campingField.BackgroundColor = ColorReserved;
                        }
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

        #endregion    
    }
}
