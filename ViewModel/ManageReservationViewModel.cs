using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Model;

namespace ViewModel
{
    public class ManageReservationViewModel : ObservableObject
    {
        private Reservation _selectedReservation;
        public Reservation SelectedReservation
        {
            get
            {
                return this._selectedReservation;
            }
            set
            {
                if (Equals(value, this._selectedReservation))
                {
                    return;
                }

                this._selectedReservation = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public ManageReservationViewModel()
        {
            ReservationCollectionViewModel.ManageReservationEvent += this.OnManageReservationEvent;
        }

        private void OnManageReservationEvent(object sender, ReservationEventArgs args)
        {
            SelectedReservation = args.Reservation;
        }
    }
}
