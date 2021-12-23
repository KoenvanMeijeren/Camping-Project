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
    public class ManageCampingCustomerViewModel : ObservableObject
    {
        #region Fields

        private readonly CampingCustomer _campingCustomerModel = new CampingCustomer();

        private ObservableCollection<CampingCustomer> _campingCustomers = new ObservableCollection<CampingCustomer>();
        private CampingCustomer _selectedCampingCustomer;
        
        private string _firstName, _lastName, _phoneNumber, _street, _postalCode, _place, _campingCustomerError, _editTitle;
        private DateTime _birthdate;

        #endregion

        #region Properties

        public string CampingCustomerError
        {
            get => this._campingCustomerError;
            set
            {
                if (value == this._campingCustomerError)
                {
                    return;
                }

                this._campingCustomerError = value;
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

        public ObservableCollection<CampingCustomer> CampingCustomers
        {
            get => this._campingCustomers;
            set
            {
                if (value == this._campingCustomers)
                {
                    return;
                }

                this._campingCustomers = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }
        
        public CampingCustomer SelectedCampingCustomer
        {
            get => this._selectedCampingCustomer;
            set
            {
                if (value == this._selectedCampingCustomer)
                {
                    return;
                }

                this._selectedCampingCustomer = value;
                
                // This calls the on property changed event.
                this.FillFields(this._selectedCampingCustomer);
            }
        }

        public string FirstName
        {
            get => this._firstName;
            set
            {
                if (value == this._firstName)
                {
                    return;
                }

                this._firstName = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

                this.CampingCustomerError = "";
                if (!Validation.IsInputFilled(this._firstName))
                {
                    this.CampingCustomerError = "Voornaam is een verplicht veld";
                }
            }
        }

        public string LastName
        {
            get => this._lastName;
            set
            {
                if (value == this._lastName)
                {
                    return;
                }

                this._lastName = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

                this.CampingCustomerError = "";
                if (!Validation.IsInputFilled(this._lastName))
                {
                    this.CampingCustomerError = "Achternaam is een verplicht veld";
                }
            }
        }

        public DateTime Birthdate
        {
            get => this._birthdate;
            set
            {
                if (value == this._birthdate)
                {
                    return;
                }

                this._birthdate = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

                this.CampingCustomerError = "";
                if (!Validation.IsBirthdateValid(this._birthdate))
                {
                    this.CampingCustomerError = "Ongeldig geboortedatum";
                }
                else if (!Validation.IsBirthdateAdult(this._birthdate))
                {
                    this.CampingCustomerError = "U bent te jong om te reserveren";
                }
            }
        }
        
        public string PhoneNumber
        {
            get => this._phoneNumber;
            set
            {
                if (value == this._phoneNumber)
                {
                    return;
                }

                this._phoneNumber = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

                this.CampingCustomerError = "";
                if (!Validation.IsInputFilled(this._phoneNumber))
                {
                    this.CampingCustomerError = "Telefoonnummer is een verplicht veld";
                }
                else if (!Validation.IsNumber(this._phoneNumber))
                {
                    this.CampingCustomerError = "Ongeldig telefoonnummer";
                }
            }
        }

        public string Street
        {
            get => this._street;
            set
            {
                if (value == this._street)
                {
                    return;
                }

                this._street = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

                this.CampingCustomerError = "";
                if (!Validation.IsInputFilled(this._street))
                {
                    this.CampingCustomerError = "Straatnaam is een verplicht veld";
                }
            }
        }

        public string PostalCode
        {
            get => this._postalCode;
            set
            {
                if (value == this._postalCode)
                {
                    return;
                }

                this._postalCode = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

                this.CampingCustomerError = "";
                if (!Validation.IsInputFilled(this._postalCode))
                {
                    this.CampingCustomerError = "Postcode is een verplicht veld";
                }
                else if (!RegexHelper.IsPostalcodeValid(this._postalCode))
                {
                    this.CampingCustomerError = "Ongeldig postcode";
                }
            }
        }

        public string Place
        {
            get => this._place;
            set
            {
                if (value == this._place)
                {
                    return;
                }

                this._place = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

                this.CampingCustomerError = "";
                if (!Validation.IsInputFilled(this._place))
                {
                    this.CampingCustomerError = "Plaatsnaam is een verplicht veld";
                }
            }
        }
        
        #endregion

        #region View construction

        public ManageCampingCustomerViewModel()
        {
            this._campingCustomers = new ObservableCollection<CampingCustomer>();
            this._editTitle = "Campingklant toevoegen";
         
            // This calls the on property changed event.
            this.InitializeCampingCustomers();
        }

        /// <summary>
        /// Sets the available camping customers. Calling this method should be avoided, because this is a heavy method.
        /// </summary>
        private void InitializeCampingCustomers()
        {
            this.CampingCustomers.Clear();
            foreach (var campingPlace in this.GetCampingCustomers())
            {
                this.CampingCustomers.Add(campingPlace);
            }
        }

        private void FillFields(CampingCustomer campingCustomer)
        {
            if (campingCustomer == null)
            {
                this.ResetInput();
                return;
            }

            this._editTitle = $"Campingklant {campingCustomer} bewerken";
            this._firstName = campingCustomer.FirstName;
            this._lastName = campingCustomer.LastName;
            this._birthdate = campingCustomer.Birthdate;
            this._phoneNumber = campingCustomer.PhoneNumber;
            this._street = campingCustomer.Address.Street;
            this._postalCode = campingCustomer.Address.PostalCode;
            this._place = campingCustomer.Address.Place;
            
            this.OnPropertyChanged(new PropertyChangedEventArgs(null));
        }

        private void ResetInput()
        {
            this._editTitle = "Campingklant toevoegen";
            this._selectedCampingCustomer = null;
            this._firstName = string.Empty;
            this._lastName = string.Empty;
            this._birthdate = DateTime.MinValue;
            this._phoneNumber = string.Empty;
            this._street = string.Empty;
            this._postalCode = string.Empty;
            this._place = string.Empty;
            this._campingCustomerError = string.Empty;
            
            this.OnPropertyChanged(new PropertyChangedEventArgs(null));
        }

        #endregion

        #region Commands
        
        private void ExecuteCancelEditAction()
        {
            this.ResetInput();
        }
        private bool CanExecuteCancelEditAction()
        {
            return Validation.IsInputFilled(this.FirstName)
                   || Validation.IsInputFilled(this.LastName)
                   || Validation.IsInputFilled(this.PhoneNumber)
                   || Validation.IsInputFilled(this.Street)
                   || Validation.IsInputFilled(this.PostalCode)
                   || Validation.IsInputFilled(this.Place)
                   || Validation.IsBirthdateValid(this.Birthdate)
                   || this.SelectedCampingCustomer != null;
        }

        public ICommand CancelEditAction => new RelayCommand(ExecuteCancelEditAction, CanExecuteCancelEditAction);
        
        private void ExecuteEditSave()
        {
            Address addressModel = new Address(this.Street, this.PostalCode, this.Place);
            var address = addressModel.FirstAndUpdateOrInsert();
            
            if (this.SelectedCampingCustomer == null)
            {
                CampingCustomer campingCustomer = new CampingCustomer(null, address, this.Birthdate.ToString(CultureInfo.InvariantCulture), this.PhoneNumber, this.FirstName, this.LastName);
                campingCustomer.Insert();
                
                this.CampingCustomers.Add(campingCustomer);
            }
            else
            {
                var campingCustomer = this.SelectedCampingCustomer.SelectById(this.SelectedCampingCustomer.Id);
                campingCustomer.Update(campingCustomer.Account, address, this.Birthdate, this.PhoneNumber,
                    this.FirstName, this.LastName);

                this.CampingCustomers[this.CampingCustomers.IndexOf(this.SelectedCampingCustomer)] = campingCustomer;
            }
            
            this.ResetInput();
            
            MessageBox.Show("De campingklanten zijn succesvol bijgewerkt.", "Campingklanten bewerken");
        }
        private bool CanExecuteEditSave()
        {
            return Validation.IsInputFilled(this.FirstName)
                   && Validation.IsInputFilled(this.LastName)
                   && Validation.IsInputFilled(this.PhoneNumber)
                   && Validation.IsInputFilled(this.Street)
                   && Validation.IsInputFilled(this.PostalCode)
                   && Validation.IsInputFilled(this.Place)
                   && Validation.IsBirthdateValid(this.Birthdate);
        }

        public ICommand EditSave => new RelayCommand(ExecuteEditSave, CanExecuteEditSave);

        private void ExecuteDelete()
        {
            var result = MessageBox.Show($"Weet u zeker dat u de campingklant {this.SelectedCampingCustomer} wilt verwijderen?", "Campingklant verwijderen", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }
            
            this.SelectedCampingCustomer.Delete();
            this.CampingCustomers.Remove(this.SelectedCampingCustomer);
            this.SelectedCampingCustomer = null;
        }
        private bool CanExecuteDelete()
        {
            return this.SelectedCampingCustomer != null && !this._campingCustomerModel.HasReservations(this.SelectedCampingCustomer);
        }

        public ICommand Delete => new RelayCommand(ExecuteDelete, CanExecuteDelete);

        #endregion

        #region Database interaction

        public virtual IEnumerable<CampingCustomer> GetCampingCustomers()
        {
            return this._campingCustomerModel.Select();
        }

        #endregion
        
    }
}