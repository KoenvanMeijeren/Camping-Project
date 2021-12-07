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
        private string _name , _birthdate, _street, _address, _mail, _phoneNumber;
        private Account CurrentAccount;

        #region properties
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
        #endregion

        public AccountViewModel()
        {
            CurrentUser.CurrentUserSetEvent += OnCurrentUserSetEvent;
        }

        public void OnCurrentUserSetEvent(object sender, EventArgs e)
        {
            this.CurrentAccount = CurrentUser.Account;

            if (CurrentAccount.Rights == Rights.Admin)
            {
                this.Name = CurrentUser.CampingOwner.FirstName + " " + CurrentUser.CampingOwner.LastName;
                this.Mail = CurrentUser.CampingOwner.Account.Email;
                this.PhoneNumber = "";
                this.Birthdate = "";
                this.Street = "";
                this.Address = "";
            }
            else
            {
                this.Name = CurrentUser.CampingCustomer.FirstName + " " + CurrentUser.CampingCustomer.LastName;
                this.Mail = CurrentUser.CampingCustomer.Account.Email;
                this.PhoneNumber = CurrentUser.CampingCustomer.PhoneNumber;
                this.Birthdate = CurrentUser.CampingCustomer.Birthdate.ToShortDateString();
                this.Street = CurrentUser.CampingCustomer.Address.Street;
                this.Address = CurrentUser.CampingCustomer.Address.PostalCode + " " + CurrentUser.CampingCustomer.Address.Place;
            }
        }

        public ICommand SignOut => new RelayCommand(ExecuteSignOut);

        private void ExecuteSignOut()
        {
            this.Name = "";
            this.Mail = "";
            this.PhoneNumber = "";
            this.Birthdate = "";
            this.Street = "";
            this.Address = "";

            SignOutEvent?.Invoke(null, new EventArgs());
        }
    }
}
