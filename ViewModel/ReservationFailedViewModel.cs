using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Model;
using ViewModel.EventArguments;

namespace ViewModel
{
    public class ReservationFailedViewModel : ObservableObject
    {
        private Reservation _reservation;
        private string _title, _confirmationText;

        public Reservation Reservation {
            get => this._reservation;
            set
            {
                this._reservation = value;
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

        public ReservationFailedViewModel()
        {
            ReservationPaymentViewModel.ReservationFailedEvent += OnReservationFailedEvent;
        }

        public void OnReservationFailedEvent(object sender, ReservationEventArgs args)
        {
            this.Reservation = args.Reservation;

            this.Title = $"Helaas {this.Reservation.CampingCustomer.FirstName} {this.Reservation.CampingCustomer.LastName},";
            this.ConfirmationText = $"Uw reservering van {this.Reservation.CheckInDate} tot {this.Reservation.CheckOutDate}";
        }
    }
}
