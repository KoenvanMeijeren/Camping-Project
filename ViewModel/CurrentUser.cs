using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public static class CurrentUser
    {
        public static Account Account { get; private set; }
        public static CampingCustomer CampingCustomer;
        public static CampingOwner CampingOwner;

        public static event EventHandler CurrentUserSetEvent;

        public static void SetCurrentUser(Account account)
        {
            Account = account;

            switch (Account.Rights)
            {
                case Rights.Customer:
                    Account = account;
                    CampingCustomer = new CampingCustomer();
                    CampingCustomer = CampingCustomer.SelectByAccount(account);
                    break;
                case Rights.Admin:
                    Account = account;
                    CampingOwner = new CampingOwner();
                    CampingOwner = CampingOwner.SelectByAccount(account);
                    break;
            }

            CurrentUserSetEvent?.Invoke(null, new EventArgs());
        }

        public static void EmptyCurrentUser()
        {
            Account = null;
            CampingCustomer = null;
            CampingOwner = null;
        }
    }
}
