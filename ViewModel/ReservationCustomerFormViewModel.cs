using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;
using SystemCore;
using ViewModel.EventArguments;

namespace ViewModel
{
    public class ReservationCustomerFormViewModel : ObservableObject
    {
        #region Fields

        private readonly Dictionary<string, string> _errorDictionary;

        private string 
            _firstName,
            _firstNameError,
            
            _lastName,
            _lastNameError,
            
            _birthdateError,
            
            _phoneNumber,
            _phoneNumberError,
            
            _streetName,
            _streetNameError,
            
            _postalCode,
            _postalCodeError,
            
            _placeName,
            _placeNameError,
            
            _emailAddress,
            _emailAddressError,

            _amountOfGuests,

            _selectedCampingPlace;

        private bool
            _emailEnabled;
        
        private DateTime _birthdate, _checkInDateTime, _checkOutDateTime;

        private CampingPlace _campingPlace;

        private CampingCustomer _currentUserCustomer;
        
        #endregion

        #region Properties
        public string FirstName
        {
            get => this._firstName;
            set
            {
                this._firstName = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

                this.FirstNameError = string.Empty;
                this.RemoveErrorFromDictionary("FirstName");
                if (Validation.IsInputFilled(value))
                {
                    return;
                }
                
                this.FirstNameError = "Ongeldige input";
                this.AddErrorToDictionary("FirstName", "Ongeldige input");
            }
        }
        public string FirstNameError
        {
            get => this._firstNameError;
            set
            {
                this._firstNameError = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string LastName
        {
            get => this._lastName;
            set
            {
                this._lastName = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
                
                this.LastNameError = string.Empty;
                this.RemoveErrorFromDictionary("LastName");
                if (Validation.IsInputFilled(value))
                {
                    return;
                }
                
                this.LastNameError = "Ongeldige input";
                this.AddErrorToDictionary("LastName", "Ongeldige input");
            }
        }
        public string LastNameError
        {
            get => this._lastNameError;
            set
            {
                this._lastNameError = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public DateTime Birthdate
        {
            get => this._birthdate;
            set
            {
                this._birthdate = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
                
                this.BirthdateError = string.Empty;
                this.RemoveErrorFromDictionary("Birthdate");
                if (!this._birthdate.Equals(DateTime.MinValue))
                {
                    return;
                }
                
                this.BirthdateError = "Ongeldige input";
                this.AddErrorToDictionary("Birthdate", "Ongeldige input");
            }
        }
        public string BirthdateError
        {
            get => this._birthdateError;
            set
            {
                this._birthdateError = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string PhoneNumber
        {
            get => this._phoneNumber;
            set
            {
                this._phoneNumber = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
                
                this.PhoneNumberError = string.Empty;
                this.RemoveErrorFromDictionary("PhoneNumber");
                if (Validation.IsInputFilled(value))
                {
                    return;
                }
                
                this.PhoneNumberError = "Ongeldige input";
                this.AddErrorToDictionary("PhoneNumber", "Ongeldige input");
            }
        }
        public string PhoneNumberError
        {
            get => this._phoneNumberError;
            set
            {
                this._phoneNumberError = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string StreetName
        {
            get => this._streetName;
            set
            {
                this._streetName = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
                
                this.StreetNameError = string.Empty;
                this.RemoveErrorFromDictionary("StreetName");
                if (Validation.IsInputFilled(value))
                {
                    return;
                }
                
                this.StreetNameError = "Ongeldige straat";
                this.AddErrorToDictionary("StreetName", "Ongeldige straat");
            }
        }
        public string StreetNameError
        {
            get => this._streetNameError;
            set
            {
                this._streetNameError = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string PostalCode
        {
            get => this._postalCode;
            set
            {
                this._postalCode = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
                
                this.PostalCodeError = string.Empty;
                this.RemoveErrorFromDictionary("PostalCode");
                if (Validation.IsInputFilled(value))
                {
                    return;
                }
                
                this.PostalCodeError = "Ongeldige postcode";
                this.AddErrorToDictionary("PostalCode", "Ongeldige postcode");
            }
        }
        public string PostalCodeError
        {
            get => this._postalCodeError;
            set
            {
                this._postalCodeError = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string PlaceName
        {
            get => this._placeName;
            set
            {
                this._placeName = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
                
                this.PlaceNameError = string.Empty;
                this.RemoveErrorFromDictionary("PlaceName");
                if (Validation.IsInputFilled(value))
                {
                    return;
                }
                
                this.PlaceNameError = "Ongeldige plaats";
                this.AddErrorToDictionary("PlaceName", "Ongeldige plaats");
            }
        }
        public string PlaceNameError
        {
            get => this._placeNameError;
            set
            {
                this._placeNameError = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string EmailAddress
        {
            get => this._emailAddress;
            set
            {
                this._emailAddress = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
                
                this.EmailAddressError = string.Empty;
                this.RemoveErrorFromDictionary("EmailAddress");
                if (RegexHelper.IsEmailValid(value))
                {
                    return;
                }
                
                this.EmailAddressError = "Ongeldige email";
                this.AddErrorToDictionary("EmailAddress", "Ongeldige email");
            }
        }
        
        public string EmailAddressError
        {
            get => this._emailAddressError;
            set
            {
                this._emailAddressError = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }
        
        public bool EmailEnabled
        {
            get => this._emailEnabled;
            set
            {
                this._emailEnabled = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string AmountOfGuests {
            get => this._amountOfGuests;
            set
            {
                this._amountOfGuests = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }
        
        public CampingPlace CampingPlace
        {
            get => this._campingPlace;
            set
            {
                this._campingPlace = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }
        
        public string SelectedCampingPlace
        {
            get => this._selectedCampingPlace;
            set
            {
                this._selectedCampingPlace = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }
        
        public CampingCustomer CurrentUserCustomer
        {
            get => this._currentUserCustomer;
            set
            {
                this._currentUserCustomer = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
                
                this.FillInputWithCustomerData(this._currentUserCustomer);
            }
        }
        #endregion

        #region Events

        public static event EventHandler<ReservationGuestEventArgs> ReservationGuestEvent;

        #endregion

        public ReservationCustomerFormViewModel()
        {
            this._errorDictionary = new Dictionary<string, string>
            {
                {"FirstName", ""},
                {"LastName", ""},
                {"Birthdate", ""},
                {"PhoneNumber", ""},
                {"StreetName", ""},
                {"PostalCode", ""},
                {"PlaceName", ""},
                {"EmailAddress", ""},
            };
            
            ReservationCampingPlaceFormViewModel.ReserveEvent += this.OnReserveEvent;
            ReservationCampingGuestViewModel.ReservationGoBackEvent += ReservationCampingGuestViewModelOnReservationGoBackEvent;
            SignInViewModel.SignInEvent += SignInViewModelOnSignInEvent;
        }

        private void ReservationCampingGuestViewModelOnReservationGoBackEvent(object? sender, ReservationEventArgs e)
        {
            this._checkInDateTime = e.Reservation.CheckInDatetime;
            this._checkOutDateTime = e.Reservation.CheckOutDatetime;
            this.CampingPlace = e.Reservation.CampingPlace;
            this.SelectedCampingPlace = $"Reservering van {this._checkInDateTime.ToShortDateString()} tot {this._checkOutDateTime.ToShortDateString()} in verblijf {this._campingPlace.Location}";

            this.CurrentUserCustomer = CurrentUser.CampingCustomer;
            //Removes the customer from NumberOfPeople.
            this.AmountOfGuests = (e.Reservation.NumberOfPeople - 1).ToString();
        }

        private void SignInViewModelOnSignInEvent(object? sender, AccountEventArgs e)
        {
            this.CurrentUserCustomer = CurrentUser.CampingCustomer;
        }
        
        private void OnReserveEvent(object sender, ReservationDurationEventArgs args)
        {
            this._checkInDateTime = args.CheckInDatetime;
            this._checkOutDateTime = args.CheckOutDatetime;
            this.CampingPlace = args.CampingPlace;
            
            this.CurrentUserCustomer = CurrentUser.CampingCustomer;
            this.SelectedCampingPlace = $"Reservering van {this._checkInDateTime.ToShortDateString()} tot {this._checkOutDateTime.ToShortDateString()} in verblijf {this._campingPlace.Location}";
        }

        #region Input validation
        private void RemoveErrorFromDictionary(string key)
        {
            if (!this._errorDictionary.ContainsKey(key))
            {
                return;
            }

            this._errorDictionary.Remove(key);
        }

        private void AddErrorToDictionary(string key, string value)
        {
            if (this._errorDictionary.ContainsKey(key))
            {
                return;
            }
            
            this._errorDictionary.Add(key, value);
        }

        private void FillInputWithCustomerData(CampingCustomer campingCustomer)
        {
            if (campingCustomer == null)
            {
                return;
            }

            this.FirstName = campingCustomer.FirstName;
            this.LastName = campingCustomer.LastName;
            this.Birthdate = campingCustomer.Birthdate;
            this.EmailAddress = campingCustomer.Account.Email;
            this.PhoneNumber = campingCustomer.PhoneNumber;
            this.StreetName = campingCustomer.Address.Street;
            this.PlaceName = campingCustomer.Address.Place;
            this.PostalCode = campingCustomer.Address.PostalCode;

            this.EmailEnabled = campingCustomer.Account.Id != -1;
        }
        
        private void ResetInput()
        {
            this.FirstName = "";
            this.FirstNameError = "";
            this.LastName = "";
            this.LastNameError = "";
            this.Birthdate = DateTime.MinValue;
            this.BirthdateError = "";
            this.EmailAddress = "";
            this.EmailAddressError = "";
            this.PhoneNumber = "";
            this.PhoneNumberError = "";
            this.StreetName = "";
            this.StreetNameError = "";
            this.AmountOfGuests = "";
            this.PlaceName = "";
            this.PlaceNameError = "";
            this.PostalCode = "";
            this.PostalCodeError = "";
            this._errorDictionary.Clear();
        }
        #endregion

        #region Commands
        private void ExecuteCustomerDataReservation()
        {
            Address addressModel = new Address(this.StreetName, this.PostalCode, this.PlaceName);
            var address = addressModel.FirstAndUpdateOrInsert();

            var customer = new CampingCustomer(this._currentUserCustomer?.Id.ToString(), this._currentUserCustomer?.Account, address, this.Birthdate.ToShortDateString(), this.PhoneNumber, this.FirstName,
                this.LastName);
            if (customer.Id == -1)
            {
                customer.Insert();
                customer = customer.SelectLast();
            }
            else
            {
                customer.Update();
                CurrentUser.SetCurrentUser(CurrentUser.Account, customer);
            }

            Reservation reservation = new Reservation(this._amountOfGuests, customer, this.CampingPlace, this._checkInDateTime.ToString(CultureInfo.InvariantCulture), this._checkOutDateTime.ToString(CultureInfo.InvariantCulture));

            ReservationGuestEvent?.Invoke(this, new ReservationGuestEventArgs(address, customer, reservation));
            this.ResetInput();
        }
        private bool CanExecuteCustomerDataReservation()
        {
            return !this._errorDictionary.Any();
        }

        public ICommand AddCustomerReservation => new RelayCommand(ExecuteCustomerDataReservation, CanExecuteCustomerDataReservation);
        #endregion
    }
}
