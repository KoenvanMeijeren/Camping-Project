using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Toolkit.Mvvm.Input;
using Model;
using System.Collections.Generic;
using System.Globalization;

namespace ViewModel
{
    public class ReservationCollectionViewModel : ObservableObject
    {
        #region Fields
        private readonly Accommodation _accommodationModel = new Accommodation();
        private readonly Reservation _reservationModel = new Reservation();
        
        private const string SelectAll = "Alle";

        private readonly ObservableCollection<string> _campingPlaceTypes;
        public ObservableCollection<ReservationViewModel> Reservations { get; private set; }
        public static event EventHandler<ReservationEventArgs> ManageReservationEvent;
        
        private ReservationViewModel _selectedReservation;
        public ReservationViewModel SelectedReservation
        {
            get => _selectedReservation;
            set
            {
                if (value == null || Equals(value, this._selectedReservation))
                {
                    return;
                }

                this._selectedReservation = value;
                ManageReservationEvent?.Invoke(this,  new ReservationEventArgs(this._selectedReservation.Reservation));
            }
        }


        
        private DateTime _checkOutDate, _checkInDate;
        private string _minTotalPrice, _maxTotalPrice, _selectedCampingPlaceType, _guests;

        #endregion

        #region Properties

        public string MinTotalPrice
        {
            get => this._minTotalPrice;
            set
            {
                if (Equals(value, this._minTotalPrice))
                {
                    return;
                }

                this._minTotalPrice = value;
                this.SetOverview();

                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string MaxTotalPrice
        {
            get => this._maxTotalPrice;
            set
            {
                if (Equals(value, this._maxTotalPrice))
                {
                    return;
                }

                this._maxTotalPrice = value;
                this.SetOverview();

                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string Guests
        {
            get => this._guests;
            set
            {
                if (Equals(value, this._guests))
                {
                    return;
                }

                this._guests = value;
                this.SetOverview();

                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public DateTime CheckInDate
        {
            get => this._checkInDate;
            set
            {
                if (Equals(value, this._checkInDate))
                {
                    return;
                }

                int daysDifference = this._checkOutDate.Subtract(this._checkInDate).Days;
                
                this._checkInDate = value;
                this.SetOverview();
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
                
                this.CheckOutDate = this._checkInDate.AddDays(daysDifference);
            }
        }

        public DateTime CheckOutDate
        {
            get => this._checkOutDate;
            set
            {
                if (Equals(value, this._checkOutDate))
                {
                    return;
                }

                this._checkOutDate = value;
                this.SetOverview();
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }
        
        public ObservableCollection<string> CampingPlaceTypes
        {
            get => this._campingPlaceTypes;
            private init
            {
                if (Equals(value, this._campingPlaceTypes))
                {
                    return;
                }
                
                this._campingPlaceTypes = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string SelectedCampingPlaceType
        {
            get => this._selectedCampingPlaceType;
            set
            {
                if (Equals(value, this._selectedCampingPlaceType))
                {
                    return;
                }

                this._selectedCampingPlaceType = value;
                this.SetOverview();
                
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        #endregion

        public ReservationCollectionViewModel()
        {
            this.Reservations = new ObservableCollection<ReservationViewModel>();
            this.CampingPlaceTypes = new ObservableCollection<string> {
                SelectAll
            };
            
            //Loop through rows in Accommodation table
            foreach (var accommodationDatabaseRow in this._accommodationModel.Select())
            {
                this.CampingPlaceTypes.Add(accommodationDatabaseRow.Name);
            }

            this.SelectedCampingPlaceType = SelectAll;
            
            DateTime date = DateTime.Today;
            this.CheckInDate = new DateTime(date.Year, date.Month, 1);
            this.CheckOutDate = this.CheckInDate.AddMonths(1).AddDays(-1);

            ReservationCustomerFormViewModel.ReservationConfirmedEvent += this.OnReservationConfirmedEvent;
            ManageReservationViewModel.UpdateReservationCollection += OnReservationConfirmedEvent;
        }

        private void OnReservationConfirmedEvent(object sender, ReservationEventArgs args)
        {
            this.SetOverview();
        }

        private void SetOverview()
        {
            this.Reservations.Clear();

            var reservationItems = this._reservationModel.Select();
            if (!this.SelectedCampingPlaceType.Equals(SelectAll))
            {
                reservationItems = reservationItems.Where(reservation => reservation.CampingPlace.Type.Accommodation.Name.Equals(this.SelectedCampingPlaceType)).ToList();
            }
            
            if (int.TryParse(this.MinTotalPrice, out int min))
            {
                reservationItems = reservationItems.Where(reservation => reservation.TotalPrice >= min).ToList();
            }
            
            if (int.TryParse(this.MaxTotalPrice, out int max))
            {
                reservationItems = reservationItems.Where(reservation => reservation.TotalPrice <= max).ToList();
            }
            
            if (this.CheckInDate != DateTime.MinValue)
            {
                reservationItems = reservationItems.Where(reservation => reservation.Duration.CheckInDatetime >= this.CheckInDate).ToList();
            }
            
            if (this.CheckOutDate != DateTime.MinValue)
            {
                reservationItems = reservationItems.Where(reservation => reservation.Duration.CheckOutDatetime <= this.CheckOutDate).ToList();
            }
            
            if (int.TryParse(this.Guests, out int guests))
            {
                reservationItems = reservationItems.Where(reservation => reservation.NumberOfPeople >= guests).ToList();
            }
            
            foreach (var reservation in reservationItems)
            {
                this.Reservations.Add(new ReservationViewModel(reservation));
            }
        }

    }

        private void ExecuteCreatePdf()
        {
            Document document = new Document(PageSize.A4, 36, 36, 70, 36);
            PageEvents pageEvents = new PageEvents();
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            PdfWriter pdfWriter = PdfWriter.GetInstance(document, new FileStream(appData + "\\Downloads\\Reserveringen Overzicht.pdf", FileMode.Create));
            pdfWriter.PageEvent = pageEvents;
            document.Open();

            foreach (var reservation in this.Reservations)
            {
                float[] columnWidths = { 3, 5, 8, 8, 8, 5, 6 };
                PdfPTable reservationTable = new PdfPTable(columnWidths);

                document.Add(new Paragraph("\n"));
                document.Add(new Paragraph("\n"));
                reservationTable.AddCell("ID");
                reservationTable.AddCell("Verblijf");
                reservationTable.AddCell("Klantnaam");
                reservationTable.AddCell("Begindatum");
                reservationTable.AddCell("Einddatum");
                reservationTable.AddCell("Prijs");
                reservationTable.AddCell("Aanwezig");

                reservationTable.AddCell(reservation.Reservation.Id.ToString());
                reservationTable.AddCell(reservation.Reservation.CampingPlace.ToString());
                reservationTable.AddCell(reservation.Reservation.CampingCustomer.FirstName + " " + reservation.Reservation.CampingCustomer.LastName);
                reservationTable.AddCell(reservation.Reservation.Duration.CheckInDate);
                reservationTable.AddCell(reservation.Reservation.Duration.CheckOutDate);
                reservationTable.AddCell(" €" + reservation.Reservation.TotalPrice.ToString(CultureInfo.InvariantCulture));
                reservationTable.AddCell(" ");

                document.Add(new Paragraph("\n"));
                document.Add(reservationTable);

                //Should be used for the CampingGuest table
                var campingGuests = reservation.Reservation.CampingGuests;
                if (!campingGuests.Any())
                {
                    continue;
                }
                
                PdfPTable campingGuestTable = new PdfPTable(3);

                campingGuestTable.AddCell("Gastnaam");
                campingGuestTable.AddCell("Geboortedatum");
                foreach (var reservationGuest in campingGuests)
                {
                    campingGuestTable.AddCell(reservationGuest.CampingGuest.FirstName + " " + reservationGuest.CampingGuest.LastName);
                    campingGuestTable.AddCell(reservationGuest.CampingGuest.Birthdate.ToShortDateString());
                }
                
                document.Add(campingGuestTable);
            }

    

            document.Close();

            Process.Start("C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe", "file:///" + appData + "\\Downloads\\Reserveringen%20Overzicht.pdf");
        }

        public ICommand CreatePdf => new RelayCommand(ExecuteCreatePdf);
    }
    public class ReservationViewModel
    {
        public Reservation Reservation { get; private init; }

        public ReservationViewModel(Reservation reservation)
        {
            this.Reservation = reservation;
        }

        #region Commands
        public void ExecuteUpdateReservation()
        {
            Console.WriteLine(Reservation.Id);
        }

        public bool CanExecuteUpdateReservation()
        {
            return false;
        }

        public ICommand UpdateReservation => new RelayCommand(ExecuteUpdateReservation, CanExecuteUpdateReservation);


        public void ExecuteDeleteReservation()
        {

        }

        public bool CanExecuteDeleteReservation()
        {
            return false;
        }
        public ICommand DeleteReservation => new RelayCommand(ExecuteDeleteReservation, CanExecuteDeleteReservation);


        #endregion
    }
}
