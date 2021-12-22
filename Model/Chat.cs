﻿using System;
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
            ColumnOwnerAccount = "OwnerAccountID",
            ColumnCustomerAccount = "CustomerAccountID",
            ColumnMessage = "Messages",
            ColumnLastMessageSeenOwner = "OwnerLastSeen",
            ColumnLastMessageSeenCustomer = "CustomerLastSeen",
            ColumnCustomerStatus = "CustomerStatus",
            ColumnOwnerStatus = "OwnerStatus";

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
            bool success = int.TryParse(id, out int idNumeric);
            this.Id = success ? idNumeric : -1;
            this.Owner = owner;
            this.Customer = customer;
            this.Messages = messages;
            this.LastMessageSeenOwner = ownerLastSeen;
            this.LastMessageSeenCustomer = customerLastSeen;
            this.OwnerStatus = ownerStatus;
            this.CustomerStatus = customerStatus;
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

            // Check there isn't a chat created yet
            if (result == null)
            {
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
                return this;
            }

            return this.ToModel(result);
        }

        public bool UpdateChat(string json)
        {
            return base.Update(Chat.ToDictionary(this.Owner, this.Customer, json, this.LastMessageSeenOwner, this.LastMessageSeenCustomer, this.OwnerStatus, this.CustomerStatus));
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
            dictionary.TryGetValue(ColumnOwnerAccount, out string owner);
            dictionary.TryGetValue(ColumnCustomerAccount, out string customer);
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
                {ColumnOwnerAccount, owner.Id.ToString()},
                {ColumnCustomerAccount, customer.Id.ToString()},
                {ColumnMessage, messages},
                {ColumnLastMessageSeenOwner, DateTimeParser.TryParseToDatabaseDateTimeFormat(ownerLastSeen)},
                {ColumnLastMessageSeenCustomer, DateTimeParser.TryParseToDatabaseDateTimeFormat(customerLastSeen)},
                {ColumnOwnerStatus, ((int)ownerStatus).ToString()},
                {ColumnCustomerStatus, ((int)customerStatus).ToString()}
            };

            return dictionary;
        }

        /// <inheritdoc/>
        protected override string BaseSelectQuery()
        {
            string query = $"SELECT * FROM {TableName} CH ";
            query += $" LEFT JOIN {Account.TableName} AC on CH.{Chat.ColumnOwnerAccount} = AC.{Account.ColumnId}";
            query += $" LEFT JOIN {Account.TableName} AC2 on CH.{Chat.ColumnCustomerAccount} = AC2.{Account.ColumnId}";

            return query;
        }
    }
}
