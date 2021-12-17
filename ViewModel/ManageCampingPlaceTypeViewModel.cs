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
    public class ManageCampingPlaceTypeViewModel : ObservableObject
    {
        #region Fields

        private Accommodation _accommodationModel = new Accommodation();
        private CampingPlaceType _campingPlaceTypeModel = new CampingPlaceType();
        
        private ObservableCollection<CampingPlaceType> _campingPlaceTypes = new ObservableCollection<CampingPlaceType>();
        private CampingPlaceType _selectedCampingPlaceType;

        private ObservableCollection<Accommodation> _accommodations = new ObservableCollection<Accommodation>();
        private Accommodation _selectedAccommodation;
        
        private string _guestLimit, _standardNightPrice, _campingPlaceTypeError, _editTitle;

        #endregion

        #region Properties

        public string CampingPlaceTypeError
        {
            get => this._campingPlaceTypeError;
            set
            {
                if (value == this._campingPlaceTypeError)
                {
                    return;
                }

                this._campingPlaceTypeError = value;
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

                this.FillFields(this._selectedCampingPlaceType);
            }
        }
        
        public ObservableCollection<Accommodation> Accommodations
        {
            get => this._accommodations;
            set
            {
                if (value == this._accommodations)
                {
                    return;
                }

                this._accommodations = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }
        
        public Accommodation SelectedAccommodation
        {
            get => this._selectedAccommodation;
            set
            {
                if (value == this._selectedAccommodation)
                {
                    return;
                }

                this._selectedAccommodation = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
                
                this.CampingPlaceTypeError = string.Empty;
                if (this._selectedAccommodation == null)
                {
                    this.CampingPlaceTypeError = "Accommodatie is een verplicht veld";
                }
            }
        }

        public string GuestLimit
        {
            get => this._guestLimit;
            set
            {
                if (value == this._guestLimit)
                {
                    return;
                }

                this._guestLimit = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

                this.CampingPlaceTypeError = "";
                if (!Validation.IsInputFilled(this._guestLimit))
                {
                    this.CampingPlaceTypeError = "Gasten limiet is een verplicht veld";
                }
                else if (!Validation.IsNumber(this._guestLimit))
                {
                    this.CampingPlaceTypeError = "Ongeldig gasten limiet opgegeven";
                }
            }
        }

        public string StandardNightPrice
        {
            get => this._standardNightPrice;
            set
            {
                if (value == this._standardNightPrice)
                {
                    return;
                }

                this._standardNightPrice = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

                this.CampingPlaceTypeError = "";
                if (!Validation.IsInputFilled(this._standardNightPrice))
                {
                    this.CampingPlaceTypeError = "Dagtarief is een verplicht veld";
                }
                else if (!Validation.IsDecimalNumber(this._standardNightPrice))
                {
                    this.CampingPlaceTypeError = "Ongeldig dagtarief opgegeven";
                }
            }
        }
        
        #endregion

        #region View construction

        public ManageCampingPlaceTypeViewModel()
        {
            this.EditTitle = "Campingplaatstype toevoegen";
         
            this.SetAccommodations();
            this.SetCampingPlaceTypes();
        }

        private void SetAccommodations()
        {
            this.Accommodations.Clear();
            foreach (var accommodation in this.GetAccommodations())
            {
                this.Accommodations.Add(accommodation);
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

        private void FillFields(CampingPlaceType campingPlaceType)
        {
            if (campingPlaceType == null)
            {
                this.ResetInput();
                return;
            }

            this.EditTitle = $"Campingplaatstype {campingPlaceType} bewerken";
            
            this.SelectedAccommodation = campingPlaceType.Accommodation;
            this.GuestLimit = campingPlaceType.GuestLimit.ToString(CultureInfo.InvariantCulture);
            this.StandardNightPrice = campingPlaceType.StandardNightPrice.ToString(CultureInfo.InvariantCulture);
        }

        private void ResetInput()
        {
            this.EditTitle = "Campingplaatstype toevoegen";
            this.SelectedAccommodation = null;
            this.SelectedCampingPlaceType = null;
            this.GuestLimit = string.Empty;
            this.StandardNightPrice = string.Empty;
            this.CampingPlaceTypeError = string.Empty;
        }

        #endregion

        #region Commands
        
        private void ExecuteCancel()
        {
            this.ResetInput();
        }
        private bool CanExecuteCancel()
        {
            return Validation.IsInputFilled(this.GuestLimit) || Validation.IsInputFilled(this.StandardNightPrice) || this.SelectedAccommodation != null;
        }

        public ICommand Cancel => new RelayCommand(ExecuteCancel, CanExecuteCancel);
        
        private void ExecuteEditSave()
        {
            if (this.SelectedCampingPlaceType == null)
            {
                CampingPlaceType campingPlaceType = new CampingPlaceType(this.GuestLimit, this.StandardNightPrice, this.SelectedAccommodation);
                campingPlaceType.Insert();
                
                this.CampingPlaceTypes.Add(campingPlaceType);
            }
            else
            {
                this.SelectedCampingPlaceType.Update(this.GuestLimit, this.StandardNightPrice, this.SelectedAccommodation);
            }
            
            this.SetCampingPlaceTypes();
            this.ResetInput();

            MessageBox.Show("De campingplaatsen zijn succesvol bijgewerkt.", "Campingplaatsen bewerken");
        }
        private bool CanExecuteEditSave()
        {
            return Validation.IsInputFilled(this.GuestLimit) 
                   && Validation.IsNumber(this.GuestLimit)
                   && Validation.IsInputFilled(this.StandardNightPrice) 
                   && Validation.IsDecimalNumber(this.StandardNightPrice) 
                   && this.SelectedAccommodation != null;
        }

        public ICommand EditSave => new RelayCommand(ExecuteEditSave, CanExecuteEditSave);

        private void ExecuteDelete()
        {
            var result = MessageBox.Show($"Weet u zeker dat u de campingplaats {this.SelectedCampingPlaceType} wilt verwijderen?", "Campingplaats verwijderen", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }
            
            this.SelectedCampingPlaceType.Delete();
            this.CampingPlaceTypes.Remove(this.SelectedCampingPlaceType);
            this.SelectedCampingPlaceType = null;
        }
        private bool CanExecuteDelete()
        {
            return this.SelectedCampingPlaceType != null && !this._campingPlaceTypeModel.HasCampingPlaces(this.SelectedCampingPlaceType);
        }

        public ICommand Delete => new RelayCommand(ExecuteDelete, CanExecuteDelete);

        #endregion

        #region Database interaction

        public virtual IEnumerable<Accommodation> GetAccommodations()
        {
            return this._accommodationModel.Select();
        }

        public virtual IEnumerable<CampingPlaceType> GetCampingPlaceTypes()
        {
            return this._campingPlaceTypeModel.Select();
        }

        #endregion
        
    }
}