﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemCore;

namespace Model
{
    // todo: henk
    public class ReservationDuration : ModelBase<ReservationDuration>
    {
        public int Id { get; private set; }
        public DateTime CheckInDatetime { get; private set; }
        public DateTime CheckOutDatetime { get; private set; }
        
        public string CheckInDate { get; private set; }
        public string CheckOutDate { get; private set; }

        public ReservationDuration()
        {

        }

        public ReservationDuration(string checkInDate, string checkOutDate): this ("-1", checkInDate, checkOutDate)
        {

        }

        public ReservationDuration(string id, string checkInDate, string checkOutDate)
        {
            this.Id = int.Parse(id);
            this.CheckInDatetime = DateTime.Parse(checkInDate);
            this.CheckOutDatetime = DateTime.Parse(checkOutDate);
            this.CheckInDate = this.CheckInDatetime.ToShortDateString();
            this.CheckOutDate = this.CheckOutDatetime.ToShortDateString();
        }

        protected override string Table()
        {
            return "ReservationDuration";
        }

        protected override string PrimaryKey()
        {
            return "ReservationDurationID";
        }

        public bool Update(string checkInDate, string checkOutDate)
        {
            this.CheckInDate = checkInDate;
            this.CheckOutDate = checkOutDate;

            return base.Update(ReservationDuration.ToDictionary(checkInDate, checkOutDate));
        }

        protected override ReservationDuration ToModel(Dictionary<string, string> dictionary)
        {
            if(dictionary == null)
            {
                return null;
            }

            dictionary.TryGetValue("ReservationDurationID", out string id);
            dictionary.TryGetValue("CheckinDateTime", out string checkInDateTime);
            dictionary.TryGetValue("CheckoutDateTime", out string checkOutDateTime);

            return new ReservationDuration(id, checkInDateTime, checkOutDateTime);
        }
        protected override Dictionary<string, string> ToDictionary()
        {
            return ReservationDuration.ToDictionary(this.CheckInDate, this.CheckOutDate);
        }

        private static Dictionary<string, string> ToDictionary(string checkInDate, string checkOutDate)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {"CheckinDateTime", checkInDate},
                {"CheckoutDateTime", checkOutDate}
            };

            return dictionary;

        }

        private Boolean InsertReservationDuration()
        {
            Query insertNewReservationDurationQuery = new Query("INSERT INTO ReservationDuration VALUES (@checkinDatetime, @checkOutDatetime) OUTPUT inserted.ReservationDurationID");
            insertNewReservationDurationQuery.AddParameter("checkinDatetime", CheckInDatetime);
            insertNewReservationDurationQuery.AddParameter("checkOutDatetime", CheckOutDatetime);
            insertNewReservationDurationQuery.Execute();

            this.GetReservationID();
           
            return insertNewReservationDurationQuery.SuccessFullyExecuted();
        }

        private void GetReservationID()
        {
            Query selectQuery = new Query("SELECT id FROM ReservationDuration WHERE CheckinDatetime=@checkinDatetime AND CheckoutDatetime=@checkoutDatetime");
            selectQuery.AddParameter("checkinDatetime", CheckInDatetime);
            selectQuery.AddParameter("checkOutDatetime", CheckOutDatetime);
            Dictionary<string, string> selectedRecord = selectQuery.SelectFirst();

            if (selectQuery.SuccessFullyExecuted())
            {
                if (selectedRecord.TryGetValue("ReservationDurationID", out string idFromDatabase))
                {
                    this.Id = Int32.Parse(idFromDatabase);                    
                }
            }
           
        }
    }
}
