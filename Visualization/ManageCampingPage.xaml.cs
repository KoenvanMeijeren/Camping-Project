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

namespace Visualization
{
    /// <summary>
    /// Interaction logic for ManageCampingPage.xaml
    /// </summary>
    public partial class ManageCampingPage : Page
    {
        public ManageCampingPage()
        {
            InitializeComponent();
        }

        private void ColorPicker_Background_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            ColorPicker.Background = new SolidColorBrush((Color)ColorPicker.SelectedColor);
        }
    }
}
