using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemCore;

namespace Model
{
    // todo: koen
    public class CampingPlaceCollection
    {
        private List<CampingPlace> _collection;
        public CampingPlaceCollection()
        {
            this._collection = new List<CampingPlace>();
        }

        public List<CampingPlace> Select()
        {
            if (this._collection.Count > 0)
            {
                return this._collection;
            }

            var campingPlaces = (new Query(this.BaseQuery())).Select();
            foreach (Dictionary<string, string> dictionary in campingPlaces)
            {
                this._collection.Add(this.ToModel(dictionary));
            }

            return this._collection;
        }

        private CampingPlace ToModel(Dictionary<string, string> dictionary)
        {
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

            Accommodation accommodation = new Accommodation(accommodationId, prefix, name);
            CampingPlaceType campingPlaceType = new CampingPlaceType(campingPlaceTypeId, guestLimit, standardNightPrice, accommodation);
            CampingPlace campingPlace = new CampingPlace(campingPlaceId, placeNumber, surface, extraNightPrice, campingPlaceType);

            return campingPlace;
        }

        private string BaseQuery()
        {
            string query = "SELECT * FROM CampingPlace ";
            query += "INNER JOIN CampingPlaceType CPT ON CPT.CampingPlaceTypeID = TypeID ";
            query += "INNER JOIN Accommodation ACM ON ACM.AccommodationID = CPT.AccommodationID";

            return query;
        }

    }
}
