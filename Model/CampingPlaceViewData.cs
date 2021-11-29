using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CampingPlaceViewData
    {

        public string Type { get; private set; }

        public string Locatie { get; private set; }
        public int Personen { get; private set; }
        public string Oppervlakte { get; private set; }
        public string Dagtarief { get; private set; }
        private int _id { get; set; }
        private float _dagtarief { get; set; }

        public CampingPlaceViewData(CampingPlace campingPlace)
        {
            this.Type = campingPlace.Type.Accommodation.Name;
            this.Locatie = campingPlace.Type.Accommodation.Prefix + " " + campingPlace.Number;
            this.Personen = campingPlace.Type.guestLimit;
            this.Oppervlakte = campingPlace.Surface + " m2";
            this.Dagtarief = "€" + (campingPlace.Type.StandardNightPrice + campingPlace.ExtraNightPrice);

            this._id = campingPlace.Id;
            this._dagtarief = campingPlace.Type.StandardNightPrice + campingPlace.ExtraNightPrice;
        }

        public int GetId() 
        {
            return _id;
        }

        public float GetNumericNightPrice()
        {
            return _dagtarief;
        }
    }
}
