using System.Collections.Generic;
using SystemCore;

namespace Model
{
    public class ReservationCollection : IModelCollection
    {
        private List<Reservation> _collection;

        public ReservationCollection()
        {
            this._collection = new List<Reservation>();
        }

        public List<Reservation> Select()
        {
            if (this._collection.Count > 0)
            {
                return this._collection;
            }

            var reservations = (new Query(this.BaseQuery())).Select();
            foreach (Dictionary<string,string> dictionary in reservations)
            {
                this._collection.Add(this.ToModel(dictionary));
            }

            return this._collection;
        }

        private Reservation ToModel(Dictionary<string, string> dictionary)
        {
            dictionary.TryGetValue("ReservationID", out string reservationId);
            dictionary.TryGetValue("CampingCustomerID", out string campingCustomerId);
            dictionary.TryGetValue("CampingPlaceID", out string campingPlaceId);
            dictionary.TryGetValue("CampingPlaceTypeID", out string campingPlaceTypeId);
            dictionary.TryGetValue("AccommodationID", out string accommodationId);
            dictionary.TryGetValue("Prefix", out string prefix);
            dictionary.TryGetValue("Name", out string name);
            dictionary.TryGetValue("GuestLimit", out string guestLimit);
            dictionary.TryGetValue("StandardNightPrice", out string standardNightPrice);
            dictionary.TryGetValue("PlaceNumber", out string placeNumber);
            dictionary.TryGetValue("Surface", out string surface);
            dictionary.TryGetValue("ExtraNightPrice", out string extraNightPrice);
            dictionary.TryGetValue("NumberOfPeople", out string peopleCount);
            dictionary.TryGetValue("AdressID", out string addressId);
            dictionary.TryGetValue("Adress", out string address);
            dictionary.TryGetValue("Postalcode", out string postalCode);
            dictionary.TryGetValue("Place", out string place);
            dictionary.TryGetValue("Birthdate", out string birthdate);
            dictionary.TryGetValue("Email", out string email);
            dictionary.TryGetValue("Phonenumber", out string phoneNumber);
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

        private string BaseQuery()
        {
            string query = "SELECT * FROM Reservation R ";
            query += "INNER JOIN CampingPlace CP ON CP.CampingPlaceID = R.CampingPlaceID ";
            query += "INNER JOIN CampingPlaceType CPT ON CPT.AccommodationID = CP.TypeID ";
            query += "INNER JOIN CampingCustomer CC ON CC.CampingCustomerID = R.CampingCustomerID ";
            query += "INNER JOIN Adress CCA ON CCA.AdressID = CC.CampingCustomerAdressID ";
            query += "INNER JOIN ReservationDuration RD ON RD.ReservationDurationID = R.ReservationDurationID ";

            return query;
        }
        
    }
}