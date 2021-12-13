using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class ReservationGuestEventArgs : EventArgs
    {
        public Address Address { get; private set; }
        public CampingCustomer CampingCustomer { get; private set; }
        public Reservation Reservation { get; private set; }

        public ReservationGuestEventArgs(Address address, CampingCustomer campingCustomer, Reservation reservation)
        {
            this.Address = address;
            this.CampingCustomer = campingCustomer;
            this.Reservation = reservation;
        }
    }
}
