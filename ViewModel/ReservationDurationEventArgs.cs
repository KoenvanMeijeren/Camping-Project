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
        public int CampingPlaceId { get; set; }
        public DateTime CheckInDatetime { get; set; }
        public DateTime CheckOutDatetime { get; set; }

        public ReservationDurationEventArgs(CampingPlace campingPlace, DateTime checkInDatetime, DateTime checkOutDatetime)
        {
            this.CampingPlaceId = campingPlace.Id;
            this.CheckInDatetime = checkInDatetime;
            this.CheckOutDatetime = checkOutDatetime;
        }
    }
}
