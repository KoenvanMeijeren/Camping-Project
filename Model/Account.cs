using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Account : ModelBase<Account>
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public int Rights { get; private set; }

        public Account()
        {
        }

        public Account(string username, string password, int rights) : this ("-1", username, password, rights)
        {
        }

        public Account(string id, string username, string password, int rights)
        {
            this.Id = int.Parse(id);
            this.Username = username;
            this.Password = password;
            this.Rights = rights;
        }

        protected override string Table()
        {
            return "Account";
        }

        protected override string PrimaryKey()
        {
            return "AccountID";
        }

        public bool Update(string username, string password, int rights)
        {
            this.Username = username;
            this.Password = password;
            this.Rights = rights;

            return base.Update(Account.ToDictionary(username, password, rights));
        }

        protected override Account ToModel(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                return null;
            }

            dictionary.TryGetValue("AccountID", out string id);
            dictionary.TryGetValue("AccountUsername", out string username);
            dictionary.TryGetValue("AccountPassword", out string password);
            dictionary.TryGetValue("AccountRights", out string rights);

            return new Account(id, username, password, int.Parse(rights));
        }

        protected override Dictionary<string, string> ToDictionary()
        {
            return Account.ToDictionary(this.Username, this.Password, this.Rights);
        }

        private static Dictionary<string, string> ToDictionary(string username, string password, int rights)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {"AccountUsername", username},
                {"AccountPassword", password},
                {"AccountRights", rights.ToString()}
            };

            return dictionary;
        }
    }
}
