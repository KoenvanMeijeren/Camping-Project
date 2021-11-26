using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Model;

namespace Visualisation.Model
{
    public class ReservationDataContext : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public List<Reservation> Reservations { get; private set; }

        public ReservationDataContext()
        {
            MainWindow.ReservationsChanged += this.MainWindowOnReservationsChanged;
        }

        private void MainWindowOnReservationsChanged(object? sender, ReservationEventArgs eventArgs)
        {
            this.Reservations = eventArgs.Reservations;

            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Reservations"));
        }
    }
}