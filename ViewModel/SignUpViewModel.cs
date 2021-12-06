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
    public class SignUpViewModel : ObservableObject
    {
        #region fields
        private string _email,  _password, _singUpError;
        #endregion

        #region properties
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

                if (!CheckEmail(this._email))
                {
                    this.SignUpError = "Ongeldig emailadres";
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

                if (!string.IsNullOrEmpty(this._password))
                {
                    this.SignUpError = "Wachtwoord is verplicht";
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
        public static event EventHandler<SignUpEventArgs> SignUpEvent;
        #endregion

        #region Commands
        private void ExecuteSignUp()
        {
            Account account = new Account();
            account = account.SelectByEmail(this.Email);

            if (account == null || this.Password != account.Password)
            {
                this.SignUpError = "Gegevens onjuist";
                return;
            }

            SignUpEvent?.Invoke(this, new SignUpEventArgs(account));
        }

        private bool CanExecuteSignUp()
        {
            return !string.IsNullOrEmpty(this.Email)  && !string.IsNullOrEmpty(this.Password);
        }

        public ICommand SignUp => new RelayCommand(ExecuteSignUp, CanExecuteSignUp);
        #endregion

        private static bool CheckEmail(string email)
        {
            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", RegexOptions.IgnoreCase);//student@mail.nl
            return regex.IsMatch(email.Trim());
        }
    }
}
