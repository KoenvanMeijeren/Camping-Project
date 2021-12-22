using System;
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
    public class ManageAccommodationViewModel : ObservableObject
    {
        #region Fields

        private readonly Accommodation _accommodationModel = new Accommodation();

        private ObservableCollection<Accommodation> _accommodations = new ObservableCollection<Accommodation>();
        private Accommodation _selectedAccommodation;
        
        private string _name, _prefix, _accommodationError, _editTitle;

        #endregion

        #region Properties

        public string AccommodationError
        {
            get => this._accommodationError;
            set
            {
                if (value == this._accommodationError)
                {
                    return;
                }

                this._accommodationError = value;
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
                
                this.FillFields(this._selectedAccommodation);
            }
        }
        
        public string Prefix
        {
            get => this._prefix;
            set
            {
                if (value == this._prefix)
                {
                    return;
                }

                this._prefix = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

                this.AccommodationError = "";
                if (!Validation.IsInputFilled(this._prefix))
                {
                    this.AccommodationError = "Prefix is een verplicht veld";
                }
                else if (!Validation.IsInputBelowMaxLength(this._prefix, 2))
                {
                    this.AccommodationError = "Prefix mag maximaal 2 letters bevatten";
                }

                if ((this.SelectedAccommodation != null && this.SelectedAccommodation.Prefix == value))
                {
                    return;
                }
                
                if (!this.IsPrefixUnique(value))
                {
                    this.AccommodationError = "Prefix moet uniek zijn";
                }
            }
        }

        public string Name
        {
            get => this._name;
            set
            {
                if (value == this._name)
                {
                    return;
                }

                this._name = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

                this.AccommodationError = "";
                if (!Validation.IsInputFilled(this._name))
                {
                    this.AccommodationError = "Naam is een verplicht veld";
                }
            }
        }

        #endregion
        
        #region Events

        public static event EventHandler AccommodationsUpdated;

        #endregion

        #region View construction

        public ManageAccommodationViewModel()
        {
            this.EditTitle = "Accommodatie toevoegen";
         
            this.SetAccommodations();
        }

        private void SetAccommodations()
        {
            this.Accommodations.Clear();
            foreach (var accommodation in this.GetAccommodations())
            {
                this.Accommodations.Add(accommodation);
            }
        }
        
        private void FillFields(Accommodation accommodation)
        {
            if (accommodation == null)
            {
                this.ResetInput();
                return;
            }

            this.EditTitle = $"Accommodatie {accommodation} bewerken";

            this.Prefix = accommodation.Prefix;
            this.Name = accommodation.Name;
        }

        private void ResetInput()
        {
            this.EditTitle = "Accommodatie toevoegen";
            this.SelectedAccommodation = null;
            this.Prefix = string.Empty;
            this.Name = string.Empty;
            this.AccommodationError = string.Empty;
        }

        #endregion

        #region Commands
        
        private void ExecuteCancelEditAction()
        {
            this.ResetInput();
        }
        private bool CanExecuteCancelEditAction()
        {
            return Validation.IsInputFilled(this.Name) || Validation.IsInputFilled(this.Prefix) || this.SelectedAccommodation != null;
        }

        public ICommand CancelEditAction => new RelayCommand(ExecuteCancelEditAction, CanExecuteCancelEditAction);
        
        private void ExecuteEditSave()
        {
            if (this.SelectedAccommodation == null)
            {
                Accommodation accommodation = new Accommodation(this.Prefix, this.Name);
                accommodation.Insert();
                
                this.Accommodations.Add(accommodation);
            }
            else
            {
                this.SelectedAccommodation.Update(this.Prefix, this.Name);
            }
            
            this.SetAccommodations();
            this.ResetInput();
            ManageAccommodationViewModel.AccommodationsUpdated?.Invoke(this, EventArgs.Empty);

            MessageBox.Show("De accommodaties zijn succesvol bijgewerkt.", "Accommodatie bewerken");
        }
        private bool CanExecuteEditSave()
        {
            if (this.SelectedAccommodation != null && this.SelectedAccommodation.Prefix != this.Prefix && !this.IsPrefixUnique(this.Prefix) 
                || (this.SelectedAccommodation == null && !this.IsPrefixUnique(this.Prefix)))
            {
                return false;
            }

            return Validation.IsInputFilled(this.Name) 
                   && Validation.IsInputFilled(this.Prefix);
        }

        public ICommand EditSave => new RelayCommand(ExecuteEditSave, CanExecuteEditSave);

        private void ExecuteDelete()
        {
            var result = MessageBox.Show($"Weet u zeker dat u de accommodatie {this.SelectedAccommodation} wilt verwijderen?", "Accommodatie verwijderen", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }
            
            this.SelectedAccommodation.Delete();
            this.Accommodations.Remove(this.SelectedAccommodation);
            this.SelectedAccommodation = null;
            
            ManageAccommodationViewModel.AccommodationsUpdated?.Invoke(this, EventArgs.Empty);
        }
        private bool CanExecuteDelete()
        {
            return this.SelectedAccommodation != null && !this.SelectedAccommodation.HasCampingPlaceTypes();
        }

        public ICommand Delete => new RelayCommand(ExecuteDelete, CanExecuteDelete);

        #endregion

        #region Database interaction

        public virtual IEnumerable<Accommodation> GetAccommodations()
        {
            return this._accommodationModel.Select();
        }

        public virtual bool IsPrefixUnique(string prefix)
        {
            return this._accommodationModel.IsPrefixUnique(prefix);
        }
        
        #endregion
        
    }
}