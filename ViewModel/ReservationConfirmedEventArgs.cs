using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{

    public class ReservationConfirmedEventArgs : EventArgs
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CheckInDatetime { get; set; }
        public DateTime CheckOutDatetime { get; set; }

        public ReservationConfirmedEventArgs(string firstName, string lastName, DateTime checkInDatetime, DateTime checkOutDatetime)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.CheckInDatetime = checkInDatetime;
            this.CheckOutDatetime = checkOutDatetime;
        }

    }
}
