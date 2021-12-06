using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemCore;

namespace Model
{
    public enum Rights
    {
        Customer,
        Admin
    }

    public class Account : ModelBase<Account>
    {
        public string Email { get; private set; }
        public string Password { get; private set; }
        public Rights Rights { get; private set; }

        public Account()
        {
        }

        public Account(string email, string password, int rights) : this ("-1", email, password, rights)
        {
        }

        public Account(string id, string email, string password, int rights)
        {
            bool success = int.TryParse(id, out int idNumeric);
            
            this.Id = success ? idNumeric : -1;
            this.Email = email;
            this.Password = password;

            if (rights == 0)
            {
                this.Rights = Rights.Customer;
            } 
            else
            {
                this.Rights = Rights.Admin;
            }
        }

        protected override string Table()
        {
            return "Account";
        }

        protected override string PrimaryKey()
        {
            return "AccountID";
        }

        public bool Update(string email, string password, int rights)
        {
            this.Email = email;
            this.Password = password;

            if (rights == 0)
            {
                this.Rights = Rights.Customer;
            }
            else
            {
                this.Rights = Rights.Admin;
            }

            return base.Update(Account.ToDictionary(email, password, rights));
        }

        protected override Account ToModel(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                return null;
            }

            dictionary.TryGetValue("AccountID", out string id);
            dictionary.TryGetValue("AccountEmail", out string email);
            dictionary.TryGetValue("AccountPassword", out string password);
            dictionary.TryGetValue("AccountRights", out string rights);

            return new Account(id, email, password, int.Parse(rights));
        }

        protected override Dictionary<string, string> ToDictionary()
        {
            return Account.ToDictionary(this.Email, this.Password, (int)this.Rights);
        }

        private static Dictionary<string, string> ToDictionary(string email, string password, int rights)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {"AccountUsername", email},
                {"AccountPassword", password},
                {"AccountRights", rights.ToString()}
            };

            return dictionary;
        }

        public Account SelectByEmail(string email)
        {
            Query query = new Query(this.BaseQuery() + " WHERE AccountEmail = @AccountEmail");
            query.AddParameter("AccountEmail", email);
            return this.ToModel(query.SelectFirst());
        }
    }
}
