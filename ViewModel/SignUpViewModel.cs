using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using SystemCore;
using ViewModel.EventArguments;

namespace ViewModel
{
    public class SignUpViewModel : ObservableObject
    {
        #region Fields
        private string 
            _firstName, 
            _lastName, 
            _street, 
            _postalCode, 
            _place, 
            _phoneNumber,
            _email, 
            _password,
            _confirmPassword,
            _registerError;

        private DateTime _birthdate;
        #endregion

        #region Properties
        public string RegisterError
        {
            get => this._registerError;
            private set
            {
                if (value == this._registerError)
                {
                    return;
                }

                this._registerError = value;
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

                this.RegisterError = "";
                if (!Validation.IsInputFilled(_firstName))
                {
                    this.RegisterError = "Voornaam is een verplicht veld";
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

                this.RegisterError = "";
                if (!Validation.IsInputFilled(_lastName))
                {
                    this.RegisterError = "Achternaam is een verplicht veld";
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

                this.RegisterError = "";
                if (!Validation.IsInputFilled(_street))
                {
                    this.RegisterError = "Straatnaam is een verplicht veld";
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

                this.RegisterError = "";
                if (!Validation.IsInputFilled(_postalCode))
                {
                    this.RegisterError = "Postcode is een verplicht veld";
                }
                if (!RegexHelper.IsPostalcodeValid(_postalCode))
                {
                    this.RegisterError = "Ongeldig postcode";
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

                this.RegisterError = "";
                if (!Validation.IsInputFilled(_place))
                {
                    this.RegisterError = "Plaatsnaam is een verplicht veld";
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

                this.RegisterError = "";
                if (!Validation.IsInputFilled(_phoneNumber))
                {
                    this.RegisterError = "Telefoonnummer is een verplicht veld";
                }
                else if (!Validation.IsNumber(_phoneNumber))
                {
                    this.RegisterError = "Ongeldig telefoonnummer";
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

                this._email = value.ToLower();
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

                this.RegisterError = "";
                if (!Validation.IsInputFilled(_email))
                {
                    this.RegisterError = "Email is een verplicht veld";
                }
                else if (!RegexHelper.IsEmailValid(_email))
                {
                    this.RegisterError = "Ongeldig email";
                }
            }
        }

        public string Password
        {
            get => this._password;
            set
            {
                if (value == this._password)
                {
                    return;
                }

                this._password = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

                this.RegisterError = "";
                if (!Validation.IsInputFilled(this._password))
                {
                    this.RegisterError = "Wachtwoord is een verplicht veld";
                }
            }
        }

        public string ConfirmPassword
        {
            get => this._confirmPassword;
            set
            {
                if (value == this._confirmPassword)
                {
                    return;
                }

                this._confirmPassword = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

                this.RegisterError = "";
                if (!Validation.IsInputFilled(this._confirmPassword))
                {
                    this.RegisterError = "Bevestig wachtwoord is een verplicht veld";
                }
                else if (!this._confirmPassword.Equals(this._password))
                {
                    this.RegisterError = "Wachtwoorden komen niet overeen";
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

                this.RegisterError = "";
                if (!Validation.IsBirthdateValid(_birthdate))
                {
                    this.RegisterError = "Ongeldig geboortedatum";
                } 
                else if (!Validation.IsBirthdateAdult(_birthdate))
                {
                    this.RegisterError = "U bent te jong om te reserveren";
                }
            }
        }

        #endregion

        #region Events

        public static event EventHandler<AccountEventArgs> SignUpEvent;

        #endregion
        
        #region Viewconstruction
        public SignUpViewModel()
        {
            this.Birthdate = new DateTime(2000,01,01);
        }
        
        private void ResetInput()
        {
            this._firstName = string.Empty;
            this._lastName = string.Empty;
            this._street = string.Empty;
            this._postalCode = string.Empty;
            this._place = string.Empty;
            this._phoneNumber = string.Empty;
            this._email = string.Empty;
            this._password = string.Empty;
            this._confirmPassword = string.Empty;
            this._registerError = string.Empty;
            
            this.OnPropertyChanged(new PropertyChangedEventArgs(null));
        }
        #endregion

        #region Commands

        private void ExecuteRegister()
        {
            Account accountModel = new Account(this.Email, PasswordHashing.HashPassword(this.Password), AccountRights.Customer.ToString());
            if (accountModel.SelectByEmail(this.Email) != null)
            {
                this.RegisterError = "Er bestaat al een account met dit email";
                return; 
            }
            
            accountModel.Insert();
            var insertedAccount = accountModel.SelectByEmail(this.Email);

            Address addressModel = new Address(this.Street, this.PostalCode, this.Place);
            var address = addressModel.FirstAndUpdateOrInsert();

            CampingCustomer campingCustomer = new CampingCustomer(insertedAccount, address, this.Birthdate.ToShortDateString(), this.PhoneNumber, this.FirstName, this.LastName);
            campingCustomer.Insert();
            
            CurrentUser.SetCurrentUser(insertedAccount, campingCustomer.SelectByAccount(insertedAccount));
            SignUpViewModel.SignUpEvent?.Invoke(this, new AccountEventArgs(insertedAccount));
            this.ResetInput();
        }

        private bool CanExecuteRegister()
        {
           return  Validation.IsInputFilled(this.FirstName) &&
                    Validation.IsInputFilled(this.LastName) &&
                    Validation.IsInputFilled(this.Street) &&
                    Validation.IsInputFilled(this.PostalCode) &&
                    RegexHelper.IsPostalcodeValid(this.PostalCode) &&
                    Validation.IsInputFilled(this.Place) &&
                    Validation.IsInputFilled(this.PhoneNumber) &&
                    Validation.IsNumber(this.PhoneNumber) &&
                    Validation.IsInputFilled(this.Email) &&
                    RegexHelper.IsEmailValid(this.Email) &&
                    Validation.IsInputFilled(this.Password) &&
                    Validation.IsInputFilled(this.ConfirmPassword) &&
                    this.ConfirmPassword == this.Password;
        }

        public ICommand Register => new RelayCommand(ExecuteRegister, CanExecuteRegister);

        #endregion

    }

}
