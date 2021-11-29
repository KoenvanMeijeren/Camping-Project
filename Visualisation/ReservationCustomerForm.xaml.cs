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
using SystemCore;

namespace Visualisation
{
    /// <summary>
    /// Interaction logic for ReservationCustomerForm.xaml
    /// </summary>
    public partial class ReservationCustomerForm : Page
    {
        private string _errorMessageFieldIsEmpty = "is een verplicht vak.";
        private string _errorMessageFieldIsIncorrect = "is incorrect ingevuld.";
        private bool _errorHasOccurred;
        public DateTime CheckInDatetime { private get; set; }
        public DateTime CheckOutDatetime { private get; set; }
        public int CampingPlaceID { private get; set; }

        public ReservationCustomerForm()
        {
            InitializeComponent();
        }

        // Deleting this causes application to crash. ¯\_(ツ)_/¯
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ReservationCustomerFormSubmit(object sender, RoutedEventArgs e)
        {
            _errorHasOccurred = false;

            string firstName = CustomerFirstName.Text.Trim();
            string lastName = CustomerLastName.Text.Trim();
            string birthdate = string.Empty;
            string phonenumber = CustomerPhonenumber.Text.Trim();
            string streetname = CustomerAddress.Text.Trim();
            string postalcode = CustomerPostalcode.Text.Trim();
            string placename = CustomerPlacename.Text.Trim();
            string emailadres = CustomerMailadres.Text.Trim();
            string amountOfGuests = CustomerGuestAmount.Text.Trim();

            // Firstname validation
            if (String.IsNullOrEmpty(firstName))
            {
                ErrorFirstName.Content = _errorMessageFieldIsEmpty;
                // TODO: Foutmelding op scherm
                _errorHasOccurred = true;
            } else
            {
                ErrorFirstName.Content = string.Empty;
            }

            // Lastname validation
            if (String.IsNullOrEmpty(lastName))
            {
                // TODO: Foutmelding op scherm
                ErrorLastName.Content = _errorMessageFieldIsEmpty;
                _errorHasOccurred = true;
            } else
            {
                ErrorLastName.Content = string.Empty;
            }

            // Birthdate validation
            // TODO: CheckLegalAge moet nog worden geïmplementeerd.
            if (CustomerBirthDate.SelectedDate != null)
            {
                birthdate = CustomerBirthDate.SelectedDate.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                ErrorBirthDate.Content = string.Empty;
            }
            else
            {
                ErrorBirthDate.Content = _errorMessageFieldIsEmpty;
            }

            // Phonenumber validation
            // TODO: Phonenumber regex toevoegen
            if (String.IsNullOrEmpty(phonenumber))
            {
                // TODO: Foutmelding op scherm
                _errorHasOccurred = true;
                ErrorPhonenumber.Content = _errorMessageFieldIsEmpty;
            }
            else
            {
                ErrorPhonenumber.Content = string.Empty;
            }

            // Streetname validation
            // TODO: Placename regex toevoegen
            if (String.IsNullOrEmpty(placename))
            {
                // TODO: Foutmelding op scherm
                _errorHasOccurred = true;
                ErrorPlacename.Content = _errorMessageFieldIsEmpty;
            } else
            {
                ErrorPlacename.Content = string.Empty;
            }

            // Postalcode validation
            // TODO: postalcode regex toevoegen
            if (String.IsNullOrEmpty(postalcode))
            {
                // TODO: Foutmelding op scherm
                _errorHasOccurred = true;
                ErrorPostalcode.Content = _errorMessageFieldIsEmpty;
            } else
            {
                ErrorPostalcode.Content = string.Empty;
            }

            // Placename validation
            if (String.IsNullOrEmpty(streetname))
            {
                // TODO: Foutmelding op scherm
                _errorHasOccurred = true;
                ErrorAddress.Content = _errorMessageFieldIsEmpty;
            } else
            {
                ErrorAddress.Content = string.Empty;
            }

            // Emailadres validation
            // TODO: emailadres regex toevoegen (later implementeren)
            if (String.IsNullOrEmpty(emailadres))
            {
                // TODO: Foutmelding op scherm
                _errorHasOccurred = true;
                ErrorMail.Content = _errorMessageFieldIsEmpty;
            } else
            {
                ErrorMail.Content = string.Empty;
            }

            // Amount of guests validation
            // TODO: emailadres regex toevoegen (later implementeren)
            if (String.IsNullOrEmpty(amountOfGuests))
            {
                // TODO: Foutmelding op scherm
                _errorHasOccurred = true;
                ErrorAmountOfGuests.Content = _errorMessageFieldIsEmpty;
            }
            else
            {
                ErrorAmountOfGuests.Content = string.Empty;
            }

            // Case submit was valid
            if (!_errorHasOccurred)
            {
                //TODO: Transactie en toevoegen aan controller
                // Insert user input address in Address table
                Query addressInsertQuery = new Query("INSERT INTO Address VALUES (@Address, @Postalcode, @Place)");
                addressInsertQuery.AddParameter("Address", streetname);
                addressInsertQuery.AddParameter("Postalcode", postalcode);
                addressInsertQuery.AddParameter("Place", placename);
                addressInsertQuery.Execute();

                // Fetch latest inserted addressID from Address table
                Query fetchInsertedAddressID = new Query("SELECT AddressID FROM Address ORDER BY AddressID DESC");
                var fetchedInsertedAddressID = fetchInsertedAddressID.SelectFirst();
                fetchedInsertedAddressID.TryGetValue("AddressID", out string addressID);

                // Insert customer into CampingCustomer table
                Query insertCustomerQuery = new Query("INSERT INTO CampingCustomer VALUES (@CampingCustomerAddressID, @Birthdate, @Email, @PhoneNumber, @CustomerFirstName, @CustomerLastName)");
                insertCustomerQuery.AddParameter("CampingCustomerAddressID", Int32.Parse(addressID));
                insertCustomerQuery.AddParameter("Birthdate", birthdate);
                insertCustomerQuery.AddParameter("Email", emailadres);
                insertCustomerQuery.AddParameter("PhoneNumber", phonenumber);
                insertCustomerQuery.AddParameter("CustomerFirstName", firstName);
                insertCustomerQuery.AddParameter("CustomerLastName", lastName);
                insertCustomerQuery.Execute();

                // Fetch latest inserted addressID from Address table
                Query fetchInsertedCustomerID = new Query("SELECT CampingCustomerID FROM CampingCustomer ORDER BY CampingCustomerID DESC");
                var fetchedInsertedCustomerID = fetchInsertedCustomerID.SelectFirst();
                fetchedInsertedCustomerID.TryGetValue("CampingCustomerID", out string campingCustomerID);

                // Insert reservation duration in ReservationDuration table
                Query insertReservationDurationQuery = new Query("INSERT INTO ReservationDuration VALUES (@CheckinDatetime, @CheckoutDatetime)");
                insertReservationDurationQuery.AddParameter("CheckinDatetime", "2020-01-01 10:00:00"); // TODO: Job, you know what to do
                insertReservationDurationQuery.AddParameter("CheckoutDatetime", "2021-01-01 10:00:00"); // TODO: Job, you know what to do
                insertReservationDurationQuery.Execute();

                // Fetch latest inserted reservation duration
                Query fetchInserterdReservationDurationID = new Query("SELECT ReservationDurationID FROM ReservationDuration ORDER BY ReservationDurationID DESC");
                var fetchedInsertedReservationDurationID = fetchInserterdReservationDurationID.SelectFirst();
                fetchedInsertedReservationDurationID.TryGetValue("ReservationDurationID", out string reservationDurationID);

                // Insert reservation in Reservation table
                Query insertReservationQuery = new Query("INSERT INTO Reservation VALUES (@CampingPlaceID, @NumberOfPeople, @CampingCustomerID, @ReservationDurationID)");
                insertReservationQuery.AddParameter("CampingPlaceID", 18); // TODO: Job, you know what to do
                insertReservationQuery.AddParameter("NumberOfPeople", Int32.Parse(amountOfGuests));
                insertReservationQuery.AddParameter("CampingCustomerID", Int32.Parse(campingCustomerID));
                insertReservationQuery.AddParameter("ReservationDurationID", reservationDurationID);
                insertReservationQuery.Execute();
            }
        }

