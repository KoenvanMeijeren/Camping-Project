using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CampingCustomer : ModelBase<CampingCustomer>
    {
        public Address Address { get; private set; }
        public DateTime Birthdate { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber  { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        
        public CampingCustomer()
        {
            
        }
        public CampingCustomer(Address address, string birtdate, string email, string phoneNumber, string firstName, string lastName): this("-1",address, birtdate, email, phoneNumber, firstName, lastName)
        {

        }

        public CampingCustomer(string id, Address address, string birthdate, string email, string phoneNumber, string firstName, string lastName)
        {
            this.Id = int.Parse(id);
            this.Address = address;
            this.Birthdate = DateTime.Parse(birthdate);
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            this.FirstName = firstName;
            this.LastName = lastName;
        }
        
        protected override string Table()
        {
            return "CampingCustomer";
        }

        protected override string PrimaryKey()
        {
            return "CampingCustomerID";
        }

        public bool Update(Address address, DateTime birthdate, string email, string phoneNumber, string firstName, string lastName)
        {
            this.Address = address;
            this.Birthdate = birthdate;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            this.FirstName = firstName;
            this.LastName = lastName;

            return base.Update(CampingCustomer.ToDictionary(address, birthdate, email, phoneNumber, firstName, lastName));
        }

        protected override CampingCustomer ToModel(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                return null;
            }
            
            dictionary.TryGetValue("CampingCustomerID", out string id);
            dictionary.TryGetValue("AddressID", out string addressId);
            dictionary.TryGetValue("Address", out string street);
            dictionary.TryGetValue("AddressPostalCode", out string postalCode);
            dictionary.TryGetValue("AddressPlace", out string place);
            dictionary.TryGetValue("CampingCustomerBirthdate", out string birthdate);
            dictionary.TryGetValue("CampingCustomerEmail", out string email);
            dictionary.TryGetValue("CampingCustomerPhoneNumber", out string phoneNumber);
            dictionary.TryGetValue("CampingCustomerFirstName", out string firstName);
            dictionary.TryGetValue("CampingCustomerLastName", out string lastName);

            Address address = new Address(addressId, street, postalCode, place);

            return new CampingCustomer(id, address, birthdate, email, phoneNumber, firstName, lastName);
        }

        protected override Dictionary<string, string> ToDictionary()
        {
            return CampingCustomer.ToDictionary(this.Address, this.Birthdate, this.Email, this.PhoneNumber, this.FirstName, this.LastName);
        }

        private static Dictionary<string, string> ToDictionary(Address address, DateTime birthdate, string email, string phoneNumber, string firstName, string lastName)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {"CampingCustomerAddressID", address.Id.ToString()},
                {"CampingCustomerBirthdate", birthdate.ToString(CultureInfo.InvariantCulture)},
                {"CampingCustomerEmail", email},
                {"CampingCustomerPhoneNumber", phoneNumber},
                {"CampingCustomerFirstName", firstName},
                {"CampingCustomerLastName", lastName}
            };

            return dictionary;
        }

        protected override string BaseQuery()
        {
            string query = base.BaseQuery();
            query += " INNER JOIN Address A ON BT.CampingCustomerAddressID = A.AddressID";

            return query;
        }
        
    }
}
