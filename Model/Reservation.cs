using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemCore;

namespace Model
{
    public class Reservation : IModel
    {
        
        public int Id { get; private set; }
        public int NumberOfPeople { get; private set; }
        
        public CampingCustomer CampingCustomer { get; private set; }
        public CampingPlace CampingPlace { get; private set; }
        public ReservationDuration Duration { get; private set; }
        public float TotalPrice { get; private set; }
        public string TotalPriceString { get; private set; }

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
        
        public bool Insert()
        {
            if (this.Id > 0)
            {
                throw new ArgumentException("You cannot insert an existing reservation.");
            }
            
            Query insertNewReservationQuery = new Query("INSERT INTO Reservation VALUES (@campingPlaceID, @numberOfPeople, @campingCustomerID, @reservationDurationID)");
            insertNewReservationQuery.AddParameter("campingPlaceID", this.CampingPlace.Id);
            insertNewReservationQuery.AddParameter("numberOfPeople", this.NumberOfPeople);
            insertNewReservationQuery.AddParameter("campingCustomerID", this.CampingCustomer.Id);
            insertNewReservationQuery.AddParameter("reservationDurationID", this.Duration.Id);
            insertNewReservationQuery.Execute();
            
            return insertNewReservationQuery.SuccessFullyExecuted();
        }

        public bool Delete()
        {
            Query deleteQuery = new Query("DELETE FROM Reservation WHERE id = @id");
            deleteQuery.AddParameter("id", this.Id);
            deleteQuery.Execute();

            return deleteQuery.SuccessFullyExecuted();
        }

        public static Reservation LoadFromId(int id)
        {
            string queryString = Reservation.BaseQuery();
            queryString += "WHERE id = @id";
            Query query = new Query(queryString);
            query.AddParameter("id", id);

            return Reservation.ToModel(query.SelectFirst());
        }
        
        public static Reservation ToModel(Dictionary<string, string> dictionary)
        {
            dictionary.TryGetValue("ReservationID", out string reservationId);
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

            Address customerAddress = new Address(addressId, address, postalCode, place);
            Accommodation accommodation = new Accommodation(accommodationId, prefix, name);
            CampingPlaceType campingPlaceType = new CampingPlaceType(campingPlaceTypeId, guestLimit, standardNightPrice, accommodation);
            CampingPlace campingPlace = new CampingPlace(campingPlaceId, placeNumber, surface, extraNightPrice, campingPlaceType);
            CampingCustomer campingCustomer = new CampingCustomer(campingCustomerId, customerAddress, birthdate, email, phoneNumber);
            ReservationDuration reservationDuration = new ReservationDuration(reservationId, checkInDateTime, checkOutDateTime);

            return new Reservation(reservationId, peopleCount, campingCustomer, campingPlace, reservationDuration);
        }
        
        public static string BaseQuery()
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
