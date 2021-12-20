using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Visualization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Visualization
{
    public class AccountViewModel : ObservableObject
    {
        #region Fields
        
        private string _name , _birthdate, _street, _address, _mail, _phoneNumber;
        private Account _currentAccount;

        #endregion
        
        #region Properties
        public string Name
        {
            get => this._name;
            set
            {
                if (value == this._name)
                {
                    return;
                }

                this._name = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string Birthdate
        {
            get => this._birthdate;
            set
            {
                if (value == this._birthdate)
                {
                    return;
                }

                this._birthdate = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string Street
        {
            get => this._street;
            set
            {
                if (value == this._street)
                {
                    return;
                }

                this._street = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string Address
        {
            get => this._address;
            set
            {
                if (value == this._address)
                {
                    return;
                }

                this._address = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string Mail
        {
            get => this._mail;
            set
            {
                if (value == this._mail)
                {
                    return;
                }

                this._mail = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string PhoneNumber
        {
            get => this._phoneNumber;
            set
            {
                if (value == this._phoneNumber)
                {
                    return;
                }

                this._phoneNumber = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }
        #endregion

        #region Events
        public static event EventHandler SignOutEvent;
        public static event EventHandler ToAccountUpdatePageEvent;
        #endregion

        #region View construction
        
        public AccountViewModel()
        {
            CurrentUser.CurrentUserSetEvent += OnCurrentUserSetEvent;
            AccountUpdateViewModel.UpdateConfirmEvent += OnUpdateConfirmEvent;
        }

        private void SetOverview()
        {
            this._currentAccount = CurrentUser.Account;

            if (this._currentAccount.Rights == AccountRights.Admin && CurrentUser.CampingOwner != null)
            {
                this.Name = CurrentUser.CampingOwner.FirstName + " " + CurrentUser.CampingOwner.LastName;
                this.Mail = CurrentUser.CampingOwner.Account.Email;
                this.PhoneNumber = "";
                this.Birthdate = "";
                this.Street = "";
                this.Address = "";
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
        }

        private void OnCurrentUserSetEvent(object sender, EventArgs e)
        {
            this.SetOverview();
        }

        private void OnUpdateConfirmEvent(object sender, EventArgs e)
        {
            this.SetOverview();
        }
        
        #endregion

        #region Commands

        public ICommand SignOut => new RelayCommand(ExecuteSignOut);
        public ICommand ToUpdate => new RelayCommand(ExecuteToUpdate);

        private void ExecuteSignOut()
        {
            this.Name = "";
            this.Mail = "";
            this.PhoneNumber = "";
            this.Birthdate = "";
            this.Street = "";
            this.Address = "";

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
