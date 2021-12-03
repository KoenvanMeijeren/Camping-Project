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
        public Dictionary<string, string> ErrorDictionary { get; private set; } = new Dictionary<string, string>
            {
                {"FirstName", ""},
                {"LastName", ""},
                {"Birthdate", ""},
                {"PhoneNumber", ""},
                {"StreetName", ""},
                {"PostalCode", ""},
                {"PlaceName", ""},
                {"EmailAdress", ""},
                {"AmountOfGuests", ""},
            };

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
                if (ErrorDictionary.ContainsKey("FirstName"))
                {
                    this.FirstNameError = string.Empty;
                    ErrorDictionary.Remove("FirstName");
                }
                if (!CheckInputIsGiven(value))
                {
                    this.FirstNameError = "Ongeldige input";
                    ErrorDictionary.Add("FirstName", "Ongeldige input");
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
                if (ErrorDictionary.ContainsKey("LastName"))
                {
                    this.LastNameError = string.Empty;
                    ErrorDictionary.Remove("LastName");
                }
                if (!CheckInputIsGiven(value))
                {
                    this.LastNameError = "Ongeldige input";
                    ErrorDictionary.Add("LastName", "Ongeldige input");
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
                if (ErrorDictionary.ContainsKey("Birthdate"))
                {
                    this.BirthdateError = string.Empty;
                    ErrorDictionary.Remove("Birthdate");
                }
                if (!ValidateBirthday(value))
                {
                    this.BirthdateError = "Ongeldige input";
                    ErrorDictionary.Add("Birthdate", "Ongeldige input");
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
                if (ErrorDictionary.ContainsKey("PhoneNumber"))
                {
                    this.PhoneNumberError = string.Empty;
                    ErrorDictionary.Remove("PhoneNumber");
                }
                if (!ValidatePhoneNumber(value))
                {
                    this.PhoneNumberError = "Ongeldige input";
                    ErrorDictionary.Add("PhoneNumber", "Ongeldige input");
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
                if (ErrorDictionary.ContainsKey("StreetName"))
                {
                    this.StreetNameError = string.Empty;
                    ErrorDictionary.Remove("StreetName");
                }
                if (!CheckInputIsGiven(value))
                {
                    this.StreetNameError = "Ongeldige input";
                    ErrorDictionary.Add("StreetName", "Ongeldige input");
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
                if (ErrorDictionary.ContainsKey("PostalCode"))
                {
                    this.PostalCodeError = string.Empty;
                    ErrorDictionary.Remove("PostalCode");
                }
                if (!ValidatePostalCode(value))
                {
                    this.PostalCodeError = "Ongeldige input";
                    ErrorDictionary.Add("PostalCode", "Ongeldige input");
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
                if (ErrorDictionary.ContainsKey("PlaceName"))
                {
                    this.PlaceNameError = string.Empty;
                    ErrorDictionary.Remove("PlaceName");
                }
                if (!CheckInputIsGiven(value))
                {
                    this.PlaceNameError = "Ongeldige input";
                    ErrorDictionary.Add("PlaceName", "Ongeldige input");
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
                if (ErrorDictionary.ContainsKey("EmailAddress"))
                {
                    this.EmailAddress = string.Empty;
                    ErrorDictionary.Remove("EmailAdress");
                }
                if (!ValidateEmailAdress(value))
                {
                    this.EmailAddressError = "Ongeldige input";
                    ErrorDictionary.Add("EmailAddress", "Ongeldige input");
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
                if (ErrorDictionary.ContainsKey("AmountOfGuests"))
                {
                    this.AmountOfGuestsError = string.Empty;
                    ErrorDictionary.Remove("AmountOfGuests");
                }
                if (!int.TryParse(value, out int x))
                {
                    this.AmountOfGuestsError = "Ongeldige input";
                    ErrorDictionary.Add("AmountOfGuests", "Ongeldige input");
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
            ErrorDictionary = new Dictionary<string, string>();
        }

        #region validation methods
        private void removeFromErrorDictionary(string key)
        {
            ErrorDictionary.Remove(key);
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
            //If there are any errors in the given user input
            if (ErrorDictionary.Count == 0)
            {
                //TODO: Transactie en toevoegen aan controller
                // Create or/and fetch address based on user input
                Address address = new Address(_streetName, _postalCode, _placeName);
                var fetchLatestAddressOrCreateOne = address.FirstOrInsert();

                // Insert customer into CampingCustomer table
                // TODO: Need to new Account() later on...
                Account testAccount = new Account("1", "admin@hotmail.com", "nimda", 1);
                CampingCustomer campingCustomer = new CampingCustomer(testAccount, fetchLatestAddressOrCreateOne, _birthdate.ToString(), _emailAddress, _phoneNumber, _firstName, _lastName);
                // 'Invalid column name 'CampingCustomerEmail'.'
                campingCustomer.Insert();
                var fetchCampingCustomer = campingCustomer.SelectLast();

                // Insert and fetch reservation duration in ReservationDuration table
                ReservationDuration reservationDuration = new ReservationDuration(CheckInDatetime.ToString(), CheckOutDatetime.ToString());
                reservationDuration.Insert();
                var fetchNewestReservationDuration = reservationDuration.SelectLast();

                // Insert reservation in Reservation table
                CampingPlace campingPlaceModel = new CampingPlace();
                CampingPlace campingPlace = campingPlaceModel.Select(CampingPlaceID);
                Reservation reservation = new Reservation(_amountOfGuests, fetchCampingCustomer, campingPlace, fetchNewestReservationDuration);
                reservation.Insert();
            }
            ReservationConfirmedEvent?.Invoke(this, new ReservationConfirmedEventArgs(this.FirstName, this.LastName, this.CheckInDatetime, this.CheckOutDatetime));
        }
        private bool CanExecuteCustomerDataReservation()
        {
            // TODO: Doesn't work...
            return this.ErrorDictionary.Count == 0;
        }

        public ICommand AddCustomerReservation => new RelayCommand(ExecuteCustomerDataReservation, CanExecuteCustomerDataReservation);
        #endregion
    }
}
