using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemCore;

namespace Model
{
    /// <inheritdoc/>
    public class ReservationCampingGuest : ModelBase<ReservationCampingGuest>
    {
        public const string
            TableName = "ReservationCampingGuest",
            ColumnId = "ReservationCampingGuestID",
            ColumnReservation = "ReservationID",
            ColumnGuest = "CampingGuestID";
        
        public Reservation Reservation { get; private set; }
        public CampingGuest CampingGuest { get; private set; }

        public ReservationCampingGuest(): base(TableName, ColumnId)
        {

        }

        public ReservationCampingGuest(Reservation reservation, CampingGuest campingGuest): this("-1", reservation, campingGuest)
        {

        }
        
        public ReservationCampingGuest(string id, Reservation reservation, CampingGuest campingGuest): base(TableName, ColumnId)
        {
            bool successId = int.TryParse(id, out int numericId);
            
            this.Id = successId ? numericId : -1;
            this.Reservation = reservation;
            this.CampingGuest = campingGuest;
        }

        /// <summary>
        /// Returns all the guests from given reservation.
        /// </summary>
        /// <param name="reservation">Reservation</param>
        /// <returns>List of ReservationCampingGuests</returns>
        public List<CampingGuest> GetReservationGuests(Reservation reservation)
        {
            Query query = new Query($"SELECT * FROM {ReservationCampingGuest.TableName} INNER JOIN CampingGuest ON ReservationCampingGuest.CampingGuestID = CampingGuest.CampingGuestID WHERE ReservationCampingGuest.ReservationID = @reservationId");
/*            Query query = new Query($"SELECT * FROM CG.{TableName} INNER JOIN RCG.{ReservationCampingGuest.TableName} CG.{ColumnId} = RCG.{ReservationCampingGuest.ColumnGuest} WHERE {ReservationCampingGuest.ColumnReservation} = @reservationId");
*/            query.AddParameter("reservationId", reservation.Id);
            var QueryResults = query.Select();

            List<CampingGuest> campingGuests = new List<CampingGuest>();
            CampingGuest campingGuestModel = new CampingGuest();
            foreach (var item in query.Select())
            {
            }

            return campingGuests;
        }

        public bool Update(Reservation reservation, CampingGuest campingGuest)
        {
            this.Reservation = reservation;
            this.CampingGuest = campingGuest;

            return base.Update(ReservationCampingGuest.ToDictionary(reservation, campingGuest));
        }

        /// <inheritdoc/>
        protected override ReservationCampingGuest ToModel(Dictionary<string, string> dictionary)
        {
            if(dictionary == null)
            {
                return null;
            }

            dictionary.TryGetValue(ColumnId, out string reservationCampingGuestId);
            dictionary.TryGetValue(ColumnReservation, out string reservationId);
            dictionary.TryGetValue(Reservation.ColumnPlace, out string campingPlaceID);
            dictionary.TryGetValue(Reservation.ColumnPeople, out string numberOfPeople);
            dictionary.TryGetValue(Reservation.ColumnCustomer, out string campingCustomerId);
            dictionary.TryGetValue(Reservation.ColumnDuration, out string reservationDurationId);
            
            dictionary.TryGetValue(ColumnGuest, out string campingGuestId);
            dictionary.TryGetValue(CampingGuest.ColumnFirstName, out string firstName);
            dictionary.TryGetValue(CampingGuest.ColumnLastName, out string lastName);
            dictionary.TryGetValue(CampingGuest.ColumnBirthdate, out string birthdate);

/*
            Reservation reservation = new Reservation(reservationId);
            CampingGuest campingGuest = new CampingGuest(campingGuestId, campingGuestName, Convert.ToDateTime(birthdate));

            return new ReservationCampingGuest(reservation, campingGuest);*/

            return null;
        }
        
        /// <inheritdoc/>
        protected override Dictionary<string, string> ToDictionary()
        {
            return ReservationCampingGuest.ToDictionary(this.Reservation, this.CampingGuest);
        }

        private static Dictionary<string, string> ToDictionary(Reservation reservation, CampingGuest campingGuest)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {ColumnReservation, reservation.Id.ToString()},
                {ColumnGuest, campingGuest.Id.ToString()}
            };

            return dictionary;
        }

        /// <inheritdoc/>
        protected override string BaseSelectQuery()
        {
            string query = base.BaseSelectQuery();
            query += $" INNER JOIN {Reservation.TableName} RE ON BT.{ColumnReservation} = RE.{Reservation.ColumnId}";
            query += $" INNER JOIN {CampingGuest.TableName} CG ON BT.{ColumnGuest} = CG.{CampingGuest.ColumnId}";

            return query;
        }
    }
}
