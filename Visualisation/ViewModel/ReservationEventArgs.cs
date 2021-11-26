using System;
using System.Collections.Generic;
using Model;

namespace Visualisation.Model
{
    public class ReservationEventArgs : EventArgs
    {
        public List<Reservation> Reservations { get; private set; }

        public ReservationEventArgs(List<Reservation> reservations)
        {
            this.Reservations = reservations;
        }
        
    }
}