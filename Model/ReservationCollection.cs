using System.Collections.Generic;
using SystemCore;

namespace Model
{
    public class ReservationCollection
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
            
            var reservations = (new Query("SELECT * FROM Reservations")).Select();
            foreach (Dictionary<string,string> dictionary in reservations)
            {
                dictionary.TryGetValue("CampingPlaceId", out string campingPlaceId);
                dictionary.TryGetValue("NumberOfPeople", out string peopleCount);
                int.TryParse(campingPlaceId, out int campingPlaceIdNumeric);
                int.TryParse(peopleCount, out int peopleCountNumeric);

                Reservation reservation = new Reservation(campingPlaceIdNumeric, peopleCountNumeric, null);
                
                this._collection.Add(reservation);
            }

            return this._collection;
        }
    }
}