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
        private static CampingCustomer CampingCustomer { get; set; }
        private static CampingOwner CampingOwner { get; set; }

        public static void SetCurrentUser(Account account)
        {
            Account = account;

            if (Account.Rights == Rights.Customer)
            {
                Account = account;
                CampingCustomer = new CampingCustomer();
                CampingCustomer = CampingCustomer.SelectByAccount(account);
                return;
            }

            if (Account.Rights == Rights.Admin)
            {
                Account = account;
                CampingOwner = new CampingOwner();
                CampingOwner = CampingOwner.SelectByAccount(account);
                return;
            }
        }
    }
}
