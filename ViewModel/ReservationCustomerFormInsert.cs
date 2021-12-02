using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    class ReservationCustomerFormInsert
    {
        public Dictionary<string, string> errorDictionary { get; private set; }
        private string _firstNameValidated { get; set; }
        private string _firstName
        {
            get
            {
                return _firstNameValidated;
            }
            set
            {
                if (CheckInputIsGiven(value))
                {
                    _firstNameValidated = value;
                    removeFromErrorDictionary("firstName");
                    return;
                }
                    errorDictionary.Add("firstName", "Ongeldige waarde ingevoerd");
            }
        }

        private string _lastNameValidated { get; set; }
        private string _lastName
        {
            get
            {
                return _lastNameValidated;
            }
            set
            {
                if (CheckInputIsGiven(value))
                {
                    _lastNameValidated = value;
                    removeFromErrorDictionary("lastName");
                    return;
                }
                errorDictionary.Add("lastName", "Ongeldige waarde ingevoerd");
            }
        }

        private string _birthdateValidated { get; set; }
        private string _birthdate
        {
            get
            {
                return _birthdateValidated;
            }
            set
            {
                if (!ValidateBirthday(value))
                {
                    errorDictionary.Add("birthdate", "Ongeldige waarde ingevoerd");
                }
            }
        }

        private string _phoneNumberValidated { get; set; }
        private string _phoneNumber
        {
            get
            {
                return _phoneNumberValidated;
            }
            set
            {
                if (!ValidatePhoneNumber(value))
                {
                    errorDictionary.Add("phoneNumber", "Onjuist telefoonnummer ingevoerd");
                }
            }
        }

        private string _streetNameValidated { get; set; }
        private string _streetName
        {
            get
            {
                return _streetNameValidated;
            }
            set
            {
                if (CheckInputIsGiven(value))
                {
                    _streetNameValidated = value;
                    return;
                }
                errorDictionary.Add("streetName", "Ongeldige waarde ingevoerd");
            }
        }

        private string _postalCodeValidated { get; set; }
        private string _postalCode
        {
            get
            {
                return _postalCodeValidated;
            }
            set
            {
                if (!ValidatePostalCode(value))
                {
                    errorDictionary.Add("postalCode", "Ongeldige waarde ingevoerd");
                }
            }
        }

        private string _placeNameValidated { get; set; }
        private string _placeName
        {
            get
            {
                return _placeNameValidated;
            }
            set
            {
                if (CheckInputIsGiven(value))
                {
                    _placeNameValidated = value;
                    return;
                }
                errorDictionary.Add("placeName", "Ongeldige waarde ingevoerd");
            }
        }

        private string _emailAddressValidated { get; set; }
        private string _emailAddress
        {
            get
            {
                return _emailAddressValidated;
            }
            set
            {
                if (!ValidateEmailAdress(value))
                {
                    errorDictionary.Add("placeName", "Ongeldige waarde ingevoerd");
                }
            }
        }

        private string _amountOfGuestsValidated { get; set; }
        private string _amountOfGuests
        {
            get
            {
                return _amountOfGuestsValidated;
            }
            set
            {
                if (!int.TryParse(value, out int x))
                {
                    errorDictionary.Add("amountOfGuests", "Ongeldige waarde ingevoerd");
                    return;
                }
                removeFromErrorDictionary("amountOfGuests");
            }
        }

        private DateTime _checkInDatetime { get; set; }
        private DateTime _checkOutDatetime { get; set; }
        private int _campingPlaceID { get; set; }

        public ReservationCustomerFormInsert(string firstName, string lastName, string birthDate, string phoneNumber, string streetName, string postalCode, string placeName, string emailAdress, string amountOfGuests, DateTime checkInDatetime, DateTime checkOutDatetime, int campingPlaceID)
        {
            this._firstName = firstName.Trim();
            this._lastName = lastName.Trim();
            this._birthdate = birthDate.Trim();
            this._phoneNumber = phoneNumber.Trim();
            this._streetName = streetName.Trim();
            this._postalCode = postalCode.Trim();
            this._placeName = placeName.Trim();
            this._emailAddress = emailAdress.Trim();
            this._amountOfGuests = amountOfGuests.Trim();
            this._checkInDatetime = checkInDatetime;
            this._checkOutDatetime = checkOutDatetime;
            this._campingPlaceID = campingPlaceID;

            if (this.errorDictionary.Count == 0)
            {
                this.InsertAllDataInDatabase();
            }
        }

        private void InsertAllDataInDatabase()
        {
            //TODO: Transactie en toevoegen aan controller
            // Create or/and fetch address based on user input
            /*Address address = new Address(_streetName, _postalCode, _placeName);
            var fetchLatestAddressOrCreateOne = address.FirstOrInsert();

            // Insert customer into CampingCustomer table
            CampingCustomer campingCustomer = new CampingCustomer(fetchLatestAddressOrCreateOne, _birthdate, _emailAddress, _phoneNumber, _firstName, _lastName);
            campingCustomer.Insert();
            var fetchCampingCustomer = campingCustomer.SelectLast();

            // Insert and fetch reservation duration in ReservationDuration table
            ReservationDuration reservationDuration = new ReservationDuration(_checkInDatetime.ToString(), _checkOutDatetime.ToString());
            reservationDuration.Insert();
            var fetchNewestReservationDuration = reservationDuration.SelectLast();

            // Insert reservation in Reservation table
            CampingPlace campingPlaceModel = new CampingPlace();
            CampingPlace campingPlace = campingPlaceModel.Select(_campingPlaceID);
            Reservation reservation = new Reservation(_amountOfGuests, fetchCampingCustomer, campingPlace, fetchNewestReservationDuration);
            reservation.Insert();*/
        }

        private void removeFromErrorDictionary(string key)
        {
            errorDictionary.Remove(key);
        }
        private Boolean CheckInputIsGiven(string input)
        {
            return (string.IsNullOrEmpty(input) || input.Length == 0) ? false : true;
        }
        // TODO: Birthday validation implementeren
        private Boolean ValidateBirthday(string input)
        {
            _birthdateValidated = input;
            removeFromErrorDictionary("birthdate");
            return true;
        }
        // TODO: Phone number validation implementeren
        private Boolean ValidatePhoneNumber(string input)
        {
            _phoneNumberValidated = input;
            removeFromErrorDictionary("phoneNumber");
            return true;
        }
        // TODO: Postalcode validation implementeren
        private Boolean ValidatePostalCode(string input) {
            _postalCodeValidated = input;
            removeFromErrorDictionary("postalCode");
            return true; 
        }
        // TODO: Email adress validation implementeren
        private Boolean ValidateEmailAdress(string input)
        {
            _emailAddressValidated = input;
            removeFromErrorDictionary("emailAdress");
            return true;
        }
    }
}