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
using SystemCore;

namespace ViewModel
{
    public class SignInViewModel : ObservableObject
    {
        #region Fields
        private string _email,  _password, _singUpError;
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

                this.SignUpError = "";
                if (!RegexHelper.IsEmailValid(this._email))
                {
                    this.SignUpError = "Ongeldig email";
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

                this.SignUpError = "";
                if (!Validation.IsInputFilled(this._password))
                {
                    this.SignUpError = "Ongeldig wachtwoord";
                }
            }
        }

        public string SignUpError
        {
            get => this._singUpError;
            set
            {
                if (value == this._singUpError)
                {
                    return;
                }

                this._singUpError = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }
        #endregion

        #region Events
        public static event EventHandler<AccountEventArgs> SignUpEvent;
        public static event EventHandler RegisterEvent;
        #endregion

        #region Input

        private void ResetInput()
        {
            this.Email = "";
            this.Password = "";
            this.SignUpError = "";
        }

        #endregion
        
        #region Commands
        private void ExecuteSignUp()
        {
            Account account = new Account();
            account = account.SelectByEmail(this.Email);

            if (account == null || this.Password != account.Password)
            {
                this.SignUpError = "Onjuiste gegevens";
                return;
            }

            CurrentUser.SetCurrentUser(account);
            SignUpEvent?.Invoke(this, new AccountEventArgs(account));
            this.ResetInput();
        }

        private bool CanExecuteSignUp()
        {
            return RegexHelper.IsEmailValid(this.Email) && Validation.IsInputFilled(this.Password);
        }

        private void ExecuteRegister()
        {
            RegisterEvent?.Invoke(this, new EventArgs());
        }

        public ICommand SignUp => new RelayCommand(ExecuteSignUp, CanExecuteSignUp);
        public ICommand Register => new RelayCommand(ExecuteRegister);
        #endregion
    }
}
