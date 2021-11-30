using System.Collections.Generic;

namespace Model
{
    public class Accommodation : ModelBase<Accommodation>
    {
        public string Prefix { get; private set; }
        public string Name { get; private set; }

        public Accommodation()
        {
            
        }
        
        public Accommodation(string prefix, string name): this("-1", prefix, name)
        {
        }
        
        public Accommodation(string id, string prefix, string name)
        {
            this.Id = int.Parse(id);
            this.Prefix = prefix;
            this.Name = name;
        }

        protected override string Table()
        {
            return "Accommodation";
        }

        protected override string PrimaryKey()
        {
            return "AccommodationID";
        }

        public bool Update(string prefix, string name)
        {
            return base.Update(Accommodation.ToDictionary(prefix, name));
        }

        protected override Accommodation ToModel(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                return null;
            }
            
            dictionary.TryGetValue("AccommodationID", out string id);
            dictionary.TryGetValue("Prefix", out string prefix);
            dictionary.TryGetValue("Name", out string name);

            return new Accommodation(id, prefix, name);
        }
        
        protected override Dictionary<string, string> ToDictionary()
        {
            return Accommodation.ToDictionary(this.Prefix, this.Name);
        }
        
        private static Dictionary<string, string> ToDictionary(string prefix, string name)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("Prefix", prefix);
            dictionary.Add("Name", name);

            return dictionary;
        }
        
    }
}