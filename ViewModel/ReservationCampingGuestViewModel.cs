using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Model;

namespace ViewModel
{
    public class ReservationCampingGuestViewModel : ObservableObject
    {
        public static event EventHandler<ReservationEventArgs> ReservationConfirmEvent;
        public static event EventHandler<ReservationDurationEventArgs> ReservationGoBackEvent;

        private void ExecuteCustomerGuestReservation()
        {
           ReservationConfirmEvent?.Invoke(this, new ReservationEventArgs(new Reservation()));
        }

        private void ExecuteCutomerGuestGoBackReservation()
        {
            ReservationGoBackEvent?.Invoke(this, new ReservationDurationEventArgs(new CampingPlace(), new ReservationDuration()));
        }

        public ICommand AddCustomerReservation => new RelayCommand(ExecuteCustomerGuestReservation);

        public ICommand CutomerGuestGoBackReservation => new RelayCommand(ExecuteCutomerGuestGoBackReservation);
    }
}
