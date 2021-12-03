using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace ViewModel
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
