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
using SystemCore;

namespace Visualization
{
    /// <summary>
    /// Interaction logic for ReservationCustomerForm.xaml
    /// </summary>
    public partial class ReservationCustomerForm : Page
    {
        private const int LegalAge = 18;
        
        private const string 
            ErrorMessageFieldIsEmpty = "is een verplicht vak.", 
            ErrorMessageFieldIsIncorrect = "is incorrect ingevuld.";
        
        private bool _hasErrors;
        
        public DateTime CheckInDatetime { private get; set; }
        public DateTime CheckOutDatetime { private get; set; }
        public int CampingPlaceID { private get; set; }
        public static event EventHandler<ReservationConfirmedEventArgs> ReservationConfirmedEvent;

        public ReservationCustomerForm()
        {
            this.InitializeComponent();
        }

        // Deleting this causes application to crash. ¯\_(ツ)_/¯
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ReservationCustomerFormSubmit(object sender, RoutedEventArgs e)
        {
            this._hasErrors = false;

            string firstName = this.CustomerFirstName.Text.Trim();
            string lastName = this.CustomerLastName.Text.Trim();
            string birthdate = string.Empty;
            string phoneNumber = this.CustomerPhonenumber.Text.Trim();
            string streetName = this.CustomerAddress.Text.Trim();
            string postalcode = this.CustomerPostalcode.Text.Trim();
            string placeName = this.CustomerPlacename.Text.Trim();
            string emailAddress = this.CustomerMailadres.Text.Trim();
            string amountOfGuests = this.CustomerGuestAmount.Text.Trim();

            // Firstname validation
            this.ErrorFirstName.Content = string.Empty;
            if (string.IsNullOrEmpty(firstName))
            {
                this.ErrorFirstName.Content = ErrorMessageFieldIsEmpty;
                this._hasErrors = true;
            }

            // Lastname validation
            this.ErrorLastName.Content = string.Empty;
            if (string.IsNullOrEmpty(lastName))
            {
                this.ErrorLastName.Content = ErrorMessageFieldIsEmpty;
                this._hasErrors = true;
            }

            // Birthdate validation
            // TODO: CheckLegalAge moet nog worden geïmplementeerd.
            this.ErrorBirthDate.Content = ErrorMessageFieldIsEmpty;
            if (this.CustomerBirthDate.SelectedDate != null)
            {
                birthdate = this.CustomerBirthDate.SelectedDate.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                this.ErrorBirthDate.Content = string.Empty;
            }

            // Phonenumber validation
            // TODO: Phonenumber regex toevoegen
            this.ErrorPhonenumber.Content = string.Empty;
            if (string.IsNullOrEmpty(phoneNumber))
            {
                this._hasErrors = true;
                this.ErrorPhonenumber.Content = ErrorMessageFieldIsEmpty;
            }

            // Streetname validation
            // TODO: Placename regex toevoegen
            this.ErrorPlacename.Content = string.Empty;
            if (string.IsNullOrEmpty(placeName))
            {
                this._hasErrors = true;
                this.ErrorPlacename.Content = ErrorMessageFieldIsEmpty;
            }

            // Postalcode validation
            // TODO: postalcode regex toevoegen
            this.ErrorPostalcode.Content = string.Empty;
            if (string.IsNullOrEmpty(postalcode))
            {
                this._hasErrors = true;
                this.ErrorPostalcode.Content = ErrorMessageFieldIsEmpty;
            }

            // Placename validation
            this.ErrorAddress.Content = string.Empty;
            if (string.IsNullOrEmpty(streetName))
            {
                this._hasErrors = true;
                this.ErrorAddress.Content = ErrorMessageFieldIsEmpty;
            }

            // Emailadres validation
            // TODO: emailadres regex toevoegen (later implementeren)
            this.ErrorMail.Content = string.Empty;
            if (string.IsNullOrEmpty(emailAddress))
            {
                this._hasErrors = true;
                this.ErrorMail.Content = ErrorMessageFieldIsEmpty;
            }

            // Amount of guests validation
            // TODO: emailadres regex toevoegen (later implementeren)
            this.ErrorAmountOfGuests.Content = string.Empty;
            if (string.IsNullOrEmpty(amountOfGuests))
            {
                this._hasErrors = true;
                this.ErrorAmountOfGuests.Content = ErrorMessageFieldIsEmpty;
            }

            // Case submit was valid
            if (this._hasErrors)
            {
                return;
            }

            //TODO: Transactie en toevoegen aan controller
            // Create or/and fetch address based on user input
            Address address = new Address(streetName, postalcode, placeName);
            var fetchLatestAddressOrCreateOne = address.FirstOrInsert();
            /*            Query addressInsertQuery = new Query("INSERT INTO Address VALUES (@Address, @Postalcode, @Place)");
                        addressInsertQuery.AddParameter("Address", streetName);
                        addressInsertQuery.AddParameter("Postalcode", postalcode);
                        addressInsertQuery.AddParameter("Place", placeName);
                        addressInsertQuery.Execute();*/

            // Fetch latest inserted addressID from Address table
            /*            Query lastAddressQuery = new Query("SELECT AddressID FROM Address ORDER BY AddressID DESC");
                        var lastAddressId = lastAddressQuery.SelectFirst();
                        lastAddressId.TryGetValue("AddressID", out string addressID);*/

            // Insert customer into CampingCustomer table
            CampingCustomer campingCustomer = new CampingCustomer(fetchLatestAddressOrCreateOne, birthdate, emailAddress, phoneNumber, firstName, lastName);
            campingCustomer.Insert();
            var fetchCampingCustomer = campingCustomer.SelectLast();
            /*            Query insertCustomerQuery = new Query("INSERT INTO CampingCustomer VALUES (@CampingCustomerAddressID, @Birthdate, @Email, @PhoneNumber, @CustomerFirstName, @CustomerLastName)");
                        insertCustomerQuery.AddParameter("CampingCustomerAddressID", Int32.Parse(addressID));
                        insertCustomerQuery.AddParameter("Birthdate", birthdate);
                        insertCustomerQuery.AddParameter("Email", emailAddress);
                        insertCustomerQuery.AddParameter("PhoneNumber", phoneNumber);
                        insertCustomerQuery.AddParameter("CustomerFirstName", firstName);
                        insertCustomerQuery.AddParameter("CustomerLastName", lastName);
                        insertCustomerQuery.Execute();*/

            // Fetch latest inserted addressID from Address table
            /*            Query lastCustomerQuery = new Query("SELECT CampingCustomerID FROM CampingCustomer ORDER BY CampingCustomerID DESC");
                        var lastCustomerId = lastCustomerQuery.SelectFirst();
                        lastCustomerId.TryGetValue("CampingCustomerID", out string campingCustomerID);*/

            // Insert and fetch reservation duration in ReservationDuration table
            ReservationDuration reservationDuration = new ReservationDuration(CheckInDatetime.ToString(), CheckOutDatetime.ToString());
            reservationDuration.Insert();
            var fetchNewestReservationDuration = reservationDuration.SelectLast();
            /*           Query insertReservationDurationQuery = new Query("INSERT INTO ReservationDuration VALUES (@CheckinDatetime, @CheckoutDatetime)");
                       insertReservationDurationQuery.AddParameter("CheckinDatetime", CheckInDatetime);
                       insertReservationDurationQuery.AddParameter("CheckoutDatetime", CheckOutDatetime);
                       insertReservationDurationQuery.Execute();*/

            /*            Query lastReservationDurationQuery = new Query("SELECT ReservationDurationID FROM ReservationDuration ORDER BY ReservationDurationID DESC");
                        var lastReservationDurationId = lastReservationDurationQuery.SelectFirst();
                        lastReservationDurationId.TryGetValue("ReservationDurationID", out string reservationDurationID);*/

            // Insert reservation in Reservation table
            CampingPlace campingPlaceModel = new CampingPlace();
            CampingPlace campingPlace = campingPlaceModel.Select(CampingPlaceID);
            Reservation reservation = new Reservation(amountOfGuests, fetchCampingCustomer, campingPlace, fetchNewestReservationDuration);
            reservation.Insert();


            /*         Query insertReservationQuery = new Query("INSERT INTO Reservation VALUES (@CampingPlaceID, @NumberOfPeople, @CampingCustomerID, @ReservationDurationID)");
                     insertReservationQuery.AddParameter("CampingPlaceID", CampingPlaceID);
                     insertReservationQuery.AddParameter("NumberOfPeople", Int32.Parse(amountOfGuests));
                     insertReservationQuery.AddParameter("CampingCustomerID", Int32.Parse(campingCustomerID));
                     insertReservationQuery.AddParameter("ReservationDurationID", reservationDurationID);
                     insertReservationQuery.Execute();*/

            ReservationConfirmedEvent?.Invoke(this, new ReservationConfirmedEventArgs(firstName, lastName, CheckInDatetime, CheckOutDatetime));
        }

