using System;
using System.Windows;
using System.Windows.Media;
using ViewModel;
using ViewModel.EventArguments;

namespace Visualization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private readonly ReservationCampingPlaceForm _reservationCampingPlaceForm;
        private readonly ReservationCollectionPage _reservationCollectionFrame;
        private readonly ReservationCustomerForm _reservationCustomerForm;
        private readonly ReservationConfirmedPage _reservationConfirmedPage;
        private readonly ReservationCampingGuestPage _reservationCampingGuestPage;
        private readonly AccountPage _accountPage;
        private readonly ManageCampingMapPage _manageCampingMapPage;
        private readonly ManageCampingCustomerPage _manageCampingCustomerPage;

        private readonly SignInPage _signInPage;
        private readonly SignUpPage _signUpPage;
        private readonly ReservationManagePage _manageReservationPage;
        private readonly ReservationCustomerOverviewPage _reservationCustomerOverviewPage;
        private readonly AccountUpdatePage _accountUpdatePage;
        private readonly ContactPage _contactPage;
        private readonly ChatPage _chatPage;
        private readonly ManageCampingPlaceTypePage _manageCampingPlaceTypePage;
        private readonly ManageAccommodationPage _manageAccommodationPage;

        public MainWindow()
        {
            this.InitializeComponent();
            
            this._reservationCampingPlaceForm = new ReservationCampingPlaceForm();
            this._reservationCustomerForm = new ReservationCustomerForm();
            this._reservationCollectionFrame = new ReservationCollectionPage();
            this._reservationConfirmedPage = new ReservationConfirmedPage();
            this._reservationCampingGuestPage = new ReservationCampingGuestPage();
            this._accountPage = new AccountPage();
            this._manageCampingMapPage = new ManageCampingMapPage();
            this._manageCampingCustomerPage = new ManageCampingCustomerPage();
            this._signInPage = new SignInPage();
            this._signUpPage = new SignUpPage();
            this._manageReservationPage = new ReservationManagePage();
            this._reservationCustomerOverviewPage = new ReservationCustomerOverviewPage();
            this._accountUpdatePage = new AccountUpdatePage();
            this._contactPage = new ContactPage();
            this._manageCampingPlaceTypePage = new ManageCampingPlaceTypePage();
            this._manageAccommodationPage = new ManageAccommodationPage();
            this._chatPage = new ChatPage();

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
            AccountViewModel.ToAccountUpdatePageEvent += this.OnToAccountUpdatePageEvent;
            AccountUpdateViewModel.UpdateCancelEvent += this.OnUpdateCancelEvent;
            AccountUpdateViewModel.UpdateConfirmEvent += this.OnUpdateConfirmEvent;
            ContactViewModel.FromContactToChatEvent += this.OnChatButton;
            ChatPageViewModel.FromChatToContactEvent += this.ContactMenuButton_Checked;

            // Sets the sign up page as the active menu and hides other menu items.
            this.SignInMenuButton.IsChecked = true;
            this.OverviewMenuButton.Visibility = Visibility.Collapsed;
            this.ReserveMenuButton.Visibility = Visibility.Collapsed;
            this.AccountMenuButton.Visibility = Visibility.Collapsed;
            this.ContactMenuButton.Visibility = Visibility.Collapsed;
            this.ManageButton.Visibility = Visibility.Collapsed;
            this.ManageCampingMapMenuButton.Visibility = Visibility.Collapsed;
            this.ManageCampingPlaceTypeButton.Visibility = Visibility.Collapsed;
            this.ManageAccommodationsButton.Visibility = Visibility.Collapsed;
            this.ManageCampingCustomerButton.Visibility = Visibility.Collapsed;
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
            this.ContactMenuButton.Visibility = Visibility.Visible;
            this.SignInMenuButton.Visibility = Visibility.Collapsed;
            this.SignUpMenuButton.Visibility = Visibility.Collapsed;

            if (CurrentUser.Account.Rights != Model.AccountRights.Admin)
            {
                return;
            }
            
            this.ManageButton.Visibility = Visibility.Visible;
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
            this.ContactMenuButton.Visibility = Visibility.Collapsed;
            this.ManageButton.Visibility = Visibility.Collapsed;
            this.ManageCampingMapMenuButton.Visibility = Visibility.Collapsed;
            this.ManageCampingPlaceTypeButton.Visibility = Visibility.Collapsed;
            this.ManageAccommodationsButton.Visibility = Visibility.Collapsed;
            this.ManageCampingCustomerButton.Visibility = Visibility.Collapsed;
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

            this.MainFrame.Content = this._reservationCustomerOverviewPage;
        }

        private void ReserveMenuButton_Checked(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Content = this._reservationCampingPlaceForm;
        }

        private void AccountMenuButton_Checked(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Content = this._accountPage;
        }
        
        private void ManageCampingPlaceTypesButton_Checked(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Content = this._manageCampingPlaceTypePage;
        }
        
        private void ManageAccommodationsButton_Checked(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Content = this._manageAccommodationPage;
        }
        
        private void ManageCampingCustomersMenuButton_Checked(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Content = this._manageCampingCustomerPage;
        }

        private void ManageCampingMapMenuButton_Checked(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Content = this._manageCampingMapPage;
        }

        private void SignInMenuButton_Checked(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Content = this._signInPage;
        }

        private void SignUpMenuButton_Checked(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Content = this._signUpPage.Content;
        }

        private void OnToAccountUpdatePageEvent(object sender, EventArgs e)
        {
            this.MainFrame.Content = this._accountUpdatePage.Content;
        }

        private void OnManageReservationEvent(object sender, ReservationEventArgs args)
        {
            this.MainFrame.Content = this._manageReservationPage.Content;
        }

        private void OnBackToDashboardEvent(object sender, ReservationEventArgs args)
        {
            this.OverviewMenuButton_Checked(sender, null);
        }

        private void ContactMenuButton_Checked(object sender, EventArgs e)
        {
            this.MainFrame.Content = this._contactPage.Content;
        }

        private void OnUpdateCancelEvent(object sender, EventArgs e)
        {
            this.AccountMenuButton_Checked(sender, null);
        }

        private void OnUpdateConfirmEvent(object sender, EventArgs e)
        {
            this.AccountMenuButton_Checked(sender, null);
        }

        private void OnChatButton(object sender, EventArgs e)
        {
            this.MainFrame.Content = this._chatPage.Content;
        }

        private void ManageButton_Checked(object sender, RoutedEventArgs e)
        {
            this.ManageButton.Content = "Beheer ⮝";

            this.ManageCampingMapMenuButton.Visibility = Visibility.Visible;
            this.ManageCampingPlaceTypeButton.Visibility = Visibility.Visible;
            this.ManageAccommodationsButton.Visibility = Visibility.Visible;
            this.ManageCampingCustomerButton.Visibility = Visibility.Visible;
        }

        private void ManageButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if ((bool)this.ManageCampingMapMenuButton.IsChecked ||
                (bool)this.ManageCampingPlaceTypeButton.IsChecked ||
                (bool)this.ManageAccommodationsButton.IsChecked ||
                (bool)this.ManageCampingCustomerButton.IsChecked)
            {
                return;
            }
            this.ManageButton.Content = "Beheer ⮟";

            this.ManageCampingMapMenuButton.Visibility = Visibility.Collapsed;
            this.ManageCampingPlaceTypeButton.Visibility = Visibility.Collapsed;
            this.ManageAccommodationsButton.Visibility = Visibility.Collapsed;
            this.ManageCampingCustomerButton.Visibility = Visibility.Collapsed;
        }
    }
}