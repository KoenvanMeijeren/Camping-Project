using System.Collections.Generic;

namespace Model
{
    public class CampingPlaceType : ModelBase<CampingPlaceType>
    {
        public int GuestLimit { get; private set; }
        public float StandardNightPrice { get; private set; }
        
        public Accommodation Accommodation { get; private set; }

        public CampingPlaceType()
        {

        }

        public CampingPlaceType(string guestLimit, string standardNightPrice, Accommodation accommodation): this("-1", guestLimit, standardNightPrice, accommodation)
        {

        }

        public CampingPlaceType(string id, string guestLimit, string standardNightPrice, Accommodation accommodation)
        {
            this.Id = int.Parse(id);
            this.GuestLimit = int.Parse(guestLimit);
            this.StandardNightPrice = float.Parse(standardNightPrice);
            this.Accommodation = accommodation;
        }

        protected override string Table()
        {
            return "CampingPlaceType";
        }

        protected override string PrimaryKey()
        {
            return "CampingPlaceTypeID";
        }

        public bool Update(int guestLimit, float standardNightPrice, Accommodation accommodation)
        {
            this.GuestLimit = guestLimit;
            this.StandardNightPrice = standardNightPrice;
            this.Accommodation = accommodation;

            return base.Update(CampingPlaceType.ToDictionary(guestLimit, standardNightPrice, accommodation));
        }

        protected override CampingPlaceType ToModel(Dictionary<string, string> dictionary)
        {
            if(dictionary == null)
            {
                return null;
            }

            dictionary.TryGetValue("CampingTypePlaceID", out string campingPlaceTypeId);
            dictionary.TryGetValue("AccommodationID", out string accommodationId);
            dictionary.TryGetValue("GuestLimit", out string guestLimit);
            dictionary.TryGetValue("StandardNightPrice", out string standardNightPrice);
            dictionary.TryGetValue("Prefix", out string prefix);
            dictionary.TryGetValue("AccommodationType", out string accommodationType);

            Accommodation accommodation = new Accommodation(prefix, accommodationType);

            return new CampingPlaceType(guestLimit, standardNightPrice, accommodation);
        }

        protected override Dictionary<string, string> ToDictionary()
        {
            return CampingPlaceType.ToDictionary(this.GuestLimit, this.StandardNightPrice, this.Accommodation);
        }

        private static Dictionary<string, string> ToDictionary(int guestLimit, float standardNightPrice, Accommodation accommodation)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {"GuestLimit", guestLimit.ToString()},
                {"StandardNightPrice", standardNightPrice.ToString()},
                {"AccommodationID", accommodation.Id.ToString()}
            };

            return dictionary;
        }

        protected override string BaseQuery()
        {
            string query = base.BaseQuery();
            query += "INNER JOIN Accommodation AC ON BT.AccommodationID = AC.AccomodationID";

            return query;
        }
    }
}