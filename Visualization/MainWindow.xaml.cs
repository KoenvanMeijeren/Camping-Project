using System;
using System.Windows;
using System.Windows.Media;
using ViewModel;

namespace Visualization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private readonly CampingPlacesCollectionPage _campingPlacesCollectionFrame;
        private readonly ReservationCollectionPage _reservationCollectionFrame;
        private readonly ReservationCustomerForm _reservationCustomerForm;
        private readonly ReservationConfirmedPage _reservationConfirmedPage;
        private readonly ReservationCampingGuestPage _reservationCampingGuestPage;
        private readonly AccountPage _accountPage;
        private readonly SignInPage _signInPage;
        private readonly SignUpPage _signUpPage;
        private readonly ReservationUpdateDeletePage _manageReservationPage;
        private readonly ReservationOverviewPage _reservationOverviewPage;

        public MainWindow()
        {
            this.InitializeComponent();
            
            this._campingPlacesCollectionFrame = new CampingPlacesCollectionPage();
            this._reservationCustomerForm = new ReservationCustomerForm();
            this._reservationCollectionFrame = new ReservationCollectionPage();
            this._reservationConfirmedPage = new ReservationConfirmedPage();
            this._reservationCampingGuestPage = new ReservationCampingGuestPage();
            this._accountPage = new AccountPage();
            this._signInPage = new SignInPage();
            this._signUpPage = new SignUpPage();
            this._manageReservationPage = new ReservationUpdateDeletePage();
            this._reservationOverviewPage = new ReservationOverviewPage();

            ReservationCampingGuestViewModel.ReservationConfirmedEvent += this.OnReservationConfirmedEvent;
            ReservationCampingGuestViewModel.ReservationGoBackEvent += this.OnReserveEvent;
            ReservationCustomerFormViewModel.ReservationGuestEvent += this.OnReservationGuestsFormEvent;
            ReservationCampingPlaceFormViewModel.ReserveEvent += this.OnReserveDurationEvent;
            SignUpViewModel.SignUpEvent += this.OnSignUpEvent;
            AccountViewModel.SignOutEvent += this.OnSignOutEvent;
            SignInViewModel.SignInEvent += this.OnSignInEvent;
            SignInViewModel.SignUpFormEvent += this.OnSignUpFormEvent;
            ReservationCollectionViewModel.ManageReservationEvent += this.OnManageReservationEvent;
            ManageReservationViewModel.FromReservationBackToDashboardEvent += this.OnBackToDashboardEvent;
            
            // Sets the sign up page as the active menu and hides other menu items.
            this.SignInMenuButton.IsChecked = true;
            this.OverviewMenuButton.Visibility = Visibility.Collapsed;
            this.ReserveMenuButton.Visibility = Visibility.Collapsed;
            this.AccountMenuButton.Visibility = Visibility.Collapsed;
        }

        private void OnReserveEvent(object sender, ReservationEventArgs args)
        {
            this.MainFrame.Content = this._reservationCustomerForm.Content;
        }
        
        private void OnReserveDurationEvent(object sender, ReservationDurationEventArgs args)
        {
            this.MainFrame.Content = this._reservationCustomerForm.Content;
        }

        private void OnReservationGuestsFormEvent(object sender, ReservationGuestEventArgs args)
        {
            this.MainFrame.Content = this._reservationCampingGuestPage;
        }

        private void OnReservationConfirmedEvent(object sender, ReservationEventArgs args)
        {
            this.MainFrame.Content = this._reservationConfirmedPage.Content;
        }

        private void OnSignInEvent(object sender, AccountEventArgs args)
        {
            this.OverviewMenuButton_Checked(sender, null);

            this.OverviewMenuButton.Visibility = Visibility.Visible;
            this.ReserveMenuButton.Visibility = Visibility.Visible;
            this.AccountMenuButton.Visibility = Visibility.Visible;
            this.SignInMenuButton.Visibility = Visibility.Collapsed;
            this.SignUpMenuButton.Visibility = Visibility.Collapsed;
        }

        private void OnSignUpEvent(object sender, AccountEventArgs args)
        {
            this.MainFrame.Content = this._signInPage.Content;
            this.SignInMenuButton.IsChecked = true;
        }


        private void OnSignOutEvent(object sender, EventArgs e)
        {
            this.MainFrame.Content = this._signInPage.Content;

            this.OverviewMenuButton.Visibility = Visibility.Collapsed;
            this.ReserveMenuButton.Visibility = Visibility.Collapsed;
            this.AccountMenuButton.Visibility = Visibility.Collapsed;
            this.SignInMenuButton.Visibility = Visibility.Visible;
            this.SignUpMenuButton.Visibility = Visibility.Visible;

            this.SignInMenuButton.IsChecked = true;
        }

        private void OnSignUpFormEvent(object sender, EventArgs e)
        {
            this.MainFrame.Content = this._signUpPage.Content;
            this.SignUpMenuButton.IsChecked = true;
        }

        private void OverviewMenuButton_Checked(object sender, RoutedEventArgs e)
        {
            this.OverviewMenuButton.IsChecked = true;
            if (CurrentUser.Account.Rights == Model.AccountRights.Admin)
            {
                this.MainFrame.Content = this._reservationCollectionFrame;
                return;
            }

            this.MainFrame.Content = this._reservationOverviewPage;
        }

        private void ReserveMenuButton_Checked(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Content = this._campingPlacesCollectionFrame;
        }

        private void AccountMenuButton_Checked(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Content = this._accountPage;
        }

        private void SignInMenuButton_Checked(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Content = this._signInPage;
        }

        private void SignUpMenuButton_Checked(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Content = this._signUpPage.Content;
        }
        
        private void OnManageReservationEvent(object sender, ReservationEventArgs args)
        {
            this.MainFrame.Content = this._manageReservationPage.Content;
        }

        private void OnBackToDashboardEvent(object sender, ReservationEventArgs args)
        {
            this.OverviewMenuButton_Checked(sender, null);
        }
    }
}
