using System.Windows;
using System.Windows.Controls;
using ViewModel;
using ViewModel.EventArguments;

namespace Visualization
{
    /// <summary>
    /// Interaction logic for ReservationCustomerForm.xaml
    /// </summary>
    public partial class ReservationCustomerForm : Page
    {
        public ReservationCustomerForm()
        {
            this.InitializeComponent();
            SignInViewModel.SignInEvent += this.OnSignInEvent;
        }

        private void OnSignInEvent(object sender, AccountEventArgs args)
        {
            if (CurrentUser.Account.Rights == Model.AccountRights.Customer)
            {
                this.EmailLabel.Visibility = Visibility.Visible;
                return;
            }

            this.EmailLabel.Visibility = Visibility.Collapsed;
        }
    }
}