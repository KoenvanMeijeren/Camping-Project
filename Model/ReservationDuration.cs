using System;
using System.Collections.Generic;
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
            /*CultureInfo culture = new CultureInfo("es-ES");
            String myDate = "15/05/2018";
            DateTime date = DateTime.Parse(myDate, culture);*/
            CheckInDatetime = checkin;
            CheckInDatetime = checkout;
        }

        private Boolean insertReservationDuration()
        {
            //ID hoeft niet worden meegegeven i.v.m. auto-increment
            Query insertNewReservationDurationQuery = new Query("INSERT INTO ReservationDuration VALUES (@checkinDatetime, @checkOutDatetime) OUTPUT inserted.ReservationDurationID");
            insertNewReservationDurationQuery.AddParameter("checkinDatetime", CheckInDatetime);
            insertNewReservationDurationQuery.AddParameter("checkOutDatetime", CheckOutDatetime);
            insertNewReservationDurationQuery.Execute();

            getReservationID();
           
            return insertNewReservationDurationQuery.SuccessFullyExecuted();
        }

        private void getReservationID()
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
