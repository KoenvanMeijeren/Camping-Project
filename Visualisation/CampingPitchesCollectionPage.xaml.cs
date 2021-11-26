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
        private String _filterAccommodationType { get; set; }
        public CampingPitchesCollectionPage()
        {
            this.InitializeComponent();
            this.CampingPitchLocationDropdown.ItemsSource = CampingPlaceViewDataCollection.SelectLocations();
            this.CampingPitchTypeDropdown.SelectedItem = this.CampingPitchTypeDropdown.Items[0];

            SetOverview();
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            _filterAccommodationType = CampingPitchTypeDropdown.Text;
            SetOverview();
        }

        public void SetOverview()
        {
            _campingPlaceViewDataCollection = CampingPlaceViewDataCollection.Select();

            if (_filterAccommodationType != null && _filterAccommodationType != "Alle")
            {
                var result = _campingPlaceViewDataCollection
                    .Where(campingPlaceViewData => campingPlaceViewData.Type.Equals(_filterAccommodationType));

                _campingPlaceViewDataCollection = result.ToList();
            }

            this.CampingViewDataGrid.ItemsSource = _campingPlaceViewDataCollection;
        }
    }
}
