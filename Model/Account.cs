using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemCore;

namespace Visualization
{
    /// <summary>
    /// Provides an enum for the various account rights.
    /// </summary>
    public enum AccountRights
    {
        Customer = 0,
        Admin = 1
    }

    /// <inheritdoc/>
    public class Account : ModelBase<Account>
    {
        public const string
            TableName = "Account",
            ColumnId = "AccountID",
            ColumnEmail = "AccountEmail",
            ColumnPassword = "AccountPassword",
            ColumnRights = "AccountRights";
        
        public string Email { get; private set; }
        public string Password { get; private set; }
        public AccountRights Rights { get; private set; }

        public Account(): base(TableName, ColumnId)
        {
        }

        public Account(string email, string password, string rights) : this ("-1", email, password, rights)
        {
        }

        public Account(string id, string email, string password, string rights): base(TableName, ColumnId)
        {
            bool successId = int.TryParse(id, out int idNumeric);
            bool successRights = int.TryParse(rights, out int rightsNumeric);
            
            this.Id = successId ? idNumeric : -1;
            this.Email = email;
            this.Password = password;

            this.Rights = AccountRights.Customer;
            if (successRights && rightsNumeric == (int) AccountRights.Admin)
            {
                this.Rights = AccountRights.Admin;
            }
        }

        public bool Update(string email, string password, int rights)
        {
            this.Email = email;
            this.Password = password;

            this.Rights = AccountRights.Customer;
            if (rights == (int) AccountRights.Admin)
            {
                this.Rights = AccountRights.Admin;
            }

            return base.Update(Account.ToDictionary(email, password, rights));
        }

        /// <inheritdoc/>
        protected override Account ToModel(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                return null;
            }

            dictionary.TryGetValue(ColumnId, out string id);
            dictionary.TryGetValue(ColumnEmail, out string email);
            dictionary.TryGetValue(ColumnPassword, out string password);
            dictionary.TryGetValue(ColumnRights, out string rights);

            return new Account(id, email, password, rights);
        }

        /// <inheritdoc/>
        protected override Dictionary<string, string> ToDictionary()
        {
            return Account.ToDictionary(this.Email, this.Password, (int) this.Rights);
        }

        private static Dictionary<string, string> ToDictionary(string email, string password, int rights)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {ColumnEmail, email},
                {ColumnPassword, password},
                {ColumnRights, rights.ToString()}
            };

            return dictionary;
        }

        public Account SelectByEmail(string email)
        {
            Query query = new Query(this.BaseSelectQuery() + $" WHERE {ColumnEmail} = @{ColumnEmail}");
            query.AddParameter(ColumnEmail, email);
            return this.ToModel(query.SelectFirst());
        }
    }
}
