using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ViewModel
{
    public class ReservationCustomerFormViewModel : ObservableObject
    {
        private string _firstname;

        public string Firstname
        {
            get => this._firstname;
            set
            {
                if (Equals(value, this._firstname))
                {
                    return;
                }

                this._firstname = value;
/*                this.SetOverview();
*/
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public ReservationCustomerFormViewModel()
        {
            
        }
        
    }
}
