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

        public MainWindow()
        {
            this.InitializeComponent();
            
            this._campingPitchesCollectionFrame = new CampingPitchesCollectionPage();
            this._reservationCollectionFrame = new ReservationCollectionPage();

        }

        private void ReserveButtonClick(object sender, RoutedEventArgs e)
        {
            this.ReserveButton.Background = (SolidColorBrush) new BrushConverter().ConvertFrom("#006837");
            this.ReserveButton.Foreground = Brushes.White;

            this.OverviewButton.Background = Brushes.White;
            this.OverviewButton.Foreground = Brushes.Black;

            this.MainFrame.Content = this._campingPitchesCollectionFrame.Content;
        }

        private void OverviewButtonClick(object sender, RoutedEventArgs e)
        {
            this.OverviewButton.Background = (SolidColorBrush) new BrushConverter().ConvertFrom("#006837");
            this.OverviewButton.Foreground = Brushes.White;

            this.ReserveButton.Background = Brushes.White;
            this.ReserveButton.Foreground = Brushes.Black;

            this.MainFrame.Content = this._reservationCollectionFrame.Content;
        }
    }
}
