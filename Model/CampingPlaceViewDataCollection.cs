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
            if (_collection != null)
            {
                return _collection;
            }

            _collection = new List<CampingPlaceViewData>();
            var campingPlaces = (new Query(CampingPlace.BaseQuery())).Select();
            foreach (Dictionary<string, string> dictionary in campingPlaces)
            {
                _collection.Add(CampingPlaceViewDataCollection.ToModel(dictionary));
            }

            return _collection;
        }

        public static CampingPlaceViewData ToModel(Dictionary<string, string> dictionary)
        {
            return new CampingPlaceViewData(CampingPlace.ToModel(dictionary));
        }
    }
}
