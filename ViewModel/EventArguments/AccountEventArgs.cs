using System;
using Model;

namespace Model.EventArguments
{
    public class AccountEventArgs : EventArgs
    {
        public Account Account { get; private set; }

        public AccountEventArgs(Account account)
        {
            this.Account = account;
        }
    }
}
