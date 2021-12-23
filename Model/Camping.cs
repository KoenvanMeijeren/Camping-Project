using System.Collections.Generic;

namespace Model
{
    /// <inheritdoc/>
    public class Camping : ModelBase<Camping>
    {
        public const string
            TableName = "Camping",
            ColumnId = "CampingID",
            ColumnName = "CampingName",
            ColumnAddress = "CampingAddressID",
            ColumnCampingOwner = "CampingOwnerID",
            ColumnPhoneNumber = "CampingPhoneNumber",
            ColumnEmailAddress = "CampingEmailAddress",
            ColumnFacebook = "CampingFacebook",
            ColumnTwitter = "CampingTwitter",
            ColumnInstagram = "CampingInstagram",
            ColumnColor = "CampingColor";


        
        public string Name { get; private set; }
        public Address Address { get; private set; }
        public CampingOwner CampingOwner { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Email { get; private set; }
        public string Facebook { get; private set; }
        public string Twitter { get; private set; }
        public string Instagram { get; private set; }
        public string Color { get; private set; }

        public Camping(): base(TableName, ColumnId)
        {
            
        }
        
        public Camping(string name, Address address, CampingOwner campingOwner, string phoneNumber, string email, string facebook, string twitter, string instagram, string color) : this("-1", name, address, campingOwner, phoneNumber, email, facebook, twitter, instagram, color)
        {
        }
        
        public Camping(string id, string name, Address address, CampingOwner campingOwner, string phoneNumber, string email, string facebook, string twitter, string instagram, string color): base(TableName, ColumnId)
        {
            bool success = int.TryParse(id, out int idNumeric);
            
            this.Id = success ? idNumeric : -1;
            this.Name = name;
            this.Address = address;
            this.CampingOwner = campingOwner;
            this.PhoneNumber = phoneNumber;
            this.Email = email;
            this.Facebook = facebook;
            this.Twitter = twitter;
            this.Instagram = instagram;
            this.Color = color;
        }

        public bool Update(string name, Address address, CampingOwner campingOwner, string phoneNumber, string emailAddress, string facebook, string twitter, string instagram, string color)
        {
            this.Name = name;
            this.Address = address;
            this.CampingOwner = campingOwner;
            this.PhoneNumber = phoneNumber;
            this.Email = emailAddress;
            this.Facebook = facebook;
            this.Twitter = twitter;
            this.Instagram = instagram;
            this.Color = color;

            return base.Update(Camping.ToDictionary(name, address, campingOwner, phoneNumber, emailAddress, facebook, twitter, instagram, color));
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
            dictionary.TryGetValue(ColumnPhoneNumber, out string phoneNumber);
            dictionary.TryGetValue(ColumnEmailAddress, out string emailAddress);
            dictionary.TryGetValue(ColumnFacebook, out string facebook);
            dictionary.TryGetValue(ColumnTwitter, out string twitter);
            dictionary.TryGetValue(ColumnInstagram, out string instagram);
            dictionary.TryGetValue(ColumnColor, out string color);


            dictionary.TryGetValue(Account.ColumnId, out string accountId);
            dictionary.TryGetValue(Account.ColumnEmail, out string email);
            dictionary.TryGetValue(Account.ColumnPassword, out string password);
            dictionary.TryGetValue(Account.ColumnRights, out string rights);

            dictionary.TryGetValue(Address.ColumnId, out string addressId);
            dictionary.TryGetValue(Address.ColumnAddress, out string street);
            dictionary.TryGetValue(Address.ColumnPostalCode, out string postalCode);
            dictionary.TryGetValue(Address.ColumnPlace, out string place);

            dictionary.TryGetValue(CampingOwner.ColumnId, out string campingOwnerId);
            dictionary.TryGetValue(CampingOwner.ColumnFirstName, out string campingOwnerFirstName);
            dictionary.TryGetValue(CampingOwner.ColumnLastName, out string campingOwnerLastName);

            Account account = new Account(accountId, email, password, rights);
            Address address = new Address(addressId, street, postalCode, place);
            CampingOwner campingOwner = new CampingOwner(campingOwnerId, account, campingOwnerFirstName, campingOwnerLastName);

            return new Camping(id, name, address, campingOwner, phoneNumber, emailAddress, facebook, twitter, instagram, color);
        }

        /// <inheritdoc/>
        protected override Dictionary<string, string> ToDictionary()
        {
            return Camping.ToDictionary(this.Name, this.Address, this.CampingOwner, this.PhoneNumber, this.Email, this.Facebook, this.Twitter, this.Instagram, this.Color);
        }

        private static Dictionary<string, string> ToDictionary(string name, Address address, CampingOwner campingOwner, string phoneNumber, string emailAddress, string facebook, string twitter, string instagram, string color)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {ColumnName, name},
                {ColumnAddress, address.Id.ToString()},
                {ColumnCampingOwner, campingOwner.Id.ToString()},
                {ColumnPhoneNumber, phoneNumber},
                {ColumnEmailAddress, emailAddress},
                {ColumnFacebook, facebook},
                {ColumnTwitter, twitter},
                {ColumnInstagram, instagram},
                {ColumnColor, color}
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