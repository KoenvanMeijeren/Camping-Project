using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

        public bool Update(Reservation reservation, CampingGuest campingGuest)
        {
            this.Reservation = reservation;
            this.CampingGuest = campingGuest;

            return base.Update(ReservationCampingGuest.ToDictionary(reservation, campingGuest));
        }

        public bool DeleteReservationCampingGuestConnection(int reservationPrimaryKey)
        {
            if (this.PrimaryKey == null || this.PrimaryKey.Length ==0)
            {
                return false;
            }

            Query query = new Query($"DELETE FROM {this.Table} WHERE {ColumnReservation} = @{ColumnReservation}");
            query.AddParameter(ColumnReservation, reservationPrimaryKey);
            query.Execute();

            return base.Delete();
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
