using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CampingPlace : IModel
    {
        public int Id { get; private set; }
        public int Number { get; private set; }
        public float Surface { get; private set; }
        public float ExtraNightPrice { get; private set; }
        public string Location { get; private set; }
        
        public float TotalPrice { get; private set; }
        
        public CampingPlaceType Type { get; private set; }

        public CampingPlace(string id, string number, string surface, string extraNightPrice, CampingPlaceType campingPlaceType)
        {
            this.Id = int.Parse(id);
            this.Number = int.Parse(number);
            this.Surface = float.Parse(surface);
            this.ExtraNightPrice = float.Parse(extraNightPrice);
            this.Type = campingPlaceType;
            this.Location = this.GetLocation();
            this.TotalPrice = this.ExtraNightPrice + this.Type.StandardNightPrice;
        }
        

        public string GetLocation()
        {
            return $"{this.Type.Accommodation.Prefix}-{this.Number}";
        }
        
        public static CampingPlace ToModel(Dictionary<string, string> dictionary)
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

            return new CampingPlace(campingPlaceId, placeNumber, surface, extraNightPrice, campingPlaceType);
        }

        public static string BaseQuery()
        {
            string query = "SELECT * FROM CampingPlace ";
            query += "INNER JOIN CampingPlaceType CPT ON CPT.CampingPlaceTypeID = TypeID ";
            query += "INNER JOIN Accommodation ACM ON ACM.AccommodationID = CPT.AccommodationID";

            return query;
        }

    }
}
