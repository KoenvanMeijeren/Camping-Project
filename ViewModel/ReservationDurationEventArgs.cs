using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace ViewModel
{
    public class ReservationDurationEventArgs : EventArgs
    {
        public CampingPlace CampingPlace { get; private set; }

        public ReservationDuration ReservationDuration { get; private set; }

        public ReservationDurationEventArgs(CampingPlace campingPlace, ReservationDuration reservationDuration)
        {
            this.CampingPlace = campingPlace;
            this.ReservationDuration = reservationDuration;
        }
    }
}
