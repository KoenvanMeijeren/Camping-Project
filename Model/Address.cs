using System.Collections.Generic;
using SystemCore;

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

        /// <summary>
        /// Fetches Address with given parameters, if address already exists in database
        /// </summary>
        /// <param name="address">given address</param>
        /// <param name="postalCode">given postalcode</param>
        /// <returns>Address object</returns>
        private Address FetchAddressByParameters(string address, string postalCode)
        {
            Query query = new Query("SELECT * FROM Address WHERE Address = @Address AND AddressPostalCode = @AddressPostalCode");
            query.AddParameter("Address", address);
            query.AddParameter("AddressPostalCode", postalCode);
            var result = query.SelectFirst();

            return result != null ? this.ToModel(result) : null;
        }

        /// <summary>
        /// Returns Address object based on if it already exists in database. If it doesn't exist it creates one and returns that one
        /// </summary>
        /// <returns>Address object</returns>
        public Address FirstOrInsert()
        {
            var result = this.FetchAddressByParameters(this.Street, this.PostalCode);
            if (result != null)
            {
                return result;
            }

            this.Insert();
            return this.FetchAddressByParameters(this.Street, this.PostalCode);
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