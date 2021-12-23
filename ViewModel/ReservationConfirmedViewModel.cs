using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Model;
using ViewModel.EventArguments;

namespace ViewModel
{
    public class ReservationConfirmedViewModel : ObservableObject
    {

        #region Fields

        private string _firstName, _lastName, _title, _confirmationText;
        private DateTime _checkInDate, _checkOutDate;
        private Reservation _reservation;
        private ObservableCollection<CampingGuest> _campingGuests;

        #endregion

        #region Properties

        public string FirstName
        {
            get => this._firstName;
            set
            {
                this._firstName = value;
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
            }
        }
        
        public DateTime CheckInDate
        {
            get => this._checkInDate;
            set
            {
                this._checkInDate = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }
        
        public DateTime CheckOutDate
        {
            get => this._checkOutDate;
            set
            {
                this._checkOutDate = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }


        public string Title
        {
            get => this._title;
            set
            {
                this._title = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }
        
        public string ConfirmationText
        {
            get => this._confirmationText;
            set
            {
                this._confirmationText = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
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

        public ObservableCollection<CampingGuest> CampingGuests
        {
            get => this._campingGuests;
            set
            {
                this._campingGuests = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        #endregion

        #region View construction

        public ReservationConfirmedViewModel()
        {
            ReservationPaymentViewModel.ReservationConfirmedEvent += this.OnReservationConfirmedEvent;
        }

        private void OnReservationConfirmedEvent(object sender, ReservationGuestEventArgs args)
        {
            this.Reservation = args.Reservation;
            this.CampingGuests = new ObservableCollection<CampingGuest>();
            foreach (var guest in args.CampingGuests)
            {
                this.CampingGuests.Add(guest);
            }

            this.FirstName = args.Reservation.CampingCustomer.FirstName;
            this.LastName = args.Reservation.CampingCustomer.LastName;
            this.CheckInDate = args.Reservation.CheckInDatetime;
            this.CheckOutDate = args.Reservation.CheckOutDatetime;

            InsertReservationAndGuests();

            this.Title = $"Gefeliciteerd {this.FirstName} {this.LastName},";
            this.ConfirmationText = $"Uw reservering van {this.CheckInDate.Date.ToShortDateString()} tot {this.CheckOutDate.Date.ToShortDateString()}";
        }

        #endregion

        /// <summary>
        /// Inserts Reservation and CampingGuests into the database.
        /// </summary>
        public void InsertReservationAndGuests()
        {
            this.Reservation.Insert();
            var lastReservation = this.Reservation.SelectLast();
            CampingGuest campingGuest = new CampingGuest();

            foreach (var guest in this.CampingGuests)
            {
                guest.Insert();
                var lastGuest = campingGuest.SelectLast();
                (new ReservationCampingGuest(lastReservation, lastGuest)).Insert();
            }
        }
    }
}