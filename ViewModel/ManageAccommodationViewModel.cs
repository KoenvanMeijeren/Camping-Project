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
using ViewModel.EventArguments;

namespace ViewModel
{
    public class ManageAccommodationViewModel : ObservableObject
    {
        #region Fields
        private const int MaxPrefixLength = 2;
        private const string PrefixRequiredFieldString = "Prefix is een verplicht veld";
        private const string PrefixLengthMaxString1 = "Prefix mag maximaal";
        private const string PrefixLengthMaxString2 = "letters bevatten";
        private const string PrefixNotUniqueString = "Prefix moet uniek zijn";

        private const string NameRequiredFieldString = "Naam is een verplicht veld";

        private const string AccommodationAddString = "Accommodatie toevoegen";
        private const string AccommodationEditString1 = "Accommodatie";
        private const string AccommodationEditString2 = "bewerken";
        private const string AccommodationEditSuccessfullString = "De accommodaties zijn succesvol bijgewerkt.";
        private const string AccommodationDeleteString = "Accommodatie verwijderen";
        private const string AccommodationDeleteQuestionString1 = "Weet u zeker dat u de accommodatie";
        private const string AccommodationDeleteQuestionString2 = "wilt verwijderen?";

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

                // This triggers the on property changed event.
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
                this.ValidatePrefix(value);
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
                    this.AccommodationError = NameRequiredFieldString;
                }
            }
        }
        #endregion
        
        #region Events
        public static event EventHandler<UpdateModelEventArgs<Accommodation>> AccommodationsUpdated;
        public static event EventHandler<UpdateModelEventArgs<Accommodation>> AccommodationStringsUpdated;
        #endregion

        #region View construction
        public ManageAccommodationViewModel()
        {
            this._editTitle = AccommodationAddString;
         
            // This triggers the on property changed event call.
            this.InitializeAccommodations();
            
            ManageAccommodationViewModel.AccommodationsUpdated += ManageAccommodationViewModelOnAccommodationsUpdated;
        }
        private void ManageAccommodationViewModelOnAccommodationsUpdated(object sender, UpdateModelEventArgs<Accommodation> e)
        {
            e.UpdateCollection(this.Accommodations);
        }
        /// <summary>
        /// Sets the available accommodations. Calling this method should be avoided, because this is a heavy method.
        /// </summary>
        private void InitializeAccommodations()
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

            this._editTitle = $"{AccommodationEditString1} {accommodation} {AccommodationEditString2}";

            this._prefix = accommodation.Prefix;
            this._name = accommodation.Name;
            
            this.OnPropertyChanged(new PropertyChangedEventArgs(null));
        }

        private void ResetInput()
        {
            this._editTitle = AccommodationAddString;
            this._selectedAccommodation = null;
            this._prefix = string.Empty;
            this._name = string.Empty;
            this._accommodationError = string.Empty;
            
            this.OnPropertyChanged(new PropertyChangedEventArgs(null));
        }

        #endregion

        #region Input validation

        private void ValidatePrefix(string prefix)
        {
            if (!Validation.IsInputFilled(prefix))
            {
                this.AccommodationError = PrefixRequiredFieldString;
                return;
            }

            if (!Validation.IsInputBelowMaxLength(prefix, MaxPrefixLength))
            {
                this.AccommodationError = $"{PrefixLengthMaxString1} {MaxPrefixLength} {PrefixLengthMaxString2}";
                return;
            }

            if ((this.SelectedAccommodation != null && this.SelectedAccommodation.Prefix == prefix))
            {
                return;
            }
                
            if (!this.IsPrefixUnique())
            {
                this.AccommodationError = PrefixNotUniqueString;
            }
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
            if (this.SelectedAccommodation != null && this.SelectedAccommodation.Prefix != this.Prefix && !this.IsPrefixUnique() 
                || (this.SelectedAccommodation == null && !this.IsPrefixUnique()))
            {
                this.AccommodationError = PrefixNotUniqueString;
                return;
            }
            
            if (this.SelectedAccommodation == null)
            {
                Accommodation accommodation = new Accommodation(this.Prefix, this.Name);
                accommodation.Insert();
                accommodation = accommodation.SelectLast();
                
                ManageAccommodationViewModel.AccommodationsUpdated?.Invoke(this, new UpdateModelEventArgs<Accommodation>(accommodation, true, false));
                ManageAccommodationViewModel.AccommodationStringsUpdated?.Invoke(this, new UpdateModelEventArgs<Accommodation>(accommodation, true, false));
            }
            else
            {
                ManageAccommodationViewModel.AccommodationStringsUpdated?.Invoke(this, new UpdateModelEventArgs<Accommodation>(this.SelectedAccommodation, false, true));
                
                this.SelectedAccommodation.Update(this.Prefix, this.Name);
                
                ManageAccommodationViewModel.AccommodationStringsUpdated?.Invoke(this, new UpdateModelEventArgs<Accommodation>(this.SelectedAccommodation, true, false));
                ManageAccommodationViewModel.AccommodationsUpdated?.Invoke(this, new UpdateModelEventArgs<Accommodation>(this.SelectedAccommodation, false, false));
            }
            
            this.ResetInput();
            MessageBox.Show(AccommodationEditSuccessfullString, $"{AccommodationEditString1} {AccommodationEditString2}");
        }
        private bool CanExecuteEditSave()
        {
            bool canSave = Validation.IsInputFilled(this.Name) 
                   && Validation.IsInputFilled(this.Prefix);
            if (!canSave)
            {
                return false;
            }
            
            this.ValidatePrefix(this.Prefix);
            return string.IsNullOrEmpty(this._accommodationError);
        }

        public ICommand EditSave => new RelayCommand(ExecuteEditSave, CanExecuteEditSave);

        private void ExecuteDelete()
        {
            var result = MessageBox.Show($"{AccommodationDeleteQuestionString1} {this.SelectedAccommodation} {AccommodationDeleteQuestionString2}", AccommodationDeleteString, MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            var accommodation = this.SelectedAccommodation;
            ManageAccommodationViewModel.AccommodationsUpdated?.Invoke(this, new UpdateModelEventArgs<Accommodation>(accommodation, false, true));
            ManageAccommodationViewModel.AccommodationStringsUpdated?.Invoke(this, new UpdateModelEventArgs<Accommodation>(accommodation, false, true));

            accommodation.Delete();
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

        public virtual bool IsPrefixUnique()
        {
            return this._accommodationModel.IsPrefixUnique(this._prefix);
        }
        
        #endregion
    }
}