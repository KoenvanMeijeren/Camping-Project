using System.Collections.Generic;

namespace Model
{
    public class CampingOwner : ModelBase<CampingOwner>
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public CampingOwner()
        {
        }

        public CampingOwner(string firstName, string lastName) : this("-1", firstName, lastName)
        {
        }

        public CampingOwner(string id, string firstName, string lastName)
        {
            this.Id = int.Parse(id);
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        protected override string Table()
        {
            return "CampingOwner";
        }

        protected override string PrimaryKey()
        {
            return "CampingOwnerID";
        }

        public bool Update(string firstName, string lastName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;

            return base.Update(CampingOwner.ToDictionary(firstName, lastName));
        }

        protected override CampingOwner ToModel(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                return null;
            }

            dictionary.TryGetValue("CampingOwnerID", out string id);
            dictionary.TryGetValue("CampingOwnerFirstName", out string firstName);
            dictionary.TryGetValue("CampingOwnerLastName", out string lastName);


            return new CampingOwner(id, firstName, lastName);
        }

        protected override Dictionary<string, string> ToDictionary()
        {
            return CampingOwner.ToDictionary(this.FirstName, this.LastName);
        }

        private static Dictionary<string, string> ToDictionary(string firstName, string lastName)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {"CampingOwnerFirstName", firstName},
                {"CampingOwnerLastName", lastName}
            };

            return dictionary;
        }
    }
}