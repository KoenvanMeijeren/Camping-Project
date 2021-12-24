using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.EventArguments;

namespace ViewModel
{
    public static class CurrentCamping
    {
        #region Properties
        public static Camping Camping { get; private set; }
        #endregion

        #region Events
        public static event EventHandler<UpdateModelEventArgs<Camping>> CurrentCampingSetEvent;
        #endregion

        #region Commmands
        public static void SetCurrentCamping(Camping camping)
        {
            CurrentCamping.Camping = camping;
            CurrentCampingSetEvent?.Invoke(null, new UpdateModelEventArgs<Camping>(camping, false, false));
        }
        #endregion
    }
}
