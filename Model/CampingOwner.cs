using System.Collections.Generic;

namespace Model
{
    public class CampingOwner : ModelBase<CampingOwner>
    {
        public int Id { get; private set; }
        
        public string Name { get; private set; }

        public CampingOwner()
        {
        }

        public CampingOwner(string name) : this("-1", name)
        {
        }

        public CampingOwner(string id, string name)
        {
            this.Id = int.Parse(id);
            this.Name = name;
        }

        protected override string Table()
        {
            return "CampingOwner";
        }

        protected override string PrimaryKey()
        {
            return "CampingOwnerID";
        }

        public bool Update(string name)
        {
            this.Name = name;

            return base.Update(CampingOwner.ToDictionary(name));
        }

        protected override CampingOwner ToModel(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                return null;
            }

            dictionary.TryGetValue("CampingOwnerID", out string id);
            dictionary.TryGetValue("CampingOwnerName", out string name);


            return new CampingOwner(id, name);
        }

        protected override Dictionary<string, string> ToDictionary()
        {
            return CampingOwner.ToDictionary(this.Name);
        }

        private static Dictionary<string, string> ToDictionary(string name)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {"CampingOwnerName", name}
            };

            return dictionary;
        }
    }
}