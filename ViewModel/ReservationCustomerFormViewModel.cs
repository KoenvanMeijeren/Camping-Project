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
            _customerReservationError,
            _firstName,
            _lastName,
            _phoneNumber,
            _street,
            _postalCode,
            _place,
            _email,
            _amountOfGuests,
            _selectedCampingPlace;

        private DateTime _birthdate, _checkInDateTime, _checkOutDateTime;

        private CampingPlace _campingPlace;

        private CampingCustomer _currentUserCustomer;

        #endregion

        #region Properties

        public string CustomerReservationError
        {
            get => this._customerReservationError;
            set
            {
                if (value == this._customerReservationError)
                {
                    return;
                }

                this._customerReservationError = value;
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

                this.CustomerReservationError = "";
                if (!Validation.IsInputFilled(this._firstName))
                {
                    this.CustomerReservationError = "Voornaam is een verplicht veld";
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

                this.CustomerReservationError = "";
                if (!Validation.IsInputFilled(this._lastName))
                {
                    this.CustomerReservationError = "Achternaam is een verplicht veld";
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

                this.CustomerReservationError = "";
                if (!Validation.IsBirthdateValid(this._birthdate))
                {
                    this.CustomerReservationError = "Ongeldig geboortedatum";
                }
                else if (!Validation.IsBirthdateAdult(this._birthdate))
                {
                    this.CustomerReservationError = "U bent te jong om te reserveren";
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

                this.CustomerReservationError = "";
                if (!Validation.IsInputFilled(this._street))
                {
                    this.CustomerReservationError = "Straatnaam is een verplicht veld";
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

                this.CustomerReservationError = "";
                if (!Validation.IsInputFilled(this._postalCode))
                {
                    this.CustomerReservationError = "Postcode is een verplicht veld";
                }
                if (!RegexHelper.IsPostalcodeValid(this._postalCode))
                {
                    this.CustomerReservationError = "Ongeldig postcode";
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

                this.CustomerReservationError = "";
                if (!Validation.IsInputFilled(this._place))
                {
                    this.CustomerReservationError = "Plaatsnaam is een verplicht veld";
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

                this.CustomerReservationError = "";
                if (!Validation.IsInputFilled(this._phoneNumber))
                {
                    this.CustomerReservationError = "Telefoonnummer is een verplicht veld";
                }
                else if (!Validation.IsNumber(this._phoneNumber))
                {
                    this.CustomerReservationError = "Ongeldig telefoonnummer";
                }
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

        public static event EventHandler<ReservationEventArgs> ReservationGuestEvent;

        #endregion

        #region View construction

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
            
            ReservationCampingMapViewModel.ReserveEvent += this.OnReserveEvent;
            ReservationCampingGuestViewModel.ReservationGoBackEvent += this.ReservationCampingGuestViewModelOnReservationGoBackEvent;
            SignInViewModel.SignInEvent += this.SignInViewModelOnSignInEvent;
            AccountViewModel.SignOutEvent += this.OnSignOutEvent;
            ReservationPaymentViewModel.ReservationConfirmedEvent += this.OnReservationConfirmedEvent;
        }

        private void ReservationCampingGuestViewModelOnReservationGoBackEvent(object sender, ReservationEventArgs e)
        {
            this._checkInDateTime = e.Reservation.CheckInDatetime;
            this._checkOutDateTime = e.Reservation.CheckOutDatetime;
            this._campingPlace = e.Reservation.CampingPlace;
            this._selectedCampingPlace = $"Reservering van {this._checkInDateTime.ToShortDateString()} tot {this._checkOutDateTime.ToShortDateString()} in verblijf {this._campingPlace}";
            
            //Removes the customer from NumberOfPeople.
            this._amountOfGuests = (e.Reservation.NumberOfPeople - 1).ToString();
            
            // This triggers the on property changed event.
            this.CurrentUserCustomer = CurrentUser.CampingCustomer;
        }

        private void OnReservationConfirmedEvent(object sender, EventArgs e)
        {
            this.ResetInput();
            this.CurrentUserCustomer = CurrentUser.CampingCustomer;
        }

        private void OnSignOutEvent(object sender, EventArgs e)
        {
            this.ResetInput();
        }

        private void SignInViewModelOnSignInEvent(object sender, AccountEventArgs e)
        {
            this.CurrentUserCustomer = CurrentUser.CampingCustomer;
        }
        
        private void OnReserveEvent(object sender, ReservationDurationEventArgs args)
        {
            this._checkInDateTime = args.CheckInDatetime;
            this._checkOutDateTime = args.CheckOutDatetime;
            this._campingPlace = args.CampingPlace;
            
            this._currentUserCustomer = CurrentUser.CampingCustomer;
            this._selectedCampingPlace = $"Reservering van {this._checkInDateTime.ToShortDateString()} tot {this._checkOutDateTime.ToShortDateString()} in verblijf {this._campingPlace}";
            
            this.OnPropertyChanged(new PropertyChangedEventArgs(null));
        }

        #endregion

        #region Input validation

        private void FillInputWithCustomerData(CampingCustomer campingCustomer)
        {
            if (campingCustomer == null)
            {
                return;
            }

            this._firstName = campingCustomer.FirstName;
            this._lastName = campingCustomer.LastName;
            this._birthdate = campingCustomer.Birthdate;
            this._email = campingCustomer.Account.Email;
            this._phoneNumber = campingCustomer.PhoneNumber;
            this._street = campingCustomer.Address.Street;
            this._place = campingCustomer.Address.Place;
            this._postalCode = campingCustomer.Address.PostalCode;
            
            this.OnPropertyChanged(new PropertyChangedEventArgs(null));
        }
        
        private void ResetInput()
        {
            this._firstName = "";
            this._lastName = "";
            this._birthdate = DateTime.MinValue;
            this._email = "";
            this._phoneNumber = "";
            this._street = "";
            this._amountOfGuests = "";
            this._place = "";
            this._postalCode = "";
            this._errorDictionary.Clear();
            this._customerReservationError = "";
            
            this.OnPropertyChanged(new PropertyChangedEventArgs(null));
        }
        #endregion

        #region Commands
        private void ExecuteCustomerDataReservation()
        {
            Address addressModel = new Address(this.Street, this.PostalCode, this.Place);
            var address = addressModel.FirstAndUpdateOrInsert();

            var customer = new CampingCustomer(this._currentUserCustomer?.Id.ToString(), this._currentUserCustomer?.Account, address, this.Birthdate.ToShortDateString(), this.PhoneNumber, this.FirstName,
                this.LastName);
            if (customer.Id == ModelBase<Reservation>.UndefinedId)
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

            ReservationCustomerFormViewModel.ReservationGuestEvent?.Invoke(this, new ReservationEventArgs(reservation));
        }
        private bool CanExecuteCustomerDataReservation()
        {
            return Validation.IsInputFilled(this._firstName) 
                   && Validation.IsInputFilled(this._lastName) 
                   && Validation.IsBirthdateValid(this._birthdate)
                   && Validation.IsBirthdateAdult(this._birthdate) 
                   && Validation.IsInputFilled(this._street) 
                   && Validation.IsInputFilled(this._postalCode) 
                   && RegexHelper.IsPostalcodeValid(this._postalCode) 
                   && Validation.IsInputFilled(this._place) 
                   && Validation.IsInputFilled(this._phoneNumber) 
                   && Validation.IsNumber(this._phoneNumber);
        }

        public ICommand AddCustomerReservation => new RelayCommand(ExecuteCustomerDataReservation, CanExecuteCustomerDataReservation);
        #endregion
    }
}
