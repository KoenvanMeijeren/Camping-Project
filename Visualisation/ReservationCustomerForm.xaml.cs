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

namespace Visualisation
{
    /// <summary>
    /// Interaction logic for ReservationCustomerForm.xaml
    /// </summary>
    public partial class ReservationCustomerForm : Page
    {
        public ReservationCustomerForm()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ReservationCustomerFormSubmit(object sender, RoutedEventArgs e)
        {
            //neem textbox mee
            string firstName = CheckInput(CustomerFirstName.Text.Trim()) && ValidateInputOnlyLetters(CustomerFirstName.Text.Trim()) ? CustomerFirstName.Text.Trim() : "";
            string lastName = CheckInput(CustomerLastName.Text.Trim()) && ValidateInputOnlyLetters(CustomerFirstName.Text.Trim()) ? CustomerLastName.Text.Trim() : "";
            string birthdate = CheckInput(CustomerBirthDate.Text.Trim()) && CheckBirthDate(CustomerBirthDate.Text.Trim()) ? CustomerBirthDate.Text.Trim() : "";
            string phonenumber = CheckInput(CustomerPhonenumber.Text.Trim()) && CheckPhoneNumber(CustomerPhonenumber.Text.Trim()) ? CustomerPhonenumber.Text.Trim() : "";
            string streetname = CheckInput(CustomerAddress.Text.Trim()) && CheckAddress(CustomerAddress.Text.Trim()) ? CustomerAddress.Text.Trim() : "";
            string postalcode = CheckInput(CustomerPostalcode.Text.Trim()) && ValidatePostalcode(CustomerPostalcode.Text.Trim()) ? CustomerPostalcode.Text.Trim() : "";
            string placename = CheckInput(CustomerPlacename.Text.Trim()) && ValidateInputOnlyLetters(CustomerFirstName.Text.Trim()) ? CustomerPlacename.Text.Trim() : "";
            string emailadres = CheckInput(CustomerMailadres.Text.Trim()) && CheckEmail(CustomerMailadres.Text.Trim()) ? CustomerMailadres.Text.Trim() : "";


        }

        private static bool CheckPostalcode(string postalcode)
        {
            var regex = new Regex(@"/^\W*[1-9]{1}[0-9]{3}\W*[a-zA-Z]{2}\W*$/", RegexOptions.IgnoreCase); //8183XY
            return regex.IsMatch(CleanPostalCode(postalcode));
        }

        /// <summary>
        /// removes all spaces and capitalize all letters from a string.
        /// </summary>
        /// <param name="postalcode">user input</param>
        /// <returns>clean postalcode</returns>
        private static string CleanPostalCode(string postalcode)
        {
            return Regex.Replace(postalcode, @"s", "").ToUpper();
        }

        private Boolean ValidatePostalcode(string inputPostalcode)
        {
            inputPostalcode = CleanPostalCode(inputPostalcode);
            if (!CheckPostalcode(inputPostalcode))
            {
                //activate postalcode label
            }
            return CheckPostalcode(inputPostalcode);
        }

        private Boolean ValidateInputOnlyLetters(string inputNameValue)
        {
            var regex = new Regex(@"([a-zA-Z]{2,}\s[a-zA-Z]{1,}'?-?[a-zA-Z]{2,}\s?([a-zA-Z]{1,})?)", RegexOptions.IgnoreCase); //only alphabetic values
            return regex.IsMatch(inputNameValue);
        }

        private Boolean CheckInput(string inputvalue)
        {
            if (inputvalue != null || inputvalue != string.Empty)
            {
                if (inputvalue.Length > 0)
                {
                    return true;
                }
            }
            return false;
        }

        private void ErrorFieldIsEmpty()
        {            
            ErrorFirstName.Content = "";
            ErrorLastName.Content = "";
            ErrorBirthDate.Content = "";
            ErrorPhonenumber.Content = "";
            ErrorPostalcode.Content = "";
            ErrorPlacename.Content = "";
        }

        private void ErrorFieldIsIncorrect()
        {

            ErrorFirstName.Content = "";
            ErrorLastName.Content = "";
            ErrorBirthDate.Content = "";
            ErrorPhonenumber.Content = "";
            ErrorPostalcode.Content = "";
            ErrorPlacename.Content = "";
        }

        private static bool CheckBirthDate(string birthDate)
        {
            //check
            var regex = new Regex(@"^(((0[1 - 9] |[12]\d | 3[01])\/ (0[13578] | 1[02])\/ ((19 |[2 - 9]\d)\d{ 2}))| ((0[1 - 9] |[12]\d | 30)\/ (0[13456789] | 1[012])\/ ((19 |[2 - 9]\d)\d{ 2}))| ((0[1 - 9] | 1\d | 2[0 - 8])\/ 02\/ ((19 |[2 - 9]\d)\d{ 2}))| (29\/ 02\/ ((1[6 - 9] |[2 - 9]\d)(0[48] |[2468][048] |[13579][26]) | ((16 |[2468][048] |[3579][26])00))))$", RegexOptions.IgnoreCase);
            return regex.IsMatch(birthDate.Trim());
        }



        private static bool CheckAddress(string address)
        {
            string validAddress = @"^[A-Za-z0-9]+(?:\s[A-Za-z0-9'_-]+)+$"; // de Geere 90
            var regex = new Regex(validAddress, RegexOptions.IgnoreCase);
            return regex.IsMatch(address.Trim());
        }

        private static bool CheckEmail(string email)
        {
            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", RegexOptions.IgnoreCase);//student@mail.nl
            return regex.IsMatch(email.Trim());
        }



        private static bool CheckPhoneNumber(string phoneNumber)
        {
            var regex = new Regex(@"^([+]31)-?06(\s?|-)([0-9]\s{0,3}){8}$", RegexOptions.IgnoreCase); //+310642165086
            return regex.IsMatch(phoneNumber.Trim());
        }



    }
}
}
