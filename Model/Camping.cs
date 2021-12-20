using System.Collections.Generic;

namespace Visualization
{
    /// <inheritdoc/>
    public class Camping : ModelBase<Camping>
    {
        public const string
            TableName = "Camping",
            ColumnId = "CampingID",
            ColumnName = "CampingName",
            ColumnAddress = "CampingAddressID",
            ColumnCampingOwner = "CampingOwnerID";
        
        public string Name { get; private set; }
        
        public Address Address { get; private set; }
        public CampingOwner CampingOwner { get; private set; }

        public Camping(): base(TableName, ColumnId)
        {
            
        }
        
        public Camping(string name, Address address, CampingOwner campingOwner): this("-1", name, address, campingOwner)
        {
        }
        
        public Camping(string id, string name, Address address, CampingOwner campingOwner): base(TableName, ColumnId)
        {
            bool success = int.TryParse(id, out int idNumeric);
            
            this.Id = success ? idNumeric : -1;
            this.Name = name;
            this.Address = address;
            this.CampingOwner = campingOwner;
        }

        public bool Update(string name, Address address, CampingOwner campingOwner)
        {
            this.Name = name;
            this.Address = address;
            this.CampingOwner = campingOwner;
            
            return base.Update(Camping.ToDictionary(name, address, campingOwner));
        }

        /// <inheritdoc/>
        protected override Camping ToModel(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                return null;
            }
            
            dictionary.TryGetValue(ColumnId, out string id);
            dictionary.TryGetValue(ColumnName, out string name);

            dictionary.TryGetValue(Account.ColumnId, out string accountId);
            dictionary.TryGetValue(Account.ColumnEmail, out string email);
            dictionary.TryGetValue(Account.ColumnPassword, out string password);
            dictionary.TryGetValue(Account.ColumnRights, out string rights);

            dictionary.TryGetValue(Address.ColumnId, out string addressId);
            dictionary.TryGetValue(Address.ColumnAddress, out string street);
            dictionary.TryGetValue(Address.ColumnPostalCode, out string postalCode);
            dictionary.TryGetValue(Address.ColumnPlace, out string place);

            dictionary.TryGetValue("CampingOwnerID", out string campingOwnerId);
            dictionary.TryGetValue("CampingOwnerName", out string campingOwnerName);

            Account account = new Account(accountId, email, password, rights);
            Address address = new Address(addressId, street, postalCode, place);
            CampingOwner campingOwner = new CampingOwner(account, campingOwnerId, campingOwnerName);

            return new Camping(id, name, address, campingOwner);
        }

        /// <inheritdoc/>
        protected override Dictionary<string, string> ToDictionary()
        {
            return Camping.ToDictionary(this.Name, this.Address, this.CampingOwner);
        }

        private static Dictionary<string, string> ToDictionary(string name, Address address, CampingOwner campingOwner)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {ColumnName, name},
                {ColumnAddress, address.Id.ToString()},
                {ColumnCampingOwner, campingOwner.Id.ToString()}
            };

            return dictionary;
        }

        protected override string BaseSelectQuery()
        {
            string query = base.BaseSelectQuery();
            query += $" INNER JOIN {CampingOwner.TableName} CO ON BT.{ColumnCampingOwner} = CO.{CampingOwner.ColumnId}";
            query += $" INNER JOIN {Address.TableName} A ON BT.{ColumnAddress} = A.{Address.ColumnId}";
            query += $" INNER JOIN {Account.TableName} AC on CO.{CampingOwner.ColumnAccount} = AC.{Account.ColumnId}";

            return query;
        }
    }
}