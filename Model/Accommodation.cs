using System.Collections.Generic;
using System.Linq;
using SystemCore;

namespace Model
{
    public enum AccommodationTypes
    {
        Bungalow,
        Camper,
        Caravan,
        Chalet,
        Tent,
        Unknown,
    }
    
    /// <inheritdoc/>
    public class Accommodation : ModelBase<Accommodation>
    {
        public const string 
            TableName = "Accommodation",
            ColumnId = "AccommodationID", 
            ColumnName = "AccommodationName", 
            ColumnPrefix = "AccommodationPrefix";
        
        public string Prefix { get; private set; }
        public string Name { get; private set; }
        public AccommodationTypes Type { get; private set; }

        public Accommodation(): base(TableName, ColumnId)
        {
            
        }
        
        public Accommodation(string prefix, string name): this("-1", prefix, name)
        {
        }
        
        public Accommodation(string id, string prefix, string name): base(TableName, ColumnId)
        {
            bool success = int.TryParse(id, out int idNumeric);
            
            this.Id = success ? idNumeric : -1;
            this.Prefix = prefix;
            this.Name = name;
            
            this.Type = this.Name switch
            {
                "Bungalow" => AccommodationTypes.Bungalow,
                "Camper" => AccommodationTypes.Camper,
                "Caravan" => AccommodationTypes.Caravan,
                "Chalet" => AccommodationTypes.Chalet,
                "Tent" => AccommodationTypes.Tent,
                _ => AccommodationTypes.Unknown,
            };
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.Name;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is Accommodation accommodation)
            {
                return accommodation.Id == this.Id;
            }

            return false;
        }
        
        public bool HasCampingPlaceTypes()
        {
            return this.HasCampingPlaceTypes(this);
        }

        public bool HasCampingPlaceTypes(Accommodation accommodation)
        {
            string queryString = this.BaseSelectQuery();
            queryString += $" INNER JOIN {CampingPlaceType.TableName} CP ON CP.{CampingPlaceType.ColumnAccommodation} = BT.{ColumnId} ";
            queryString += $" WHERE BT.{ColumnId} = @{ColumnId} ";

            Query query = new Query(queryString);
            query.AddParameter(ColumnId, accommodation.Id);
            var results = query.Select();

            return results != null && results.Any();
        }
        
        public bool Update(string prefix, string name)
        {
            return base.Update(Accommodation.ToDictionary(prefix, name));
        }

        /// <inheritdoc/>
        protected override Accommodation ToModel(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                return null;
            }
            
            dictionary.TryGetValue(ColumnId, out string id);
            dictionary.TryGetValue(ColumnPrefix, out string prefix);
            dictionary.TryGetValue(ColumnName, out string name);

            return new Accommodation(id, prefix, name);
        }
        
        /// <inheritdoc/>
        protected override Dictionary<string, string> ToDictionary()
        {
            return Accommodation.ToDictionary(this.Prefix, this.Name);
        }
        
        private static Dictionary<string, string> ToDictionary(string prefix, string name)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {ColumnPrefix, prefix},
                {ColumnName, name}
            };

            return dictionary;
        }
        
    }
}