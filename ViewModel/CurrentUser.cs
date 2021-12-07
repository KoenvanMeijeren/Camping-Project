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
        private static CampingCustomer _campingCustomer;
        private static CampingOwner _campingOwner;

        public static void SetCurrentUser(Account account)
        {
            Account = account;

            switch (Account.AccountRights)
            {
                case AccountRights.Customer:
                    Account = account;
                    _campingCustomer = new CampingCustomer();
                    _campingCustomer = _campingCustomer.SelectByAccount(account);
                    break;
                case AccountRights.Admin:
                    Account = account;
                    _campingOwner = new CampingOwner();
                    _campingOwner = _campingOwner.SelectByAccount(account);
                    break;
            }
        }
    }
}
