using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemCore;

namespace Visualization
{
    /// <summary>
    /// Enum used for database columns: ReservationDeleted, ReservationPayed, ReservationRestitutionPayed
    /// </summary>
    public enum ReservationColumnStatus
    {
        False = 0,
        True = 1
    }
    
    /// <inheritdoc/>
    public class Reservation : ModelBase<Reservation>
    {
        public const string
            TableName = "Reservation",
            ColumnId = "ReservationID",
            ColumnPlace = "ReservationCampingPlaceID",
            ColumnCustomer = "ReservationCampingCustomerID",
            ColumnPeople = "ReservationNumberOfPeople",
            ColumnDeleted = "ReservationDeleted",
            ColumnPaid = "ReservationPaid",
            ColumnRestitutionPaid = "ReservationRestitutionPaid",
            ColumnDeletedTime = "ReservationDeletedTime",
            ColumnCheckInDate = "ReservationCheckInDate",
            ColumnCheckOutDate = "ReservationCheckOutDate";
        
        public int NumberOfPeople { get; private set; }
        public CampingCustomer CampingCustomer { get; private set; }
        public CampingPlace CampingPlace { get; private set; }
        public float TotalPrice { get; private set; }
        public string TotalPriceString { get; private set; }
        public ReservationColumnStatus ReservationDeleted { get; private set; }
        public ReservationColumnStatus ReservationPaid { get; private set; }
        public ReservationColumnStatus ReservationRestitutionPaid { get; private set; }
        public DateTime? ReservationDeletedTime { get; private set; }
        public DateTime CheckInDatetime { get; private set; }
        public DateTime CheckOutDatetime { get; private set; }
        
        public string CheckInDate { get; private set; }
        public string CheckOutDate { get; private set; }

        public List<ReservationCampingGuest> CampingGuests { get; private set; }

        /// <summary>
        /// Constructs this object for accessing the select methods.
        /// </summary>
        public Reservation(): base(TableName, ColumnId)
        {
        }
        
        /// <summary>
        /// Constructs the object for inserting it in the database.
        /// </summary>
        /// <param name="numberOfPeople">The number of people.</param>
        /// <param name="campingCustomer">The camping customer.</param>
        /// <param name="campingPlace">The camping place.</param>
        /// <param name="checkInDate">The check in date.</param>
        /// <param name="checkOutDate">The check out date.</param>
        public Reservation(string numberOfPeople, CampingCustomer campingCustomer, CampingPlace campingPlace, string checkInDate, string checkOutDate) : this("-1", numberOfPeople, campingCustomer, campingPlace, ReservationColumnStatus.False, ReservationColumnStatus.False, ReservationColumnStatus.False, null, checkInDate, checkOutDate)
        {
        }
        
        /// <summary>
        /// Constructs the object for updating and deleting it in the database.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="numberOfPeople">The number of people.</param>
        /// <param name="campingCustomer">The camping customer.</param>
        /// <param name="campingPlace">The camping place.</param>
        /// <param name="reservationDeleted">If it has been deleted.</param>
        /// <param name="reservationPaid">If it has been paid.</param>
        /// <param name="reservationRestitutionPaid">If restitution has been paid.</param>
        /// <param name="reservationDeletedTime">The time where the reservation was deleted.</param>
        /// <param name="checkInDate">The check in date.</param>
        /// <param name="checkOutDate">The check out date.</param>
        public Reservation(string id, string numberOfPeople, CampingCustomer campingCustomer, CampingPlace campingPlace, ReservationColumnStatus reservationDeleted, ReservationColumnStatus reservationPaid, ReservationColumnStatus reservationRestitutionPaid, string reservationDeletedTime, string checkInDate, string checkOutDate) : base(TableName, ColumnId)
        {
            bool successId = int.TryParse(id, out int numericId);
            bool successPeople = int.TryParse(numberOfPeople, out int numericPeople);
            DateTime dateTime = DateTimeParser.TryParse(reservationDeletedTime);
            this.ParseInputDates(checkInDate, checkOutDate);
            
            this.Id = successId ? numericId : -1;
            // Add one, else it doesn't include the customer
            this.NumberOfPeople = successPeople ? numericPeople : 0;
            if (this.Id == -1 && campingCustomer != null)
            {
                this.NumberOfPeople++;
            }
            
            this.CampingCustomer = campingCustomer;
            this.CampingPlace = campingPlace;
            this.TotalPrice = this.CalculateTotalPrice();
            this.TotalPriceString = $"€{this.TotalPrice}";
            this.ReservationDeleted = reservationDeleted;
            this.ReservationPaid = reservationPaid;
            this.ReservationRestitutionPaid = reservationRestitutionPaid;
            this.ReservationDeletedTime = dateTime != DateTime.MinValue ? dateTime : null;
        }

