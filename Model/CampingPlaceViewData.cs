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

        public string Nummer { get; private set; }
        public int Personen { get; private set; }
        public string Oppervlakte { get; private set; }
        public string Dagtarief { get; private set; }

        public CampingPlaceViewData(CampingPlace campingPlace)
        {
            this.Type = campingPlace.Type.Accommodation.Name;
            this.Nummer = campingPlace.Type.Accommodation.Prefix + " " + campingPlace.Number;
            this.Personen = campingPlace.Type.guestLimit;
            this.Oppervlakte = campingPlace.Surface + " m2";
            this.Dagtarief = "€" + (campingPlace.Type.StandardNightPrice + campingPlace.ExtraNightPrice);
        }
    }
}
