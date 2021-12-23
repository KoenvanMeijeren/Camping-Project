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
using ViewModel.EventArguments;

namespace ViewModel
{
    public class ReservationCampingGuestViewModel : ObservableObject
    {
        #region Fields

        private string _firstNameGuest, _lastNameGuest, _amountOfPeopleError, _firstNameError, _lastNameError, _birthDateError;
        
        private ObservableCollection<CampingGuest> _campingGuests;
        private CampingGuest _selectedCampingGuest;
        
        private DateTime _birthDate;
        private Reservation _reservation;
        
        private int _numberOfAddedGuest;
        
        private readonly Dictionary<string, string> _errorDictionary;
        
        #endregion

        #region Properties

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

        public CampingGuest SelectedCampingGuest 
        {
            get => this._selectedCampingGuest;
            set
            {
                if (Equals(value, this._selectedCampingGuest))
                {
                    return;
                }

                this._selectedCampingGuest = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public ObservableCollection<CampingGuest> CampingGuests
        {
            get => this._campingGuests;
            private set
            {
                if (Equals(value, this._campingGuests))
                {
                    return;
                }

                this._campingGuests = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        #endregion

        #region Events

        public static event EventHandler<UpdateModelEventArgs<Reservation>> ReservationConfirmedEvent;
        public static event EventHandler<ReservationGuestEventArgs> ReservationGuestsConfirmedEvent;
        public static event EventHandler<ReservationEventArgs> ReservationGoBackEvent;

        #endregion

        #region View construction

        public ReservationCampingGuestViewModel()
        {
            this._errorDictionary = new Dictionary<string, string>
            {
                {"FirstName", ""},
                {"LastName", ""},
                {"BirthDate", ""},
            };

            this.CampingGuests = new ObservableCollection<CampingGuest>();
            this.BirthDate = DateTime.Today.AddYears(-18);
            
            ReservationCustomerFormViewModel.ReservationGuestEvent += this.OnReservationConfirmedEvent;
            ReservationPaymentViewModel.ReservationGuestGoBackEvent += this.OnReservationGuestGoBackEvent;
            AccountViewModel.SignOutEvent += this.OnSignOutEvent;
        }

        private void ResetInput()
        {
            this._firstNameGuest = "";
            this._lastNameGuest = "";
            this._firstNameError = "";
            this._lastNameError = "";
            this._amountOfPeopleError = "";
            
            // Triggers the on property changed call.
            this.BirthDate = DateTime.Today.AddYears(-18);
        }
        
        private void OnSignOutEvent(object sender, EventArgs e)
        {
            this.ResetInput();
        }

        private void OnReservationConfirmedEvent(object sender, ReservationGuestEventArgs args)
        {
            this._reservation = args.Reservation;
            this._numberOfAddedGuest = this.CampingGuests.Count();
            
            this.OnPropertyChanged(new PropertyChangedEventArgs(null));
        }

        private void OnReservationGuestGoBackEvent(object sender, ReservationGuestEventArgs args)
        {
            this.Reservation = args.Reservation;
            foreach (var campingGuest in args.CampingGuests)
            {
                this.CampingGuests.Add(campingGuest);
            }

        }

        #endregion

        #region Input validation

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

        #endregion

        #region Commands
        
        /// <summary>
        /// Inserts campingGuest into the database.
        /// </summary>
        private void ExecuteAddGuestReservation()
        {
            string birthDate = this.BirthDate.ToShortDateString();

            CampingGuest campingGuest = new CampingGuest(this.FirstNameGuest, this.LastNameGuest, birthDate);
            
            //Removes the customer from NumberOfPeople.
            if (this._numberOfAddedGuest >= (this.Reservation.CampingPlace.Type.GuestLimit - 1))
            {
                this.AmountOfPeopleError = "Maximaal aantal gasten is bereikt";
                return;
            }
            
            this._numberOfAddedGuest++;
            this.CampingGuests.Add(campingGuest);

            this.ResetInput();
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
            
            this._numberOfAddedGuest--;
            this.CampingGuests.Remove(SelectedCampingGuest);
        }
        /// <summary>
        /// Checks if button can be pressed/
        /// </summary>
        /// <returns>true or false</returns>
        private bool CanExecuteRemoveGuestReservation()
        {
            return this.SelectedCampingGuest != null;
            
        }
        /// <summary>
        /// Inserts Reservation into the database.
        /// </summary>
        private void ExecuteCustomerGuestReservation()
        {
            ReservationGuestsConfirmedEvent?.Invoke(this, new ReservationGuestEventArgs(Reservation, CampingGuests));

            this.ResetInput();
            this.CampingGuests.Clear();
        }
        /// <summary>
        /// Returns to former page.
        /// </summary>
        private void ExecuteCustomerGuestGoBackReservation()
        {
            ReservationCampingGuestViewModel.ReservationGoBackEvent?.Invoke(this, new ReservationEventArgs(this.Reservation));
        }

        public ICommand AddCustomerReservation => new RelayCommand(ExecuteCustomerGuestReservation);

        public ICommand CustomerGuestGoBackReservation => new RelayCommand(ExecuteCustomerGuestGoBackReservation);

        public ICommand AddGuestReservation => new RelayCommand(ExecuteAddGuestReservation, CanExecuteAddGuestReservation);

        public ICommand RemoveGuestReservation => new RelayCommand(ExecuteRemoveGuestReservation, CanExecuteRemoveGuestReservation);

        #endregion
    }
}
