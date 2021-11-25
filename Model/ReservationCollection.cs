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
                this._collection.Add(Reservation.ToModel(dictionary));
            }

            return this._collection;
        }

        private string BaseQuery()
        {
            string query = "SELECT * FROM Reservation R ";
            query += "INNER JOIN CampingPlace CP ON CP.CampingPlaceID = R.CampingPlaceID ";
            query += "INNER JOIN CampingPlaceType CPT ON CPT.AccommodationID = CP.TypeID ";
            query += "INNER JOIN CampingCustomer CC ON CC.CampingCustomerID = R.CampingCustomerID ";
            query += "INNER JOIN Address CCA ON CCA.AddressID = CC.CampingCustomerAddressID ";
            query += "INNER JOIN ReservationDuration RD ON RD.ReservationDurationID = R.ReservationDurationID ";

            return query;
        }
        
    }
}