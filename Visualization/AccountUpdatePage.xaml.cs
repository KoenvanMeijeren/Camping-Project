using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using ViewModel.EventArguments;

namespace Visualization
{
    /// <summary>
    /// Interaction logic for AccountUpdatePage.xaml
    /// </summary>
    public partial class AccountUpdatePage : Page
    {
        public AccountUpdatePage()
        {
            InitializeComponent();
            SignInViewModel.SignInEvent += this.OnSignInEvent;
        }

        private void OnSignInEvent(object sender, AccountEventArgs args)
        {
            if (CurrentUser.Account.Rights == Model.AccountRights.Customer)
            {
                this.BirthDateLabel.Visibility = Visibility.Visible;
                this.PhonenumberLabel.Visibility = Visibility.Visible;
                this.StreetLabel.Visibility = Visibility.Visible;
                this.PostalcodeLabel.Visibility = Visibility.Visible;
                this.PlaceLabel.Visibility = Visibility.Visible;

                this.CustomerBirthDate.Visibility = Visibility.Visible;
                this.PhonenumberTextbox.Visibility = Visibility.Visible;
                this.StreetTextbox.Visibility = Visibility.Visible;
                this.PostalcodeTextbox.Visibility = Visibility.Visible;
                this.PlaceTextbox.Visibility = Visibility.Visible;
                return;
            }

            this.BirthDateLabel.Visibility = Visibility.Collapsed;
            this.PhonenumberLabel.Visibility = Visibility.Collapsed;
            this.StreetLabel.Visibility = Visibility.Collapsed;
            this.PostalcodeLabel.Visibility = Visibility.Collapsed;
            this.PlaceLabel.Visibility = Visibility.Collapsed;

            this.CustomerBirthDate.Visibility = Visibility.Collapsed;
            this.PhonenumberTextbox.Visibility = Visibility.Collapsed;
            this.StreetTextbox.Visibility = Visibility.Collapsed;
            this.PostalcodeTextbox.Visibility = Visibility.Collapsed;
            this.PlaceTextbox.Visibility = Visibility.Collapsed;
        }
    }
}
