using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for CampingPitchesCollectionPage.xaml
    /// </summary>
    public partial class CampingPitchesCollectionPage : Page
    {
        private static List<CampingPlaceViewData> _campingPlaceViewDataCollection { get;  set; }
        private static List<String> _selectionList { get; set; }
        public CampingPitchesCollectionPage()
        {
            this.InitializeComponent();
            this.CampingPitchTypeDropdown.SelectedItem = this.CampingPitchTypeDropdown.Items[0];

            SetOverview();
        }

        public void SetOverview()
        {
            if (this.CheckoutDatetime.SelectedDate != null && this.CheckinDatetime.SelectedDate != null)
            {
                _campingPlaceViewDataCollection = CampingPlaceViewDataCollection.Select();
                _selectionList = new List<string>();

                if (this.CampingPitchTypeDropdown.Text != null && this.CampingPitchTypeDropdown.Text != "Alle")
                {
                    _campingPlaceViewDataCollection = _campingPlaceViewDataCollection.Where(campingPlaceViewData => campingPlaceViewData.Type.Equals(this.CampingPitchTypeDropdown.Text)).ToList();
                }

                if (int.TryParse(this.MinNightPrice.Text, out int min))
                {
                    _campingPlaceViewDataCollection = _campingPlaceViewDataCollection.Where(campingPlaceViewData => campingPlaceViewData.GetNumericNightPrice() >= min).ToList();
                }

                if (int.TryParse(this.MaxNightPrice.Text, out int max))
                {
                    _campingPlaceViewDataCollection = _campingPlaceViewDataCollection.Where(campingPlaceViewData => campingPlaceViewData.GetNumericNightPrice() <= max).ToList();
                }


                _campingPlaceViewDataCollection = CampingPlaceViewDataCollection.FilterReserved(_campingPlaceViewDataCollection, (DateTime)this.CheckinDatetime.SelectedDate, (DateTime)this.CheckoutDatetime.SelectedDate);
                

                foreach (CampingPlaceViewData campingPlaceViewData in _campingPlaceViewDataCollection)
                {
                    _selectionList.Add(campingPlaceViewData.Locatie);
                }

                this.OverviewTitle.Content = "Beschikbare verblijven:";

                if (CampingPitchLocationDropdown.SelectedItem != null)
                {
                    this.ReserveButton.IsEnabled = true;
                }
                else
                {
                    this.ReserveButton.IsEnabled = false;
                }

                this.CampingPitchLocationDropdown.ItemsSource = _selectionList;
                this.CampingViewDataGrid.ItemsSource = _campingPlaceViewDataCollection;
            }
            else
            {
                this.OverviewTitle.Content = "Selecteer uw verblijfstermijn";
                this.ReserveButton.IsEnabled = false;
                this.CampingPitchLocationDropdown.ItemsSource = null;
                this.CampingViewDataGrid.ItemsSource = null;

            }
        }
        private void CheckinDatetimeChanged(object sender, RoutedEventArgs e)
        {
            if (this.CheckinDatetime.SelectedDate > this.CheckoutDatetime.SelectedDate)
            {
                this.CheckinDatetime.SelectedDate = null;
            }
            SetOverview();
        }

        private void CheckoutDatetimeChanged(object sender, RoutedEventArgs e)
        {
            if (this.CheckoutDatetime.SelectedDate < this.CheckinDatetime.SelectedDate)
            {
                this.CheckoutDatetime.SelectedDate = null;
            }
            SetOverview();
        }

        private void CampingPitchTypeDropdownChanged(object sender, EventArgs e)
        {
            SetOverview();
        }

        private void MinNightPriceChanged(object sender, KeyEventArgs e)
        {
            SetOverview();
        }

        private void MaxNightPriceChanged(object sender, KeyEventArgs e)
        {
            SetOverview();
        }

        private void CampingPitchLocationDropdownChanged(object sender, SelectionChangedEventArgs e)
        {
            this.ReserveButton.IsEnabled = CampingPitchLocationDropdown.SelectedItem != null;
        }

        private void ReserveButtonClicked(object sender, RoutedEventArgs e)
        {
            /*this.CheckinDatetime.SelectedDate*/
            /*this.CheckoutDatetime.SelectedDate*/

            foreach(CampingPlaceViewData campingPlaceViewData in _campingPlaceViewDataCollection)
            {
                if (campingPlaceViewData.Locatie.Equals(this.CampingPitchLocationDropdown.SelectedItem))
                {
                    /*campingPlaceViewData.GetId*/
                }
            }
        }
    }
}
