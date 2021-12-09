using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Model;

namespace ViewModel
{
    public class ReservationCollectionViewModel : ObservableObject
    {
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


        public ReservationCollectionViewModel()
        {
            this.Reservations = new ObservableCollection<ReservationViewModel>();
            
            var reservationModel = new Reservation();
            foreach (var reservation in reservationModel.Select())
            {
                this.Reservations.Add(new ReservationViewModel(reservation));
            }
            
            ReservationCustomerFormViewModel.ReservationConfirmedEvent += this.OnReservationConfirmedEvent;
            ManageReservationViewModel.UpdateReservationCollection += OnReservationConfirmedEvent;
        }

        private void OnReservationConfirmedEvent(object sender, ReservationEventArgs args)
        {
            while (this.Reservations.Any())
            {
                this.Reservations.RemoveAt(0);
            }
            
            var reservationModel = new Reservation();
            foreach (var reservation in reservationModel.Select())
            {
                this.Reservations.Add(new ReservationViewModel(reservation));
            }
        }

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
