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
using ViewModel;

namespace Visualization
{
    /// <summary>
    /// Interaction logic for ReservationConfirmedPage.xaml
    /// </summary>
    public partial class ReservationConfirmedPage : Page
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CheckInDatetime { private get; set; }
        public DateTime CheckOutDatetime { private get; set; }
        public ReservationConfirmedPage()
        {
            this.InitializeComponent();
            
            ReservationCustomerFormViewModel.ReservationConfirmedEvent += this.OnReservationConfirmedEvent;
        }

        private void OnReservationConfirmedEvent(object sender, ReservationEventArgs args)
        {
            this.FirstName = args.Reservation.CampingCustomer.FirstName;
            this.LastName = args.Reservation.CampingCustomer.LastName;
            this.CheckInDatetime = args.Reservation.Duration.CheckInDatetime;
            this.CheckOutDatetime = args.Reservation.Duration.CheckOutDatetime;

            this.Title.Content = $"Gefeliciteerd {FirstName} {LastName},";
            this.ConfirmationText.Content = $"Uw reservering van {this.CheckInDatetime.Date.ToShortDateString()} tot {this.CheckOutDatetime.Date.ToShortDateString()}";
        }
    }
}
