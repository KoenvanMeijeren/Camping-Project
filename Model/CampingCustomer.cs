using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemCore;

namespace Model
{
    /// <inheritdoc/>
    public class CampingCustomer : ModelBase<CampingCustomer>
    {
        public const string
            TableName = "CampingCustomer",
            ColumnId = "CampingCustomerID",
            ColumnAccount = "CampingCustomerAccountID",
            ColumnAddress = "CampingCustomerAddressID",
            ColumnFirstName = "CampingCustomerFirstName",
            ColumnLastName = "CampingCustomerLastName",
            ColumnBirthdate = "CampingCustomerBirthdate",
            ColumnPhoneNumber = "CampingCustomerPhoneNumber";
        
        public Account Account { get; private set; }
        public Address Address { get; private set; }
        public DateTime Birthdate { get; private set; }
        public string PhoneNumber  { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        
        public CampingCustomer(): base(TableName, ColumnId)
        {
            
        }

        public CampingCustomer(Account account, Address address, string birthdate, string phoneNumber, string firstName, string lastName): this("-1", account, address, birthdate, phoneNumber, firstName, lastName)
        {

        }

        public CampingCustomer(string id, Account account, Address address, string birthdate, string phoneNumber, string firstName, string lastName): base(TableName, ColumnId)
        {
            bool success = int.TryParse(id, out int idNumeric);
            bool successDate = DateTime.TryParse(birthdate, out DateTime dateTime);
            
            this.Id = success ? idNumeric : -1;
            this.Account = account;
            this.Address = address;
            this.Birthdate = successDate ? dateTime : DateTime.MinValue;
            this.PhoneNumber = phoneNumber;
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        public bool Update()
        {
            return base.Update(CampingCustomer.ToDictionary(this.Account, this.Address, this.Birthdate, this.PhoneNumber, this.FirstName, this.LastName));
        }
        
        public bool Update(Account account, Address address, DateTime birthdate, string phoneNumber, string firstName, string lastName)
        {
            this.Account = account;
            this.Address = address;
            this.Birthdate = birthdate;
            this.PhoneNumber = phoneNumber;
            this.FirstName = firstName;
            this.LastName = lastName;

            return base.Update(CampingCustomer.ToDictionary(account, address, birthdate, phoneNumber, firstName, lastName));
        }

        /// <inheritdoc/>
        protected override CampingCustomer ToModel(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                return null;
            }
            
            dictionary.TryGetValue(ColumnId, out string id);
            dictionary.TryGetValue(ColumnBirthdate, out string birthdate);
            dictionary.TryGetValue(ColumnPhoneNumber, out string phoneNumber);
            dictionary.TryGetValue(ColumnFirstName, out string firstName);
            dictionary.TryGetValue(ColumnLastName, out string lastName);
            
            dictionary.TryGetValue(Account.ColumnId, out string accountId);
            dictionary.TryGetValue(Account.ColumnEmail, out string email);
            dictionary.TryGetValue(Account.ColumnPassword, out string password);
            dictionary.TryGetValue(Account.ColumnRights, out string rights);

            dictionary.TryGetValue(Address.ColumnId, out string addressId);
            dictionary.TryGetValue(Address.ColumnAddress, out string street);
            dictionary.TryGetValue(Address.ColumnPostalCode, out string postalCode);
            dictionary.TryGetValue(Address.ColumnPlace, out string place);

            Account account = new Account(accountId, email, password, rights);
            Address address = new Address(addressId, street, postalCode, place);

            return new CampingCustomer(id, account, address, birthdate, phoneNumber, firstName, lastName);
        }

        /// <inheritdoc/>
        protected override Dictionary<string, string> ToDictionary()
        {
            return CampingCustomer.ToDictionary(this.Account, this.Address, this.Birthdate, this.PhoneNumber, this.FirstName, this.LastName);
        }

        private static Dictionary<string, string> ToDictionary(Account account, Address address, DateTime birthdate, string phoneNumber, string firstName, string lastName)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {ColumnAddress, address.Id.ToString()},
                {ColumnBirthdate, birthdate.ToString(CultureInfo.InvariantCulture)},
                {ColumnPhoneNumber, phoneNumber},
                {ColumnFirstName, firstName},
                {ColumnLastName, lastName}
            };

            if (account != null)
            {
                dictionary.Add(ColumnAccount, account.Id.ToString());
            }

            return dictionary;
        }

        /// <inheritdoc/>
        protected override string BaseSelectQuery()
        {
            string query = base.BaseSelectQuery();
            query += $" INNER JOIN {Address.TableName} A ON BT.{ColumnAddress} = A.{Address.ColumnId}";
            query += $" LEFT JOIN {Account.TableName} AC ON BT.{ColumnAccount} = AC.{Account.ColumnId}";

            return query;
        }

        public CampingCustomer SelectByAccount(Account account)
        {
            Query query = new Query(this.BaseSelectQuery() + $" WHERE {ColumnAccount} = @AccountID");
            query.AddParameter("AccountID", account.Id);
            return this.ToModel(query.SelectFirst());
        }

    }
}
