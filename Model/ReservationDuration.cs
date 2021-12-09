using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemCore;

namespace Model
{
    /// <inheritdoc/>
    public class ReservationDuration : ModelBase<ReservationDuration>
    {
        public const string
            TableName = "ReservationDuration",
            ColumnId = "ReservationDurationID",
            ColumnCheckInDate = "ReservationDurationCheckInDatetime",
            ColumnCheckOutDate = "ReservationDurationCheckOutDatetime";
        
        public DateTime CheckInDatetime { get; private set; }
        public DateTime CheckOutDatetime { get; private set; }
        
        public string CheckInDate { get; private set; }
        public string CheckOutDate { get; private set; }
        
        public string CheckInDateDatabaseFormat { get; private set; }
        public string CheckOutDateDatabaseFormat { get; private set; }

        public ReservationDuration(): base(TableName, ColumnId)
        {

        }

        public ReservationDuration(string checkInDate, string checkOutDate): this ("-1", checkInDate, checkOutDate)
        {

        }

        public ReservationDuration(string id, string checkInDate, string checkOutDate): base(TableName, ColumnId)
        {
            bool successId = int.TryParse(id, out int numericId);

            this.Id = successId ? numericId : -1;
            this.ParseInputDates(checkInDate, checkOutDate);
        }
        
        public bool Update()
        {
            return this.Update(this.CheckInDate.ToString(CultureInfo.InvariantCulture), this.CheckOutDate.ToString(CultureInfo.InvariantCulture));
        }

        public bool Update(string checkInDate, string checkOutDate)
        {
            this.ParseInputDates(checkInDate, checkOutDate);
            
            return base.Update(ReservationDuration.ToDictionary(this.CheckInDateDatabaseFormat, this.CheckOutDateDatabaseFormat));
        }


        private void ParseInputDates(string checkInDate, string checkOutDate)
        {
            this.CheckInDatetime = DateTimeParser.TryParse(checkInDate);
            this.CheckOutDatetime = DateTimeParser.TryParse(checkOutDate);
            
            this.CheckInDate = this.CheckInDatetime.ToShortDateString();
            this.CheckOutDate = this.CheckOutDatetime.ToShortDateString();
            
            this.CheckInDateDatabaseFormat = DateTimeParser.TryParseToDatabaseFormat(this.CheckInDatetime);
            this.CheckOutDateDatabaseFormat = DateTimeParser.TryParseToDatabaseFormat(this.CheckOutDatetime);
        }

        /// <inheritdoc/>
        protected override ReservationDuration ToModel(Dictionary<string, string> dictionary)
        {
            if(dictionary == null)
            {
                return null;
            }

            dictionary.TryGetValue(ColumnId, out string id);
            dictionary.TryGetValue(ColumnCheckInDate, out string checkInDateTime);
            dictionary.TryGetValue(ColumnCheckOutDate, out string checkOutDateTime);

            return new ReservationDuration(id, checkInDateTime, checkOutDateTime);
        }
        
        /// <inheritdoc/>
        protected override Dictionary<string, string> ToDictionary()
        {
            return ReservationDuration.ToDictionary(this.CheckInDateDatabaseFormat, this.CheckOutDateDatabaseFormat);
        }

        private static Dictionary<string, string> ToDictionary(string checkInDate, string checkOutDate)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {ColumnCheckInDate, checkInDate},
                {ColumnCheckOutDate, checkOutDate}
            };

            return dictionary;
        }

        /// <summary>
        /// Creates a dictionary with The object ReservationDuration as value
        /// </summary>
        /// <param name="checkInDate">check in date</param>
        /// <param name="checkOutDate">check out date</param>
        /// <returns></returns>
        private static Dictionary<string, DateTime> ToDictionaryDateTime(DateTime checkindate, DateTime checkoutdate)
        {
            Dictionary<string, DateTime> dictionary = new Dictionary<string, DateTime>
            {
                {ColumnCheckInDate,checkindate},
                {ColumnCheckOutDate, checkoutdate}
            };

            return dictionary;
        }

    }
}
