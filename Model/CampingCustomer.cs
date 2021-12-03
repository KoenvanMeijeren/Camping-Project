﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CampingCustomer : ModelBase<CampingCustomer>
    {
        public Account Account { get; private set; }
        public Address Address { get; private set; }
        public DateTime Birthdate { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber  { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        
        public CampingCustomer()
        {
            
        }

        public CampingCustomer(Account account, Address address, string birtdate, string email, string phoneNumber, string firstName, string lastName): this("-1", account, address, birtdate, email, phoneNumber, firstName, lastName)
        {

        }

        public CampingCustomer(string id, Account account, Address address, string birthdate, string email, string phoneNumber, string firstName, string lastName)
        {
            bool success = int.TryParse(id, out int idNumeric);
            bool successDate = DateTime.TryParse(birthdate, out DateTime dateTime);
            
            this.Id = success ? idNumeric : -1;
            this.Account = account;
            this.Address = address;
            this.Birthdate = successDate ? dateTime : DateTime.MinValue;
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

        public bool Update(Account account, Address address, DateTime birthdate, string email, string phoneNumber, string firstName, string lastName)
        {
            this.Account = account;
            this.Address = address;
            this.Birthdate = birthdate;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            this.FirstName = firstName;
            this.LastName = lastName;

            return base.Update(CampingCustomer.ToDictionary(account, address, birthdate, email, phoneNumber, firstName, lastName));
        }

        protected override CampingCustomer ToModel(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                return null;
            }
            
            dictionary.TryGetValue("CampingCustomerID", out string id);

            dictionary.TryGetValue("AccountID", out string accountId);
            dictionary.TryGetValue("AccountUsername", out string username);
            dictionary.TryGetValue("AccountPassword", out string password);
            dictionary.TryGetValue("AccountRights", out string rights);

            dictionary.TryGetValue("AddressID", out string addressId);
            dictionary.TryGetValue("Address", out string street);
            dictionary.TryGetValue("AddressPostalCode", out string postalCode);
            dictionary.TryGetValue("AddressPlace", out string place);

            dictionary.TryGetValue("CampingCustomerBirthdate", out string birthdate);
            dictionary.TryGetValue("CampingCustomerEmail", out string email);
            dictionary.TryGetValue("CampingCustomerPhoneNumber", out string phoneNumber);
            dictionary.TryGetValue("CampingCustomerFirstName", out string firstName);
            dictionary.TryGetValue("CampingCustomerLastName", out string lastName);

            Account account = new Account(accountId, username, password, int.Parse(rights));
            Address address = new Address(addressId, street, postalCode, place);

            return new CampingCustomer(id, account, address, birthdate, email, phoneNumber, firstName, lastName);
        }

        protected override Dictionary<string, string> ToDictionary()
        {
            return CampingCustomer.ToDictionary(this.Account, this.Address, this.Birthdate, this.Email, this.PhoneNumber, this.FirstName, this.LastName);
        }

        private static Dictionary<string, string> ToDictionary(Account account, Address address, DateTime birthdate, string email, string phoneNumber, string firstName, string lastName)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {"CampingCustomerAccountID", account.Id.ToString()},
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
            query += " INNER JOIN Account AC ON BT.CampingCustomerAccountID = AC.AccountID";

            return query;
        }
        
    }
}
