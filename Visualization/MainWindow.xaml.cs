using System;
using System.Windows;
using System.Windows.Media;
using Model;
using ViewModel;
using ViewModel.EventArguments;

namespace Visualization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private readonly ReservationCollectionPage _reservationCollectionFrame;
        private readonly ReservationCampingMapPage _reservationCampingMapPage;
        private readonly ReservationCustomerForm _reservationCustomerForm;
        private readonly ReservationConfirmedPage _reservationConfirmedPage;
        private readonly ReservationFailedPage _reservationFailedPage;
        private readonly ReservationCampingGuestPage _reservationCampingGuestPage;
        private readonly ReservationPaymentPage _reservationPaymentPage;
        private readonly AccountPage _accountPage;
        private readonly ManageCampingMapPage _manageCampingMapPage;
        private readonly ManageCampingPage _manageCampingPage;
        private readonly ManageCampingCustomerPage _manageCampingCustomerPage;

        private readonly SignInPage _signInPage;
        private readonly SignUpPage _signUpPage;
        private readonly ReservationManagePage _manageReservationPage;
        private readonly ReservationCustomerOverviewPage _reservationCustomerOverviewPage;
        private readonly AccountUpdatePage _accountUpdatePage;
        private readonly ContactPage _contactPage;
        private readonly ChatPage _chatPage;
        private readonly MultipleChatPage _multipleChatPage;
        private readonly ManageCampingPlaceTypePage _manageCampingPlaceTypePage;
        private readonly ManageAccommodationPage _manageAccommodationPage;

        public MainWindow()
        {
            this.InitializeComponent();
            
            this._reservationCustomerForm = new ReservationCustomerForm();
            this._reservationCampingMapPage = new ReservationCampingMapPage();
            this._reservationCollectionFrame = new ReservationCollectionPage();
            this._reservationConfirmedPage = new ReservationConfirmedPage();
            this._reservationFailedPage = new ReservationFailedPage();
            this._reservationCampingGuestPage = new ReservationCampingGuestPage();
            this._reservationPaymentPage = new ReservationPaymentPage();
            this._accountPage = new AccountPage();
            this._manageCampingMapPage = new ManageCampingMapPage();
            this._manageCampingPage = new ManageCampingPage();
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
            this._multipleChatPage = new MultipleChatPage();
            
            // Sets the current camping for the application.
            CurrentCamping.SetCurrentCamping((new Camping()).SelectLast());

            ReservationPaymentViewModel.ReservationConfirmedEvent += this.OnReservationConfirmedEvent;
            ReservationPaymentViewModel.ReservationGuestGoBackEvent += this.OnReservationGuestGoBackEvent;
            ReservationPaymentViewModel.ReservationFailedEvent += this.OnReservationFailedEvent;
            ReservationCampingGuestViewModel.ReservationGuestsConfirmedEvent += this.OnReservationGuestsConfirmedEvent;
            ReservationCampingGuestViewModel.ReservationGoBackEvent += this.OnReserveEvent;
            ReservationCustomerFormViewModel.ReservationGuestEvent += this.OnReservationGuestsFormEvent;
            ReservationCampingMapViewModel.ReserveEvent += this.OnReserveDurationEvent;
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
            MultipleChatPageViewModel.FromChatToContactEvent += this.ContactMenuButton_Checked;

            // Sets the sign up page as the active menu and hides other menu items.
            this.SignInMenuButton.IsChecked = true;
            this.OverviewMenuButton.Visibility = Visibility.Collapsed;
            this.ReserveMapMenuButton.Visibility = Visibility.Collapsed;
            this.AccountMenuButton.Visibility = Visibility.Collapsed;
            this.ContactMenuButton.Visibility = Visibility.Collapsed;
            this.ManageButton.Visibility = Visibility.Collapsed;
            this.ManageCampingButton.Visibility = Visibility.Collapsed;
            this.ManageCampingMapMenuButton.Visibility = Visibility.Collapsed;
            this.ManageCampingPlaceTypeButton.Visibility = Visibility.Collapsed;
            this.ManageAccommodationsButton.Visibility = Visibility.Collapsed;
            this.ManageCampingCustomerButton.Visibility = Visibility.Collapsed;
        }

        #region Menubutton checked events
        private void SignInMenuButton_Checked(object sender, RoutedEventArgs e)
        {
            this.CollapseManageDropdown();
            this.MainFrame.Content = this._signInPage;
        }

        private void SignUpMenuButton_Checked(object sender, RoutedEventArgs e)
        {
            this.CollapseManageDropdown();
            this.MainFrame.Content = this._signUpPage.Content;
        }

        private void OverviewMenuButton_Checked(object sender, RoutedEventArgs e)
        {
            this.CollapseManageDropdown();

            this.OverviewMenuButton.IsChecked = true;
            if (CurrentUser.Account.Rights == Model.AccountRights.Admin)
            {
                this.MainFrame.Content = this._reservationCollectionFrame;
                return;
            }

            this.MainFrame.Content = this._reservationCustomerOverviewPage;
        }

        private void ReserveMapMenuButton_Checked(object sender, RoutedEventArgs e)
        {
            this.CollapseManageDropdown();
            this.MainFrame.Content = this._reservationCampingMapPage;
        }

        private void AccountMenuButton_Checked(object sender, RoutedEventArgs e)
        {
            this.CollapseManageDropdown();
            this.MainFrame.Content = this._accountPage;
        }

        private void ContactMenuButton_Checked(object sender, EventArgs e)
        {
            this.CollapseManageDropdown();
            this.MainFrame.Content = this._contactPage.Content;
        }
        #endregion

        #region Managebutton checked events

        private void CollapseManageDropdown()
        {
            this.ManageButton.Content = "Beheer ⮟";

            this.ManageCampingButton.Visibility = Visibility.Collapsed;
            this.ManageCampingMapMenuButton.Visibility = Visibility.Collapsed;
            this.ManageCampingPlaceTypeButton.Visibility = Visibility.Collapsed;
            this.ManageAccommodationsButton.Visibility = Visibility.Collapsed;
            this.ManageCampingCustomerButton.Visibility = Visibility.Collapsed;
        }

        private void ManageButton_Clicked(object sender, RoutedEventArgs e)
        {
            if ((string)this.ManageButton.Content == "Beheer ⮝")
            {
                this.CollapseManageDropdown();
                return;
            }

            this.ManageButton.Content = "Beheer ⮝";

            this.ManageCampingButton.Visibility = Visibility.Visible;
            this.ManageCampingMapMenuButton.Visibility = Visibility.Visible;
            this.ManageCampingPlaceTypeButton.Visibility = Visibility.Visible;
            this.ManageAccommodationsButton.Visibility = Visibility.Visible;
            this.ManageCampingCustomerButton.Visibility = Visibility.Visible;
        }

        private void ManageCampingMenuButton_Checked(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Content = this._manageCampingPage;
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
        #endregion

        private void OnReserveEvent(object sender, EventArgs args)
        {
            this.MainFrame.Content = this._reservationCustomerForm.Content;
        }
        
        private void OnReserveDurationEvent(object sender, ReservationDurationEventArgs args)
        {
            this.MainFrame.Content = this._reservationCustomerForm.Content;
        }

        private void OnReservationGuestsFormEvent(object sender, ReservationEventArgs args)
        {
            this.MainFrame.Content = this._reservationCampingGuestPage;
        }

        private void OnReservationGuestGoBackEvent(object sender, ReservationGuestEventArgs args)
        {
            this.MainFrame.Content = this._reservationCampingGuestPage;
        }

        private void OnReservationConfirmedEvent(object sender, UpdateModelEventArgs<Reservation> args)
        {
            this.MainFrame.Content = this._reservationConfirmedPage.Content;
        }

        private void OnReservationFailedEvent(object sender, ReservationEventArgs args)
        {
            this.MainFrame.Content = this._reservationFailedPage.Content;
        }

        private void OnReservationGuestsConfirmedEvent(object sender, ReservationGuestEventArgs args)
        {
            this.MainFrame.Content = this._reservationPaymentPage.Content;
        }

        private void OnSignInEvent(object sender, AccountEventArgs args)
        {
            this.OverviewMenuButton_Checked(sender, null);

            this.OverviewMenuButton.Visibility = Visibility.Visible;
            this.ReserveMapMenuButton.Visibility = Visibility.Visible;
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
            this.ReserveMapMenuButton.Visibility = Visibility.Collapsed;
            this.AccountMenuButton.Visibility = Visibility.Collapsed;
            this.ContactMenuButton.Visibility = Visibility.Collapsed;
            this.ManageButton.Visibility = Visibility.Collapsed;
            this.ManageCampingButton.Visibility = Visibility.Collapsed;
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

        private void OnToAccountUpdatePageEvent(object sender, EventArgs e)
        {
            this.MainFrame.Content = this._accountUpdatePage.Content;
        }

        private void OnManageReservationEvent(object sender, ReservationEventArgs args)
        {
            this.MainFrame.Content = this._manageReservationPage.Content;
        }

        private void OnBackToDashboardEvent(object sender, EventArgs args)
        {
            this.OverviewMenuButton_Checked(sender, null);
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
            this.MainFrame.Content = CurrentUser.Account.Rights == AccountRights.Customer ? this._chatPage.Content : this._multipleChatPage.Content;
        }

        
    }
}