using System;
using Model;

namespace Model.EventArguments
{

    public class ReservationEventArgs : EventArgs
    {
        public Reservation Reservation { get; private set; }

        public ReservationEventArgs(Reservation reservation)
        {
            this.Reservation = reservation;
        }

    }
}
