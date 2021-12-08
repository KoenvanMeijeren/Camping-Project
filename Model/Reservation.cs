using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemCore;

namespace Model
{
    /// <inheritdoc/>
    public class Reservation : ModelBase<Reservation>
    {
        public const string
            TableName = "Reservation",
            ColumnId = "ReservationID",
            ColumnPlace = "ReservationCampingPlaceID",
            ColumnCustomer = "ReservationCampingCustomerID",
            ColumnPeople = "ReservationNumberOfPeople",
            ColumnDuration = "ReservationDurationID";
        
        public int NumberOfPeople { get; private set; }
        public CampingCustomer CampingCustomer { get; private set; }
        public CampingPlace CampingPlace { get; private set; }
        public ReservationDuration Duration { get; private set; }
        public float TotalPrice { get; private set; }
        public string TotalPriceString { get; private set; }

        public Reservation(): base(TableName, ColumnId)
        {
        }
        
        public Reservation(string numberOfPeople, CampingCustomer campingCustomer, CampingPlace campingPlace, 
            ReservationDuration duration): this("-1", numberOfPeople, campingCustomer, campingPlace, duration)
        {
        }
        
        public Reservation(string id, string numberOfPeople, CampingCustomer campingCustomer, CampingPlace campingPlace, ReservationDuration duration): base(TableName, ColumnId)
        {
            bool successId = int.TryParse(id, out int numericId);
            bool successPeople = int.TryParse(numberOfPeople, out int numericPeople);

            this.Id = successId ? numericId : -1;
            this.NumberOfPeople = successPeople ? numericPeople : 0;
            this.CampingCustomer = campingCustomer;
            this.CampingPlace = campingPlace;
            this.Duration = duration;
            this.TotalPrice = this.CalculateTotalPrice();
            this.TotalPriceString = $"€{this.TotalPrice}";
        }

        /// <summary>
        /// Calculates the total price of the reservation, inclusive all nights.
        /// </summary>
        /// <returns>The total price of the reservation.</returns>
        public float CalculateTotalPrice()
        {
            if (this.Duration == null && this.CampingPlace == null)
            {
                return 0;
            }
            
            int days = 0;
            if (this.Duration == null)
            {
                return this.CampingPlace.TotalPrice * days;
            }
            
            var timeSpan = this.Duration.CheckOutDatetime.Subtract(this.Duration.CheckInDatetime);
            days = timeSpan.Days;

            return this.CampingPlace.TotalPrice * days;
        }

        /// <inheritdoc/>
        public override IEnumerable<Reservation> Select()
        {
            Query query = new Query(this.BaseSelectQuery() + $" ORDER BY {ReservationDuration.ColumnCheckInDate}");
            var items = query.Select();
            foreach (Dictionary<string, string> dictionary in items)
            {
                this.Collection.Add(this.ToModel(dictionary));
            }

            return this.Collection;
        }

        public bool Update(string numberOfPeople, CampingCustomer campingCustomer, CampingPlace campingPlace, ReservationDuration duration)
        {
            return base.Update(Reservation.ToDictionary(numberOfPeople, campingCustomer, campingPlace, duration));
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
            dictionary.TryGetValue(ColumnDuration, out string durationId);
            
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
            
            dictionary.TryGetValue(ReservationDuration.ColumnCheckInDate, out string checkInDateTime);
            dictionary.TryGetValue(ReservationDuration.ColumnCheckOutDate, out string checkOutDateTime);

            Address customerAddress = new Address(addressId, address, postalCode, place);
            Accommodation accommodation = new Accommodation(accommodationId, prefix, name);
            CampingPlaceType campingPlaceType = new CampingPlaceType(campingPlaceTypeId, guestLimit, standardNightPrice, accommodation);
            CampingPlace campingPlace = new CampingPlace(campingPlaceId, placeNumber, surface, extraNightPrice, campingPlaceType);
            Account account = new Account(accountId, email, password, rights);
            CampingCustomer campingCustomer = new CampingCustomer(campingCustomerId, account, customerAddress, birthdate, phoneNumber, firstName, lastName);
            ReservationDuration reservationDuration = new ReservationDuration(durationId, checkInDateTime, checkOutDateTime);

            return new Reservation(reservationId, peopleCount, campingCustomer, campingPlace, reservationDuration);
        }
        
        /// <inheritdoc/>
        protected override Dictionary<string, string> ToDictionary()
        {
            return Reservation.ToDictionary(this.NumberOfPeople.ToString(), this.CampingCustomer, this.CampingPlace, this.Duration);
        }

        private static Dictionary<string, string> ToDictionary(string numberOfPeople, CampingCustomer campingCustomer, CampingPlace campingPlace, ReservationDuration duration)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {ColumnPeople, numberOfPeople},
                {ColumnCustomer, campingCustomer.Id.ToString()},
                {ColumnPlace, campingPlace.Id.ToString()},
                {ColumnDuration, duration.Id.ToString()}
            };

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
            query += $" INNER JOIN {ReservationDuration.TableName} RD ON RD.{ReservationDuration.ColumnId} = R.{ColumnDuration} ";

            return query;
        }
    }
}
