using System;
using Model;

namespace ViewModel.EventArguments
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
