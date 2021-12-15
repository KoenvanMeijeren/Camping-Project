﻿using Microsoft.Toolkit.Mvvm.ComponentModel;
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

            this._campingGuestsList = new List<CampingGuest>();
            this.CampingGuests = new ObservableCollection<CampingGuest>();
            this.BirthDate = DateTime.Today.AddYears(-1);
            
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
            string birthDate = this.BirthDate.ToShortDateString();
            CampingGuest campingGuestModel = new CampingGuest();

            CampingGuest campingGuest = new CampingGuest(this.FirstNameGuest, this.LastNameGuest, birthDate);
            //Removes the customer from NumberOfPeople.
            if (this._numberOfAddedGuest >= this.Reservation.CampingPlace.Type.GuestLimit-1)
            {
                this.AmountOfPeopleError = "Het maximaal aantal gasten is bereikt!";
                return;
            }
            
            campingGuest.Insert();
            var lastCampingGuest = campingGuestModel.SelectLast();
            
            this._campingGuestsList.Add(lastCampingGuest);
            this.CampingGuestsTypes.Add(lastCampingGuest);
            this._numberOfAddedGuest++;

            this.FirstNameGuest = "";
            this.LastNameGuest = "";
            this.BirthDate = DateTime.Today.AddYears(-1);
            this.AmountOfPeopleError = "";
            this.FirstNameError = "";
            this.LastNameError = "";
            this.BirthDateError = "";
        }

        private bool CanExecuteAddGuestReservation()
        {
            return !this._errorDictionary.Any();
        }
        /// <summary>
        /// Removes campingGuest from database.
        /// </summary>
        private void ExecuteRemoveGuestReservation()
        {
            if (this.SelectedCampingGuest == null)
            {
                return;
            }
            
            this.SelectedCampingGuest.Delete();
            this._campingGuestsList.Remove(SelectedCampingGuest);
            this.CampingGuestsTypes.Remove(SelectedCampingGuest);
        }
        /// <summary>
        /// Checks if button can be pressed/
        /// </summary>
        /// <returns>true or false</returns>
        private bool CanExecuteRemoveGuestReservation()
        {
            return this._campingGuestsList.Count > 0;
            
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
                (new ReservationCampingGuest(lastReservation, guest)).Insert();
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
            this._numberOfAddedGuest = this._campingGuestsList.Count();
        }

        public ICommand AddCustomerReservation => new RelayCommand(ExecuteCustomerGuestReservation);

        public ICommand CustomerGuestGoBackReservation => new RelayCommand(ExecuteCustomerGuestGoBackReservation);

        public ICommand AddGuestReservation => new RelayCommand(ExecuteAddGuestReservation, CanExecuteAddGuestReservation);

        public ICommand RemoveGuestReservation => new RelayCommand(ExecuteRemoveGuestReservation, CanExecuteRemoveGuestReservation);
    }
}
