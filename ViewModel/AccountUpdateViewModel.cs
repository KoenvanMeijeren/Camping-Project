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
        private string _firstName, _lastName, _street, _postalCode, _place, _email, _phoneNumber, _updateError;
        private DateTime _birthdate;
        private Account _currentAccount;

        #region properties
        public string UpdateError
        {
            get => this._updateError;
            set
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
                if (!RegexHelper.IsPostalcodeValid(this._postalCode))
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
                if (!Validation.IsInputFilled(_phoneNumber))
                {
                    this.UpdateError = "Telefoonnummer is een verplicht veld";
                }
                else if (!Validation.IsNumber(_phoneNumber))
                {
                    this.UpdateError = "Ongeldig telefoonnummer";
                }
            }
        }
        #endregion

        public static event EventHandler UpdateCancelEvent;
        public static event EventHandler UpdateConfirmEvent;
        public AccountUpdateViewModel()
        {
            AccountViewModel.ToAccountUpdatePageEvent += this.OnToAccountUpdatePageEvent;
        }

        private void OnToAccountUpdatePageEvent(object sender, EventArgs e)
        {
            this._currentAccount = CurrentUser.Account;

            if (this._currentAccount.Rights == AccountRights.Admin)
            {
                this.FirstName = CurrentUser.CampingOwner.FirstName;
                this.LastName = CurrentUser.CampingOwner.LastName;
                this.Email = CurrentUser.CampingOwner.Account.Email;
                this.PhoneNumber = "";
                this.Birthdate = new DateTime();
                this.Street = "";
                this.Place = "";
            }
            else
            {
                this.FirstName = CurrentUser.CampingCustomer.FirstName;
                this.LastName = CurrentUser.CampingCustomer.LastName;
                this.Email = CurrentUser.CampingCustomer.Account.Email;
                this.PhoneNumber = CurrentUser.CampingCustomer.PhoneNumber;
                this.Birthdate = CurrentUser.CampingCustomer.Birthdate;
                this.Street = CurrentUser.CampingCustomer.Address.Street;
                this.PostalCode = CurrentUser.CampingCustomer.Address.PostalCode;
                this.Place = CurrentUser.CampingCustomer.Address.Place;
            }
        }

        public ICommand UpdateCancel => new RelayCommand(ExecuteUpdateCancel);
        public ICommand UpdateConfirm => new RelayCommand(ExecuteUpdateConfirm, CanExecuteUpdateConfirm);

        private void ExecuteUpdateCancel()
        {
            UpdateCancelEvent?.Invoke(this, EventArgs.Empty);
        }

        private bool CanExecuteUpdateConfirm()
        {
            return  Validation.IsInputFilled(this._firstName) &&
                    Validation.IsInputFilled(this._lastName) &&
                    Validation.IsBirthdateValid(this._birthdate) &&
                    Validation.IsBirthdateAdult(this._birthdate) &&
                    Validation.IsInputFilled(this._street) &&
                    Validation.IsInputFilled(this._postalCode) &&
                    RegexHelper.IsPostalcodeValid(this._postalCode) &&
                    Validation.IsInputFilled(this._place) &&
                    Validation.IsInputFilled(this._phoneNumber) &&
                    Validation.IsNumber(this._phoneNumber);
        }

        private void ExecuteUpdateConfirm()
        {
            if (this._currentAccount.Rights == AccountRights.Admin)
            {
                CurrentUser.CampingOwner.Update(CurrentUser.CampingOwner.Account, FirstName, LastName);
                CurrentUser.SetCurrentUser(CurrentUser.Account, CurrentUser.CampingOwner);
            }
            else
            {
                CurrentUser.CampingCustomer.Address.Update(this.Street, this.PostalCode, this.Place);
                CurrentUser.CampingCustomer.Update(CurrentUser.CampingCustomer.Account, CurrentUser.CampingCustomer.Address, this.Birthdate, this.PhoneNumber, this.FirstName, this.LastName);
                CurrentUser.SetCurrentUser(CurrentUser.Account, CurrentUser.CampingCustomer);
            }

            UpdateConfirmEvent?.Invoke(this, EventArgs.Empty);
        }

    }
}
