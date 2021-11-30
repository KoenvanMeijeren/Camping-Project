using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visualization
{
    public class ReserveEventArgs : EventArgs
    {
        public int CampingPlaceID { get; set; }
        public DateTime CheckInDatetime { get; set; }
        public DateTime CheckOutDatetime { get; set; }

        public ReserveEventArgs(int campingPlaceId, DateTime checkInDatetime, DateTime checkOutDatetime)
        {
            this.CampingPlaceID = campingPlaceId;
            this.CheckInDatetime = checkInDatetime;
            this.CheckOutDatetime = checkOutDatetime;
        }
    }
}
