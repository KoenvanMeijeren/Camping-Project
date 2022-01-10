using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using SystemCore;

namespace ViewModel
{
    public class AccountUpdateViewModel : ObservableObject
    {
        #region Fields

        private string _firstName, _lastName, _street, _postalCode, _place, _email, _phoneNumber, _updateError;
        private DateTime _birthdate;
        private Account _currentAccount;

        #endregion

        #region Properties
        public string UpdateError
        {
            get => this._updateError;
            private set
            {
                if (value == this._updateError)
                {
                    return;
                }

                this._updateError = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
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

                this.UpdateError = "";
                if (!Validation.IsInputFilled(this._firstName))
                {
                    this.UpdateError = "Voornaam is een verplicht veld";
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

                this.UpdateError = "";
                if (!Validation.IsInputFilled(this._lastName))
                {
                    this.UpdateError = "Achternaam is een verplicht veld";
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

                this.UpdateError = "";
                if (!Validation.IsBirthdateValid(this._birthdate))
                {
                    this.UpdateError = "Ongeldig geboortedatum";
                }
                else if (!Validation.IsBirthdateAdult(this._birthdate))
                {
                    this.UpdateError = "U bent te jong om te reserveren";
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

                this.UpdateError = "";
                if (!Validation.IsInputFilled(this._street))
                {
                    this.UpdateError = "Straatnaam is een verplicht veld";
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

                this.UpdateError = "";
                if (!Validation.IsInputFilled(this._postalCode))
                {
                    this.UpdateError = "Postcode is een verplicht veld";
                }
                else if (!RegexHelper.IsPostalcodeValid(this._postalCode))
                {
                    this.UpdateError = "Ongeldig postcode";
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

                this.UpdateError = "";
                if (!Validation.IsInputFilled(this._place))
                {
                    this.UpdateError = "Plaatsnaam is een verplicht veld";
                }
            }
        }

        public string Email
        {
            get => this._email;
            set
            {
                if (value == this._email)
                {
                    return;
                }

                this._email = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
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

                this.UpdateError = "";
                if (!Validation.IsInputFilled(this._phoneNumber))
                {
                    this.UpdateError = "Telefoonnummer is een verplicht veld";
                }
                else if (!Validation.IsNumber(this._phoneNumber))
                {
                    this.UpdateError = "Ongeldig telefoonnummer";
                }
            }
        }
        #endregion

        #region Events
        
        public static event EventHandler UpdateCancelEvent;
        public static event EventHandler UpdateConfirmEvent;

        #endregion

        #region View construction

        public AccountUpdateViewModel()
        {
            AccountViewModel.ToAccountUpdatePageEvent += this.OnToAccountUpdatePageEvent;
        }

        private void OnToAccountUpdatePageEvent(object sender, EventArgs e)
        {
            this._currentAccount = CurrentUser.Account;

            if (this._currentAccount.Rights == AccountRights.Admin)
            {
                this._firstName = CurrentUser.CampingOwner.FirstName;
                this._lastName = CurrentUser.CampingOwner.LastName;
                this._email = CurrentUser.CampingOwner.Account.Email;
            }
            else
            {
                this._firstName = CurrentUser.CampingCustomer.FirstName;
                this._lastName = CurrentUser.CampingCustomer.LastName;
                this._email = CurrentUser.CampingCustomer.Account.Email;
                this._phoneNumber = CurrentUser.CampingCustomer.PhoneNumber;
                this._birthdate = CurrentUser.CampingCustomer.Birthdate;
                this._street = CurrentUser.CampingCustomer.Address.Street;
                this._postalCode = CurrentUser.CampingCustomer.Address.PostalCode;
                this._place = CurrentUser.CampingCustomer.Address.Place;
            }
            
            this.OnPropertyChanged(new PropertyChangedEventArgs(null));
        }

        #endregion

        #region Commands

        public ICommand UpdateCancel => new RelayCommand(ExecuteUpdateCancel);
        public ICommand UpdateConfirm => new RelayCommand(ExecuteUpdateConfirm, CanExecuteUpdateConfirm);

        private void ExecuteUpdateCancel()
        {
            AccountUpdateViewModel.UpdateCancelEvent?.Invoke(this, EventArgs.Empty);
        }

        private bool CanExecuteUpdateConfirm()
        {
            if (CurrentUser.Account == null)
            {
                return false;
            }

            if (CurrentUser.Account.Rights == AccountRights.Customer)
            {
                return Validation.IsInputFilled(this.FirstName) 
                       && Validation.IsInputFilled(this.LastName) 
                       && Validation.IsBirthdateValid(this.Birthdate) 
                       && Validation.IsBirthdateAdult(this.Birthdate) 
                       && Validation.IsInputFilled(this.Street) 
                       && Validation.IsInputFilled(this.PostalCode) 
                       && RegexHelper.IsPostalcodeValid(this.PostalCode) 
                       && Validation.IsInputFilled(this.Place) 
                       && Validation.IsInputFilled(this.PhoneNumber) 
                       && Validation.IsNumber(this.PhoneNumber);
            }

            return Validation.IsInputFilled(this.FirstName) && Validation.IsInputFilled(this.LastName);
        }

        private void ExecuteUpdateConfirm()
        {
            if (this._currentAccount.Rights == AccountRights.Admin)
            {
                CurrentUser.CampingOwner.Update(CurrentUser.CampingOwner.Account, this.FirstName, this.LastName);
                CurrentUser.SetCurrentUser(CurrentUser.Account, CurrentUser.CampingOwner);
            }
            else
            {
                CurrentUser.CampingCustomer.Address.Update(this.Street, this.PostalCode, this.Place);
                CurrentUser.CampingCustomer.Update(CurrentUser.CampingCustomer.Account, CurrentUser.CampingCustomer.Address, this.Birthdate, this.PhoneNumber, this.FirstName, this.LastName);
                CurrentUser.SetCurrentUser(CurrentUser.Account, CurrentUser.CampingCustomer);
            }

            AccountUpdateViewModel.UpdateConfirmEvent?.Invoke(this, EventArgs.Empty);
        }

        #endregion

    }
}
