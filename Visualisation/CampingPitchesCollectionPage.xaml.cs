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
        private static List<CampingPlaceViewData> _campingPlaceViewDataCollection;
        private static List<String> _selectionList;
        public static event EventHandler<ReserveEventArgs> ReserveEvent;
        public CampingPitchesCollectionPage()
        {
            this.InitializeComponent();
            this.CampingPitchTypeDropdown.SelectedItem = this.CampingPitchTypeDropdown.Items[0];

            this.SetOverview();
        }

        public void SetOverview()
        {
            if (this.CheckinDatetime.SelectedDate == null || this.CheckoutDatetime.SelectedDate == null)
            {
                this.OverviewTitle.Content = "Selecteer uw verblijfstermijn";
                this.ReserveButton.IsEnabled = false;
                this.CampingPitchLocationDropdown.ItemsSource = null;
                this.CampingViewDataGrid.ItemsSource = null;
                return;
            }

            CampingPitchesCollectionPage._campingPlaceViewDataCollection = CampingPlaceViewDataCollection.Select();
            CampingPitchesCollectionPage._selectionList = new List<string>();

            if (this.CampingPitchTypeDropdown.Text != null && this.CampingPitchTypeDropdown.Text != "Alle")
            {
                CampingPitchesCollectionPage._campingPlaceViewDataCollection = CampingPitchesCollectionPage._campingPlaceViewDataCollection.Where(campingPlaceViewData => campingPlaceViewData.Type.Equals(this.CampingPitchTypeDropdown.Text)).ToList();
            }

            if (int.TryParse(this.MinNightPrice.Text, out int min))
            {
                CampingPitchesCollectionPage._campingPlaceViewDataCollection = CampingPitchesCollectionPage._campingPlaceViewDataCollection.Where(campingPlaceViewData => campingPlaceViewData.GetNumericNightPrice() >= min).ToList();
            }

            if (int.TryParse(this.MaxNightPrice.Text, out int max))
            {
                CampingPitchesCollectionPage._campingPlaceViewDataCollection = CampingPitchesCollectionPage._campingPlaceViewDataCollection.Where(campingPlaceViewData => campingPlaceViewData.GetNumericNightPrice() <= max).ToList();
            }


            CampingPitchesCollectionPage._campingPlaceViewDataCollection = CampingPlaceViewDataCollection.ToFilteredOnReservedCampingPitches(_campingPlaceViewDataCollection, (DateTime)this.CheckinDatetime.SelectedDate, (DateTime)this.CheckoutDatetime.SelectedDate);


            foreach (CampingPlaceViewData campingPlaceViewData in _campingPlaceViewDataCollection)
            {
                CampingPitchesCollectionPage._selectionList.Add(campingPlaceViewData.Locatie);
            }

            this.OverviewTitle.Content = "Beschikbare verblijven:";
            this.ReserveButton.IsEnabled = CampingPitchLocationDropdown.SelectedItem != null;
            this.CampingPitchLocationDropdown.ItemsSource = _selectionList;
            this.CampingViewDataGrid.ItemsSource = _campingPlaceViewDataCollection;
        }
        private void CheckinDatetimeChanged(object sender, RoutedEventArgs e)
        {
            if (this.CheckinDatetime.SelectedDate > this.CheckoutDatetime.SelectedDate)
            {
                this.CheckinDatetime.SelectedDate = null;
            }
            this.SetOverview();
        }

        private void CheckoutDatetimeChanged(object sender, RoutedEventArgs e)
        {
            if (this.CheckoutDatetime.SelectedDate < this.CheckinDatetime.SelectedDate)
            {
                this.CheckoutDatetime.SelectedDate = null;
            }
            this.SetOverview();
        }

        private void CampingPitchTypeDropdownChanged(object sender, EventArgs e)
        {
            this.SetOverview();
        }

        private void MinNightPriceChanged(object sender, KeyEventArgs e)
        {
            this.SetOverview();
        }

        private void MaxNightPriceChanged(object sender, KeyEventArgs e)
        {
            this.SetOverview();
        }

        private void CampingPitchLocationDropdownChanged(object sender, SelectionChangedEventArgs e)
        {
            this.ReserveButton.IsEnabled = CampingPitchLocationDropdown.SelectedItem != null;
        }

        private void ReserveButtonClicked(object sender, RoutedEventArgs e)
        {
            DateTime CheckInDatetime = (DateTime)this.CheckinDatetime.SelectedDate;
            DateTime CheckOutDatetime = (DateTime)this.CheckoutDatetime.SelectedDate;

            CampingPlaceViewData selectedCampingPlace = _campingPlaceViewDataCollection.Find(campingPlace => campingPlace.Locatie.Equals(this.CampingPitchLocationDropdown.SelectedItem));
            int CampingPlaceID = selectedCampingPlace.GetId();

            ReserveEvent?.Invoke(this, new ReserveEventArgs(CampingPlaceID, CheckInDatetime, CheckOutDatetime));
        }
    }
}
