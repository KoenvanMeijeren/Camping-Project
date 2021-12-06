using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class SignUpEventArgs : EventArgs
    {
        public Account Account { get; private set; }

        public SignUpEventArgs(Account account)
        {
            this.Account = account;
        }
    }
}
