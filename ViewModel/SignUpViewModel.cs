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
    public class RegisterViewModel : ObservableObject
    {
        #region Fields
        private string 
            _firstName, 
            _lastName, 
            _streetName, 
            _streetNumber, 
            _postalCode, 
            _place, 
            _phonenumber,
            _email, 
            _password, 
            _registerError;

        private DateTime _birthdate;
        #endregion

        #region Properties
        public string RegisterError
        {
            get => this._registerError;
            set
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

        public string StreetName
        {
            get => this._streetName;
            set
            {
                if (value == this._streetName)
                {
                    return;
                }

                this._streetName = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

                this.RegisterError = "";
                if (!Validation.IsInputFilled(_streetName))
                {
                    this.RegisterError = "Straatnaam is een verplicht veld";
                }
            }
        }

        public string StreetNumber
        {
            get => this._streetNumber;
            set
            {
                if (value == this._streetNumber)
                {
                    return;
                }

                this._streetNumber = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

                this.RegisterError = "";
                if (!Validation.IsInputFilled(_streetNumber))
                {
                    this.RegisterError = "Huisnummer is een verplicht veld";
                } 
                else if (!Validation.IsNumber(_streetNumber))
                {
                    this.RegisterError = "Ongeldig huisnummer";
                }
            }
        }

        public string Postalcode
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

        public string Phonenumber
        {
            get => this._phonenumber;
            set
            {
                if (value == this._phonenumber)
                {
                    return;
                }

                this._phonenumber = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

                this.RegisterError = "";
                if (!Validation.IsInputFilled(_phonenumber))
                {
                    this.RegisterError = "Telefoonnummer is een verplicht veld";
                }
                else if (!Validation.IsNumber(_phonenumber))
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

                this._email = value;
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
                    this.RegisterError = "Ongeldig wachtwoord";
                }
                else
                {
                    this.RegisterError = "";
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

        #region Viewconstruction
        public RegisterViewModel()
        {
            this.Birthdate = new DateTime(2000,01,01);
        }
        #endregion

        public static event EventHandler<AccountEventArgs> SignUpEvent;

        private void ResetInput()
        {
            this.FirstName = "";
            this.LastName = "";
            this.StreetName = "";
            this.StreetNumber = "";
            this.Postalcode = "";
            this.Place = "";
            this.Phonenumber = "";
            this.Email = "";
            this.Password = "";
            this.RegisterError = "";
        }

        private void ExecuteRegister()
        {
            Account account = new Account(this.Email, this.Password, "0");
            if (account.SelectByEmail(this.Email) != null)
            {
                this.RegisterError = "Er bestaat al een account met dit email";
                return; 
            }
            account.Insert();

            Address address = new Address(this.StreetName + " " + this.StreetNumber, this.Postalcode, this.Place);
            address = address.FirstOrInsert();

            CampingCustomer campingCustomer = new CampingCustomer(account.SelectByEmail(this.Email), address, this.Birthdate.ToShortDateString(), this.Phonenumber, this.FirstName, this.LastName);
            campingCustomer.Insert();

            CurrentUser.SetCurrentUser(account);
            SignUpEvent?.Invoke(this, new AccountEventArgs(account));
            this.ResetInput();
        }

        private bool CanExecuteRegister()
        {
            return  Validation.IsInputFilled(_firstName) &&
                    Validation.IsInputFilled(_lastName) &&
                    Validation.IsInputFilled(_streetName) &&
                    Validation.IsInputFilled(_streetNumber) &&
                    Validation.IsNumber(_streetNumber) &&
                    Validation.IsInputFilled(_postalCode) &&
                    RegexHelper.IsPostalcodeValid(_postalCode) &&
                    Validation.IsInputFilled(_place) &&
                    Validation.IsInputFilled(_phonenumber) &&
                    Validation.IsNumber(_phonenumber) &&
                    Validation.IsNumber(_phonenumber) &&
                    Validation.IsInputFilled(_email) &&
                    RegexHelper.IsEmailValid(_email) &&
                    Validation.IsInputFilled(this._password);
        }

        public ICommand Register => new RelayCommand(ExecuteRegister, CanExecuteRegister);


    }

}
