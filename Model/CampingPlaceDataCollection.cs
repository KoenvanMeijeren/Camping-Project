using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemCore;

namespace Model
{
    public static class CampingPlaceDataCollection
    {
        private static List<CampingPlaceData> _collection;

        public static List<CampingPlaceData> Select()
        {
            if (_collection != null)
            {
                return _collection;
            }

            _collection = new List<CampingPlaceData>();
            var campingPlaces = (new Query(BaseQuery())).Select();
            foreach (Dictionary<string, string> dictionary in campingPlaces)
            {
                _collection.Add(ToModel(dictionary));
            }

            return _collection;
        }

        private static CampingPlaceData ToModel(Dictionary<string, string> dictionary)
        {
            dictionary.TryGetValue("CampingPlaceID", out string campingPlaceId);
            dictionary.TryGetValue("CampingPlaceTypeID", out string campingPlaceTypeId);
            dictionary.TryGetValue("AccommodationID", out string accommodationId);
            dictionary.TryGetValue("Prefix", out string prefix);
            dictionary.TryGetValue("AccommodationName", out string accommodationName);
            dictionary.TryGetValue("GuestLimit", out string guestLimit);
            dictionary.TryGetValue("StandardNightPrice", out string standardNightPrice);
            dictionary.TryGetValue("PlaceNumber", out string placeNumber);
            dictionary.TryGetValue("Surface", out string surface);
            dictionary.TryGetValue("ExtraNightPrice", out string extraNightPrice);

            Accommodation accommodation = new Accommodation(accommodationId, prefix, accommodationName);
            CampingPlaceType campingPlaceType = new CampingPlaceType(campingPlaceTypeId, guestLimit, standardNightPrice, accommodation);
            CampingPlace campingPlace = new CampingPlace(campingPlaceId, placeNumber, surface, extraNightPrice, campingPlaceType);

            CampingPlaceData campingPlaceData = new CampingPlaceData(accommodationName, int.Parse(guestLimit), int.Parse(surface), (int.Parse(standardNightPrice)+ int.Parse(extraNightPrice)));

            return campingPlaceData;
        }

        private static string BaseQuery()
        {
            string query = "SELECT * FROM CampingPlace ";
            query += "INNER JOIN CampingPlaceType CPT ON CPT.CampingPlaceTypeID = TypeID ";
            query += "INNER JOIN Accommodation ACM ON ACM.AccommodationID = CPT.AccommodationID";

            return query;
        }
    }
}