        // All the checks underneath should be put in their own class.
        private bool CheckInputTemporary(string input)
        {
            return (input != null || input != string.Empty); 
        }

        private bool CheckLegalAge(DatePicker selectedBirthDate)
        {
            DateTime birthdate = selectedBirthDate.DisplayDate;
            DateTime today = DateTime.Today;

            int customerAge = today.Year - birthdate.Year;

            // Go back to the year in which the person was born in case of a leap year
            if (birthdate.Date > today.AddYears(-customerAge)) customerAge--;


            if(customerAge >= 18)
            {
                return true;
            }
            return false;
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
            return CheckPostalcode(inputPostalcode);
        }

        private Boolean ValidateInputOnlyLetters(string inputNameValue)
        {
            var regex = new Regex(@"([a-zA-Z]{2,}\s[a-zA-Z]{1,}'?-?[a-zA-Z]{2,}\s?([a-zA-Z]{1,})?)", RegexOptions.IgnoreCase); //only alphabetic values
            return regex.IsMatch(inputNameValue);
        }

        private Boolean CheckInput(TextBox input)
        {
            if (input != null || input.Text != string.Empty)
            {
                if (input.Text.Trim().Length > 0)
                {
                    return true;
                }
                else
                {
                    ErrorFieldIsEmpty(input);
                }
            }
            return false;
        }

