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
        private string _lastName;
        private DateTime _birthdate;
        private string _phoneNumber;
        private string _streetName;
        private string _postalCode;
        private string _placeNameValidated;
        private string _emailAddress;
        private string _amountOfGuests;

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
                if (CheckInputIsGiven(value))
                {
                    this._firstName = value;
                    removeFromErrorDictionary("firstName");
                    //Idea
                    //create property for error label.
                    //create two way binding
                    //change content of error label here
                    //no dictionary needed
                    return;
                }
                errorDictionary.Add("firstName", "Ongeldige waarde ingevoerd");
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
                if (CheckInputIsGiven(value))
                {
                    this._lastName = value;
                    removeFromErrorDictionary("lastName");
                    return;
                }
                errorDictionary.Add("lastName", "Ongeldige waarde ingevoerd");
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
                if (!ValidateBirthday(value))
                {
                    errorDictionary.Add("birthDate", "Ongeldige waarde ingevoerd");
                }
                this._birthdate = value;
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
                if (!ValidatePhoneNumber(value))
                {
                    errorDictionary.Add("phoneNumber", "Onjuist telefoonnummer ingevoerd");
                }
                this._phoneNumber = value;
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
                if (CheckInputIsGiven(value))
                {
                    this._streetName = value;
                    return;
                }
                errorDictionary.Add("streetName", "Ongeldige waarde ingevoerd");
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
                if (!ValidatePostalCode(value))
                {
                    errorDictionary.Add("postalCode", "Ongeldige waarde ingevoerd");
                }
                this._postalCode = value;
            }
        }

        public string PlaceName
        {
            get
            {
                return this._placeNameValidated;
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

        public string EmailAddress
        {
            get
            {
                return this._emailAddress;
            }
            set
            {
                if (!ValidateEmailAdress(value))
                {
                    errorDictionary.Add("emailAdress", "Ongeldige waarde ingevoerd");
                }
                this._emailAddress = value;
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
                if (!int.TryParse(value, out int x))
                {
                    errorDictionary.Add("amountOfGuests", "Ongeldige waarde ingevoerd");
                    return;
                }
                removeFromErrorDictionary("amountOfGuests");
                this._amountOfGuests = value;
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
