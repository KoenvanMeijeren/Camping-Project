using System.Collections.Generic;
using SystemCore;

namespace Model
{
    /// <inheritdoc/>
    public class CampingOwner : ModelBase<CampingOwner>
    {
        public const string
            TableName = "CampingOwner",
            ColumnId = "CampingOwnerID",
            ColumnAccount = "CampingOwnerAccountID",
            ColumnFirstName = "CampingOwnerFirstName",
            ColumnLastName = "CampingOwnerLastName";
        
        public Account Account { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public CampingOwner(): base(TableName, ColumnId)
        {
        }

        public CampingOwner(Account account, string firstName, string lastName) : this("-1", account, firstName, lastName)
        {
        }

        public CampingOwner(string id, Account account, string firstName, string lastName): base(TableName, ColumnId)
        {
            bool success = int.TryParse(id, out int numericId);

            this.Id = success ? numericId : -1;
            this.Account = account;
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        public bool Update(Account account, string firstName, string lastName)
        {
            this.Account = account;
            this.FirstName = firstName;
            this.LastName = lastName;

            return base.Update(CampingOwner.ToDictionary(account, firstName, lastName));
        }

        /// <inheritdoc/>
        protected override CampingOwner ToModel(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                return null;
            }

            dictionary.TryGetValue(ColumnId, out string id);
            dictionary.TryGetValue(ColumnFirstName, out string firstName);
            dictionary.TryGetValue(ColumnLastName, out string lastName);

            dictionary.TryGetValue(Account.ColumnId, out string accountId);
            dictionary.TryGetValue(Account.ColumnEmail, out string email);
            dictionary.TryGetValue(Account.ColumnPassword, out string password);
            dictionary.TryGetValue(Account.ColumnRights, out string rights);

            Account account = new Account(accountId, email, password, rights);

            return new CampingOwner(id, account, firstName, lastName);
        }

        /// <inheritdoc/>
        protected override Dictionary<string, string> ToDictionary()
        {
            return CampingOwner.ToDictionary(this.Account, this.FirstName, this.LastName);
        }

        private static Dictionary<string, string> ToDictionary(Account account, string firstName, string lastName)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {ColumnAccount, account.Id.ToString()},
                {ColumnFirstName, firstName},
                {ColumnLastName, lastName}
            };

            return dictionary;
        }

        /// <inheritdoc/>
        protected override string BaseSelectQuery()
        {
            string query = base.BaseSelectQuery();
            query += $" INNER JOIN {Account.TableName} AC ON BT.{ColumnAccount} = AC.{Account.ColumnId}";

            return query;
        }

        public CampingOwner SelectByAccount(Account account)
        {
            Query query = new Query(this.BaseSelectQuery() + $" WHERE {ColumnAccount} = @AccountID");
            query.AddParameter("AccountID", account.Id);
            return this.ToModel(query.SelectFirst());
        }
    }
}