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

        private bool
            _emailEnabled;
        
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
            
            ReservationCampingPlaceFormViewModel.ReserveEvent += this.OnReserveEvent;
            ReservationCampingGuestViewModel.ReservationGoBackEvent += ReservationCampingGuestViewModelOnReservationGoBackEvent;
            SignInViewModel.SignInEvent += SignInViewModelOnSignInEvent;
            AccountViewModel.SignOutEvent += onSignOutEvent;
            ReservationCampingGuestViewModel.ReservationConfirmedEvent += OnReservationConfirmedEvent;
        }

        private void ReservationCampingGuestViewModelOnReservationGoBackEvent(object sender, ReservationEventArgs e)
        {
            this._checkInDateTime = e.Reservation.CheckInDatetime;
            this._checkOutDateTime = e.Reservation.CheckOutDatetime;
            this.CampingPlace = e.Reservation.CampingPlace;
            this.SelectedCampingPlace = $"Reservering van {this._checkInDateTime.ToShortDateString()} tot {this._checkOutDateTime.ToShortDateString()} in verblijf {this._campingPlace.Location}";

            this.CurrentUserCustomer = CurrentUser.CampingCustomer;
            //Removes the customer from NumberOfPeople.
            this.AmountOfGuests = (e.Reservation.NumberOfPeople - 1).ToString();
        }

        private void OnReservationConfirmedEvent(object sender, EventArgs e)
        {
            this.ResetInput();
        }

        private void onSignOutEvent(object sender, EventArgs e)
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
            this.CampingPlace = args.CampingPlace;
            
            this.CurrentUserCustomer = CurrentUser.CampingCustomer;
            this.SelectedCampingPlace = $"Reservering van {this._checkInDateTime.ToShortDateString()} tot {this._checkOutDateTime.ToShortDateString()} in verblijf {this._campingPlace.Location}";
        }

        #endregion

        #region Input validation

        private void FillInputWithCustomerData(CampingCustomer campingCustomer)
        {
            if (campingCustomer == null)
            {
                return;
            }

            this.FirstName = campingCustomer.FirstName;
            this.LastName = campingCustomer.LastName;
            this.Birthdate = campingCustomer.Birthdate;
            this.Email = campingCustomer.Account.Email;
            this.PhoneNumber = campingCustomer.PhoneNumber;
            this.Street = campingCustomer.Address.Street;
            this.Place = campingCustomer.Address.Place;
            this.PostalCode = campingCustomer.Address.PostalCode;

            this.EmailEnabled = campingCustomer.Account.Id != -1;
        }
        
        private void ResetInput()
        {
            this.FirstName = "";
            this.LastName = "";
            this.Birthdate = DateTime.MinValue;
            this.Email = "";
            this.PhoneNumber = "";
            this.Street = "";
            this.AmountOfGuests = "";
            this.Place = "";
            this.PostalCode = "";
            this._errorDictionary.Clear();
            this._customerReservationError = "";
        }
        #endregion

        #region Commands
        private void ExecuteCustomerDataReservation()
        {
            Address addressModel = new Address(this.Street, this.PostalCode, this.Place);
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
        }
        private bool CanExecuteCustomerDataReservation()
        {
            return   Validation.IsInputFilled(this._firstName) &&
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

        public ICommand AddCustomerReservation => new RelayCommand(ExecuteCustomerDataReservation, CanExecuteCustomerDataReservation);
        #endregion
    }
}
