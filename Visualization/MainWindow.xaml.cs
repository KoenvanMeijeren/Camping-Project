﻿using System;
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
        private readonly AccountPage _accountPage;
        private readonly SignUpPage _signUpPage;
        private readonly RegisterPage _registerPage;
        private readonly TestInputPage _testInputPage;
        private readonly TestPage _testPage;

        public MainWindow()
        {
            this.InitializeComponent();
            
            this._campingPlacesCollectionFrame = new CampingPlacesCollectionPage();
            this._reservationCustomerForm = new ReservationCustomerForm();
            this._reservationCollectionFrame = new ReservationCollectionPage();
            this._reservationConfirmedPage = new ReservationConfirmedPage();
            this._accountPage = new AccountPage();
            this._signUpPage = new SignUpPage();
            this._registerPage = new RegisterPage();
            this._testPage = new TestPage();
            this._testInputPage = new TestInputPage();

            ReservationCustomerFormViewModel.ReservationConfirmedEvent += this.OnReservationConfirmedEvent;
            ReservationSelectCampingPlaceViewModel.ReserveEvent += this.OnReserveEvent;
            SignUpViewModel.SignUpEvent += this.OnSignUpEvent;
            AccountViewModel.SignOutEvent += this.OnSignOutEvent;
            SignUpViewModel.RegisterEvent += this.OnRegisterEvent;

            AccountButton.Visibility = Visibility.Collapsed;
        }

        private void DashboardButtonClick(object sender, RoutedEventArgs e)
        {
            this.DashboardButton.Background = (SolidColorBrush) new BrushConverter().ConvertFrom("#006837");
            this.DashboardButton.Foreground = Brushes.White;

            this.CampingPitchesButton.Background = Brushes.White;
            this.CampingPitchesButton.Foreground = Brushes.Black;
            
            this.TestButton.Background = Brushes.White;
            this.TestButton.Foreground = Brushes.Black;
            
            this.TestInputButton.Background = Brushes.White;
            this.TestInputButton.Foreground = Brushes.Black;

            this.MainFrame.Content = this._reservationCollectionFrame.Content;
        }
        
        private void CampingPitchesButtonClick(object sender, RoutedEventArgs e)
        {
            this.CampingPitchesButton.Background = (SolidColorBrush) new BrushConverter().ConvertFrom("#006837");
            this.CampingPitchesButton.Foreground = Brushes.White;

            this.DashboardButton.Background = Brushes.White;
            this.DashboardButton.Foreground = Brushes.Black;
            
            this.TestButton.Background = Brushes.White;
            this.TestButton.Foreground = Brushes.Black;
            
            this.TestInputButton.Background = Brushes.White;
            this.TestInputButton.Foreground = Brushes.Black;

            this.MainFrame.Content = this._campingPlacesCollectionFrame.Content;
        }
        
        private void TestClick(object sender, RoutedEventArgs e)
        {
            this.TestButton.Background = (SolidColorBrush) new BrushConverter().ConvertFrom("#006837");
            this.TestButton.Foreground = Brushes.White;

            this.CampingPitchesButton.Background = Brushes.White;
            this.CampingPitchesButton.Foreground = Brushes.Black;
            
            this.DashboardButton.Background = Brushes.White;
            this.DashboardButton.Foreground = Brushes.Black;
            
            this.TestButton.Background = Brushes.White;
            this.TestButton.Foreground = Brushes.Black;
            
            this.TestInputButton.Background = Brushes.White;
            this.TestInputButton.Foreground = Brushes.Black;

            this.MainFrame.Content = this._testPage.Content;
        }
        
        private void TestInputClick(object sender, RoutedEventArgs e)
        {
            this.TestInputButton.Background = (SolidColorBrush) new BrushConverter().ConvertFrom("#006837");
            this.TestInputButton.Foreground = Brushes.White;

            this.TestButton.Background = Brushes.White;
            this.TestButton.Foreground = Brushes.Black;
            
            this.CampingPitchesButton.Background = Brushes.White;
            this.CampingPitchesButton.Foreground = Brushes.Black;
            
            this.DashboardButton.Background = Brushes.White;
            this.DashboardButton.Foreground = Brushes.Black;

            this.MainFrame.Content = this._testInputPage.Content;
        }

        private void SignUpButtonClick(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Content = this._signUpPage.Content;
        }

        private void AccountButtonClick(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Content = this._accountPage.Content;
        }

        private void OnReserveEvent(object sender, ReservationDurationEventArgs args)
        {
            this.MainFrame.Content = this._reservationCustomerForm.Content;
        }

        private void OnReservationConfirmedEvent(object sender, ReservationEventArgs args)
        {
            this.MainFrame.Content = this._reservationConfirmedPage.Content;
        }

        private void OnSignUpEvent(object sender, SignUpEventArgs args)
        {
            SignUpButton.Visibility = Visibility.Collapsed;
            AccountButton.Visibility = Visibility.Visible;

            this.MainFrame.Content = this._accountPage.Content;
        }

        private void OnSignOutEvent(object sender, EventArgs e)
        {
            SignUpButton.Visibility = Visibility.Visible;
            AccountButton.Visibility = Visibility.Collapsed;

            this.MainFrame.Content = this._signUpPage.Content;
        }

        private void OnRegisterEvent(object sender, EventArgs e)
        {
            this.MainFrame.Content = this._registerPage.Content;
        }


    }
}
