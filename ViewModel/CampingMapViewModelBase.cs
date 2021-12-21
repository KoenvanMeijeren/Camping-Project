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
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

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

        protected void InitializeCampingPlaces()
        {
            this.InitializeInternalCampingFields();
            this.SetCampingPlacesToFields();
        }

        protected void InitializeInternalCampingFields()
        {
            this.CampingFields = new Dictionary<int, CampingMapItemViewModel>
            {
                {1, this.CampingField1 = new CampingMapItemViewModel(1, CampingMapItemViewModel.UnselectedColor, null)},
                {2, this.CampingField2 = new CampingMapItemViewModel(2, CampingMapItemViewModel.UnselectedColor, null)},
                {3, this.CampingField3 = new CampingMapItemViewModel(3, CampingMapItemViewModel.UnselectedColor, null)},
                {4, this.CampingField4 = new CampingMapItemViewModel(4, CampingMapItemViewModel.UnselectedColor, null)},
                {5, this.CampingField5 = new CampingMapItemViewModel(5, CampingMapItemViewModel.UnselectedColor, null)},
                {6, this.CampingField6 = new CampingMapItemViewModel(6, CampingMapItemViewModel.UnselectedColor, null)},
                {7, this.CampingField7 = new CampingMapItemViewModel(7, CampingMapItemViewModel.UnselectedColor, null)},
                {8, this.CampingField8 = new CampingMapItemViewModel(8, CampingMapItemViewModel.UnselectedColor, null)},
                {9, this.CampingField9 = new CampingMapItemViewModel(9, CampingMapItemViewModel.UnselectedColor, null)},
                {10, this.CampingField10 = new CampingMapItemViewModel(10, CampingMapItemViewModel.UnselectedColor, null)},
                {11, this.CampingField11 = new CampingMapItemViewModel(11, CampingMapItemViewModel.UnselectedColor, null)},
                {12, this.CampingField12 = new CampingMapItemViewModel(12, CampingMapItemViewModel.UnselectedColor, null)},
                {13, this.CampingField13 = new CampingMapItemViewModel(13, CampingMapItemViewModel.UnselectedColor, null)},
                {14, this.CampingField14 = new CampingMapItemViewModel(14, CampingMapItemViewModel.UnselectedColor, null)},
                {15, this.CampingField15 = new CampingMapItemViewModel(15, CampingMapItemViewModel.UnselectedColor, null)},
                {16, this.CampingField16 = new CampingMapItemViewModel(16, CampingMapItemViewModel.UnselectedColor, null)},
                {17, this.CampingField17 = new CampingMapItemViewModel(17, CampingMapItemViewModel.UnselectedColor, null)},
                {18, this.CampingField18 = new CampingMapItemViewModel(18, CampingMapItemViewModel.UnselectedColor, null)},
                {19, this.CampingField19 = new CampingMapItemViewModel(19, CampingMapItemViewModel.UnselectedColor, null)},
                {20, this.CampingField20 = new CampingMapItemViewModel(20, CampingMapItemViewModel.UnselectedColor, null)},
                {21, this.CampingField21 = new CampingMapItemViewModel(21, CampingMapItemViewModel.UnselectedColor, null)},
                {22, this.CampingField22 = new CampingMapItemViewModel(22, CampingMapItemViewModel.UnselectedColor, null)},
                {23, this.CampingField23 = new CampingMapItemViewModel(23, CampingMapItemViewModel.UnselectedColor, null)},
                {24, this.CampingField24 = new CampingMapItemViewModel(24, CampingMapItemViewModel.UnselectedColor, null)},
                {25, this.CampingField25 = new CampingMapItemViewModel(25, CampingMapItemViewModel.UnselectedColor, null)},
                {26, this.CampingField26 = new CampingMapItemViewModel(26, CampingMapItemViewModel.UnselectedColor, null)},
                {27, this.CampingField27 = new CampingMapItemViewModel(27, CampingMapItemViewModel.UnselectedColor, null)},
                {28, this.CampingField28 = new CampingMapItemViewModel(28, CampingMapItemViewModel.UnselectedColor, null)},
                {29, this.CampingField29 = new CampingMapItemViewModel(29, CampingMapItemViewModel.UnselectedColor, null)},
                {30, this.CampingField30 = new CampingMapItemViewModel(30, CampingMapItemViewModel.UnselectedColor, null)},
                {31, this.CampingField31 = new CampingMapItemViewModel(31, CampingMapItemViewModel.UnselectedColor, null)},
                {32, this.CampingField32 = new CampingMapItemViewModel(32, CampingMapItemViewModel.UnselectedColor, null)},
                {33, this.CampingField33 = new CampingMapItemViewModel(33, CampingMapItemViewModel.UnselectedColor, null)},
                {34, this.CampingField34 = new CampingMapItemViewModel(34, CampingMapItemViewModel.UnselectedColor, null)}
            };
        }

        protected void SetCampingPlacesToFields()
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

        public virtual void SetSelectedCampingField(string selectedImageComponent)
        {
            if (SelectedCampingMapItemViewModel != null)
            {
                this.SelectedCampingMapItemViewModel.BackgroundColor = CampingMapItemViewModel.UnselectedColor;
            }

            this.SelectedCampingMapItemViewModel = selectedImageComponent switch
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
                _ => this.SelectedCampingMapItemViewModel
            };
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