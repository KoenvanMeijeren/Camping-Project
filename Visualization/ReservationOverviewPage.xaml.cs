using System;
using System.Windows;
using System.Data;
using System.Windows.Controls;

namespace Visualization
{
    /// <summary>
    /// Interaction logic for ReservationOverviewPage.xaml
    /// </summary>
    public partial class ReservationOverviewPage : Page
    {
        public ReservationOverviewPage()
        {
            this.InitializeComponent();
        }

        private void DataGrid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        public void ClickedReservationCustomerRow(object sender, EventArgs e)
        {
            DataRowView rowview = CustomerReservationTableX.SelectedItem as DataRowView;
            string id = rowview.Row[0].ToString();

            Console.WriteLine(id);
        }
    }
}