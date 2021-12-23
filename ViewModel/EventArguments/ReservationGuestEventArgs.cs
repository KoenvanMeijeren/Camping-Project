using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Model;

namespace ViewModel.EventArguments
{
    public class ReservationGuestEventArgs : EventArgs
    {
        public Reservation Reservation { get; private set; }
        public ObservableCollection<CampingGuest> CampingGuests { get; private set; }

        public ReservationGuestEventArgs(Reservation reservation, ObservableCollection<CampingGuest> campingGuests)
        {
            this.Reservation = reservation;
            this.CampingGuests = campingGuests;
        }
    }
}
