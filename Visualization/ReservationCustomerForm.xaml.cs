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
using ViewModel;

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

    /*    private Label[] errorLabels = new Label[this.ErrorFirstName, this.ErrorLastName, this.ErrorBirthDate, this.ErrorPhonenumber, this.ErrorPlacename, this.ErrorPostalcode, this.ErrorAddress, this.ErrorMail, this.ErrorAmountOfGuests];
        private void ClearErrorLabels()
        {
            foreach (Label element in errorLabels)
            {
                element.Content = "";
            }
        }*/
        private void ReservationCustomerFormSubmit(object sender, RoutedEventArgs e)
        {
            string firstName = this.CustomerFirstName.Text;
            string lastName = this.CustomerLastName.Text;
            string birthdate = string.Empty;
            string phoneNumber = this.CustomerPhonenumber.Text;
            string streetName = this.CustomerAddress.Text;
            string postalcode = this.CustomerPostalcode.Text;
            string placeName = this.CustomerPlacename.Text;
            string emailAddress = this.CustomerMailadres.Text;
            string amountOfGuests = this.CustomerGuestAmount.Text;
            
            ReservationCustomerFormInsert reservationCustomerFormInsert = new ReservationCustomerFormInsert(firstName, lastName, birthdate, phoneNumber, streetName, postalcode, placeName, emailAddress, amountOfGuests, this.CheckInDatetime, this.CheckOutDatetime, this.CampingPlaceID);

          /*  this.ClearErrorMessages();
            if (ReservationConfirmedEvent.errorDictionary.Count > 0)
            {
                foreach(KeyValuePair<string, string> error in ReservationConfirmedEvent.errorDictionary)
                {
                    var element; 
                    switch(error.Key)
                    {
                        case "firstName":
                            element = this.ErrorFirstName;
                            break;
                        case "lastName":
                            element = this.ErrorLastName;
                            break;
                        case "birthDate":
                            element = this.ErrorBirthDate;
                            break;
                        case "phoneNumber":
                            element = this.ErrorPhonenumber;
                            break;
                        case "streetName":
                            element = this.ErrorPlacename;
                            break;
                        case "postalCode":
                            element = this.ErrorPostalcode;
                            break;
                        case "placeName":
                            element = this.ErrorPlacename;
                            break;
                        case "emailAdress":
                            element = this.ErrorMail;
                            break;
                        case "amountOfGuests":
                            element = this.CustomerGuestAmount;
                            break;
                        default: 
                            Console.WriteLine("Hier had hij niet mogen komen");
                            break;
                    }
                    element.Content = error.Value;
                }
                return;
            }
            ReservationConfirmedEvent?.Invoke(this, new ReservationConfirmedEventArgs(firstName, lastName, CheckInDatetime, CheckOutDatetime));*/
        }

        /*        // All the checks underneath should be put in their own class.
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
            }*/
    }
}