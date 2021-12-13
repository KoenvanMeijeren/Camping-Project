using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Model;
using System.ComponentModel;
using System.Collections.ObjectModel;
using SystemCore;

namespace ViewModel
{
    public class ReservationCampingGuestViewModel : ObservableObject
    {
        private string _id, _firstNameGuest, _lastNameGuest, _amountOfPeopleError, _firstNameError, _lastNameError, _birthDateError;
        private readonly List<CampingGuest> _campingGuestsList;
        private DateTime _birthDate;
        public ObservableCollection<CampingGuest> CampingGuests { get; private set; }
        private Reservation _reservation;
        private int _numberOfAddedGuest;
        private Dictionary<string, string> _errorDictionary;

        public string IdGuest {
            get => this._id;
            set
            {
                this._id = value;

            }
        }

        public string FirstNameGuest
        {
            get => this._firstNameGuest;
            set
            {
                this._firstNameGuest = value;
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

        public string LastNameGuest
        {
            get => this._lastNameGuest;
            set
            {
                this._lastNameGuest = value;
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

        public string AmountOfPeopleError
        {
            get => this._amountOfPeopleError;
            set
            {
                this._amountOfPeopleError = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
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

        public string LastNameError
        {
            get => this._lastNameError;
            set
            {
                this._lastNameError = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string BirthDateError
        {
            get => this._birthDateError;
            set
            {
                this._birthDateError = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public DateTime BirthDate
        {
            get => this._birthDate;
            set
            {
                this._birthDate = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

                this.BirthDateError = string.Empty;
                this.RemoveErrorFromDictionary("BirthDate");
                if (!this._birthDate.Equals(DateTime.MinValue))
                {
                    return;
                }

                this.BirthDateError = "Ongeldige input";
                this.AddErrorToDictionary("BirthDate", "Ongeldige input");
            }
        }

        public Reservation Reservation
        {
            get => this._reservation;
            set
            {
                this._reservation = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingGuest SelectedCampingGuest { get; set; }

        public ObservableCollection<CampingGuest> CampingGuestsTypes
        {
            get => this.CampingGuests;
            private init
            {
                if (Equals(value, this.CampingGuests))
                {
                    return;
                }

                this.CampingGuestsTypes = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public static event EventHandler<ReservationEventArgs> ReservationConfirmedEvent;
        public static event EventHandler<ReservationEventArgs> ReservationGoBackEvent;

        public ReservationCampingGuestViewModel()
        {
            this._errorDictionary = new Dictionary<string, string>
            {
                {"FirstName", ""},
                {"LastName", ""},
                {"BirthDate", ""},
            };

            _campingGuestsList = new List<CampingGuest>();
            CampingGuests = new ObservableCollection<CampingGuest>();
            ReservationCustomerFormViewModel.ReservationGuestEvent += this.OnReservationConfirmedEvent;
        }

        private void AddErrorToDictionary(string key, string value)
        {
            this._errorDictionary.TryAdd(key, value);
        }

        private void RemoveErrorFromDictionary(string key)
        {
            if (!this._errorDictionary.ContainsKey(key))
            {
                return;
            }

            this._errorDictionary.Remove(key);
        }
        /// <summary>
        /// Inserts campingGuest into the database.
        /// </summary>
        private void ExecuteAddGuestReservation()
        {
            string birthDate = BirthDate.ToShortDateString();
            CampingGuest campingGuestGetID = new CampingGuest();
            IdGuest = (campingGuestGetID.SelectLast().Id + 1).ToString();

            CampingGuest campingGuest = new CampingGuest(IdGuest, FirstNameGuest, LastNameGuest, birthDate);
            //Removes the customer from NumberOfPeople.
            if (_numberOfAddedGuest >= this.Reservation.CampingPlace.Type.GuestLimit)
            {
                this.AmountOfPeopleError = "Het maximaal aantal gasten is bereikt!";
                return;
            }
            campingGuest.Insert();
            this._campingGuestsList.Add(campingGuest);
            this.CampingGuestsTypes.Add(campingGuest);
            this._numberOfAddedGuest++;

            FirstNameGuest = "";
            LastNameGuest = "";
            BirthDate = new DateTime(1 / 1 / 0001);

            AmountOfPeopleError = "";
            FirstNameError = "";
            LastNameError = "";
            BirthDateError = "";
        }

        private bool CanExecuteExecuteAddGuestReservation()
        {
            return !this._errorDictionary.Any();
        }
        /// <summary>
        /// Removes campingGuest from database.
        /// </summary>
        private void ExecuteRemoveGuestReservation()
        {
            if (this.SelectedCampingGuest != null)
            {
                this.SelectedCampingGuest.Delete();
                _campingGuestsList.Remove(SelectedCampingGuest);
                CampingGuestsTypes.Remove(SelectedCampingGuest);
            }
        }
        /// <summary>
        /// Checks if button can be pressed/
        /// </summary>
        /// <returns>true or false</returns>
        private bool CanExecuteRemoveGuestReservation()
        {
            if(_campingGuestsList.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Inserts Reservation into the database.
        /// </summary>
        private void ExecuteCustomerGuestReservation()
        {
            this.Reservation.Insert();
            var lastReservation = this.Reservation.SelectLast();

            foreach (var guest in this._campingGuestsList)
            {
                var lastGuest = guest.SelectLast();

                var reservationCampingGuest = new ReservationCampingGuest(lastReservation, lastGuest);
                reservationCampingGuest.Insert();
            }

            ReservationConfirmedEvent?.Invoke(this, new ReservationEventArgs(lastReservation));
        }
        /// <summary>
        /// Returns to former page.
        /// </summary>
        private void ExecuteCustomerGuestGoBackReservation()
        {
            ReservationGoBackEvent?.Invoke(this, new ReservationEventArgs(this.Reservation));
        }

        private void OnReservationConfirmedEvent(object sender, ReservationGuestEventArgs args)
        {
            this.Reservation = args.Reservation;
            //Add customer
            this._numberOfAddedGuest = this._campingGuestsList.Count() + 1;
        }

        public ICommand AddCustomerReservation => new RelayCommand(ExecuteCustomerGuestReservation);

        public ICommand CustomerGuestGoBackReservation => new RelayCommand(ExecuteCustomerGuestGoBackReservation);

        public ICommand AddGuestReservation => new RelayCommand(ExecuteAddGuestReservation, CanExecuteExecuteAddGuestReservation);

        public ICommand RemoveGuestReservation => new RelayCommand(ExecuteRemoveGuestReservation, CanExecuteRemoveGuestReservation);
    }
}
