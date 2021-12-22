using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemCore;

namespace Model
{
    public enum ChatStatus
    {
        Offline = 0,
        Online = 1,
        Typing = 2
    }

    public class Chat : ModelBase<Chat>
    {
        public const string
            TableName = "Chat",
            ColumnId = "ChatID",
            ColumnOwner = "OwnerID",
            ColumnCustomer = "CustomerID",
            ColumnMessage = "Messages",
            ColumnLastMessageSeenOwner = "OwnerLastSeen",
            ColumnLastMessageSeenCustomer = "CustomerLastSeen",
            ColumnCustomerStatus = "CustomerStatus",
            ColumnOwnerStatus = "OwnerStatus",
            ColumnIsSolved = "IsSolved";

        public Account Owner { get; private set; }
        public Account Customer { get; private set; }
        public string Messages { get; private set; }
        public DateTime LastMessageSeenOwner { get; private set; }
        public DateTime LastMessageSeenCustomer { get; private set; }
        public ChatStatus OwnerStatus { get; private set; }
        public ChatStatus CustomerStatus { get; private set; }

        public Chat() : base(TableName, ColumnId)
        {
        }

        public Chat(Account owner, Account customer, string messages, DateTime ownerLastSeen, DateTime customerLastSeen, ChatStatus ownerStatus, ChatStatus customerStatus) : this("-1", owner, customer, messages, ownerLastSeen, customerLastSeen, ownerStatus, customerStatus)
        {
        }

        public Chat(string id, Account owner, Account customer, string messages, DateTime ownerLastSeen, DateTime customerLastSeen, ChatStatus ownerStatus, ChatStatus customerStatus) : base(TableName, ColumnId)
        {
            this.Owner = owner;
            this.Customer = customer;
            this.Messages = messages;
            this.LastMessageSeenOwner = ownerLastSeen;
            this.LastMessageSeenCustomer = customerLastSeen;
            this.OwnerStatus = ownerStatus;
            this.CustomerStatus = customerStatus;
        }

        public bool Update(Account owner, Account customer, string messages, DateTime ownerLastSeen, DateTime customerLastSeen, ChatStatus ownerStatus, ChatStatus customerStatus)
        {
            this.Owner = owner;
            this.Customer = customer;
            this.Messages = messages;
            this.LastMessageSeenOwner = ownerLastSeen;
            this.LastMessageSeenCustomer = customerLastSeen;
            this.OwnerStatus = ownerStatus;
            this.CustomerStatus = customerStatus;

            return base.Update(Chat.ToDictionary(owner, customer, messages, ownerLastSeen, customerLastSeen, ownerStatus, customerStatus));
        }

        /// <inheritdoc/>
        protected override Chat ToModel(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                return null;
            }

            dictionary.TryGetValue(ColumnId, out string id);
            dictionary.TryGetValue(ColumnOwner, out string owner);
            dictionary.TryGetValue(ColumnCustomer, out string customer);
            dictionary.TryGetValue(ColumnMessage, out string messages);
            dictionary.TryGetValue(ColumnLastMessageSeenOwner, out string lastMessageSeenOwner);
            dictionary.TryGetValue(ColumnLastMessageSeenCustomer, out string lastMessageSeenCustomer);
            dictionary.TryGetValue(ColumnOwnerStatus, out string ownerStatus);
            dictionary.TryGetValue(ColumnCustomerStatus, out string customerStatus);

            dictionary.TryGetValue(Account.ColumnId, out string ownerAccountId);
            dictionary.TryGetValue(Account.ColumnEmail, out string ownerEmail);
            dictionary.TryGetValue(Account.ColumnPassword, out string ownerPassword);
            dictionary.TryGetValue(Account.ColumnRights, out string ownerRights);
            Account ownerAccount = new Account(ownerAccountId, ownerEmail, ownerPassword, ownerRights);

            dictionary.TryGetValue(Account.ColumnId, out string customerAccountId);
            dictionary.TryGetValue(Account.ColumnEmail, out string customerEmail);
            dictionary.TryGetValue(Account.ColumnPassword, out string customerPassword);
            dictionary.TryGetValue(Account.ColumnRights, out string customerRights);
            Account customerAccount = new Account(customerAccountId, customerEmail, customerPassword, customerRights);

            return new Chat(id, ownerAccount, customerAccount, messages, DateTimeParser.TryParse(lastMessageSeenOwner), DateTimeParser.TryParse(lastMessageSeenCustomer), (ChatStatus)Int32.Parse(ownerStatus), (ChatStatus)Int32.Parse(customerStatus));
        }

        /// <inheritdoc/>
        protected override Dictionary<string, string> ToDictionary()
        {
            return Chat.ToDictionary(this.Owner, this.Customer, this.Messages, this.LastMessageSeenOwner, this.LastMessageSeenCustomer, this.OwnerStatus, this.CustomerStatus);
        }

        private static Dictionary<string, string> ToDictionary(Account owner, Account customer, string messages, DateTime ownerLastSeen, DateTime customerLastSeen, ChatStatus ownerStatus, ChatStatus customerStatus)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {ColumnCustomer, owner.Id.ToString()},
                {ColumnCustomer, customer.Id.ToString()},
                {ColumnMessage, messages},
                {ColumnLastMessageSeenOwner, DateTimeParser.TryParseToDatabaseDateTimeFormat(ownerLastSeen)},
                {ColumnLastMessageSeenCustomer, DateTimeParser.TryParseToDatabaseDateTimeFormat(customerLastSeen)},
                {ColumnOwnerStatus, ownerStatus.ToString()},
                {ColumnCustomerStatus, customerStatus.ToString()}
            };

            return dictionary;
        }

        /// <inheritdoc/>
        protected override string BaseSelectQuery()
        {
            string query = $"SELECT * FROM {TableName} R ";
            query += $" LEFT JOIN {Account.TableName} AC on CC.{Chat.ColumnOwner} = AC.{Account.ColumnId}";
            query += $" LEFT JOIN {Account.TableName} AC on CC.{Chat.ColumnCustomer} = AC.{Account.ColumnId}";

            return query;
        }
    }
}
