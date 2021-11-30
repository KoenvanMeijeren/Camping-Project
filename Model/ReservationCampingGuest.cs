using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    // todo: henk
    public class ReservationCampingGuest : IModel
    {
        public int Id { get; private set; }
        public Reservation Reservation { get; private set; }
        public CampingGuest CampingGuest { get; private set; }
        
        public ReservationCampingGuest(string id, Reservation reservation, CampingGuest campingGuest)
        {
            this.Id = int.Parse(id);
            this.Reservation = reservation;
            this.CampingGuest = campingGuest;
        }
        
    }
}
