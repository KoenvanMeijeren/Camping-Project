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
    /// Interaction logic for ReservationCampingMapPage.xaml
    /// </summary>
    public partial class ReservationCampingMapPage : Page
    {
        public ReservationCampingMapPage()
        {
            InitializeComponent();
        }

        private void CampingFieldClicked(object sender, MouseButtonEventArgs e)
        {
            ReservationCampingMapViewModel viewModel = (ReservationCampingMapViewModel)this.DataContext;
            string selectedImage = ((Image)sender).Name;
            viewModel.StartReservation(selectedImage);
        }
    }
}
