﻿using System;
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

namespace ViewModel
{
    public class ReservationCustomerFormViewModel : ObservableObject
    {
        #region Fields

        private readonly Dictionary<string, string> _errorDictionary;

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
        private DateTime _checkInDatetime;
        private DateTime _checkOutDatetime;
        private int _campingPlaceId;
        
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
                if (this.IsInputFilled(value))
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
                if (this.IsInputFilled(value))
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
                if (this.IsInputFilled(value))
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
                if (this.IsInputFilled(value))
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
                if (this.IsInputFilled(value))
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
                if (this.IsInputFilled(value))
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

        public string AmountOfGuests
        {
            get => this._amountOfGuests;
            set
            {
                this._amountOfGuests = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
                
                this.AmountOfGuestsError = string.Empty;
                this.RemoveErrorFromDictionary("AmountOfGuests");
                if (this.IsInputFilled(this._amountOfGuests) || !int.TryParse(value, out int x))
                {
                    return;
                }
                
                this.AmountOfGuestsError = "Ongeldige aantal gasten";
                this.AddErrorToDictionary("AmountOfGuests", "Ongeldige aantal gasten");
            }
        }
        public string AmountOfGuestsError
        {
            get => this._amountOfGuestsError;
            set
            {
                this._amountOfGuestsError = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }
        #endregion

        #region Events

        public static event EventHandler<ReservationEventArgs> ReservationConfirmedEvent;

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
                {"AmountOfGuests", ""},
            };
            
            ReservationSelectCampingPlaceViewModel.ReserveEvent += this.OnReserveEvent;
        }
        
        private void OnReserveEvent(object sender, ReservationDurationEventArgs args)
        {
            this._campingPlaceId = args.CampingPlaceId;
            this._checkInDatetime = args.CheckInDatetime;
            this._checkOutDatetime = args.CheckOutDatetime;
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
        
        private bool IsInputFilled(string input)
        {
            return (!string.IsNullOrEmpty(input) && input.Length != 0);
        }
        #endregion

        #region Commands
        private void ExecuteCustomerDataReservation()
        {
            //TODO: Insert with transaction
            Address addressModel = new Address(this.StreetName, this.PostalCode, this.PlaceName);
            var address = addressModel.FirstOrInsert();

            var customer = new CampingCustomer(null, address, this.Birthdate.ToShortDateString(), this.PhoneNumber, this.FirstName,
                this.LastName);
            customer.Insert();
            customer.SelectById(1);
            var lastCustomer = customer.SelectLast();

            ReservationDuration reservationDuration = new ReservationDuration(
                this._checkInDatetime.ToShortDateString(), 
                this._checkOutDatetime.ToShortDateString()
            );
            
            reservationDuration.Insert();
            var fetchNewestReservationDuration = reservationDuration.SelectLast();

            CampingPlace campingPlaceModel = new CampingPlace();
            CampingPlace campingPlace = campingPlaceModel.SelectById(this._campingPlaceId);
            Reservation reservation = new Reservation(this._amountOfGuests, lastCustomer, campingPlace, fetchNewestReservationDuration);
            reservation.Insert();
            
            ReservationConfirmedEvent?.Invoke(this, new ReservationEventArgs(reservation.SelectLast()));
        }
        private bool CanExecuteCustomerDataReservation()
        {
            return !this._errorDictionary.Any();
        }

        public ICommand AddCustomerReservation => new RelayCommand(ExecuteCustomerDataReservation, CanExecuteCustomerDataReservation);
        #endregion
    }
}
