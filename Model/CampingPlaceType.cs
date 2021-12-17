using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SystemCore;

namespace Model
{
    /// <inheritdoc/>
    public class CampingPlaceType : ModelBase<CampingPlaceType>
    {
        public const string
            TableName = "CampingPlaceType",
            ColumnId = "CampingPlaceTypeID",
            ColumnAccommodation = "CampingPlaceTypeAccommodationID",
            ColumnGuestLimit = "CampingPlaceTypeGuestLimit",
            ColumnNightPrice = "CampingPlaceTypeStandardNightPrice";
        
        public int GuestLimit { get; private set; }
        public float StandardNightPrice { get; private set; }
        
        public Accommodation Accommodation { get; private set; }

        public CampingPlaceType(): base(TableName, ColumnId)
        {

        }

        public CampingPlaceType(string guestLimit, string standardNightPrice, Accommodation accommodation): this("-1", guestLimit, standardNightPrice, accommodation)
        {

        }

        public CampingPlaceType(string id, string guestLimit, string standardNightPrice, Accommodation accommodation): base(TableName, ColumnId)
        {
            bool successId = int.TryParse(id, out int numericId);
            bool successGuestLimit = int.TryParse(guestLimit, out int numericGuestLimit);
            bool successStandardNightPrice = float.TryParse(standardNightPrice, out float numericStandardNightPrice);
            
            this.Id = successId ? numericId : -1;
            this.GuestLimit = successGuestLimit ? numericGuestLimit : 0;
            this.StandardNightPrice = successStandardNightPrice ? numericStandardNightPrice : 0;
            this.Accommodation = accommodation;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.Accommodation.ToString();
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj is CampingPlaceType campingPlaceType)
            {
                return campingPlaceType.Id == this.Id;
            }

            return false;
        }

        public bool HasCampingPlaces(CampingPlaceType campingPlaceType)
        {
            string queryString = this.BaseSelectQuery();
            queryString += $" INNER JOIN {CampingPlace.TableName} CP ON CP.{CampingPlace.ColumnType} = BT.{ColumnId} ";
            queryString += $" WHERE BT.{ColumnId} = @{ColumnId} ";

            Query query = new Query(queryString);
            query.AddParameter(ColumnId, campingPlaceType.Id);
            var results = query.Select();

            return results != null && results.Any();
        }
        
        public bool Update(string guestLimit, string standardNightPrice, Accommodation accommodation)
        {
            bool successGuestLimit = int.TryParse(guestLimit, out int numericGuestLimit);
            bool successStandardNightPrice = float.TryParse(standardNightPrice, out float numericStandardNightPrice);
            
            this.GuestLimit = successGuestLimit ? numericGuestLimit : 0;
            this.StandardNightPrice = successStandardNightPrice ? numericStandardNightPrice : 0;
            this.Accommodation = accommodation;

            return base.Update(CampingPlaceType.ToDictionary(this.GuestLimit, this.StandardNightPrice, this.Accommodation));
        }

        /// <inheritdoc/>
        protected override CampingPlaceType ToModel(Dictionary<string, string> dictionary)
        {
            if(dictionary == null)
            {
                return null;
            }

            dictionary.TryGetValue(ColumnId, out string campingPlaceTypeId);
            dictionary.TryGetValue(ColumnGuestLimit, out string guestLimit);
            dictionary.TryGetValue(ColumnNightPrice, out string standardNightPrice);
            
            dictionary.TryGetValue(ColumnAccommodation, out string accommodationId);
            dictionary.TryGetValue(Accommodation.ColumnPrefix, out string prefix);
            dictionary.TryGetValue(Accommodation.ColumnName, out string accommodationName);

            Accommodation accommodation = new Accommodation(accommodationId, prefix, accommodationName);

            return new CampingPlaceType(campingPlaceTypeId, guestLimit, standardNightPrice, accommodation);
        }

        /// <inheritdoc/>
        protected override Dictionary<string, string> ToDictionary()
        {
            return CampingPlaceType.ToDictionary(this.GuestLimit, this.StandardNightPrice, this.Accommodation);
        }

        private static Dictionary<string, string> ToDictionary(int guestLimit, float standardNightPrice, Accommodation accommodation)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {ColumnGuestLimit, guestLimit.ToString()},
                {ColumnNightPrice, standardNightPrice.ToString(CultureInfo.InvariantCulture)},
                {ColumnAccommodation, accommodation.Id.ToString()}
            };

            return dictionary;
        }

        /// <inheritdoc/>
        protected override string BaseSelectQuery()
        {
            string query = base.BaseSelectQuery();
            query += $" INNER JOIN {Accommodation.TableName} AC ON BT.{ColumnAccommodation} = AC.{Accommodation.ColumnId}";

            return query;
        }
    }
}