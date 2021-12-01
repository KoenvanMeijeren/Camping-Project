using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CampingPlace : ModelBase<CampingPlace>
    {
        public int Number { get; private set; }
        public float Surface { get; private set; }
        public float ExtraNightPrice { get; private set; }
        public string Location { get; private set; }
        public string LocationSelect { get; private set; }
        public float TotalPrice { get; private set; }
        public CampingPlaceType Type { get; private set; }

        public CampingPlace()
        {
        }
        
        public CampingPlace(string number, string surface, string extraNightPrice, CampingPlaceType campingPlaceType): this("-1", number, surface, extraNightPrice, campingPlaceType)
        {
        }
        
        public CampingPlace(string id, string number, string surface, string extraNightPrice, CampingPlaceType campingPlaceType)
        {
            this.Id = int.Parse(id);
            this.Number = int.Parse(number);
            this.Surface = float.Parse(surface);
            this.ExtraNightPrice = float.Parse(extraNightPrice);
            this.Type = campingPlaceType;
            this.Location = this.GetLocation();
            this.LocationSelect = this.GetLocationSelect();
            this.TotalPrice = this.ExtraNightPrice + this.Type.StandardNightPrice;
        }

        public string GetLocation()
        {
            return $"{this.Type.Accommodation.Prefix}-{this.Number}";
        }

        public string GetLocationSelect()
        {
            return $"{this.GetLocation()} ({this.Id})";
        }
        
        protected override string Table()
        {
            return "CampingPlace";
        }

        protected override string PrimaryKey()
        {
            return "CampingPlaceID";
        }

        public bool Update(int number, float surface, float extraNightPrice, CampingPlaceType campingPlaceType)
        {
            this.Number = number;
            this.Surface = surface;
            this.ExtraNightPrice = extraNightPrice;
            this.Type = campingPlaceType;
            
            return base.Update(CampingPlace.ToDictionary(number, surface, extraNightPrice, campingPlaceType));
        }

        protected override CampingPlace ToModel(Dictionary<string, string> dictionary)
        {
            dictionary.TryGetValue("CampingPlaceID", out string campingPlaceId);
            dictionary.TryGetValue("CampingPlaceTypeID", out string campingPlaceTypeId);
            dictionary.TryGetValue("CampingPlaceTypeAccommodationID", out string accommodationId);
            dictionary.TryGetValue("AccommodationPrefix", out string prefix);
            dictionary.TryGetValue("AccommodationName", out string accommodationName);
            dictionary.TryGetValue("CampingPlaceTypeGuestLimit", out string guestLimit);
            dictionary.TryGetValue("CampingPlaceTypeStandardNightPrice", out string standardNightPrice);
            dictionary.TryGetValue("CampingPlaceNumber", out string placeNumber);
            dictionary.TryGetValue("CampingPlaceSurface", out string surface);
            dictionary.TryGetValue("CampingPlaceExtraNightPrice", out string extraNightPrice);

            Accommodation accommodation = new Accommodation(accommodationId, prefix, accommodationName);
            CampingPlaceType campingPlaceType = new CampingPlaceType(campingPlaceTypeId, guestLimit, standardNightPrice, accommodation);

            return new CampingPlace(campingPlaceId, placeNumber, surface, extraNightPrice, campingPlaceType);
        }

        protected override Dictionary<string, string> ToDictionary()
        {
            return CampingPlace.ToDictionary(this.Number, this.Surface, this.ExtraNightPrice, this.Type);
        }

        private static Dictionary<string, string> ToDictionary(int number, float surface, float extraNightPrice, CampingPlaceType campingPlaceType)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {"CampingPlaceTypeID", campingPlaceType.Id.ToString()},
                {"CampingPlaceNumber", number.ToString()},
                {"CampingPlaceSurface", surface.ToString(CultureInfo.InvariantCulture)},
                {"CampingPlaceExtraNightPrice", extraNightPrice.ToString(CultureInfo.InvariantCulture)}
            };

            return dictionary;
        }
        
        protected override string BaseQuery()
        {
            string query = base.BaseQuery();
            query += " INNER JOIN CampingPlaceType CPT ON CPT.CampingPlaceTypeID = BT.CampingPlaceTypeID";
            query += " INNER JOIN Accommodation ACM ON ACM.AccommodationID = CPT.CampingPlaceTypeAccommodationID";

            return query;
        }

    }
}
