using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using SystemCore;
using Model.EventArguments;

namespace Model
{
    public class SignInViewModel : ObservableObject
    {
        #region Fields
        private string _email,  _password, _singInError;
        #endregion

        #region Properties
        public string Email
        {
            get => this._email;
            set
            {
                if (value == this._email)
                { 
                    return; 
                }

                this._email = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

                this.SignInError = "";
                if (!RegexHelper.IsEmailValid(this._email))
                {
                    this.SignInError = "Ongeldig email";
                }
            }
        }

        public string Password
        {
            get => this._password;
            set
            {
                if (value == this._password)
                {
                    return;
                }

                this._password = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

                this.SignInError = "";
                if (!Validation.IsInputFilled(this._password))
                {
                    this.SignInError = "Ongeldig wachtwoord";
                }
            }
        }

        public string SignInError
        {
            get => this._singInError;
            set
            {
                if (value == this._singInError)
                {
                    return;
                }

                this._singInError = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }
        #endregion

        #region Events
        public static event EventHandler<AccountEventArgs> SignInEvent;
        
        /// <summary>
        /// Event for displaying the sign up form.
        /// </summary>
        public static event EventHandler SignUpFormEvent;
        #endregion

        #region Input

        private void ResetInput()
        {
            this.Email = "";
            this.Password = "";
            this.SignInError = "";
        }

        #endregion

        #region Commands
        private void ExecuteSignIn()
        {
            Account account = new Account();
            account = account.SelectByEmail(this.Email);

            if (account == null)
            {
                this.SignInError = "Onjuiste gegevens";
                return;
            }

            if (!PasswordHashing.SignInHashValidation(account.Password, this.Password))
            {
                this.SignInError = "Onjuiste gegevens";
                return;
            }

            switch (account.Rights)
            {
                case AccountRights.Customer:
                    var campingCustomer = new CampingCustomer();
                    
                    CurrentUser.SetCurrentUser(account, campingCustomer.SelectByAccount(account));
                    break;
                case AccountRights.Admin:
                    var campingOwner = new CampingOwner();
                    
                    CurrentUser.SetCurrentUser(account, campingOwner.SelectByAccount(account));
                    break;
            }
            
            SignInEvent?.Invoke(this, new AccountEventArgs(account));
            this.ResetInput();
        }

        private bool CanExecuteSignIn()
        {
            return RegexHelper.IsEmailValid(this.Email) && Validation.IsInputFilled(this.Password);
        }

        private void ExecuteRegister()
        {
            SignUpFormEvent?.Invoke(this, EventArgs.Empty);
        }

        public ICommand SignIn => new RelayCommand(ExecuteSignIn, CanExecuteSignIn);
        public ICommand Register => new RelayCommand(ExecuteRegister);
        #endregion
    }
}
