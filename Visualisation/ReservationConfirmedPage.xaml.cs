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

namespace Visualisation
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
        private readonly ReservationConfirmedPage _reservationConfirmedPage;
        public ReservationConfirmedPage()
        {
            InitializeComponent();
            this._reservationConfirmedPage = new ReservationConfirmedPage();
            ReservationCustomerForm.ReservationConfirmedEvent += onReservationConfirmedEvent;
        }

        public void onReservationConfirmedEvent(object sender, ReservationConfirmedEventArgs args)
        {
            this.FirstName = args.FirstName;
            this.LastName = args.LastName;
            this.CheckInDatetime = args.CheckInDatetime;
            this.CheckOutDatetime = args.CheckOutDatetime;

            this.Title.Content = $"Gefeliciteerd {FirstName} {LastName},";
            this.ConfirmationText.Content = $"Uw reservering van {CheckInDatetime.Date} tot {CheckInDatetime.Date}";
        }
    }
}
