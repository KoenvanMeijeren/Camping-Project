using System.Collections.Generic;
using SystemCore;

namespace Model
{
    public class CampingOwner : ModelBase<CampingOwner>
    {
        public Account Account { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public CampingOwner()
        {
        }

        public CampingOwner(Account account, string firstName, string lastName) : this("-1", account, firstName, lastName)
        {
        }

        public CampingOwner(string id, Account account, string firstName, string lastName)
        {
            this.Id = int.Parse(id);
            this.Account = account;
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        protected override string Table()
        {
            return "CampingOwner";
        }

        protected override string PrimaryKey()
        {
            return "CampingOwnerID";
        }

        public bool Update(Account account, string firstName, string lastName)
        {
            this.Account = account;
            this.FirstName = firstName;
            this.LastName = lastName;

            return base.Update(CampingOwner.ToDictionary(account, firstName, lastName));
        }

        protected override CampingOwner ToModel(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                return null;
            }

            dictionary.TryGetValue("CampingOwnerID", out string id);

            dictionary.TryGetValue("AccountID", out string accountId);
            dictionary.TryGetValue("AccountEmail", out string email);
            dictionary.TryGetValue("AccountPassword", out string password);
            dictionary.TryGetValue("AccountRights", out string rights);

            dictionary.TryGetValue("CampingOwnerFirstName", out string firstName);
            dictionary.TryGetValue("CampingOwnerLastName", out string lastName);

            Account account = new Account(accountId, email, password, int.Parse(rights));


            return new CampingOwner(id, account, firstName, lastName);
        }

        protected override Dictionary<string, string> ToDictionary()
        {
            return CampingOwner.ToDictionary(this.Account, this.FirstName, this.LastName);
        }

        private static Dictionary<string, string> ToDictionary(Account account, string firstName, string lastName)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {"CampingCustomerAccountID", account.Id.ToString()},
                {"CampingOwnerFirstName", firstName},
                {"CampingOwnerLastName", lastName}
            };

            return dictionary;
        }

        protected override string BaseQuery()
        {
            string query = base.BaseQuery();
            query += " INNER JOIN Account AC ON BT.CampingOwnerAccountID = AC.AccountID";

            return query;
        }

        public CampingOwner SelectByAccount(Account account)
        {
            Query query = new Query(this.BaseQuery() + " WHERE CampingOwnerAccountID = @AccountID");
            query.AddParameter("AccountID", account.Id);
            return this.ToModel(query.SelectFirst());
        }
    }
}