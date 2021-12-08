using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Toolkit.Mvvm.Input;
using Model;

namespace ViewModel
{
    public class ReservationCollectionViewModel
    {
        public ObservableCollection<ReservationViewModel> Reservations { get; private set; }

        public ReservationCollectionViewModel()
        {
            this.Reservations = new ObservableCollection<ReservationViewModel>();
            
            var reservationModel = new Reservation();
            foreach (var reservation in reservationModel.Select())
            {
                this.Reservations.Add(new ReservationViewModel(reservation));
            }
            
            ReservationCustomerFormViewModel.ReservationConfirmedEvent += this.OnReservationConfirmedEvent;
        }

        private void OnReservationConfirmedEvent(object sender, ReservationEventArgs args)
        {
            this.Reservations.Add(new ReservationViewModel(args.Reservation));
        }

        private void ExecuteCreatePdf()
        {
            var reservationModel = new Reservation();
            var campingGuestModel = new CampingGuest();
            var reservationCampingGuest = new ReservationCampingGuest();

            Document document = new Document(PageSize.A4, 36, 36, 70, 36);
            PageEvents pageEvents = new PageEvents();
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            PdfWriter pdfWriter = PdfWriter.GetInstance(document, new FileStream(appData + "\\Downloads\\Reserveringen Overzicht.pdf", FileMode.Create));
            pdfWriter.PageEvent = pageEvents;
            document.Open();

            foreach (var reservation in reservationModel.Select())
            {
                document.Add(new Paragraph(" "));
                float[] columnWidths = { 3, 5, 5, 8, 8, 5, 6 };
                PdfPTable reservationTable = new PdfPTable(columnWidths);

                reservationTable.AddCell("ID");
                reservationTable.AddCell("Camping plaats");
                reservationTable.AddCell("Naam");
                reservationTable.AddCell("Checkin-datum");
                reservationTable.AddCell("Checkout-datum");
                reservationTable.AddCell("Prijs");
                reservationTable.AddCell("Aanwezig");

                reservationTable.AddCell(reservation.Id.ToString());
                reservationTable.AddCell(reservation.CampingPlace.ToString());
                reservationTable.AddCell(reservation.CampingCustomer.FirstName.ToString() + " " + reservation.CampingCustomer.LastName.ToString());
                reservationTable.AddCell(reservation.Duration.CheckInDate.ToString());
                reservationTable.AddCell(reservation.Duration.CheckOutDate.ToString());
                reservationTable.AddCell(" €" + reservation.TotalPrice.ToString());
                reservationTable.AddCell(" ");

                document.Add(reservationTable);

                
                //Should be used for the CampingGuest table
                /*foreach (var reservationGuest in reservationCampingGuest.Select())
                {
                    PdfPTable campingGuestTable = new PdfPTable(3);

                    campingGuestTable.AddCell("ID");
                    campingGuestTable.AddCell("Naam");
                    campingGuestTable.AddCell("Geboortedatum");

                    campingGuestTable.AddCell(reservationGuest.CampingGuest.Id.ToString());
                    campingGuestTable.AddCell(reservationGuest.CampingGuest.FirstName.ToString() + " " + reservationGuest.CampingGuest.LastName.ToString());
                    campingGuestTable.AddCell(reservationGuest.CampingGuest.Birthdate.ToShortDateString());

                    document.Add(campingGuestTable);

                }*/
            }


            document.Close();


            var prs = new ProcessStartInfo("C:\\Program Files\\Internet Explorer\\iexplore.exe")
            {
                Arguments = "C:\\Users\\Henk\\Downloads\\Reserveringen Overzicht.pdf"
            };
            Process.Start(prs);
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
    }
}
