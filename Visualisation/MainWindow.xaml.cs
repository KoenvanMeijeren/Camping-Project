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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PlattegrondFrame plattegrondFrame { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            plattegrondFrame = new PlattegrondFrame();
        }

        private void ReserveButtonClick(object sender, RoutedEventArgs e)
        {
            ReserveButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#006837");
            ReserveButton.Foreground = Brushes.White;
            MainFrame.Content = plattegrondFrame.Content;
        }
    }
}
