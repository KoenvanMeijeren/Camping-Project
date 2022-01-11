using Microsoft.Toolkit.Mvvm.Input;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using SystemCore;
using ViewModel.EventArguments;

namespace ViewModel
{
    public class ManageCampingMapViewModel : CampingMapViewModelBase
    {

        #region Fields

        private readonly CampingPlace _campingPlaceModel = new CampingPlace();
        private readonly CampingPlaceType _campingPlaceTypeModel = new CampingPlaceType();
        
        private ObservableCollection<CampingPlaceType> _campingPlaceTypes = new ObservableCollection<CampingPlaceType>();
        private CampingPlaceType _selectedCampingPlaceType;

        private string _number, _surface, _extraNightPrice, _campingPlaceError, _editTitle;

        #endregion

        #region Properties

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
        
        #endregion
        
        #region Events

        public static event EventHandler<UpdateModelEventArgs<CampingPlace>> CampingPlacesUpdated;

        #endregion

        #region View construction

        public ManageCampingMapViewModel()
        {
            this._editTitle = "Campingplaats toevoegen";
         
            this.InitializeCampingPlaces();
            this.InitializeCampingPlaceTypes();
            
            ManageCampingPlaceTypeViewModel.CampingPlaceTypesUpdated += this.ManageCampingPlaceTypeViewModelOnCampingPlaceTypesUpdated;
            ManageAccommodationViewModel.AccommodationsUpdated += this.ManageAccommodationViewModelOnAccommodationsUpdated;
        }
        
        private void ManageAccommodationViewModelOnAccommodationsUpdated(object sender, UpdateModelEventArgs<Accommodation> e)
        {
            this.InitializeCampingPlaceTypes();
            this.ResetInput();
        }

        private void ManageCampingPlaceTypeViewModelOnCampingPlaceTypesUpdated(object sender, UpdateModelEventArgs<CampingPlaceType> e)
        {
            e.UpdateCollection(this.CampingPlaceTypes);
            this.ResetInput();
        }

        /// <summary>
        /// Sets the available camping place types. Calling this method should be avoided, because this is a heavy
        /// method.
        /// </summary>
        private void InitializeCampingPlaceTypes()
        {
            this.CampingPlaceTypes.Clear();
            foreach (var campingPlaceType in this.GetCampingPlaceTypes())
            {
                this.CampingPlaceTypes.Add(campingPlaceType);
            }
        }

        public void FillFields(CampingPlace campingPlace)
        {
            if (campingPlace == null)
            {
                this.ResetInput();
                return;
            }

            this._editTitle = $"Campingplaats {campingPlace} bewerken";
            
            this._selectedCampingPlaceType = campingPlace.Type;
            this._number = campingPlace.Number.ToString(CultureInfo.InvariantCulture);
            this._surface = campingPlace.Surface.ToString(CultureInfo.InvariantCulture);
            this._extraNightPrice = campingPlace.ExtraNightPrice.ToString(CultureInfo.InvariantCulture);
            
            this.OnPropertyChanged(new PropertyChangedEventArgs(null));
        }
        
        protected override void FillFieldsForSelectedCampingField(CampingMapItemViewModel campingMapItemViewModel)
        {
            base.FillFieldsForSelectedCampingField(campingMapItemViewModel);
            if (campingMapItemViewModel == null)
            {
                this.ResetInput();
                return;
            }
            
            this.FillFields(campingMapItemViewModel.CampingPlace);
            this.Number = campingMapItemViewModel.LocationNumber.ToString(CultureInfo.InvariantCulture);
        }

        private void ResetInput()
        {
            this._editTitle = "Campingplaats toevoegen";
            this._selectedCampingPlaceType = null;
            this._number = string.Empty;
            this._surface = string.Empty;
            this._extraNightPrice = string.Empty;
            this._campingPlaceError = string.Empty;
            
            this.OnPropertyChanged(new PropertyChangedEventArgs(null));
        }

        #endregion
        
        #region Commands
        
        private void ExecuteCancelEditAction()
        {
            this.ResetInput();
            this.SelectedCampingMapItemViewModel.BackgroundColor = CampingMapItemViewModel.UnselectedColor;
            this.SelectedCampingMapItemViewModel = null;
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
            if (this.SelectedCampingMapItemViewModel.CampingPlace == null)
            {
                CampingPlace campingPlace = new CampingPlace(this.Number, this.Surface, this.ExtraNightPrice,
                    this.SelectedCampingPlaceType);
                campingPlace.Insert();
                
                this.CampingFields[campingPlace.Number].CampingPlace = campingPlace;
                ManageCampingMapViewModel.CampingPlacesUpdated?.Invoke(this, new UpdateModelEventArgs<CampingPlace>(campingPlace.SelectLast(), true, false));
            }
            else
            {
                var campingPlace = this.SelectedCampingMapItemViewModel.CampingPlace;
                campingPlace.Update(this.Number, this.Surface, this.ExtraNightPrice, this.SelectedCampingPlaceType);

                this.SelectedCampingMapItemViewModel.Update(campingPlace);
                ManageCampingMapViewModel.CampingPlacesUpdated?.Invoke(this, new UpdateModelEventArgs<CampingPlace>(campingPlace, false, false));
            }
            
            this.ResetInput();
            this.SelectedCampingMapItemViewModel.BackgroundColor = CampingMapItemViewModel.UnselectedColor;

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
            var result = MessageBox.Show($"Weet u zeker dat u de campingplaats {this.SelectedCampingMapItemViewModel.CampingPlace} wilt verwijderen?", "Campingplaats verwijderen", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }
            
            this.SelectedCampingMapItemViewModel.CampingPlace.Delete();
            this.ResetInput();
            this.SelectedCampingMapItemViewModel.BackgroundColor = CampingMapItemViewModel.UnselectedColor;

            ManageCampingMapViewModel.CampingPlacesUpdated?.Invoke(this, new UpdateModelEventArgs<CampingPlace>(this.SelectedCampingMapItemViewModel.CampingPlace, false, true));
            
            this.SelectedCampingMapItemViewModel.CampingPlace = null;
        }
        private bool CanExecuteDelete()
        {
            return this.SelectedCampingMapItemViewModel != null && this.SelectedCampingMapItemViewModel.CampingPlace != null && !this._campingPlaceModel.HasReservations(this.SelectedCampingMapItemViewModel.CampingPlace);
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

        protected override CampingPlace GetCampingPlaceByNumber(CampingMapItemViewModel campingField)
        {
            return this._campingPlaceModel.SelectByPlaceNumber(campingField.LocationNumber);
        }

        #endregion
    }
}
