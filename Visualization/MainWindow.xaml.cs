using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using SystemCore;
using Model;
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
        private readonly TestInputPage _testInputPage;
        private readonly TestPage _testPage;

        public MainWindow()
        {
            this.InitializeComponent();
            
            this._campingPlacesCollectionFrame = new CampingPlacesCollectionPage();
            this._reservationCustomerForm = new ReservationCustomerForm();
            this._reservationCollectionFrame = new ReservationCollectionPage();
            this._reservationConfirmedPage = new ReservationConfirmedPage();
            this._testPage = new TestPage();
            this._testInputPage = new TestInputPage();

            ReservationCustomerForm.ReservationConfirmedEvent += this.OnReservationConfirmedEvent;
            CampingPlacesCollectionViewModel.ReserveEvent += this.OnReserveEvent;
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


        private void OnReserveEvent(object sender, ReserveEventArgs args)
        {
            this._reservationCustomerForm.CampingPlaceID = args.CampingPlaceId;
            this._reservationCustomerForm.CheckInDatetime = args.CheckInDatetime;
            this._reservationCustomerForm.CheckOutDatetime = args.CheckOutDatetime;

            this.MainFrame.Content = this._reservationCustomerForm.Content;
        }

        private void OnReservationConfirmedEvent(object sender, ReservationConfirmedEventArgs args)
        {
            this.MainFrame.Content = this._reservationConfirmedPage.Content;
        }

    }
}
