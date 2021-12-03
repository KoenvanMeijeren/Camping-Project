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
using System.Windows.Input;

namespace ViewModel
{
    public class ReservationCustomerFormViewModel : ObservableObject
    {
        #region fields
        public Dictionary<string, string> errorDictionary { get; private set; }

        private string _firstName;
        private string _firstNameError;
        private string _lastName;
        private string _lastNameError;
        private DateTime _birthdate;
        private string _birthdateError;
        private string _phoneNumber;
        private string _phoneNumberError;
        private string _streetName;
        private string _streetNameError;
        private string _postalCode;
        private string _postalCodeError;
        private string _placeName;
        private string _placeNameError;
        private string _emailAddress;
        private string _emailAddressError;
        private string _amountOfGuests;
        private string _amountOfGuestsError;

        private DateTime CheckInDatetime { get; set; }
        private DateTime CheckOutDatetime { get; set; }
        private int CampingPlaceID { get; set; }

        public static event EventHandler<ReservationConfirmedEventArgs> ReservationConfirmedEvent;
        #endregion

        #region properties
        public string FirstName
        {
            get
            {
                return this._firstName;
            }
            set
            {
                this._firstName = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
                if (errorDictionary.ContainsKey("FirstName"))
                {
                    this.FirstNameError = string.Empty;
                    errorDictionary.Remove("FirstName");
                }
                if (!CheckInputIsGiven(value))
                {
                    this.FirstNameError = "Ongeldige input";
                    errorDictionary.Add("FirstName", "Ongeldige input");
                }
            }
        }
        public string FirstNameError
        {
            get
            {
                return this._firstNameError;
            }
            set
            {
                this._firstNameError = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string LastName
        {
            get
            {
                return this._lastName;
            }
            set
            {
                this._lastName = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
                if (errorDictionary.ContainsKey("LastName"))
                {
                    this.LastNameError = string.Empty;
                    errorDictionary.Remove("LastName");
                }
                if (!CheckInputIsGiven(value))
                {
                    this.LastNameError = "Ongeldige input";
                    errorDictionary.Add("LastName", "Ongeldige input");
                }
            }
        }
        public string LastNameError
        {
            get
            {
                return this._lastNameError;
            }
            set
            {
                this._lastNameError = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public DateTime Birthdate
        {
            get
            {
                return this._birthdate;
            }
            set
            {
                this._birthdate = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
                if (errorDictionary.ContainsKey("Birthdate"))
                {
                    this.BirthdateError = string.Empty;
                    errorDictionary.Remove("Birthdate");
                }
                if (!ValidateBirthday(value))
                {
                    this.BirthdateError = "Ongeldige input";
                    errorDictionary.Add("Birthdate", "Ongeldige input");
                }
            }
        }
        public string BirthdateError
        {
            get
            {
                return this._birthdateError;
            }
            set
            {
                this._birthdateError = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string PhoneNumber
        {
            get
            {
                return this._phoneNumber;
            }
            set
            {
                this._phoneNumber = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
                if (errorDictionary.ContainsKey("PhoneNumber"))
                {
                    this.PhoneNumberError = string.Empty;
                    errorDictionary.Remove("PhoneNumber");
                }
                if (!ValidatePhoneNumber(value))
                {
                    this.PhoneNumberError = "Ongeldige input";
                    errorDictionary.Add("PhoneNumber", "Ongeldige input");
                }
            }
        }
        public string PhoneNumberError
        {
            get
            {
                return this._phoneNumberError;
            }
            set
            {
                this._phoneNumberError = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string StreetName
        {
            get
            {
                return this._streetName;
            }
            set
            {
                this._streetName = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
                if (errorDictionary.ContainsKey("StreetName"))
                {
                    this.StreetNameError = string.Empty;
                    errorDictionary.Remove("StreetName");
                }
                if (!CheckInputIsGiven(value))
                {
                    this.StreetNameError = "Ongeldige input";
                    errorDictionary.Add("StreetName", "Ongeldige input");
                }
            }
        }
        public string StreetNameError
        {
            get
            {
                return this._streetNameError;
            }
            set
            {
                this._streetNameError = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string PostalCode
        {
            get
            {
                return this._postalCode;
            }
            set
            {
                this._postalCode = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
                if (errorDictionary.ContainsKey("PostalCode"))
                {
                    this.PostalCodeError = string.Empty;
                    errorDictionary.Remove("PostalCode");
                }
                if (!ValidatePostalCode(value))
                {
                    this.PostalCodeError = "Ongeldige input";
                    errorDictionary.Add("PostalCode", "Ongeldige input");
                }
            }
        }
        public string PostalCodeError
        {
            get
            {
                return this._postalCodeError;
            }
            set
            {
                this._postalCodeError = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string PlaceName
        {
            get
            {
                return this._placeName;
            }
            set
            {
                this._placeName = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
                if (errorDictionary.ContainsKey("PlaceName"))
                {
                    this.PlaceNameError = string.Empty;
                    errorDictionary.Remove("PlaceName");
                }
                if (!CheckInputIsGiven(value))
                {
                    this.PlaceNameError = "Ongeldige input";
                    errorDictionary.Add("PlaceName", "Ongeldige input");
                }
            }
        }
        public string PlaceNameError
        {
            get
            {
                return this._placeNameError;
            }
            set
            {
                this._placeNameError = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string EmailAddress
        {
            get
            {
                return this._emailAddress;
            }
            set
            {
                this._emailAddress = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
                if (errorDictionary.ContainsKey("EmailAddress"))
                {
                    this.EmailAddress = string.Empty;
                    errorDictionary.Remove("EmailAdress");
                }
                if (!ValidateEmailAdress(value))
                {
                    this.EmailAddressError = "Ongeldige input";
                    errorDictionary.Add("EmailAddress", "Ongeldige input");
                }
            }
        }
        public string EmailAddressError
        {
            get
            {
                return this._emailAddressError;
            }
            set
            {
                this._emailAddressError = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string AmountOfGuests
        {
            get
            {
                return this._amountOfGuests;
            }
            set
            {
                this._amountOfGuests = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
                if (errorDictionary.ContainsKey("AmountOfGuests"))
                {
                    this.AmountOfGuestsError = string.Empty;
                    errorDictionary.Remove("AmountOfGuests");
                }
                if (!int.TryParse(value, out int x))
                {
                    this.AmountOfGuestsError = "Ongeldige input";
                    errorDictionary.Add("AmountOfGuests", "Ongeldige input");
                }
            }
        }
        public string AmountOfGuestsError
        {
            get
            {
                return this._amountOfGuestsError;
            }
            set
            {
                this._amountOfGuestsError = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }
        #endregion

        public ReservationCustomerFormViewModel()
        {
            CampingPlacesCollectionViewModel.ReserveEvent += this.OnReserveEvent;
            errorDictionary = new Dictionary<string, string>();
        }

        #region validation methods
        private void removeFromErrorDictionary(string key)
        {
            errorDictionary.Remove(key);
        }
        private Boolean CheckInputIsGiven(string input)
        {
            return (string.IsNullOrEmpty(input) || input.Length == 0) ? false : true;
        }
        // TODO: Birthday validation implementeren
        private Boolean ValidateBirthday(DateTime input)
        {
            _birthdate = input;
            removeFromErrorDictionary("birthdate");
            return true;
        }
        // TODO: Phone number validation implementeren
        private Boolean ValidatePhoneNumber(string input)
        {
            _phoneNumber = input;
            removeFromErrorDictionary("phoneNumber");
            return true;
        }
        // TODO: Postalcode validation implementeren
        private Boolean ValidatePostalCode(string input)
        {
            _postalCode = input;
            removeFromErrorDictionary("postalCode");
            return true;
        }
        // TODO: Email adress validation implementeren
        private Boolean ValidateEmailAdress(string input)
        {
            _emailAddress = input;
            removeFromErrorDictionary("emailAdress");
            return true;
        }
        #endregion

        public void OnReserveEvent(object sender, ReservationEventArgs args)
        {
            CampingPlaceID = args.CampingPlaceId;
            CheckInDatetime = args.CheckInDatetime;
            CheckOutDatetime = args.CheckOutDatetime;
        }

        #region Commands
        private void ExecuteCustomerDataReservation()
        {
            //insert call?
            ReservationConfirmedEvent?.Invoke(this, new ReservationConfirmedEventArgs(this.FirstName, this.LastName, this.CheckInDatetime, this.CheckOutDatetime));
        }
        private bool CanExecuteCustomerDataReservation()
        {            
            return this.errorDictionary.Count == 0;
        }

        public ICommand AddCustomerReservation => new RelayCommand(ExecuteCustomerDataReservation, CanExecuteCustomerDataReservation);

        #endregion

        
    }
}
