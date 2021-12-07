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
        public int CampingPlaceId { get; private set; }

        public ReservationDuration ReservationDuration { get; private set; }

        public ReservationDurationEventArgs(CampingPlace campingPlace, ReservationDuration reservationDuration)
        {
            this.CampingPlaceId = campingPlace.Id;
            this.ReservationDuration = reservationDuration;
        }
    }
}
