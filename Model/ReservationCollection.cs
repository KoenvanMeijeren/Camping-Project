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

            var reservations = (new Query(Reservation.BaseQuery())).Select();
            foreach (Dictionary<string,string> dictionary in reservations)
            {
                this._collection.Add(Reservation.ToModel(dictionary));
            }

            return this._collection;
        }

    }
}