using System.Collections.Generic;

namespace Model
{
    public class Camping : ModelBase<Camping>
    {
        public int Id { get; private set; }
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
            this.Id = int.Parse(id);
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
            dictionary.TryGetValue("AddressID", out string addressId);
            dictionary.TryGetValue("Address", out string street);
            dictionary.TryGetValue("PostalCode", out string postalCode);
            dictionary.TryGetValue("Place", out string place);
            dictionary.TryGetValue("CampingOwnerID", out string campingOwnerId);
            dictionary.TryGetValue("CampingOwnerName", out string campingOwnerName);

            Address address = new Address(addressId, street, postalCode, place);
            CampingOwner campingOwner = new CampingOwner(campingOwnerId, campingOwnerName);

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
                {"Name", name},
                {"Address", address.Id.ToString()},
                {"CampingOwner", campingOwner.Id.ToString()}
            };

            return dictionary;
        }

        protected override string BaseQuery()
        {
            string query = base.BaseQuery();
            query += " INNER JOIN CampingOwner CO ON BT.OwnerID = CO.CampingOwnerID";
            query += " INNER JOIN Address A ON BT.CampingAddressID = A.AddressID";

            return query;
        }
    }
}