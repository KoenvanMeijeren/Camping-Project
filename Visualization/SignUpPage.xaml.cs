using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ViewModel;

namespace Visualization
{
    /// <summary>
    /// Interaction logic for SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : Page
    {
        public static event EventHandler<SignUpEventArgs> SignUpEvent;

        public SignUpPage()
        {
            this.InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EmailTextbox_Selected(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (this.EmailTextbox.Text == "Voer hier uw email in")
            {
                this.EmailTextbox.Foreground = Brushes.Black;
                this.EmailTextbox.Text = "";
            }
        }

        private void EmailTextbox_Deselected(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (this.EmailTextbox.Text == "")
            {
                this.EmailTextbox.Foreground = Brushes.Gray;
                this.EmailTextbox.Text = "Voer hier uw email in";
            }
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.EmailTextbox.Text == "Voer hier uw email in" || this.PasswordTextbox.Password == "")
            {
                this.ErrorText.Content = "Vul alle velden in";
                return;
            }

            if (!CheckEmail(this.EmailTextbox.Text))
            {
                this.ErrorText.Content = "Ongeldig mailadres";
                return;
            }

            Account account = new Account();
            account = account.SelectByEmail(this.EmailTextbox.Text);

            if (account == null || this.PasswordTextbox.Password != account.Password)
            {
                this.ErrorText.Content = "Gegevens onjuist";
                return;
            }

            SignUpEvent?.Invoke(this, new SignUpEventArgs(account));
        }

        private static bool CheckEmail(string email)
        {
            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", RegexOptions.IgnoreCase);//student@mail.nl
            return regex.IsMatch(email.Trim());
        }
    }
}
