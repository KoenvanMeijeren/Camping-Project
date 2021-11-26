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
using Visualisation.Model;

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

        public static event EventHandler<ReservationEventArgs> ReservationsChanged;
        
        public MainWindow()
        {
            this.InitializeComponent();
            
            this._campingPitchesCollectionFrame = new CampingPitchesCollectionPage();
            this._reservationCollectionFrame = new ReservationCollectionPage();
            this._reservationCustomerForm = new ReservationCustomerForm();
        }

        private void DashboardButtonClick(object sender, RoutedEventArgs e)
        {
            this.DashboardButton.Background = (SolidColorBrush) new BrushConverter().ConvertFrom("#006837");
            this.DashboardButton.Foreground = Brushes.White;

            this.DashboardButton.Background = Brushes.White;
            this.DashboardButton.Foreground = Brushes.Black;

            this.MainFrame.Content = this._reservationCollectionFrame.Content;
        }

        private void CampingPitchesButtonClick(object sender, RoutedEventArgs e)
        {
            this.CampingPitchesButton.Background = (SolidColorBrush) new BrushConverter().ConvertFrom("#006837");
            this.CampingPitchesButton.Foreground = Brushes.White;

            this.CampingPitchesButton.Background = Brushes.White;
            this.CampingPitchesButton.Foreground = Brushes.Black;

            this.MainFrame.Content = this._campingPitchesCollectionFrame.Content;
            
            MainWindow.ReservationsChanged?.Invoke(this, new ReservationEventArgs(ReservationCollection.Select()));
        }
        
        private void ReservationAddFormButtonClick(object sender, RoutedEventArgs e)
        {
            this.ReservationAddFormButton.Background = (SolidColorBrush) new BrushConverter().ConvertFrom("#006837");
            this.ReservationAddFormButton.Foreground = Brushes.White;

            this.ReservationAddFormButton.Background = Brushes.White;
            this.ReservationAddFormButton.Foreground = Brushes.Black;

            this.MainFrame.Content = this._reservationCustomerForm.Content;
            
            MainWindow.ReservationsChanged?.Invoke(this, new ReservationEventArgs(ReservationCollection.Select()));
        }
    }
}