        private void ErrorFieldIsEmpty(TextBox emptyFieldTextbox)
        {
            if (!_errorHasOccurred)
            {
                _errorHasOccurred = true;
            }

            if (emptyFieldTextbox.Name.ToLower().Contains("name"))
            {
                if (emptyFieldTextbox.Name.Equals("CustomerFirstName"))
                {
                    ErrorPlacename.Content = "Voornaam" + _errorMessageFieldIsIncorrect;
                }
                else if (emptyFieldTextbox.Name.Equals("CustomerLastName"))
                {
                    ErrorLastName.Content = "Achternaam" + _errorMessageFieldIsIncorrect;
                }
                else
                {
                    ErrorFirstName.Content = "Plaatsnaam" + _errorMessageFieldIsIncorrect;
                }         
            } else if (emptyFieldTextbox.Name.Equals("CustomerPhonenumber"))
            {
                ErrorPhonenumber.Content = "Telefoonnummer" + _errorMessageFieldIsIncorrect;
            }else 
            {
                ErrorPostalcode.Content = "Poscode" + _errorMessageFieldIsIncorrect;
            }
            
        }

        private void ErrorFieldIsIncorrect(TextBox incorrectFieldTextbox)
        {
            if (!_errorHasOccurred)
            {
                _errorHasOccurred = true;
            }

            if (incorrectFieldTextbox.Name.ToLower().Contains("name"))
            {
                if (incorrectFieldTextbox.Name.Equals("CustomerFirstName"))
                {
                    ErrorPlacename.Content = "Voornaam" + _errorMessageFieldIsIncorrect;
                }
                else if (incorrectFieldTextbox.Name.Equals("CustomerLastName"))
                {
                    ErrorLastName.Content = "Achternaam" + _errorMessageFieldIsIncorrect;
                }
                else
                {
                    ErrorFirstName.Content = "Plaatsnaam" + _errorMessageFieldIsIncorrect;
                }
            }
            else if (incorrectFieldTextbox.Name.Equals("CustomerPhonenumber"))
            {
                ErrorPhonenumber.Content = "Telefoonnummer" + _errorMessageFieldIsIncorrect;
            }
            else
            {
                ErrorPostalcode.Content = "Poscode" + _errorMessageFieldIsIncorrect;
            }
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