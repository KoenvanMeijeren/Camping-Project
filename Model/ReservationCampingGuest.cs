using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ReservationCampingGuest : ModelBase<ReservationCampingGuest>
    {
        public Reservation Reservation { get; private set; }
        public CampingGuest CampingGuest { get; private set; }

        public ReservationCampingGuest()
        {

        }

        public ReservationCampingGuest(Reservation reservation, CampingGuest campingGuest): this("-1", reservation, campingGuest)
        {

        }
        
        public ReservationCampingGuest(string id, Reservation reservation, CampingGuest campingGuest)
        {
            this.Id = int.Parse(id);
            this.Reservation = reservation;
            this.CampingGuest = campingGuest;
        }

        protected override string Table()
        {
            return "ReservationCampingGuest";
        }

        protected override string PrimaryKey()
        {
            return "ReservationCampingGuestID";
        }

        public bool Update(Reservation reservation, CampingGuest campingGuest)
        {
            this.Reservation = reservation;
            this.CampingGuest = campingGuest;

            return base.Update(ReservationCampingGuest.ToDictionary(reservation, campingGuest));
        }

        protected override ReservationCampingGuest ToModel(Dictionary<string, string> dictionary)
        {
            if(dictionary == null)
            {
                return null;
            }

            dictionary.TryGetValue("ReservationCampingGuestID", out string reservationCampingGuestId);
            dictionary.TryGetValue("ReservationID", out string reservationId);
            dictionary.TryGetValue("ReservationCampingPlaceID", out string campingPlaceID);
            dictionary.TryGetValue("ReservationNumberOfPeople", out string numberOfPeople);
            dictionary.TryGetValue("ReservationCampingCustomerID", out string campingCustomerId);
            dictionary.TryGetValue("ReservationDurationID", out string reservationDurationId);
            dictionary.TryGetValue("CampingGuestID", out string campingGuestId);
            dictionary.TryGetValue("CampingGuestName", out string campingGuestName);
            dictionary.TryGetValue("Birthdate", out string birthdate);

/*
            Reservation reservation = new Reservation(reservationId);
            CampingGuest campingGuest = new CampingGuest(campingGuestId, campingGuestName, Convert.ToDateTime(birthdate));

            return new ReservationCampingGuest(reservation, campingGuest);*/

            return null;
        }
        protected override Dictionary<string, string> ToDictionary()
        {
            return ReservationCampingGuest.ToDictionary(this.Reservation, this.CampingGuest);
        }

        private static Dictionary<string, string> ToDictionary(Reservation reservation, CampingGuest campingGuest)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                { "ReservationID", reservation.Id.ToString()},
                { "CampingGuestID", campingGuest.Id.ToString()}
            };

            return dictionary;
        }

        protected override string BaseQuery()
        {
            string query = base.BaseQuery();
            query += "INNER JOIN Reservation RE ON BT.ReservationID = RE.ReservationID";
            query += "INNER JOIN CampingGuest CG ON BT.CampingGuestID = CG.CampingGuestID";

            return query;
        }
    }
}
