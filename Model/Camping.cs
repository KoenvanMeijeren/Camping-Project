using System.Collections.Generic;

namespace Model
{
    public class Camping : ModelBase<Camping>
    {
        public string Name { get; private set; }
        
        public Address Address { get; private set; }
        public CampingOwner CampingOwner { get; private set; }

        public Camping()
        {
            
        }
        
        public Camping(string name, Address address, CampingOwner campingOwner): this("-1", name, address, campingOwner)
        {
        }
        
        public Camping(string id, string name, Address address, CampingOwner campingOwner)
        {
            bool success = int.TryParse(id, out int idNumeric);
            
            this.Id = success ? idNumeric : -1;
            this.Name = name;
            this.Address = address;
            this.CampingOwner = campingOwner;
        }
        
        protected override string Table()
        {
            return "Camping";
        }

        protected override string PrimaryKey()
        {
            return "CampingID";
        }

        public bool Update(string name, Address address, CampingOwner campingOwner)
        {
            this.Name = name;
            this.Address = address;
            this.CampingOwner = campingOwner;
            
            return base.Update(Camping.ToDictionary(name, address, campingOwner));
        }

        protected override Camping ToModel(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                return null;
            }
            
            dictionary.TryGetValue("CampingID", out string id);
            dictionary.TryGetValue("CampingName", out string name);

            dictionary.TryGetValue("AccountID", out string accountId);
            dictionary.TryGetValue("AccountEmail", out string email);
            dictionary.TryGetValue("AccountPassword", out string password);
            dictionary.TryGetValue("AccountRights", out string rights);

            dictionary.TryGetValue("CampingAddressID", out string addressId);
            dictionary.TryGetValue("CampingAddress", out string street);
            dictionary.TryGetValue("CampingPostalCode", out string postalCode);
            dictionary.TryGetValue("CampingPlace", out string place);

            dictionary.TryGetValue("CampingOwnerID", out string campingOwnerId);
            dictionary.TryGetValue("CampingOwnerName", out string campingOwnerName);

            Account account = new Account(accountId, email, password, int.Parse(rights));
            Address address = new Address(addressId, street, postalCode, place);
            CampingOwner campingOwner = new CampingOwner(account, campingOwnerId, campingOwnerName);

            return new Camping(id, name, address, campingOwner);
        }

        protected override Dictionary<string, string> ToDictionary()
        {
            return Camping.ToDictionary(this.Name, this.Address, this.CampingOwner);
        }

        private static Dictionary<string, string> ToDictionary(string name, Address address, CampingOwner campingOwner)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {"CampingName", name},
                {"CampingAddressID", address.Id.ToString()},
                {"CampingOwnerID", campingOwner.Id.ToString()}
            };

            return dictionary;
        }

        protected override string BaseQuery()
        {
            string query = base.BaseQuery();
            query += " INNER JOIN CampingOwner CO ON BT.CampingOwnerID = CO.CampingOwnerID";
            query += " INNER JOIN Address A ON BT.CampingAddressID = A.AddressID";
            query += " INNER JOIN Account AC on CO.CampingOwnerAccountID = AC.AccountID";

            return query;
        }
    }
}