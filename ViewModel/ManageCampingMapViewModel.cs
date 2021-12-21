using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using SystemCore;
using ViewModel.EventArguments;

namespace ViewModel
{
    public class ManageCampingMapViewModel : ObservableObject
    {

        #region Fields

        private Dictionary<int, CampingField> _campingFields;
        private readonly CampingPlace _campingPlaceModel = new CampingPlace();
        private readonly CampingPlaceType _campingPlaceTypeModel = new CampingPlaceType();
        
        private ObservableCollection<CampingPlaceType> _campingPlaceTypes = new ObservableCollection<CampingPlaceType>();
        private CampingPlaceType _selectedCampingPlaceType;

        private CampingField _selectedCampingField;
        
        private string _number, _surface, _extraNightPrice, _campingPlaceError, _editTitle;

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

        #region Properties

        public CampingField SelectedCampingField 
        {
            get => this._selectedCampingField;
            set
            {
                if (value == this._selectedCampingField)
                {
                    return;
                }

                this._selectedCampingField = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }
        
        public string CampingPlaceError
        {
            get => this._campingPlaceError;
            set
            {
                if (value == this._campingPlaceError)
                {
                    return;
                }

                this._campingPlaceError = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string EditTitle
        {
            get => this._editTitle;
            set
            {
                if (value == this._editTitle)
                {
                    return;
                }

                this._editTitle = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public ObservableCollection<CampingPlaceType> CampingPlaceTypes
        {
            get => this._campingPlaceTypes;
            set
            {
                if (value == this._campingPlaceTypes)
                {
                    return;
                }

                this._campingPlaceTypes = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingPlaceType SelectedCampingPlaceType
        {
            get => this._selectedCampingPlaceType;
            set
            {
                if (Equals(value, this._selectedCampingPlaceType))
                {
                    return;
                }

                this._selectedCampingPlaceType = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

                this.CampingPlaceError = string.Empty;
                if (this._selectedCampingPlaceType == null)
                {
                    this.CampingPlaceError = "Verblijf is een verplicht veld";
                }
            }
        }

        public string Number
        {
            get => this._number;
            set
            {
                if (value == this._number)
                {
                    return;
                }

                this._number = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

                this.CampingPlaceError = "";
                if (!Validation.IsInputFilled(this._number))
                {
                    this.CampingPlaceError = "Locatienummer is een verplicht veld";
                }
                else if (!Validation.IsNumber(this._number))
                {
                    this.CampingPlaceError = "Ongeldig locatienummer opgegeven";
                }
            }
        }

        public string Surface
        {
            get => this._surface;
            set
            {
                if (value == this._surface)
                {
                    return;
                }

                this._surface = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

                this.CampingPlaceError = "";
                if (!Validation.IsInputFilled(this._surface))
                {
                    this.CampingPlaceError = "Oppervlakte is een verplicht veld";
                }
                else if (!Validation.IsDecimalNumber(this._surface))
                {
                    this.CampingPlaceError = "Ongeldig oppervlakte opgegeven";
                }
            }
        }
        
        public string ExtraNightPrice
        {
            get => this._extraNightPrice;
            set
            {
                if (value == this._extraNightPrice)
                {
                    return;
                }

                this._extraNightPrice = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

                this.CampingPlaceError = "";
                if (!Validation.IsInputFilled(this._extraNightPrice))
                {
                    this.CampingPlaceError = "Extra dagtarief is een verplicht veld";
                }
                else if (!Validation.IsDecimalNumber(this._extraNightPrice))
                {
                    this.CampingPlaceError = "Ongeldig extra dagtarief opgegeven";
                }
            }
        }
        
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
        
        #region Events

        public static event EventHandler CampingPlacesUpdated;

        #endregion

        #region View construction

        public ManageCampingMapViewModel()
        {
            this.EditTitle = "Campingplaats toevoegen";
         
            this.SetCampingPlaces();
            this.SetCampingPlaceTypes();
            
            ManageCampingPlaceTypeViewModel.CampingPlaceTypesUpdated += this.ManageCampingPlaceTypeViewModelOnCampingPlaceTypesUpdated;
            ManageAccommodationViewModel.AccommodationsUpdated += this.ManageAccommodationViewModelOnAccommodationsUpdated;
        }
        
        private void ManageAccommodationViewModelOnAccommodationsUpdated(object sender, EventArgs e)
        {
            this.SetCampingPlaceTypes();
            this.ResetInput();
        }

        private void ManageCampingPlaceTypeViewModelOnCampingPlaceTypesUpdated(object sender, EventArgs e)
        {
            this.SetCampingPlaceTypes();
            this.ResetInput();
        }

        private void SetCampingPlaces()
        {
            this.InitializeCampingFields();
            this.SetCampingPlacesToFields();
        }

        private void SetCampingPlaceTypes()
        {
            this.CampingPlaceTypes.Clear();
            foreach (var campingPlaceType in this.GetCampingPlaceTypes())
            {
                this.CampingPlaceTypes.Add(campingPlaceType);
            }
        }

        private void FillFields(CampingPlace campingPlace)
        {
            if (campingPlace == null)
            {
                this.ResetInput();
                return;
            }

            this.EditTitle = $"Campingplaats {campingPlace} bewerken";
            
            this.SelectedCampingPlaceType = campingPlace.Type;
            this.Number = campingPlace.Number.ToString(CultureInfo.InvariantCulture);
            this.Surface = campingPlace.Surface.ToString(CultureInfo.InvariantCulture);
            this.ExtraNightPrice = campingPlace.ExtraNightPrice.ToString(CultureInfo.InvariantCulture);
        }
        
        private void FillFieldsForSelectedCampingField(CampingField campingField)
        {
            if (campingField == null)
            {
                this.ResetInput();
                return;
            }
            
            this.FillFields(campingField.CampingPlace);
            this.Number = campingField.LocationNumber.ToString(CultureInfo.InvariantCulture);
        }

        private void ResetInput()
        {
            this.EditTitle = "Campingplaats toevoegen";
            this.SelectedCampingPlaceType = null;
            this.Number = string.Empty;
            this.Surface = string.Empty;
            this.ExtraNightPrice = string.Empty;
            this.CampingPlaceError = string.Empty;
        }

        private void InitializeCampingFields()
        {
            this._campingFields = new Dictionary<int, CampingField>
            {
                {1, this.CampingField1 = new CampingField(1, CampingField.UnselectedColor, null)},
                {2, this.CampingField2 = new CampingField(2, CampingField.UnselectedColor, null)},
                {3, this.CampingField3 = new CampingField(3, CampingField.UnselectedColor, null)},
                {4, this.CampingField4 = new CampingField(4, CampingField.UnselectedColor, null)},
                {5, this.CampingField5 = new CampingField(5, CampingField.UnselectedColor, null)},
                {6, this.CampingField6 = new CampingField(6, CampingField.UnselectedColor, null)},
                {7, this.CampingField7 = new CampingField(7, CampingField.UnselectedColor, null)},
                {8, this.CampingField8 = new CampingField(8, CampingField.UnselectedColor, null)},
                {9, this.CampingField9 = new CampingField(9, CampingField.UnselectedColor, null)},
                {10, this.CampingField10 = new CampingField(10, CampingField.UnselectedColor, null)},
                {11, this.CampingField11 = new CampingField(11, CampingField.UnselectedColor, null)},
                {12, this.CampingField12 = new CampingField(12, CampingField.UnselectedColor, null)},
                {13, this.CampingField13 = new CampingField(13, CampingField.UnselectedColor, null)},
                {14, this.CampingField14 = new CampingField(14, CampingField.UnselectedColor, null)},
                {15, this.CampingField15 = new CampingField(15, CampingField.UnselectedColor, null)},
                {16, this.CampingField16 = new CampingField(16, CampingField.UnselectedColor, null)},
                {17, this.CampingField17 = new CampingField(17, CampingField.UnselectedColor, null)},
                {18, this.CampingField18 = new CampingField(18, CampingField.UnselectedColor, null)},
                {19, this.CampingField19 = new CampingField(19, CampingField.UnselectedColor, null)},
                {20, this.CampingField20 = new CampingField(20, CampingField.UnselectedColor, null)},
                {21, this.CampingField21 = new CampingField(21, CampingField.UnselectedColor, null)},
                {22, this.CampingField22 = new CampingField(22, CampingField.UnselectedColor, null)},
                {23, this.CampingField23 = new CampingField(23, CampingField.UnselectedColor, null)},
                {24, this.CampingField24 = new CampingField(24, CampingField.UnselectedColor, null)},
                {25, this.CampingField25 = new CampingField(25, CampingField.UnselectedColor, null)},
                {26, this.CampingField26 = new CampingField(26, CampingField.UnselectedColor, null)},
                {27, this.CampingField27 = new CampingField(27, CampingField.UnselectedColor, null)},
                {28, this.CampingField28 = new CampingField(28, CampingField.UnselectedColor, null)},
                {29, this.CampingField29 = new CampingField(29, CampingField.UnselectedColor, null)},
                {30, this.CampingField30 = new CampingField(30, CampingField.UnselectedColor, null)},
                {31, this.CampingField31 = new CampingField(31, CampingField.UnselectedColor, null)},
                {32, this.CampingField32 = new CampingField(32, CampingField.UnselectedColor, null)},
                {33, this.CampingField33 = new CampingField(33, CampingField.UnselectedColor, null)},
                {34, this.CampingField34 = new CampingField(34, CampingField.UnselectedColor, null)}
            };
        }

        private void SetCampingPlacesToFields()
        {
            foreach (CampingField campingField in this._campingFields.Values)
            {
                campingField.CampingPlace = this._campingPlaceModel.SelectByPlaceNumber(campingField.LocationNumber);
            }
        }

        public void SetSelectedCampingField(string selectedImageComponent)
        {
            if (SelectedCampingField != null)
            {
                this.SelectedCampingField.BackgroundColor = CampingField.UnselectedColor;
            }

            this.SelectedCampingField = selectedImageComponent switch
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
                _ => this.SelectedCampingField
            };

            if (this.SelectedCampingField == null)
            {
                return;
            }

            this.SelectedCampingField.BackgroundColor = CampingField.SelectedColor;
            this.FillFieldsForSelectedCampingField(this.SelectedCampingField);
        }

        #endregion
        
        #region Commands
        
        private void ExecuteCancelEditAction()
        {
            this.ResetInput();
            this.SelectedCampingField.BackgroundColor = CampingField.UnselectedColor;
            this.SelectedCampingField = null;
        }
        private bool CanExecuteCancelEditAction()
        {
            return Validation.IsInputFilled(this.Number)
                   || Validation.IsNumber(this.Surface)
                   || Validation.IsInputFilled(this.ExtraNightPrice)
                   || this.SelectedCampingPlaceType != null;
        }

        public ICommand CancelEditAction => new RelayCommand(ExecuteCancelEditAction, CanExecuteCancelEditAction);
        
        private void ExecuteEditSave()
        {
            if (this.SelectedCampingField.CampingPlace == null)
            {
                CampingPlace campingPlace = new CampingPlace(this.Number, this.Surface, this.ExtraNightPrice,
                    this.SelectedCampingPlaceType);
                campingPlace.Insert();
                
                this._campingFields[campingPlace.Number].CampingPlace = campingPlace;
            }
            else
            {
                var campingPlace = this.SelectedCampingField.CampingPlace;
                campingPlace.Update(this.Number, this.Surface, this.ExtraNightPrice, this.SelectedCampingPlaceType);

                this.SelectedCampingField.Update(campingPlace);
            }
            
            this.ResetInput();
            this.SelectedCampingField.BackgroundColor = CampingField.UnselectedColor;
            
            ManageCampingMapViewModel.CampingPlacesUpdated?.Invoke(this, EventArgs.Empty);

            MessageBox.Show("De campingplaatsen zijn succesvol bijgewerkt.", "Campingplaatsen bewerken");
        }
        private bool CanExecuteEditSave()
        {
            return Validation.IsInputFilled(this.Number)
                   && Validation.IsNumber(this.Number)
                   && Validation.IsInputFilled(this.Surface)
                   && Validation.IsDecimalNumber(this.Surface)
                   && Validation.IsInputFilled(this.ExtraNightPrice)
                   && Validation.IsDecimalNumber(this.ExtraNightPrice)
                   && this.SelectedCampingPlaceType != null;
        }

        public ICommand EditSave => new RelayCommand(ExecuteEditSave, CanExecuteEditSave);

        private void ExecuteDelete()
        {
            var result = MessageBox.Show($"Weet u zeker dat u de campingplaats {this.SelectedCampingField.CampingPlace} wilt verwijderen?", "Campingplaats verwijderen", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }
            
            this.SelectedCampingField.CampingPlace.Delete();
            this.ResetInput();
            this.SelectedCampingField.BackgroundColor = CampingField.UnselectedColor;
            this.SelectedCampingField.CampingPlace = null;

            ManageCampingMapViewModel.CampingPlacesUpdated?.Invoke(this, EventArgs.Empty);
        }
        private bool CanExecuteDelete()
        {
            return this.SelectedCampingField != null && this.SelectedCampingField.CampingPlace != null && !this._campingPlaceModel.HasReservations(this.SelectedCampingField.CampingPlace);
        }

        public ICommand Delete => new RelayCommand(ExecuteDelete, CanExecuteDelete);

        #endregion

        #region Database interaction

        public virtual IEnumerable<CampingPlace> GetCampingPlaces()
        {
            return this._campingPlaceModel.Select();
        }

        public virtual IEnumerable<CampingPlaceType> GetCampingPlaceTypes()
        {
            return this._campingPlaceTypeModel.Select();
        }

        #endregion
    }
}
