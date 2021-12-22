using System;
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

        #region View construction

        public ReservationConfirmedViewModel()
        {
            ReservationCampingGuestViewModel.ReservationConfirmedEvent += this.OnReservationConfirmedEvent;
        }

        private void OnReservationConfirmedEvent(object sender, UpdateModelEventArgs<Reservation> args)
        {
            this._firstName = args.Model.CampingCustomer.FirstName;
            this._lastName = args.Model.CampingCustomer.LastName;
            this._checkInDate = args.Model.CheckInDatetime;
            this._checkOutDate = args.Model.CheckOutDatetime;

            this._title = $"Gefeliciteerd {this.FirstName} {this.LastName},";
            this._confirmationText = $"Uw reservering van {this.CheckInDate.Date.ToShortDateString()} tot {this.CheckOutDate.Date.ToShortDateString()}";
            
            this.OnPropertyChanged(new PropertyChangedEventArgs(null));
        }

        #endregion

    }
}