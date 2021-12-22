using System;
using Model;

namespace ViewModel.EventArguments
{
    public class UpdateCampingPlaceEventArgs : EventArgs
    {
        public CampingPlace CampingPlace { get; private init; }
        
        public bool Inserted { get; private init; }
        
        public bool Removed { get; private init; }

        public UpdateCampingPlaceEventArgs(CampingPlace campingPlace, bool inserted, bool removed)
        {
            this.CampingPlace = campingPlace;
            this.Inserted = inserted;
            this.Removed = removed;
        }
    }
}