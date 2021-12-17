using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Model;
using SystemCore;

namespace ViewModel
{
    public class ManageCampingPlaceViewModel : ObservableObject
    {
        #region Fields

        private CampingPlace _campingPlaceModel = new CampingPlace();
        private CampingPlaceType _campingPlaceTypeModel = new CampingPlaceType();
        
        private ObservableCollection<CampingPlaceType> _campingPlaceTypes = new ObservableCollection<CampingPlaceType>();
        private CampingPlaceType _selectedCampingPlaceType;

        private ObservableCollection<CampingPlace> _campingPlaces = new ObservableCollection<CampingPlace>();
        private CampingPlace _selectedCampingPlace;
        
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
        
        public ObservableCollection<CampingPlace> CampingPlaces
        {
            get => this._campingPlaces;
            set
            {
                if (value == this._campingPlaces)
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
                if (value == this._selectedCampingPlace)
                {
                    return;
                }

                this._selectedCampingPlace = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
                
                this.FillFields(this._selectedCampingPlace);
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

        #region View construction

        public ManageCampingPlaceViewModel()
        {
            this.EditTitle = "Campingplaats toevoegen";
         
            this.SetCampingPlaces();
            this.SetCampingPlaceTypes();
        }

        private void SetCampingPlaces()
        {
            this.CampingPlaces.Clear();
            foreach (var campingPlace in this.GetCampingPlaces())
            {
                this.CampingPlaces.Add(campingPlace);
            }
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

            this.EditTitle = $"Campingplaats {this.SelectedCampingPlace} bewerken";
            
            this.SelectedCampingPlaceType = campingPlace.Type;
            this.Number = campingPlace.Number.ToString(CultureInfo.InvariantCulture);
            this.Surface = campingPlace.Surface.ToString(CultureInfo.InvariantCulture);
            this.ExtraNightPrice = campingPlace.ExtraNightPrice.ToString(CultureInfo.InvariantCulture);
        }

        private void ResetInput()
        {
            this.EditTitle = "Campingplaats toevoegen";
            this.SelectedCampingPlace = null;
            this.SelectedCampingPlaceType = null;
            this.Number = string.Empty;
            this.Surface = string.Empty;
            this.ExtraNightPrice = string.Empty;
            this.CampingPlaceError = string.Empty;
        }

        #endregion

        #region Commands
        
        private void ExecuteCancel()
        {
            this.ResetInput();
        }
        private bool CanExecuteCancel()
        {
            return Validation.IsInputFilled(this.Number)
                   || Validation.IsNumber(this.Surface)
                   || Validation.IsInputFilled(this.ExtraNightPrice)
                   || this.SelectedCampingPlaceType != null;
        }

        public ICommand Cancel => new RelayCommand(ExecuteCancel, CanExecuteCancel);
        
        private void ExecuteEditSave()
        {
            if (this.SelectedCampingPlace == null)
            {
                CampingPlace campingPlace = new CampingPlace(this.Number, this.Surface, this.ExtraNightPrice,
                    this.SelectedCampingPlaceType);
                campingPlace.Insert();
                
                this.CampingPlaces.Add(campingPlace);
            }
            else
            {
                this.SelectedCampingPlace.Update(this.Number, this.Surface, this.ExtraNightPrice, this.SelectedCampingPlaceType);
            }
            
            this.SetCampingPlaces();
            this.ResetInput();

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
            var result = MessageBox.Show($"Weet u zeker dat u de campingplaats {this.SelectedCampingPlace} wilt verwijderen?", "Campingplaats verwijderen", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }
            
            this.SelectedCampingPlace.Delete();
            this.CampingPlaces.Remove(this.SelectedCampingPlace);
            this.SelectedCampingPlace = null;
        }
        private bool CanExecuteDelete()
        {
            return this.SelectedCampingPlace != null && !this._campingPlaceModel.HasReservations(this.SelectedCampingPlace);
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