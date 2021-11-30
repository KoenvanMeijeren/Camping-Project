using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CampingGuest : ModelBase<CampingGuest>
    {
        public string Name { get; private set; }
        
        public DateTime Birthdate { get; private set; }

        public CampingGuest()
        {
        }

        public CampingGuest(string name, string birthdate) : this("-1", name, birthdate)
        {
        }

        public CampingGuest(string id, string name, string birthdate)
        {
            this.Id = int.Parse(id);
            this.Name = name;
            this.Birthdate = DateTime.Parse(birthdate);
        }

        protected override string Table()
        {
            return "CampingGuest";
        }

        protected override string PrimaryKey()
        {
            return "CampingGuestID";
        }

        public bool Update(string name, DateTime birthdate)
        {
            this.Name = name;
            this.Birthdate = birthdate;

            return base.Update(CampingGuest.ToDictionary(name, birthdate));
        }

        protected override CampingGuest ToModel(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                return null;
            }

            dictionary.TryGetValue("CampingID", out string id);
            dictionary.TryGetValue("CampingGuestName", out string name);
            dictionary.TryGetValue("Birthdate", out string birthdate);

            return new CampingGuest(id, name, birthdate);
        }

        protected override Dictionary<string, string> ToDictionary()
        {
            return CampingGuest.ToDictionary(this.Name, this.Birthdate);
        }

        private static Dictionary<string, string> ToDictionary(string name, DateTime birthdate)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {"CampingGuestName", name},
                {"Birthdate", birthdate.ToShortDateString()}
            };

            return dictionary;
        }
    }
}
