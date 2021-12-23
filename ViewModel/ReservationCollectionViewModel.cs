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
using ViewModel.EventArguments;

namespace ViewModel
{
    public class ReservationCollectionViewModel : ObservableObject
    {
        #region Fields
        private readonly Accommodation _accommodationModel = new Accommodation();
        private readonly Reservation _reservationModel = new Reservation();
        
        private const string SelectAll = "Alle";

        private readonly ObservableCollection<string> _accommodations;

        private Reservation _selectedReservation;

        private DateTime _checkOutDate, _checkInDate;
        private string _minTotalPrice, _maxTotalPrice, _selectedAccommodation, _guests;

        #endregion

        #region Properties
        public ObservableCollection<Reservation> Reservations { get; private set; }

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

                if (daysDifference > 0)
                {
                    this.CheckOutDate = this._checkInDate.AddDays(daysDifference);
                }
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

                if (this._checkOutDate < this.CheckInDate)
                {
                    this.CheckInDate = this._checkOutDate.AddDays(-1);
                }
            }
        }
        
        public ObservableCollection<string> Accommodations
        {
            get => this._accommodations;
            private init
            {
                if (Equals(value, this._accommodations))
                {
                    return;
                }
                
                this._accommodations = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string SelectedAccommodation
        {
            get => this._selectedAccommodation;
            set
            {
                if (Equals(value, this._selectedAccommodation))
                {
                    return;
                }

                this._selectedAccommodation = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
                
                this.SetOverview();
            }
        }
        
        public Reservation SelectedReservation
        {
            get => _selectedReservation;
            set
            {
                if (value == null || Equals(value, this._selectedReservation))
                {
                    return;
                }

                this._selectedReservation = value;
                ManageReservationEvent?.Invoke(this, new ReservationEventArgs(this._selectedReservation));
            }
        }

        #endregion

        #region Events

        public static event EventHandler<ReservationEventArgs> ManageReservationEvent;

        #endregion

        #region View construction

        public ReservationCollectionViewModel()
        {
            this.Reservations = new ObservableCollection<Reservation>();
            this._accommodations = new ObservableCollection<string>();
            
            this.InitializeAccommodations();
            DateTime date = DateTime.Today;
            this._checkInDate = new DateTime(date.Year, date.Month, 1);
            this._checkOutDate = this.CheckInDate.AddMonths(1).AddDays(-1);
            
            // This calls the on property changed event.
            this.SelectedAccommodation = SelectAll;

            ReservationPaymentViewModel.ReservationConfirmedEvent += this.OnReservationConfirmedEvent;
            ManageAccommodationViewModel.AccommodationStringsUpdated += this.ManageAccommodationViewModelOnAccommodationsUpdated;
            ManageReservationViewModel.ReservationUpdated += this.ManageReservationViewModelOnReservationUpdated;
        }

        private void ManageReservationViewModelOnReservationUpdated(object sender, UpdateModelEventArgs<Reservation> e)
        {
            e.UpdateCollection(this.Reservations);
            
            this._selectedReservation = e.Model;
            this.OnPropertyChanged(new PropertyChangedEventArgs(null));
        }

        private void ManageAccommodationViewModelOnAccommodationsUpdated(object sender, UpdateModelEventArgs<Accommodation> e)
        {
            if (e.Inserted)
            {
                this.Accommodations.Add(e.Model.ToString());
            }
            else if (e.Removed)
            {
                this.Accommodations.Remove(e.Model.ToString());
            }
            
            // This calls the on property changed event.
            this.SelectedAccommodation = SelectAll;
        }

        private void OnReservationConfirmedEvent(object sender, UpdateModelEventArgs<Reservation> args)
        {
            args.UpdateCollection(this.Reservations);
        }

        /// <summary>
        /// Sets the available accommodations. Calling this method should be avoided, because this is a heavy method.
        /// </summary>
        private void InitializeAccommodations()
        {
            this.Accommodations.Clear();
            
            this.Accommodations.Add(SelectAll);
            foreach (var accommodation in this.GetAccommodations())
            {
                this.Accommodations.Add(accommodation.ToString());
            }
        }

        private void SetOverview()
        {
            this.Reservations.Clear();

            bool ReservationsFilter(Reservation reservation) => 
                (this._selectedAccommodation != null && (this._selectedAccommodation.Equals(SelectAll) || reservation.CampingPlace.Type.Accommodation.Name.Equals(this._selectedAccommodation)))
                && (!int.TryParse(this.MinTotalPrice, out int min) || reservation.TotalPrice >= min) 
                && (!int.TryParse(this.MaxTotalPrice, out int max) || reservation.TotalPrice <= max) 
                && (this.CheckInDate == DateTime.MinValue || reservation.CheckInDatetime >= this.CheckInDate)
                && (this.CheckOutDate == DateTime.MinValue || reservation.CheckOutDatetime <= this.CheckOutDate)
                && (!int.TryParse(this.Guests, out int guests) || reservation.NumberOfPeople >= guests);

            var reservationItems = this.GetReservations().Where(ReservationsFilter);
            foreach (var reservation in reservationItems)
            {
                this.Reservations.Add(reservation);
            }
        }

        #endregion

        #region Commands

        private void ExecuteCreatePdf()
        {
            Document document = new Document(PageSize.A4, 36, 36, 70, 36);
            PageEvents pageEvents = new PageEvents();
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            PdfWriter pdfWriter = PdfWriter.GetInstance(document, new FileStream(appData + "\\Downloads\\Reserveringen Overzicht.pdf", FileMode.Create));
            pdfWriter.PageEvent = pageEvents;
            document.Open();

            document.Add(new Paragraph("     "));
            foreach (var reservation in this.Reservations)
            {
                float[] columnWidths = { 3, 5, 8, 8, 8, 5, 6 };
                PdfPTable reservationTable = new PdfPTable(columnWidths);

                document.Add(new Paragraph("\n"));
                reservationTable.AddCell("ID");
                reservationTable.AddCell("Verblijf");
                reservationTable.AddCell("Klantnaam");
                reservationTable.AddCell("Begindatum");
                reservationTable.AddCell("Einddatum");
                reservationTable.AddCell("Prijs");
                reservationTable.AddCell("Aanwezig");

                reservationTable.AddCell(reservation.Id.ToString());
                reservationTable.AddCell(reservation.CampingPlace.ToString());
                reservationTable.AddCell(reservation.CampingCustomer.FirstName + " " + reservation.CampingCustomer.LastName);
                reservationTable.AddCell(reservation.CheckInDate);
                reservationTable.AddCell(reservation.CheckOutDate);
                reservationTable.AddCell(" €" + reservation.TotalPrice.ToString(CultureInfo.InvariantCulture));
                reservationTable.AddCell(" ");

                document.Add(reservationTable);

                //Should be used for the CampingGuest table
                var campingGuests = reservation.CampingGuests;
                if (!campingGuests.Any())
                {
                    continue;
                }
                
                PdfPTable campingGuestTable = new PdfPTable(2);

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

        #endregion

        #region Database interaction

        public virtual IEnumerable<Reservation> GetReservations()
        {
            return this._reservationModel.Select();
        }
        
        public virtual IEnumerable<Accommodation> GetAccommodations()
        {
            return this._accommodationModel.Select();
        }

        #endregion
    }
}
