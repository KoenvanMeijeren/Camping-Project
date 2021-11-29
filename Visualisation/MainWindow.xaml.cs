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

namespace Visualisation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private readonly CampingPitchesCollectionPage _campingPitchesCollectionFrame;
        private readonly ReservationCollectionPage _reservationCollectionFrame;
        private readonly ReservationCustomerForm _reservationCustomerForm;
        private readonly ReservationConfirmedPage _reservationConfirmedPage;

        public MainWindow()
        {
            this.InitializeComponent();
            
            this._campingPitchesCollectionFrame = new CampingPitchesCollectionPage();
            this._reservationCustomerForm = new ReservationCustomerForm();
            this._reservationCollectionFrame = new ReservationCollectionPage(ReservationCollection.Select());
            this._reservationConfirmedPage = new ReservationConfirmedPage();

            ReservationCustomerForm.ReservationConfirmedEvent += this.OnReservationConfirmedEvent;
            CampingPitchesCollectionPage.ReserveEvent += this.OnReserveEvent;
        }

        private void DashboardButtonClick(object sender, RoutedEventArgs e)
        {
            this.DashboardButton.Background = (SolidColorBrush) new BrushConverter().ConvertFrom("#006837");
            this.DashboardButton.Foreground = Brushes.White;

            this.CampingPitchesButton.Background = Brushes.White;
            this.CampingPitchesButton.Foreground = Brushes.Black;

            this.MainFrame.Content = this._reservationCollectionFrame.Content;
        }
        
        private void CampingPitchesButtonClick(object sender, RoutedEventArgs e)
        {
            this.CampingPitchesButton.Background = (SolidColorBrush) new BrushConverter().ConvertFrom("#006837");
            this.CampingPitchesButton.Foreground = Brushes.White;

            this.DashboardButton.Background = Brushes.White;
            this.DashboardButton.Foreground = Brushes.Black;

            this.MainFrame.Content = this._campingPitchesCollectionFrame.Content;
        }

        private void OnReserveEvent(object sender, ReserveEventArgs args)
        {
            this._reservationCustomerForm.CampingPlaceID = args.CampingPlaceID;
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
