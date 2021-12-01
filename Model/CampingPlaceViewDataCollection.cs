using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemCore;

namespace Model
{
    public static class CampingPlaceViewDataCollection
    {
        private static List<CampingPlaceViewData> _collection;

        public static List<CampingPlaceViewData> Select()
        {
            CampingPlace campingPlaceModel = new CampingPlace();
            
            CampingPlaceViewDataCollection._collection = new List<CampingPlaceViewData>();
            var campingPlaces = campingPlaceModel.Select();
            foreach (CampingPlace campingPlace in campingPlaces)
            {
                CampingPlaceViewDataCollection._collection.Add(new CampingPlaceViewData(campingPlace));
            }

            return CampingPlaceViewDataCollection._collection;
        }

        public static List<CampingPlaceViewData> ToFilteredOnReservedCampingPitches(List<CampingPlaceViewData> viewData, DateTime checkinDate, DateTime checkoutDate)
        {
            Reservation reservationModel = new Reservation();
            
            var reservations = reservationModel.Select();
            foreach (Reservation reservation in reservations)
            {
                ReservationDuration reservationDuration = reservation.Duration;
                if (reservationDuration.CheckInDatetime.Date < checkoutDate.Date && checkinDate.Date < reservationDuration.CheckOutDatetime.Date)
                {
                    viewData = viewData.Where(campingPlaceViewData => campingPlaceViewData.GetId() != reservation.CampingPlace.Id).ToList();
                }
            }

            return viewData;
        }

    }
}
