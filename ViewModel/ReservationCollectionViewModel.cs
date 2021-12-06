using System.Collections.ObjectModel;
using System.Windows.Input;
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
