using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModel
{
    public class AccountViewModel : ObservableObject
    {
        #region Fields

        private Account _currentAccount;

        #endregion
        
        #region Properties
        public string Name { get; private set; }

        public string Birthdate { get; private set; }

        public string Street { get; private set; }

        public string Address { get; private set; }

        public string Mail { get; private set; }

        public string PhoneNumber { get; private set; }

        #endregion

        #region Events
        public static event EventHandler SignOutEvent;
        public static event EventHandler ToAccountUpdatePageEvent;
        #endregion

        #region View construction
        
        public AccountViewModel()
        {
            CurrentUser.SetCurrentUserEvent += OnSetCurrentUserEvent;
            AccountUpdateViewModel.UpdateConfirmEvent += OnUpdateConfirmEvent;
        }

        private void SetOverview()
        {
            this._currentAccount = CurrentUser.Account;
            this.ResetInput();

            if (this._currentAccount.Rights == AccountRights.Admin && CurrentUser.CampingOwner != null)
            {
                this.Name = CurrentUser.CampingOwner.FirstName + " " + CurrentUser.CampingOwner.LastName;
                this.Mail = CurrentUser.CampingOwner.Account.Email;
            }
            else if (CurrentUser.CampingCustomer != null)
            {
                this.Name = CurrentUser.CampingCustomer.FirstName + " " + CurrentUser.CampingCustomer.LastName;
                this.Mail = CurrentUser.CampingCustomer.Account.Email;
                this.PhoneNumber = CurrentUser.CampingCustomer.PhoneNumber;
                this.Birthdate = CurrentUser.CampingCustomer.Birthdate.ToShortDateString();
                this.Street = CurrentUser.CampingCustomer.Address.Street;
                this.Address = CurrentUser.CampingCustomer.Address.PostalCode + " " + CurrentUser.CampingCustomer.Address.Place;
            }
            
            this.OnPropertyChanged(new PropertyChangedEventArgs(null));
        }

        private void OnSetCurrentUserEvent(object sender, EventArgs e)
        {
            this.SetOverview();
        }

        private void OnUpdateConfirmEvent(object sender, EventArgs e)
        {
            this.SetOverview();
        }

        private void ResetInput()
        {
            this.Name = string.Empty;
            this.Mail = string.Empty;
            this.PhoneNumber = string.Empty;
            this.Birthdate = string.Empty;
            this.Street = string.Empty;
            this.Address = null;
            
            this.OnPropertyChanged(new PropertyChangedEventArgs(null));
        }
        
        #endregion

        #region Commands

        public ICommand SignOut => new RelayCommand(ExecuteSignOut);
        public ICommand ToUpdate => new RelayCommand(ExecuteToUpdate);

        private void ExecuteSignOut()
        {
            CurrentUser.EmptyCurrentUser();
            SignOutEvent?.Invoke(this, EventArgs.Empty);
        }

        private void ExecuteToUpdate()
        {
            ToAccountUpdatePageEvent?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