        /// <summary>
        /// Calculates the total price of the reservation, inclusive all nights.
        /// </summary>
        /// <returns>The total price of the reservation.</returns>
        public float CalculateTotalPrice()
        {
            if (this.CampingPlace == null)
            {
                return 0;
            }

            var timeSpan = this.CheckOutDatetime.Subtract(this.CheckInDatetime);
            var days = timeSpan.Days;

            return this.CampingPlace.TotalPrice * days;
        }

        private void ParseInputDates(string checkInDate, string checkOutDate)
        {
            this.CheckInDatetime = DateTimeParser.TryParse(checkInDate);
            this.CheckOutDatetime = DateTimeParser.TryParse(checkOutDate);
            
            this.CheckInDate = this.CheckInDatetime.ToShortDateString();
            this.CheckOutDate = this.CheckOutDatetime.ToShortDateString();
        }
        
        /// <summary>
        /// Returns all the reservation of the given customer id.
        /// </summary>
        /// <param name="customerId">ID of customer</param>
        /// <returns>Database records of given customer's reservations</returns>
        public List<Reservation> GetCustomersReservations(int customerId)
        {
            Query query = new Query(this.BaseSelectQuery() + $" WHERE {ColumnCustomer} = @customerId AND {ColumnDeleted} = @{ColumnDeleted} ORDER BY {ColumnCheckInDate} ");
            query.AddParameter("customerId", customerId);
            query.AddParameter(ColumnDeleted, ReservationDeleted == ReservationColumnStatus.True);

            List<Reservation> reservations = new List<Reservation>();
            foreach(var item in query.Select())
            {
                reservations.Add(this.ToModel(item));
            }

            return reservations;
        }

        /// <inheritdoc/>
        public override IEnumerable<Reservation> Select()
        {
            Query query = new Query(this.BaseSelectQuery() + $" ORDER BY {ColumnCheckInDate}");
            var items = query.Select();
            this.Collection = new List<Reservation>();
            foreach (Dictionary<string, string> dictionary in items)
            {
                this.Collection.Add(this.ToModel(dictionary));
            }

            return this.Collection;
        }

        public bool Update()
        {
            return this.Update(this.NumberOfPeople.ToString(), this.CampingCustomer, this.CampingPlace, this.ReservationDeleted, this.ReservationPaid, this.ReservationRestitutionPaid, this.ReservationDeletedTime, this.CheckInDatetime, this.CheckOutDatetime);
        }
        
        public bool Update(string numberOfPeople, CampingCustomer campingCustomer, CampingPlace campingPlace, ReservationColumnStatus reservationDeleted, ReservationColumnStatus reservationPaid, ReservationColumnStatus reservationRestitutionPaid, DateTime? reservationDeletedTime, DateTime checkInDate, DateTime checkOutDate)
        {
            return base.Update(Reservation.ToDictionary(numberOfPeople, campingCustomer, campingPlace, reservationDeleted, reservationPaid, reservationRestitutionPaid, reservationDeletedTime, checkInDate, checkOutDate));
        }

        /// <inheritdoc/>
        protected override Reservation ToModel(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                return null;
            }

            dictionary.TryGetValue(ColumnId, out string reservationId);
            dictionary.TryGetValue(ColumnPlace, out string campingPlaceId);
            dictionary.TryGetValue(ColumnPeople, out string peopleCount);
            dictionary.TryGetValue(ColumnCustomer, out string campingCustomerId);
            dictionary.TryGetValue(ColumnDeleted, out string reservationDeleted);
            dictionary.TryGetValue(ColumnPaid, out string reservationPaid);
            dictionary.TryGetValue(ColumnRestitutionPaid, out string reservationRestitutionPaid);
            dictionary.TryGetValue(ColumnDeletedTime, out string reservationDeletedTime);
            dictionary.TryGetValue(ColumnCheckInDate, out string checkInDate);
            dictionary.TryGetValue(ColumnCheckOutDate, out string checkOutDate);
            
            dictionary.TryGetValue(CampingPlaceType.ColumnId, out string campingPlaceTypeId);
            dictionary.TryGetValue(CampingPlaceType.ColumnGuestLimit, out string guestLimit);
            dictionary.TryGetValue(CampingPlaceType.ColumnNightPrice, out string standardNightPrice);
            
            dictionary.TryGetValue(Accommodation.ColumnId, out string accommodationId);
            dictionary.TryGetValue(Accommodation.ColumnPrefix, out string prefix);
            dictionary.TryGetValue(Accommodation.ColumnName, out string name);
            
            dictionary.TryGetValue(CampingPlace.ColumnId, out string placeNumber);
            dictionary.TryGetValue(CampingPlace.ColumnSurface, out string surface);
            dictionary.TryGetValue(CampingPlace.ColumnExtraNightPrice, out string extraNightPrice);
            
            dictionary.TryGetValue(Address.ColumnId, out string addressId);
            dictionary.TryGetValue(Address.ColumnAddress, out string address);
            dictionary.TryGetValue(Address.ColumnPostalCode, out string postalCode);
            dictionary.TryGetValue(Address.ColumnPlace, out string place);

