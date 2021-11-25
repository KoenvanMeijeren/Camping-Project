using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemCore;

namespace Model
{
    public class ReservationDuration : IModel
    {
        public int Id { get; private set; }
        private DateTime CheckInDatetime { get; set; }
        private DateTime CheckOutDatetime { get; set; }

        public ReservationDuration(string id, string checkInDate, string checkOutDate)
        {
            this.Id = int.Parse(id);
            this.CheckInDatetime = DateTime.Parse(checkInDate);
            this.CheckOutDatetime = DateTime.Parse(checkOutDate);
        }

        private Boolean InsertReservationDuration()
        {
            Query insertNewReservationDurationQuery = new Query("INSERT INTO ReservationDuration VALUES (@checkinDatetime, @checkOutDatetime) OUTPUT inserted.ReservationDurationID");
            insertNewReservationDurationQuery.AddParameter("checkinDatetime", CheckInDatetime);
            insertNewReservationDurationQuery.AddParameter("checkOutDatetime", CheckOutDatetime);
            insertNewReservationDurationQuery.Execute();

            GetReservationID();
           
            return insertNewReservationDurationQuery.SuccessFullyExecuted();
        }

        private void GetReservationID()
        {
            string idFromDatabase = "";

            Query selectQuery = new Query("SELECT id FROM ReservationDuration WHERE CheckinDatetime=@checkinDatetime AND CheckoutDatetime=@checkoutDatetime");
            selectQuery.AddParameter("checkinDatetime", CheckInDatetime);
            selectQuery.AddParameter("checkOutDatetime", CheckOutDatetime);
            Dictionary<string, string> selectedRecord = selectQuery.SelectFirst();

            if (selectQuery.SuccessFullyExecuted())
            {
                if (selectedRecord.TryGetValue("ReservationDurationID", out idFromDatabase))
                {
                    this.Id = Int32.Parse(idFromDatabase);                    
                }
            }
           
        }
    }
}
