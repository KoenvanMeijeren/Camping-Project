using Model;
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
using ViewModel.EventArguments;

namespace Visualization
{
    /// <summary>
    /// Interaction logic for ManageCampingMapPage.xaml
    /// </summary>
    public partial class ManageCampingMapPage : Page
    {
        public ManageCampingMapPage()
        {
            this.InitializeComponent();
        }

        private void CampingFieldClicked(object sender, MouseButtonEventArgs e)
        {
            ManageCampingMapViewModel viewModel = (ManageCampingMapViewModel)this.DataContext;
            string selectedImage = ((Image)sender).Name;
            viewModel.SetSelectedCampingField(selectedImage);
        }
    }
}
