using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemCore;

namespace Model
{
    public class ReservationDuration : ModelBase<ReservationDuration>
    {
        public const string DatabaseDateTimeFormatString = "MM/dd/yyyy hh:mm:00";
        
        public DateTime CheckInDatetime { get; private set; }
        public DateTime CheckOutDatetime { get; private set; }
        
        public string CheckInDate { get; private set; }
        public string CheckOutDate { get; private set; }
        public string CheckInDateDatabaseFormat { get; private set; }
        public string CheckOutDateDatabaseFormat { get; private set; }

        public ReservationDuration()
        {

        }

        public ReservationDuration(string checkInDate, string checkOutDate): this ("-1", checkInDate, checkOutDate)
        {

        }

        public ReservationDuration(string id, string checkInDate, string checkOutDate)
        {
            bool successId = int.TryParse(id, out int numericId);
            bool successCheckInDate = DateTime.TryParseExact(checkInDate, 
                DatabaseDateTimeFormatString, 
                CultureInfo.InvariantCulture, 
                DateTimeStyles.None, 
                out DateTime checkInDateObject);
            bool successCheckOutDate = DateTime.TryParseExact(checkOutDate, 
                DatabaseDateTimeFormatString, 
                CultureInfo.InvariantCulture,
                DateTimeStyles.None, 
                out DateTime checkOutDateObject);
            
            this.Id = successId ? numericId : -1;
            this.CheckInDatetime = successCheckInDate ? checkInDateObject : DateTime.MinValue;
            this.CheckOutDatetime = successCheckOutDate ? checkOutDateObject : DateTime.MinValue;
            this.CheckInDate = this.CheckInDatetime.ToShortDateString();
            this.CheckOutDate = this.CheckOutDatetime.ToShortDateString();
            this.CheckInDateDatabaseFormat = checkInDate;
            this.CheckOutDateDatabaseFormat = checkOutDate;
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
            this.CheckInDateDatabaseFormat = checkInDate;
            this.CheckOutDateDatabaseFormat = checkOutDate;

            return base.Update(ReservationDuration.ToDictionary(checkInDate, checkOutDate));
        }

        protected override ReservationDuration ToModel(Dictionary<string, string> dictionary)
        {
            if(dictionary == null)
            {
                return null;
            }

            dictionary.TryGetValue("ReservationDurationID", out string id);
            dictionary.TryGetValue("ReservationDurationCheckInDatetime", out string checkInDateTime);
            dictionary.TryGetValue("ReservationDurationCheckOutDatetime", out string checkOutDateTime);

            return new ReservationDuration(id, checkInDateTime, checkOutDateTime);
        }
        protected override Dictionary<string, string> ToDictionary()
        {
            return ReservationDuration.ToDictionary(this.CheckInDateDatabaseFormat, this.CheckOutDateDatabaseFormat);
        }

        private static Dictionary<string, string> ToDictionary(string checkInDate, string checkOutDate)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {"ReservationDurationCheckInDateTime", checkInDate},
                {"ReservationDurationCheckOutDateTime", checkOutDate}
            };

            return dictionary;

        }

    }
}
