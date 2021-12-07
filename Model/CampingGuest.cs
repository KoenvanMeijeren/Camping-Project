using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <inheritdoc/>
    public class CampingGuest : ModelBase<CampingGuest>
    {
        public const string
            TableName = "CampingGuest",
            ColumnId = "CampingGuestID",
            ColumnBirthdate = "CampingGuestBirthdate",
            ColumnFirstName = "CampingGuestFirstName",
            ColumnLastName = "CampingGuestLastName";
        
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime Birthdate { get; private set; }

        public CampingGuest(): base(TableName, ColumnId)
        {
        }

        public CampingGuest(string firstName, string lastName, string birthdate) : this("-1", firstName, lastName, birthdate)
        {
        }

        public CampingGuest(string id, string firstName, string lastName, string birthdate): base(TableName, ColumnId)
        {
            bool success = int.TryParse(id, out int numericId);
            bool successDate = DateTime.TryParse(birthdate, out DateTime dateTime);
            
            this.Id = success ? numericId : -1;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Birthdate = successDate ? dateTime : DateTime.MinValue;
        }

        public bool Update(string firstName, string lastName, DateTime birthdate)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Birthdate = birthdate;

            return base.Update(CampingGuest.ToDictionary(firstName, lastName, birthdate));
        }

        /// <inheritdoc/>
        protected override CampingGuest ToModel(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                return null;
            }

            dictionary.TryGetValue(ColumnId, out string id);
            dictionary.TryGetValue(ColumnFirstName, out string firstName);
            dictionary.TryGetValue(ColumnLastName, out string lastName);
            dictionary.TryGetValue(ColumnBirthdate, out string birthdate);

            return new CampingGuest(id, firstName, lastName, birthdate);
        }

        /// <inheritdoc/>
        protected override Dictionary<string, string> ToDictionary()
        {
            return CampingGuest.ToDictionary(this.FirstName, this.LastName, this.Birthdate);
        }

        private static Dictionary<string, string> ToDictionary(string firstName, string lastName, DateTime birthdate)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {ColumnFirstName, firstName},
                {ColumnLastName, lastName},
                {ColumnBirthdate, birthdate.ToShortDateString()}
            };

            return dictionary;
        }
    }
}
