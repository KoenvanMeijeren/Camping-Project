using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CampingPlaceData
    {
        public string Type { get; private set; }
        public int Personen { get; private set; }
        public string Oppervlakte { get; private set; }
        public string Dagtarief { get; private set; }
        public string Location { get; private set; }

        public CampingPlaceData(string type, int personen, int oppervlakte, int dagtarief, int placenumber, string prefix)
        {
            this.Type = type;
            this.Personen = personen;
            this.Oppervlakte = oppervlakte + " m2";
            this.Dagtarief = "€" + dagtarief;
            this.Location = prefix + "-" + placenumber;
        }
    }
}
