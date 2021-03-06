using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Model;

namespace ViewModel
{
    public abstract class CampingMapViewModelBase : ObservableObject
    {
        #region Fields

        private CampingMapItemViewModel _selectedCampingMapItemViewModel;
        
        private CampingMapItemViewModel
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

        #region Properties

        public Dictionary<int, CampingMapItemViewModel> CampingFields { get; protected set; }

        public CampingMapItemViewModel SelectedCampingMapItemViewModel 
        {
            get => this._selectedCampingMapItemViewModel;
            set
            {
                if (value == this._selectedCampingMapItemViewModel)
                {
                    return;
                }

                this._selectedCampingMapItemViewModel = value;

                // This calls the on property changed event.
                this.FillFieldsForSelectedCampingField(value);
            }
        }
        
        public CampingMapItemViewModel CampingField1
        {
            get => this._campingField1;
            set
            {
                this._campingField1 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingMapItemViewModel CampingField2
        {
            get => this._campingField2;
            set
            {
                this._campingField2 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingMapItemViewModel CampingField3
        {
            get => this._campingField3;
            set
            {
                this._campingField3 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingMapItemViewModel CampingField4
        {
            get => this._campingField4;
            set
            {
                this._campingField4 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingMapItemViewModel CampingField5
        {
            get => this._campingField5;
            set
            {
                this._campingField5 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingMapItemViewModel CampingField6
        {
            get => this._campingField6;
            set
            {
                this._campingField6 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingMapItemViewModel CampingField7
        {
            get => this._campingField7;
            set
            {
                this._campingField7 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingMapItemViewModel CampingField8
        {
            get => this._campingField8;
            set
            {
                this._campingField8 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingMapItemViewModel CampingField9
        {
            get => this._campingField9;
            set
            {
                this._campingField9 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingMapItemViewModel CampingField10
        {
            get => this._campingField10;
            set
            {
                this._campingField10 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingMapItemViewModel CampingField11
        {
            get => this._campingField11;
            set
            {
                this._campingField11 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingMapItemViewModel CampingField12
        {
            get => this._campingField12;
            set
            {
                this._campingField12 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingMapItemViewModel CampingField13
        {
            get => this._campingField13;
            set
            {
                this._campingField13 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingMapItemViewModel CampingField14
        {
            get => this._campingField14;
            set
            {
                this._campingField14 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingMapItemViewModel CampingField15
        {
            get => this._campingField15;
            set
            {
                this._campingField15 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingMapItemViewModel CampingField16
        {
            get => this._campingField16;
            set
            {
                this._campingField16 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingMapItemViewModel CampingField17
        {
            get => this._campingField17;
            set
            {
                this._campingField17 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingMapItemViewModel CampingField18
        {
            get => this._campingField18;
            set
            {
                this._campingField18 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingMapItemViewModel CampingField19
        {
            get => this._campingField19;
            set
            {
                this._campingField19 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingMapItemViewModel CampingField20
        {
            get => this._campingField20;
            set
            {
                this._campingField20 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingMapItemViewModel CampingField21
        {
            get => this._campingField21;
            set
            {
                this._campingField21 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingMapItemViewModel CampingField22
        {
            get => this._campingField22;
            set
            {
                this._campingField22 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingMapItemViewModel CampingField23
        {
            get => this._campingField23;
            set
            {
                this._campingField23 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingMapItemViewModel CampingField24
        {
            get => this._campingField24;
            set
            {
                this._campingField24 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingMapItemViewModel CampingField25
        {
            get => this._campingField25;
            set
            {
                this._campingField25 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingMapItemViewModel CampingField26
        {
            get => this._campingField26;
            set
            {
                this._campingField26 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingMapItemViewModel CampingField27
        {
            get => this._campingField27;
            set
            {
                this._campingField27 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingMapItemViewModel CampingField28
        {
            get => this._campingField28;
            set
            {
                this._campingField28 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingMapItemViewModel CampingField29
        {
            get => this._campingField29;
            set
            {
                this._campingField29 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingMapItemViewModel CampingField30
        {
            get => this._campingField30;
            set
            {
                this._campingField30 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingMapItemViewModel CampingField31
        {
            get => this._campingField31;
            set
            {
                this._campingField31 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingMapItemViewModel CampingField32
        {
            get => this._campingField32;
            set
            {
                this._campingField32 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingMapItemViewModel CampingField33
        {
            get => this._campingField33;
            set
            {
                this._campingField33 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingMapItemViewModel CampingField34
        {
            get => this._campingField34;
            set
            {
                this._campingField34 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }
        
        #endregion

        #region View construction

        protected void InitializeCampingPlaces(string defaultColor = CampingMapItemViewModel.UnselectedColor)
        {
            this.InitializeInternalCampingFields(defaultColor);
            this.SetCampingPlacesToFields();
        }

        protected void InitializeInternalCampingFields(string defaultColor = CampingMapItemViewModel.UnselectedColor)
        {
            this.CampingFields = new Dictionary<int, CampingMapItemViewModel>
            {
                {1, this._campingField1 = new CampingMapItemViewModel(1, defaultColor, null)},
                {2, this._campingField2 = new CampingMapItemViewModel(2, defaultColor, null)},
                {3, this._campingField3 = new CampingMapItemViewModel(3, defaultColor, null)},
                {4, this._campingField4 = new CampingMapItemViewModel(4, defaultColor, null)},
                {5, this._campingField5 = new CampingMapItemViewModel(5, defaultColor, null)},
                {6, this._campingField6 = new CampingMapItemViewModel(6, defaultColor, null)},
                {7, this._campingField7 = new CampingMapItemViewModel(7, defaultColor, null)},
                {8, this._campingField8 = new CampingMapItemViewModel(8, defaultColor, null)},
                {9, this._campingField9 = new CampingMapItemViewModel(9, defaultColor, null)},
                {10, this._campingField10 = new CampingMapItemViewModel(10, defaultColor, null)},
                {11, this._campingField11 = new CampingMapItemViewModel(11, defaultColor, null)},
                {12, this._campingField12 = new CampingMapItemViewModel(12, defaultColor, null)},
                {13, this._campingField13 = new CampingMapItemViewModel(13, defaultColor, null)},
                {14, this._campingField14 = new CampingMapItemViewModel(14, defaultColor, null)},
                {15, this._campingField15 = new CampingMapItemViewModel(15, defaultColor, null)},
                {16, this._campingField16 = new CampingMapItemViewModel(16, defaultColor, null)},
                {17, this._campingField17 = new CampingMapItemViewModel(17, defaultColor, null)},
                {18, this._campingField18 = new CampingMapItemViewModel(18, defaultColor, null)},
                {19, this._campingField19 = new CampingMapItemViewModel(19, defaultColor, null)},
                {20, this._campingField20 = new CampingMapItemViewModel(20, defaultColor, null)},
                {21, this._campingField21 = new CampingMapItemViewModel(21, defaultColor, null)},
                {22, this._campingField22 = new CampingMapItemViewModel(22, defaultColor, null)},
                {23, this._campingField23 = new CampingMapItemViewModel(23, defaultColor, null)},
                {24, this._campingField24 = new CampingMapItemViewModel(24, defaultColor, null)},
                {25, this._campingField25 = new CampingMapItemViewModel(25, defaultColor, null)},
                {26, this._campingField26 = new CampingMapItemViewModel(26, defaultColor, null)},
                {27, this._campingField27 = new CampingMapItemViewModel(27, defaultColor, null)},
                {28, this._campingField28 = new CampingMapItemViewModel(28, defaultColor, null)},
                {29, this._campingField29 = new CampingMapItemViewModel(29, defaultColor, null)},
                {30, this._campingField30 = new CampingMapItemViewModel(30, defaultColor, null)},
                {31, this._campingField31 = new CampingMapItemViewModel(31, defaultColor, null)},
                {32, this._campingField32 = new CampingMapItemViewModel(32, defaultColor, null)},
                {33, this._campingField33 = new CampingMapItemViewModel(33, defaultColor, null)},
                {34, this._campingField34 = new CampingMapItemViewModel(34, defaultColor, null)}
            };
        }

        protected virtual void SetCampingPlacesToFields()
        {
            if (this.CampingFields == null || !this.CampingFields.Any())
            {
                this.InitializeInternalCampingFields();
            }
            
            foreach (CampingMapItemViewModel campingField in this.CampingFields.Values)
            {
                campingField.CampingPlace = this.GetCampingPlaceByNumber(campingField);
            }
        }

        public void SetSelectedCampingField(string selectedImageComponent)
        {
            if (SelectedCampingMapItemViewModel != null)
            {
                this.SelectedCampingMapItemViewModel.BackgroundColor = CampingMapItemViewModel.UnselectedColor;
            }

            this.SelectedCampingMapItemViewModel = this.GetSelectedCampingField(selectedImageComponent);
        }
        
        protected CampingMapItemViewModel GetSelectedCampingField(string selectedImageComponent)
        {
            CampingMapItemViewModel selectedCampingField = selectedImageComponent switch
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
        
        protected virtual void FillFieldsForSelectedCampingField(CampingMapItemViewModel campingMapItemViewModel)
        {
            if (campingMapItemViewModel == null)
            {
                return;
            }
            
            this.SelectedCampingMapItemViewModel.BackgroundColor = CampingMapItemViewModel.SelectedColor;
        }

        protected abstract CampingPlace GetCampingPlaceByNumber(CampingMapItemViewModel campingField);

        #endregion
    }
}