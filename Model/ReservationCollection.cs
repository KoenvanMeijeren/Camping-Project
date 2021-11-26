using System.Collections.Generic;
using SystemCore;

namespace Model
{
    public static class ReservationCollection
    {
        private static List<Reservation> Collection = new List<Reservation>();

        public static List<Reservation> Select()
        {
            var reservations = (new Query(Reservation.BaseQuery())).Select();
            foreach (Dictionary<string,string> dictionary in reservations)
            {
                ReservationCollection.Collection.Add(Reservation.ToModel(dictionary));
            }

            return ReservationCollection.Collection;
        }

    }
}