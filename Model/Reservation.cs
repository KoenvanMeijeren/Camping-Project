using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemCore;

namespace Model
{
    public class Reservation : IModel
    {
        
        public int Id;
        public int NumberOfPeople { get; private set; }
        
        public CampingCustomer CampingCustomer { get; private set; }
        public CampingPlace CampingPlace { get; private set; }
        public ReservationDuration Duration { get; private set; }

        public Reservation(string numberOfPeople, CampingCustomer campingCustomer, CampingPlace campingPlace, 
            ReservationDuration duration): this("0", numberOfPeople, campingCustomer, campingPlace, duration)
        {
        }
        
        public Reservation(string id, string numberOfPeople, CampingCustomer campingCustomer, CampingPlace campingPlace, ReservationDuration duration)
        {
            this.Id = int.Parse(id);
            this.NumberOfPeople = int.Parse(numberOfPeople);
            this.CampingCustomer = campingCustomer;
            this.CampingPlace = campingPlace;
            this.Duration = duration;
        }
        
        public bool Insert()
        {
            if (this.Id > 0)
            {
                throw new ArgumentException("You cannot insert an existing reservation.");
            }
            
            Query insertNewReservationQuery = new Query("INSERT INTO Reservation VALUES (@campingPlaceID, @numberOfPeople, @campingCustomerID, @reservationDurationID)");
            insertNewReservationQuery.AddParameter("campingPlaceID", this.CampingPlace.Id);
            insertNewReservationQuery.AddParameter("numberOfPeople", this.NumberOfPeople);
            insertNewReservationQuery.AddParameter("campingCustomerID", this.CampingCustomer.Id);
            insertNewReservationQuery.AddParameter("reservationDurationID", this.Duration.Id);
            insertNewReservationQuery.Execute();
            
            return insertNewReservationQuery.SuccessFullyExecuted();
        }

        public bool Delete()
        {
            Query deleteQuery = new Query("DELETE FROM Reservation WHERE id = @id");
            deleteQuery.AddParameter("id", this.Id);
            deleteQuery.Execute();

            return deleteQuery.SuccessFullyExecuted();
        }
    }
}
