using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CampingGuest : ModelBase<CampingGuest>
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime Birthdate { get; private set; }

        public CampingGuest()
        {
        }

        public CampingGuest(string firstName, string lastName, string birthdate) : this("-1", firstName, lastName, birthdate)
        {
        }

        public CampingGuest(string id, string firstName, string lastName, string birthdate)
        {
            var success = int.TryParse(id, out int numericId);
            
            this.Id = success ? numericId : -1;
            this.FirstName = firstName;
            this.LastName = lastName;
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

        public bool Update(string firstName, string lastName, DateTime birthdate)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Birthdate = birthdate;

            return base.Update(CampingGuest.ToDictionary(firstName, lastName, birthdate));
        }

        protected override CampingGuest ToModel(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                return null;
            }

            dictionary.TryGetValue("CampingID", out string id);
            dictionary.TryGetValue("CampingGuestFirstName", out string firstName);
            dictionary.TryGetValue("CampingGuestLastName", out string lastName);
            dictionary.TryGetValue("CampingGuestBirthdate", out string birthdate);

            return new CampingGuest(id, firstName, lastName, birthdate);
        }

        protected override Dictionary<string, string> ToDictionary()
        {
            return CampingGuest.ToDictionary(this.FirstName, this.LastName, this.Birthdate);
        }

        private static Dictionary<string, string> ToDictionary(string firstName, string lastName, DateTime birthdate)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {"CampingGuestFirstName", firstName},
                {"CampingGuestLastName", lastName},
                {"CampingGuestBirthdate", birthdate.ToShortDateString()}
            };

            return dictionary;
        }
    }
}
