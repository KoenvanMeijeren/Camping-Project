using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemCore;

namespace Model
{
    public class Reservation
    {
        public int CampingPlaceID { get; private set; }
        public int NumberOfPeople { get; private set; }
        public int CampingCustomerID { get; private set; }
        public ReservationDuration duration { get; private set; }

        private Boolean InsertReservation()
        {
            //ID hoeft niet worden meegegeven i.v.m. auto-increment
            Query insertNewReservationQuery = new Query("INSERT INTO Reservation VALUES (@campingPlaceID, @numberOfPeople, @campingCustomerID, @reservationDurationID)");
            insertNewReservationQuery.AddParameter("campingPlaceID", CampingPlaceID);
            insertNewReservationQuery.AddParameter("numberOfPeople", NumberOfPeople);
            insertNewReservationQuery.AddParameter("campingCustomerID", CampingCustomerID);
            insertNewReservationQuery.AddParameter("reservationDurationID", duration.ReservationID);//ophalen van id
            insertNewReservationQuery.Execute();
            return insertNewReservationQuery.SuccessFullyExecuted();
        }
    }
}
