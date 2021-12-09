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
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class SignUpPage : Page
    {
        public SignUpPage()
        {
            this.InitializeComponent();
        }

        private void PasswordTextChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext == null)
            {
                return;
            }

            ((SignUpViewModel)this.DataContext).Password = ((PasswordBox)sender).Password;
        }

        private void ConfirmPasswordTextChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext == null)
            {
                return;
            }

            ((SignUpViewModel)this.DataContext).ConfirmPassword = ((PasswordBox)sender).Password;
        }
    }
}
