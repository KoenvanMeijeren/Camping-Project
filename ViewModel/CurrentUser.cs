using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    /// <summary>
    /// The current user of the application.
    /// </summary>
    public static class CurrentUser
    {
        #region Properties
        public static Account Account { get; private set; }
        public static CampingCustomer CampingCustomer { get; private set; }
        public static CampingOwner CampingOwner { get; private set; }
        #endregion

        #region Events
        public static event EventHandler CurrentUserSetEvent;
        #endregion

        #region Input
        /// <summary>
        /// Empties the current user, usually done on log out.
        /// </summary>
        public static void EmptyCurrentUser()
        {
            CurrentUser.Account = null;
            CurrentUser.CampingCustomer = null;
            CurrentUser.CampingOwner = null;
        }
        #endregion

        #region Commands
        /// <summary>
        /// Sets the current camping customer user.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="campingCustomer">The camping customer.</param>
        public static void SetCurrentUser(Account account, CampingCustomer campingCustomer)
        {
            CurrentUser.Account = account;
            CurrentUser.CampingCustomer = campingCustomer;

            CurrentUserSetEvent?.Invoke(null, EventArgs.Empty);
        }
        
        /// <summary>
        /// Sets the current camping owner user.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="campingOwner">The camping owner.</param>
        public static void SetCurrentUser(Account account, CampingOwner campingOwner)
        {
            CurrentUser.Account = account;
            CurrentUser.CampingOwner = campingOwner;

            CurrentUserSetEvent?.Invoke(null, EventArgs.Empty);
        }
        #endregion
    }
}
