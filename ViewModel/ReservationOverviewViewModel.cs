using Microsoft.Toolkit.Mvvm.ComponentModel;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class ReservationOverviewViewModel : ObservableObject
    {
        // TODO: Change this to session variable
        private int _customerID = 107;
        public List<Reservation> Reservations { get; private set; } = new List<Reservation>();
        public Dictionary<string, string> ReservationLabels { get; private set; } = new Dictionary<string, string>();
        private Reservation _currentSelectedReservationGet;
        public Reservation CurrentSelectedReservation
        {
            get
            {
                return _currentSelectedReservationGet;
            }
            set
            {
                this._currentSelectedReservationGet = value;
                this.DisplayNewReservationValues(value.Id);
            }
        }
        private DataTable _customerReservationGuestTable;
        public DataTable CustomerReservationGuestTable
        {
            get { return _customerReservationGuestTable; }
            set
            {
                _customerReservationGuestTable = value;
                OnPropertyChanged("CustomerReservationTable");
            }
        }
        private DataTable _customerReservationTable;
        public DataTable CustomerReservationTable
        {
            get { return _customerReservationTable; }
            set
            {
                _customerReservationTable = value;
                OnPropertyChanged("CustomerReservationTable");
            }
        }

        #region Properties
        private string _infoStartDate = "Begindatum: ";
        public string InfoStartDate
        {
            get
            {
                return _infoStartDate;
            }
            set
            {
                this._infoStartDate = "Begindatum: " + value;
            }
        }

        private string _infoEndDate = "Einddatum: ";
        public string InfoEndDate
        {
            get
            {
                return _infoEndDate;
            }
            set
            {
                this._infoEndDate = "Einddatum: " + value;
            }
        }

        private string _infoAmountOfGuests = "Aantal personen: ";
        public string InfoAmountOfGuests
        {
            get
            {
                return _infoAmountOfGuests;
            }
            set
            {
                this._infoAmountOfGuests = "Aantal personen: " + value;
            }
        }

        private string _infoAccommodationType = "Type: ";
        public string InfoAccommodationType
        {
            get
            {
                return _infoAccommodationType;
            }
            set
            {
                this._infoAccommodationType = "Type: " + value;
            }
        }

        private string _infoSurface = "Oppervlakte: ";
        public string InfoSurface
        {
            get
            {
                return _infoSurface;
            }
            set
            {
                this._infoSurface = "Oppervlakte: " + value + "m2";
            }
        }

        private string _infoLocation = "Locatie: ";
        public string InfoLocation
        {
            get
            {
                return _infoLocation;
            }
            set
            {
                this._infoLocation = "Locatie: " + value;
            }
        }

        private string _InfoTotalPrice = "Totaalprijs: ";
        public string InfoTotalPrice
        {
            get
            {
                return _InfoTotalPrice;
            }
            set
            {
                this._InfoTotalPrice = "Totaalprijs: €" + value;
            }
        }
        #endregion

        public ReservationOverviewViewModel()
        {
            Reservation reservationModel = new Reservation();
            this.Reservations = reservationModel.GetCustomersReservations(_customerID);
            this.ReservationLabels = this.GenerateReservationLabels();
            this.DisplayNewReservationValues(107);
        }

        /// <summary>
        /// Method to fill the customer reservation table
        /// </summary>
        public void FillCustomerReservationTable()
        {
            _customerReservationTable = new DataTable();
            _customerReservationTable.Columns.Add("ID");
            _customerReservationTable.Columns.Add("Duur");
            foreach (var item in this.Reservations)
            {
                _customerReservationTable.Rows.Add($"{item.Id}", $"{item.Duration.CheckInDate} - {item.Duration.CheckOutDate}");
            }
        }

        /// <summary>
        /// Method to fill the customer reservation guest table
        /// </summary>
        public void FillCustomerReservationGuestsTable()
        {
            _customerReservationGuestTable = new DataTable();
            _customerReservationGuestTable.Columns.Add("Voornaam");
            _customerReservationGuestTable.Columns.Add("Achternaam");
            _customerReservationGuestTable.Columns.Add("Geboortedatum");
            foreach (var item in this.Reservations)
            {
                _customerReservationGuestTable.Rows.Add("Voorbeeld", "Voorbeeld", "Voorbeeld");
            }
        }

        /// <summary>
        /// Generates the label names for the reservation overview table.
        /// </summary>
        /// <returns>Key, value-pair reservation id and reservation label</returns>
        public Dictionary<string, string> GenerateReservationLabels()
        {
            Dictionary<string, string> dictionary = new();
            foreach (var item in this.Reservations)
            {
                dictionary.Add(item.Id.ToString(), $"{item.CampingPlace.Type.Accommodation.Name} | {item.Duration.CheckInDate} - {item.Duration.CheckOutDate}");
            }
            return dictionary;
        }

        /// <summary>
        /// Function to execute all display changes in reservation overview at once
        /// </summary>
        /// <param name="id"></param>
        public void DisplayNewReservationValues(int id)
        {
            // Finds reservation with corresponding id (makes it unnecessary to do another databasecall)
            foreach (var item in this.Reservations)
                if (item.Id == id)
                    this.DisplayNewReservationInfoData(item);

            this.FillCustomerReservationTable();
            this.FillCustomerReservationGuestsTable();
            return;
        }

        /// <summary>
        /// Replaces the values of the reservation info in the reservation overview
        /// </summary>
        /// <param name="reservation">Reservation object of the selected reservation</param>
        public void DisplayNewReservationInfoData(Reservation reservation)
        {
            this.InfoStartDate = reservation.Duration.CheckInDate;
            this.InfoEndDate = reservation.Duration.CheckOutDate;
            this.InfoAmountOfGuests = reservation.NumberOfPeople.ToString();
            this.InfoAccommodationType = reservation.CampingPlace.Type.Accommodation.Name;
            this.InfoSurface = reservation.CampingPlace.Surface.ToString();
            this.InfoLocation = reservation.CampingPlace.Location;
            this.InfoTotalPrice = reservation.TotalPrice.ToString();
        }
    }
}