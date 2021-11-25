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
using Model;

namespace Visualisation
{
    /// <summary>
    /// Interaction logic for ReservationCollectionPage.xaml
    /// </summary>
    public partial class ReservationCollectionPage : Page
    {

        private ReservationCollection _reservationCollection = new ReservationCollection();

        public ReservationCollectionPage()
        {
            this.InitializeComponent();

            this.ReservationsViewDataGrid.ItemsSource = this._reservationCollection.Select();
        }
    }
}
