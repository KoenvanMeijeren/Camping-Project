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

namespace Visualization
{
    /// <summary>
    /// Interaction logic for ReservationCollectionPage.xaml
    /// </summary>
    public partial class ReservationCollectionPage : Page
    {
        private readonly Reservation _reservation = new Reservation();

        public ReservationCollectionPage()
        {
            this.InitializeComponent();

            ReservationCustomerForm.ReservationConfirmedEvent += OnReservationConfirmedEvent;
            
           this.SetReservations();
        }

        private void OnReservationConfirmedEvent(object sender, ReservationConfirmedEventArgs args)
        {
            this.SetReservations();
        }

        private void SetReservations()
        {
            this.ReservationsViewDataGrid.ItemsSource = this._reservation.Select();
        }
        
    }
}