        // All the checks underneath should be put in their own class.
        private bool CheckLegalAge(DatePicker selectedBirthDate)
        {
            DateTime birthdate = selectedBirthDate.DisplayDate;
            DateTime today = DateTime.Today;

            int customerAge = today.Year - birthdate.Year;

            // Go back to the year in which the person was born in case of a leap year
            if (birthdate.Date > today.AddYears(-customerAge))
            {
                customerAge--;
            }
            
            return customerAge >= LegalAge;
        }

        private static bool CheckPostalcode(string postalcode)
        {
            // Only dutch postal codes. For example: 8183XY.
            var regex = new Regex(@"/^\W*[1-9]{1}[0-9]{3}\W*[a-zA-Z]{2}\W*$/", RegexOptions.IgnoreCase);
            return regex.IsMatch(CleanPostalCode(postalcode));
        }

        /// <summary>
        /// Removes all spaces and capitalize all letters from a string.
        /// </summary>
        /// <param name="postalcode">User input</param>
        /// <returns>Cleaned postalcode.</returns>
        private static string CleanPostalCode(string postalcode)
        {
            return Regex.Replace(postalcode, @"s", "").ToUpper();
        }

        private Boolean ValidatePostalcode(string inputPostalcode)
        {
            inputPostalcode = CleanPostalCode(inputPostalcode);
            return ReservationCustomerForm.CheckPostalcode(inputPostalcode);
        }

