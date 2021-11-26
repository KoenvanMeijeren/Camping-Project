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
        
        private static List<string> _locations;
        private static List<string> _campingPlaceTypes;


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
        
        public static List<string> SelectLocations()
        {
            CampingPlaceViewDataCollection._locations = new List<string>();
            var campingPlaces = (new Query(CampingPlace.BaseQuery())).Select();
            foreach (Dictionary<string, string> dictionary in campingPlaces)
            {
                var campingPlace = CampingPlaceViewDataCollection.ToCampingPlaceModel(dictionary);
                
                CampingPlaceViewDataCollection._locations.Add(campingPlace.Location);
            }
            
            return CampingPlaceViewDataCollection._locations;
        }


        public static CampingPlaceViewData ToModel(Dictionary<string, string> dictionary)
        {
            return new CampingPlaceViewData(CampingPlace.ToModel(dictionary));
        }
        
        public static CampingPlace ToCampingPlaceModel(Dictionary<string, string> dictionary)
        {
            return CampingPlace.ToModel(dictionary);
        }
        
    }
}
