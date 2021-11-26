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
        public CampingPitchesCollectionPage()
        {
            this.InitializeComponent();
            this.CampingPitchLocationDropdown.ItemsSource = CampingPlaceDataCollection.SelectLocations();

            this.CampingPitchTypeDropdown.SelectedItem = this.CampingPitchTypeDropdown.Items[0];

            this.CampingViewDataGrid.ItemsSource = CampingPlaceDataCollection.Select();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void CampingPitchTypeDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CampingPitchLocationDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CampingViewDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ReserveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
    }
}