            dictionary.TryGetValue(Account.ColumnId, out string accountId);
            dictionary.TryGetValue(Account.ColumnEmail, out string email);
            dictionary.TryGetValue(Account.ColumnPassword, out string password);
            dictionary.TryGetValue(Account.ColumnRights, out string rights);

            dictionary.TryGetValue(CampingCustomer.ColumnBirthdate, out string birthdate);
            dictionary.TryGetValue(CampingCustomer.ColumnPhoneNumber, out string phoneNumber);
            dictionary.TryGetValue(CampingCustomer.ColumnFirstName, out string firstName);
            dictionary.TryGetValue(CampingCustomer.ColumnLastName, out string lastName);

            Address customerAddress = new Address(addressId, address, postalCode, place);
            Accommodation accommodation = new Accommodation(accommodationId, prefix, name);
            CampingPlaceType campingPlaceType = new CampingPlaceType(campingPlaceTypeId, guestLimit, standardNightPrice, accommodation);
            CampingPlace campingPlace = new CampingPlace(campingPlaceId, placeNumber, surface, extraNightPrice, campingPlaceType);
            Account account = new Account(accountId, email, password, rights);
            CampingCustomer campingCustomer = new CampingCustomer(campingCustomerId, account, customerAddress, birthdate, phoneNumber, firstName, lastName);

            Reservation reservation = new Reservation(reservationId, peopleCount, campingCustomer, campingPlace, (ReservationColumnStatus)Int32.Parse(reservationDeleted), (ReservationColumnStatus)Int32.Parse(reservationPaid), (ReservationColumnStatus)Int32.Parse(reservationRestitutionPaid), reservationDeletedTime, checkInDate, checkOutDate);
            
            ReservationCampingGuest reservationCampingGuestModel = new ReservationCampingGuest();
            reservation.CampingGuests = reservationCampingGuestModel.SelectByReservation(reservation);

            return reservation;
        }
        
        /// <inheritdoc/>
        protected override Dictionary<string, string> ToDictionary()
        {
            return Reservation.ToDictionary(this.NumberOfPeople.ToString(), this.CampingCustomer, this.CampingPlace, this.ReservationDeleted, this.ReservationPaid, this.ReservationRestitutionPaid, this.ReservationDeletedTime, this.CheckInDatetime, this.CheckOutDatetime);
        }

        private static Dictionary<string, string> ToDictionary(string numberOfPeople, CampingCustomer campingCustomer, CampingPlace campingPlace, ReservationColumnStatus reservationDeleted, ReservationColumnStatus reservationPaid, ReservationColumnStatus reservationRestitutionPaid, DateTime? reservationDeletedTime, DateTime checkInDate, DateTime checkOutDate)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {ColumnPeople, numberOfPeople},
                {ColumnCustomer, campingCustomer.Id.ToString()},
                {ColumnPlace, campingPlace.Id.ToString()},
                {ColumnDeleted, Convert.ToInt32(reservationDeleted).ToString()},
                {ColumnPaid, Convert.ToInt32(reservationPaid).ToString()},
                {ColumnRestitutionPaid, Convert.ToInt32(reservationRestitutionPaid).ToString()},
                {ColumnCheckInDate, DateTimeParser.TryParseToDatabaseDateTimeFormat(checkInDate)},
                {ColumnCheckOutDate, DateTimeParser.TryParseToDatabaseDateTimeFormat(checkOutDate)},
            };

            if (reservationDeletedTime != null && reservationDeletedTime != DateTime.MinValue)
            {
                dictionary.Add(ColumnDeletedTime, DateTimeParser.TryParseToDatabaseDateTimeFormat(reservationDeletedTime));
            }

            return dictionary;
        }
        
        /// <inheritdoc/>
        protected override string BaseSelectQuery()
        {
            string query = $"SELECT * FROM {TableName} R ";
            query += $" INNER JOIN {CampingPlace.TableName} CP ON CP.{CampingPlace.ColumnId} = R.{ColumnPlace} ";
            query += $" INNER JOIN {CampingPlaceType.TableName} CPT ON CPT.{CampingPlaceType.ColumnId} = CP.{CampingPlace.ColumnType} ";
            query += $" INNER JOIN {Accommodation.TableName} AM ON AM.{Accommodation.ColumnId} = CPT.{CampingPlaceType.ColumnAccommodation} ";
            query += $" INNER JOIN {CampingCustomer.TableName} CC ON CC.{CampingCustomer.ColumnId} = R.{ColumnCustomer} ";
            query += $" LEFT JOIN {Account.TableName} AC on CC.{CampingCustomer.ColumnAccount} = AC.{Account.ColumnId}";
            query += $" INNER JOIN {Address.TableName} CCA ON CCA.{Address.ColumnId} = CC.{CampingCustomer.ColumnAddress} ";

            return query;
        }
    }
}
