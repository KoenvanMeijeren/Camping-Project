using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Visualization
{
    /// <summary>
    /// Interaction logic for SignUpPage.xaml
    /// </summary>
    public partial class SignInPage : Page
    {
        public SignInPage()
        {
            this.InitializeComponent();
        }

        private void PasswordTextChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext == null)
            {
                return;
            }

            ((SignInViewModel)this.DataContext).Password = ((PasswordBox)sender).Password;
        }
    }
}
