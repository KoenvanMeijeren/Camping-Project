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

    /// <summary>
    /// This class defines each chat message sent by the user or owner of the camping.
    /// </summary>
    public class MessageJSON
    {
        public string Message { get; private set; }
        public string MessageSentTime { get; private set; }
        public string UserRole { get; private set; }

        public MessageJSON(string message, string userRole)
        {
            this.Message = message;
            this.MessageSentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.UserRole = userRole;
        }
    }

    public class Chat : ModelBase<Chat>
    {
        public const string
            TableName = "Chat",
            ColumnId = "ChatID",
            ColumnOwnerAccount = "OwnerAccountID",
            ColumnCustomerAccount = "CustomerAccountID",
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
        public int IsSolved { get; private set; }
        public string CustomerName { get; private set; }

        public Chat() : base(TableName, ColumnId)
        {
        }

        public Chat(Account owner, Account customer, string messages, DateTime ownerLastSeen, DateTime customerLastSeen, ChatStatus ownerStatus, ChatStatus customerStatus) : this("-1", owner, customer, messages, ownerLastSeen, customerLastSeen, ownerStatus, customerStatus , "Klant")
        {
        }

        public Chat(string id, Account owner, Account customer, string messages, DateTime ownerLastSeen, DateTime customerLastSeen, ChatStatus ownerStatus, ChatStatus customerStatus, string name) : base(TableName, ColumnId)
        {
            bool success = int.TryParse(id, out int idNumeric);
            this.Id = success ? idNumeric : -1;
            this.Owner = owner;
            this.Customer = customer;
            this.Messages = messages;
            this.LastMessageSeenOwner = ownerLastSeen;
            this.LastMessageSeenCustomer = customerLastSeen;
            this.OwnerStatus = ownerStatus;
            this.CustomerStatus = customerStatus;
            this.CustomerName = name;
            this.IsSolved = 0;
        }

        /// <summary>
        /// Selects the chat of the given user OR creates a new (empty) chat for this user.
        /// </summary>
        /// <param name="customer">Object of the camping user</param>
        /// <returns>All chat data for the given user</returns>
        public Chat SelectOrCreateNewChatForLoggedInUser(CampingCustomer customer)
        {
            Query query = new Query(this.BaseSelectQuery() + $" WHERE {ColumnCustomerAccount} = @campingCustomerId");
            query.AddParameter("campingCustomerId", customer.Account.Id);
            var result = query.SelectFirst();

            // Check chat is already created
            if (result != null)
            {
                return this.ToModel(result);
            }

            CampingOwner campingOwner = new CampingOwner();
            CampingCustomer campingCustomer = new CampingCustomer();

            this.Owner = campingOwner.SelectLast().Account;
            this.Customer = customer.Account;
            // Empty JSON
            this.Messages = "[]";
            this.LastMessageSeenOwner = DateTime.Now;
            this.LastMessageSeenCustomer = DateTime.Now;
            this.OwnerStatus = ChatStatus.Offline;
            this.CustomerStatus = ChatStatus.Offline;

            this.Insert();

            //TODO: Dit later aanpassen, nu geen tijd voor (return this werkte niet)
            Query query2 = new Query(this.BaseSelectQuery() + $" WHERE {ColumnCustomerAccount} = @campingCustomerId");
            query2.AddParameter("campingCustomerId", customer.Account.Id);
            var result2 = query.SelectFirst();

            return this.ToModel(result);
        }

        public string GetChatMessagesForCampingGuest(Account account)
        {
            Query query = new Query($"SELECT {ColumnMessage} FROM {TableName} WHERE {ColumnCustomerAccount} = @campingCustomerId");
            query.AddParameter("campingCustomerId", account.Id);
            var result = query.SelectFirst();

            return result[$"{ColumnMessage}"];
        }

        public bool UpdateChat(string json)
        {
            return base.Update(Chat.ToDictionary(this.Owner, this.Customer, json, this.LastMessageSeenOwner, this.LastMessageSeenCustomer, this.OwnerStatus, this.CustomerStatus, this.IsSolved));
        }

        public bool Update(Account owner, Account customer, string messages, DateTime ownerLastSeen, DateTime customerLastSeen, ChatStatus ownerStatus, ChatStatus customerStatus, int isSolved)
        {
            this.Owner = owner;
            this.Customer = customer;
            this.Messages = messages;
            this.LastMessageSeenOwner = ownerLastSeen;
            this.LastMessageSeenCustomer = customerLastSeen;
            this.OwnerStatus = ownerStatus;
            this.CustomerStatus = customerStatus;

            return base.Update(Chat.ToDictionary(owner, customer, messages, ownerLastSeen, customerLastSeen, ownerStatus, customerStatus, isSolved));
        }

        /// <inheritdoc/>
        protected override Chat ToModel(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                return null;
            }

            dictionary.TryGetValue(ColumnId, out string id);
            dictionary.TryGetValue(ColumnOwnerAccount, out string owner);
            dictionary.TryGetValue(ColumnCustomerAccount, out string customer);
            dictionary.TryGetValue(ColumnMessage, out string messages);
            dictionary.TryGetValue(ColumnLastMessageSeenOwner, out string lastMessageSeenOwner);
            dictionary.TryGetValue(ColumnLastMessageSeenCustomer, out string lastMessageSeenCustomer);
            dictionary.TryGetValue(ColumnOwnerStatus, out string ownerStatus);
            dictionary.TryGetValue(ColumnCustomerStatus, out string customerStatus);
            dictionary.TryGetValue(CampingCustomer.ColumnFirstName, out string firstname);
            dictionary.TryGetValue(CampingCustomer.ColumnLastName, out string lastname);
            string name = firstname + " " + lastname;

            // Fetch owner account
            CampingOwner campingOwnerModel = new();
            CampingOwner campingOwner = campingOwnerModel.SelectLast();

            dictionary.TryGetValue(Account.ColumnId, out string customerAccountId);
            dictionary.TryGetValue(Account.ColumnEmail, out string customerEmail);
            dictionary.TryGetValue(Account.ColumnPassword, out string customerPassword);
            dictionary.TryGetValue(Account.ColumnRights, out string customerRights);
            Account customerAccount = new Account(customerAccountId, customerEmail, customerPassword, customerRights);

            return new Chat(id, campingOwner.Account, customerAccount, messages, DateTimeParser.TryParse(lastMessageSeenOwner), DateTimeParser.TryParse(lastMessageSeenCustomer), (ChatStatus)Int32.Parse(ownerStatus), (ChatStatus)Int32.Parse(customerStatus), name);
        }

        /// <inheritdoc/>
        protected override Dictionary<string, string> ToDictionary()
        {
            return Chat.ToDictionary(this.Owner, this.Customer, this.Messages, this.LastMessageSeenOwner, this.LastMessageSeenCustomer, this.OwnerStatus, this.CustomerStatus, this.IsSolved);
        }

        private static Dictionary<string, string> ToDictionary(Account owner, Account customer, string messages, DateTime ownerLastSeen, DateTime customerLastSeen, ChatStatus ownerStatus, ChatStatus customerStatus, int isSolved)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {ColumnOwnerAccount, owner.Id.ToString()},
                {ColumnCustomerAccount, customer.Id.ToString()},
                {ColumnMessage, messages},
                {ColumnLastMessageSeenOwner, DateTimeParser.TryParseToDatabaseDateTimeFormat(ownerLastSeen)},
                {ColumnLastMessageSeenCustomer, DateTimeParser.TryParseToDatabaseDateTimeFormat(customerLastSeen)},
                {ColumnOwnerStatus, ((int)ownerStatus).ToString()},
                {ColumnCustomerStatus, ((int)customerStatus).ToString()},
                {ColumnIsSolved, isSolved.ToString() }
            };

            return dictionary;
        }

        /// <inheritdoc/>
        protected override string BaseSelectQuery()
        {
            //TODO: kijken welke manier van gegevens ophalen sneller gaat
            //string query = $"SELECT * FROM {TableName} CH ";
            string query = $"SELECT CH.{ColumnId}, CH.{ColumnOwnerAccount}, CH.{ColumnCustomerAccount}, CH.{ColumnMessage}," +
                $" CH.{ColumnLastMessageSeenCustomer}, CH.{ColumnLastMessageSeenOwner}, CH.{ColumnOwnerStatus}, CH.{ColumnCustomerStatus},  CH.{ColumnIsSolved}," +
                $" CC.{CampingCustomer.ColumnFirstName}, CC.{CampingCustomer.ColumnLastName}";
            query += $" FROM {TableName} CH ";

            query += $" LEFT JOIN {Account.TableName} AC on CH.{Chat.ColumnCustomerAccount} = AC.{Account.ColumnId}";
            query += $" INNER JOIN {CampingCustomer.TableName} CC ON AC.{Account.ColumnId} = CC.{CampingCustomer.ColumnAccount}";

            return query;
        }

    }
}