        private Boolean ValidateInputOnlyLetters(string inputNameValue)
        {
            var regex = new Regex(@"([a-zA-Z]{2,}\s[a-zA-Z]{1,}'?-?[a-zA-Z]{2,}\s?([a-zA-Z]{1,})?)", RegexOptions.IgnoreCase); //only alphabetic values
            return regex.IsMatch(inputNameValue);
        }

        private Boolean CheckInput(TextBox input)
        {
            if (string.IsNullOrEmpty(input.Text) || input.Text.Trim().Length < 1)
            {
                this.ErrorFieldIsEmpty(input);
                return false;
            }

            return true;
        }

        private void ErrorFieldIsEmpty(TextBox emptyFieldTextbox)
        {
            if (!this._hasErrors)
            {
                this._hasErrors = true;
            }

            if (emptyFieldTextbox.Name.ToLower().Contains("name"))
            {
                switch (emptyFieldTextbox.Name)
                {
                    case "CustomerFirstName":
                        this.ErrorPlacename.Content = "Voornaam" + ErrorMessageFieldIsIncorrect;
                        break;
                    case "CustomerLastName":
                        this.ErrorLastName.Content = "Achternaam" + ErrorMessageFieldIsIncorrect;
                        break;
                    default:
                        this.ErrorFirstName.Content = "Plaatsnaam" + ErrorMessageFieldIsIncorrect;
                        break;
                }
            } 
            else if (emptyFieldTextbox.Name.Equals("CustomerPhonenumber"))
            {
                this.ErrorPhonenumber.Content = "Telefoonnummer" + ErrorMessageFieldIsIncorrect;
            }
            else 
            {
                this.ErrorPostalcode.Content = "Poscode" + ErrorMessageFieldIsIncorrect;
            }
            
        }

        private void ErrorFieldIsIncorrect(TextBox incorrectFieldTextbox)
        {
            if (!this._hasErrors)
            {
                this._hasErrors = true;
            }

            if (incorrectFieldTextbox.Name.ToLower().Contains("name"))
            {
                switch (incorrectFieldTextbox.Name)
                {
                    case "CustomerFirstName":
                        this.ErrorPlacename.Content = "Voornaam" + ErrorMessageFieldIsIncorrect;
                        break;
                    case "CustomerLastName":
                        this.ErrorLastName.Content = "Achternaam" + ErrorMessageFieldIsIncorrect;
                        break;
                    default:
                        this.ErrorFirstName.Content = "Plaatsnaam" + ErrorMessageFieldIsIncorrect;
                        break;
                }
            }
            else if (incorrectFieldTextbox.Name.Equals("CustomerPhonenumber"))
            {
                this.ErrorPhonenumber.Content = "Telefoonnummer" + ErrorMessageFieldIsIncorrect;
            }
            else
            {
                this.ErrorPostalcode.Content = "Poscode" + ErrorMessageFieldIsIncorrect;
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