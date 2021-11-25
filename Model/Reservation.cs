using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemCore;

namespace Model
{
    public class Reservation
    {
        
        public int Id;
        public int NumberOfPeople { get; private set; }
        
        public CampingCustomer CampingCustomer { get; private set; }
        public CampingPlace CampingPlace { get; private set; }
        public ReservationDuration Duration { get; private set; }

        public Reservation(string id, string numberOfPeople, CampingCustomer campingCustomer, CampingPlace campingPlace, ReservationDuration duration)
        {
            this.Id = int.Parse(id);
            this.NumberOfPeople = int.Parse(numberOfPeople);
            this.CampingCustomer = campingCustomer;
            this.CampingPlace = campingPlace;
            this.Duration = duration;
        }
        
        public Boolean Insert(CampingPlace campingPlace, ReservationDuration reservationDuration)
        {
            if (this.Id > 0)
            {
                throw new ArgumentException("You cannot insert an existing reservation.");
                return false;
            }
            
            Query insertNewReservationQuery = new Query("INSERT INTO Reservation VALUES (@campingPlaceID, @numberOfPeople, @campingCustomerID, @reservationDurationID)");
            insertNewReservationQuery.AddParameter("campingPlaceID", campingPlace.Id);
            insertNewReservationQuery.AddParameter("numberOfPeople", this.NumberOfPeople);
            insertNewReservationQuery.AddParameter("campingCustomerID", this.CampingCustomer.Id);
            insertNewReservationQuery.AddParameter("reservationDurationID", reservationDuration.Id);
            insertNewReservationQuery.Execute();
            
            return insertNewReservationQuery.SuccessFullyExecuted();
        }

        public Boolean Delete()
        {
            Query deleteQuery = new Query("DELETE FROM Reservation WHERE id = @id");
            deleteQuery.AddParameter("id", this.Id);
            deleteQuery.Execute();

            return deleteQuery.SuccessFullyExecuted();
        }
    }
}
