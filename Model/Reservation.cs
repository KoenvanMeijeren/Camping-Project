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
        
        public int Id { get; private set; }
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
            dictionary.TryGetValue("ReservationDurationID", out string durationId);
            dictionary.TryGetValue("CampingCustomerID", out string campingCustomerId);
            dictionary.TryGetValue("CampingPlaceID", out string campingPlaceId);
            dictionary.TryGetValue("CampingPlaceTypeID", out string campingPlaceTypeId);
            dictionary.TryGetValue("AccommodationID", out string accommodationId);
            dictionary.TryGetValue("Prefix", out string prefix);
            dictionary.TryGetValue("AccommodationName", out string name);
            dictionary.TryGetValue("GuestLimit", out string guestLimit);
            dictionary.TryGetValue("StandardNightPrice", out string standardNightPrice);
            dictionary.TryGetValue("PlaceNumber", out string placeNumber);
            dictionary.TryGetValue("Surface", out string surface);
            dictionary.TryGetValue("ExtraNightPrice", out string extraNightPrice);
            dictionary.TryGetValue("NumberOfPeople", out string peopleCount);
            dictionary.TryGetValue("AddressID", out string addressId);
            dictionary.TryGetValue("Address", out string address);
            dictionary.TryGetValue("Postalcode", out string postalCode);
            dictionary.TryGetValue("Place", out string place);
            dictionary.TryGetValue("Birthdate", out string birthdate);
            dictionary.TryGetValue("Email", out string email);
            dictionary.TryGetValue("PhoneNumber", out string phoneNumber);
            dictionary.TryGetValue("CheckinDatetime", out string checkInDateTime);
            dictionary.TryGetValue("CheckoutDatetime", out string checkOutDateTime);
            dictionary.TryGetValue("CustomerFirstName", out string firstName);
            dictionary.TryGetValue("CustomerLastName", out string lastName);

            Address customerAddress = new Address(addressId, address, postalCode, place);
            Accommodation accommodation = new Accommodation(accommodationId, prefix, name);
            CampingPlaceType campingPlaceType = new CampingPlaceType(campingPlaceTypeId, guestLimit, standardNightPrice, accommodation);
            CampingPlace campingPlace = new CampingPlace(campingPlaceId, placeNumber, surface, extraNightPrice, campingPlaceType);
            CampingCustomer campingCustomer = new CampingCustomer(campingCustomerId, customerAddress, birthdate, email, phoneNumber, firstName, lastName);
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
                {"NumberOfPeople", numberOfPeople},
                {"CampingCustomerID", campingCustomer.Id.ToString()},
                {"CampingPlaceID", campingPlace.Id.ToString()},
                {"ReservationDurationID", duration.Id.ToString()}
            };

            return dictionary;
        }
        
        protected override string BaseQuery()
        {
            string query = "SELECT * FROM Reservation R ";
            query += "INNER JOIN CampingPlace CP ON CP.CampingPlaceID = R.CampingPlaceID ";
            query += "INNER JOIN CampingPlaceType CPT ON CPT.CampingPlaceTypeID = CP.TypeID ";
            query += "INNER JOIN Accommodation AM ON AM.AccommodationID = CPT.AccommodationID ";
            query += "INNER JOIN CampingCustomer CC ON CC.CampingCustomerID = R.CampingCustomerID ";
            query += "INNER JOIN Address CCA ON CCA.AddressID = CC.CampingCustomerAddressID ";
            query += "INNER JOIN ReservationDuration RD ON RD.ReservationDurationID = R.ReservationDurationID ";

            return query;
        }
    }
}
