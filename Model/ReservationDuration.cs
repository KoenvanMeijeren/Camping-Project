using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemCore;

namespace Model
{
    public class ReservationDuration
    {
        public int ReservationID { get; private set; }
        private DateTime CheckInDatetime { get; set; }
        private DateTime CheckOutDatetime { get; set; }


        public void SetDuration(DateTime checkin, DateTime checkout)
        {
            CheckInDatetime = checkin;
            CheckInDatetime = checkout;
        }


        private Boolean InsertReservationDuration()
        {
            //ID hoeft niet worden meegegeven i.v.m. auto-increment
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
                    this.ReservationID = Int32.Parse(idFromDatabase);                    
                }
            }
           
        }
    }
}
