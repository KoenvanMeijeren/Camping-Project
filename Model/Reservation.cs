using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemCore;

namespace Model
{
    public class Reservation : ModelBase<Reservation>
    {
        public int NumberOfPeople { get; private set; }
        public CampingCustomer CampingCustomer { get; private set; }
        public CampingPlace CampingPlace { get; private set; }
        public ReservationDuration Duration { get; private set; }
        public float TotalPrice { get; private set; }
        public string TotalPriceString { get; private set; }

        public Reservation()
        {
        }
        
        public Reservation(string numberOfPeople, CampingCustomer campingCustomer, CampingPlace campingPlace, 
            ReservationDuration duration): this("-1", numberOfPeople, campingCustomer, campingPlace, duration)
        {
        }
        
        public Reservation(string id, string numberOfPeople, CampingCustomer campingCustomer, CampingPlace campingPlace, ReservationDuration duration)
        {
            this.Id = int.Parse(id);
            this.NumberOfPeople = int.Parse(numberOfPeople);
            this.CampingCustomer = campingCustomer;
            this.CampingPlace = campingPlace;
            this.Duration = duration;
            this.TotalPrice = this.CalculateTotalPrice();
            this.TotalPriceString = $"€{this.TotalPrice}";
        }

        public float CalculateTotalPrice()
        {
            var timeSpan = this.Duration.CheckOutDatetime.Subtract(this.Duration.CheckInDatetime);
            int days = timeSpan.Days;

            return this.CampingPlace.TotalPrice * days;
        }
        
        protected override string Table()
        {
            return "Reservation";
        }

        protected override string PrimaryKey()
        {
            return "ReservationID";
        }

        public override IEnumerable<Reservation> Select()
        {
            Query query = new Query(this.BaseQuery());
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

        protected override Reservation ToModel(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                return null;
            }

            dictionary.TryGetValue("ReservationID", out string reservationId);
            dictionary.TryGetValue("ReservationCampingPlaceID", out string campingPlaceId);
            dictionary.TryGetValue("ReservationNumberOfPeople", out string peopleCount);
            dictionary.TryGetValue("ReservationCampingCustomerID", out string campingCustomerId);
            dictionary.TryGetValue("ReservationDurationID", out string durationId);
            
            dictionary.TryGetValue("CampingPlaceTypeID", out string campingPlaceTypeId);
            dictionary.TryGetValue("CampingPlaceTypeGuestLimit", out string guestLimit);
            dictionary.TryGetValue("CampingPlaceTypeStandardNightPrice", out string standardNightPrice);
            
            dictionary.TryGetValue("AccommodationID", out string accommodationId);
            dictionary.TryGetValue("AccommodationPrefix", out string prefix);
            dictionary.TryGetValue("AccommodationName", out string name);
            
            dictionary.TryGetValue("CampingPlaceNumber", out string placeNumber);
            dictionary.TryGetValue("CampingPlaceSurface", out string surface);
            dictionary.TryGetValue("CampingPlaceExtraNightPrice", out string extraNightPrice);
            
            dictionary.TryGetValue("AddressID", out string addressId);
            dictionary.TryGetValue("Address", out string address);
            dictionary.TryGetValue("AddressPostalcode", out string postalCode);
            dictionary.TryGetValue("AddressPlace", out string place);

            dictionary.TryGetValue("AccountID", out string accountId);
            dictionary.TryGetValue("AccountUsername", out string username);
            dictionary.TryGetValue("AccountPassword", out string password);
            dictionary.TryGetValue("AccountRights", out string rights);

            dictionary.TryGetValue("CampingCustomerBirthdate", out string birthdate);
            dictionary.TryGetValue("CampingCustomerEmail", out string email);
            dictionary.TryGetValue("CampingCustomerPhoneNumber", out string phoneNumber);
            dictionary.TryGetValue("CampingCustomerFirstName", out string firstName);
            dictionary.TryGetValue("CampingCustomerLastName", out string lastName);
            
            dictionary.TryGetValue("ReservationDurationCheckInDatetime", out string checkInDateTime);
            dictionary.TryGetValue("ReservationDurationCheckOutDatetime", out string checkOutDateTime);

            Address customerAddress = new Address(addressId, address, postalCode, place);
            Accommodation accommodation = new Accommodation(accommodationId, prefix, name);
            CampingPlaceType campingPlaceType = new CampingPlaceType(campingPlaceTypeId, guestLimit, standardNightPrice, accommodation);
            CampingPlace campingPlace = new CampingPlace(campingPlaceId, placeNumber, surface, extraNightPrice, campingPlaceType);
            Account account = new Account(accountId, username, password, int.Parse(rights));
            CampingCustomer campingCustomer = new CampingCustomer(campingCustomerId, account, customerAddress, birthdate, email, phoneNumber, firstName, lastName);
            ReservationDuration reservationDuration = new ReservationDuration(durationId, checkInDateTime, checkOutDateTime);

            return new Reservation(reservationId, peopleCount, campingCustomer, campingPlace, reservationDuration);
        }
        
        protected override Dictionary<string, string> ToDictionary()
        {
            return Reservation.ToDictionary(this.NumberOfPeople.ToString(), this.CampingCustomer, this.CampingPlace, this.Duration);
        }

        private static Dictionary<string, string> ToDictionary(string numberOfPeople, CampingCustomer campingCustomer, CampingPlace campingPlace, ReservationDuration duration)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {"ReservationNumberOfPeople", numberOfPeople},
                {"ReservationCampingCustomerID", campingCustomer.Id.ToString()},
                {"ReservationCampingPlaceID", campingPlace.Id.ToString()},
                {"ReservationDurationID", duration.Id.ToString()}
            };

            return dictionary;
        }
        
        protected override string BaseQuery()
        {
            string query = "SELECT * FROM Reservation R ";
            query += " INNER JOIN CampingPlace CP ON CP.CampingPlaceID = R.ReservationCampingPlaceID ";
            query += " INNER JOIN CampingPlaceType CPT ON CPT.CampingPlaceTypeID = CP.CampingPlaceTypeID ";
            query += " INNER JOIN Accommodation AM ON AM.AccommodationID = CPT.CampingPlaceTypeAccommodationID ";
            query += " INNER JOIN CampingCustomer CC ON CC.CampingCustomerID = R.ReservationCampingCustomerID ";
            query += " INNER JOIN Account AC on CC.CampingCustomerAccountID = AC.AccountID";
            query += " INNER JOIN Address CCA ON CCA.AddressID = CC.CampingCustomerAddressID ";
            query += " INNER JOIN ReservationDuration RD ON RD.ReservationDurationID = R.ReservationDurationID ";

            return query;
        }
    }
}
