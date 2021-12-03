using System.Collections.Generic;

namespace Model
{
    public class Address : ModelBase<Address>
    {
        public string Street { get; private set; }
        
        public string PostalCode { get; private set; }
        
        public string Place { get; private set; }

        public Address()
        {
            
        }
        
        public Address(string address, string postalCode, string place): this(null, address, postalCode, place)
        {
            
        }
        
        public Address(string id, string address, string postalCode, string place)
        {
            bool success = int.TryParse(id, out int idNumeric);
            
            this.Id = success ? idNumeric : -1;
            this.Street = address;
            this.PostalCode = postalCode;
            this.Place = place;
        }

        protected override string Table()
        {
            return "Address";
        }

        protected override string PrimaryKey()
        {
            return "AddressID";
        }

        public bool Update(string address, string postalCode, string place)
        {
            return base.Update(Address.ToDictionary(address, postalCode, place));
        }

        protected override Address ToModel(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                return null;
            }
            
            dictionary.TryGetValue("AddressID", out string id);
            dictionary.TryGetValue("Address", out string address);
            dictionary.TryGetValue("AddressPostalCode", out string postalCode);
            dictionary.TryGetValue("AddressPlace", out string place);

            return new Address(id, address, postalCode, place);
        }

        protected override Dictionary<string, string> ToDictionary()
        {
            return Address.ToDictionary(this.Street, this.PostalCode, this.PostalCode);
        }

        private static Dictionary<string, string> ToDictionary(string address, string postalCode, string place)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {"Address", address},
                {"AddressPostalCode", postalCode},
                {"AddressPlace", place}
            };

            return dictionary;
        }
        
    }
}