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
        public static string FilterAccommodationType { get; set; }


        public static List<CampingPlaceViewData> Select()
        {
            CampingPlaceViewDataCollection._collection = new List<CampingPlaceViewData>();
            var campingPlaces = (new Query(CampingPlace.BaseQuery())).Select();
            foreach (Dictionary<string, string> dictionary in campingPlaces)
            {
                CampingPlaceViewDataCollection._collection.Add(CampingPlaceViewDataCollection.ToModel(dictionary));
            }

            return CampingPlaceViewDataCollection._collection;
        }


        public static CampingPlaceViewData ToModel(Dictionary<string, string> dictionary)
        {
            return new CampingPlaceViewData(CampingPlace.ToModel(dictionary));
        }
        
        public static CampingPlace ToCampingPlaceModel(Dictionary<string, string> dictionary)
        {
            return CampingPlace.ToModel(dictionary);
        }

        public static List<CampingPlaceViewData> ToFilteredOnReservedCampingPitches(List<CampingPlaceViewData> campingPlaceViewDatas, DateTime checkinDate, DateTime checkoutDate)
        {
            var reservations = (new Query(FilterReservedQuery())).Select();

            foreach (Dictionary<string, string> dictionary in reservations)
            {
                dictionary.TryGetValue("CampingPlaceID", out string CampingPlaceID);
                dictionary.TryGetValue("CheckinDatetime", out string checkinDatetime);
                dictionary.TryGetValue("CheckoutDatetime", out string checkoutDatetime);

                int.TryParse(CampingPlaceID, out int campingPlaceNumber);
                DateTime.TryParse(checkinDatetime, out DateTime checkinDateCurrent);
                DateTime.TryParse(checkoutDatetime, out DateTime checkoutDateCurrent);

                if (checkinDateCurrent.Date < checkoutDate.Date && checkinDate.Date < checkoutDateCurrent.Date)
                {
                    campingPlaceViewDatas = campingPlaceViewDatas.Where(campingPlaceViewData => campingPlaceViewData.GetId() != campingPlaceNumber).ToList();
                } 

            }

            return campingPlaceViewDatas;
        }

        private static string FilterReservedQuery()
        {
            string query = "SELECT CampingPlaceID, CheckinDatetime, CheckoutDatetime FROM Reservation R ";
            query += "INNER JOIN ReservationDuration RD ON RD.ReservationDurationID = R.ReservationDurationID";

            return query;
        }

    }
}
