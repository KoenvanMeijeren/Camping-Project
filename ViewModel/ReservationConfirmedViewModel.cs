using System;
using System.ComponentModel;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace ViewModel
{
    public class ReservationConfirmedViewModel : ObservableObject
    {

        #region Fields

        private string _firstName, _lastName, _title, _confirmationText;
        private DateTime _checkInDate, _checkOutDate;

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
        
        #endregion

        public ReservationConfirmedViewModel()
        {
            ReservationCampingGuestViewModel.ReservationConfirmedEvent += this.OnReservationConfirmedEvent;
        }

        private void OnReservationConfirmedEvent(object sender, ReservationEventArgs args)
        {
            this.FirstName = args.Reservation.CampingCustomer.FirstName;
            this.LastName = args.Reservation.CampingCustomer.LastName;
            this.CheckInDate = args.Reservation.Duration.CheckInDatetime;
            this.CheckOutDate = args.Reservation.Duration.CheckOutDatetime;

            this.Title = $"Gefeliciteerd {this.FirstName} {this.LastName},";
            this.ConfirmationText = $"Uw reservering van {this.CheckInDate.Date.ToShortDateString()} tot {this.CheckOutDate.Date.ToShortDateString()}";
        }
        
    }
}