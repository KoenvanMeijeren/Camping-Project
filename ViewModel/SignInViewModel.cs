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

namespace ViewModel
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
        /// <summary>
        /// Check if string is a Base64 string. Used to check if database password is a base64 string.
        /// </summary>
        /// <param name="base64">database account password</param>
        /// <returns>Boolean value for given question</returns>
        public  bool IsBase64String(string base64)
        {
            Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
            return Convert.TryFromBase64String(base64, buffer, out int bytesParsed);
        }

        private void ExecuteSignIn()
        {
            Account account = new Account();
            account = account.SelectByEmail(this.Email);

            if (account == null)
            {
                this.SignInError = "Onjuiste gegevens";
                return;
            }

            if (!IsBase64String(account.Password))
            {
                this.SignInError = "Wachtwoord onjuist, neem contact op met de camping";
                return;

            }
            // Extract the bytes
            byte[] hashBytes = Convert.FromBase64String(account.Password);

            // Get the salt
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            // Compute the hash on the password the user entered
            var pbkdf2 = new Rfc2898DeriveBytes(this.Password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);

            // Compare the results
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    this.SignInError = "Onjuiste gegevens";
                    return;
                }
            }

            CurrentUser.SetCurrentUser(account);
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
