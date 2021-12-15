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
        public DateTime CheckInDatetime { get; private set; }
        public DateTime CheckOutDatetime { get; private set; }

        public ReservationDurationEventArgs(CampingPlace campingPlace, DateTime checkInDate, DateTime checkOutDate)
        {
            this.CampingPlace = campingPlace;
            this.CheckInDatetime = checkInDate;
            this.CheckOutDatetime = checkOutDate;
        }
    }
}
