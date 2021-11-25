using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CampingPlace : IModel
    {
        public int Id { get; private set; }
        public int Number { get; private set; }
        public float Surface { get; private set; }
        public float ExtraNightPrice { get; private set; }
        public string Location { get; private set; }

        public CampingPlaceType Type { get; private set; }

        public CampingPlace(string id, string number, string surface, string extraNightPrice, CampingPlaceType campingPlaceType)
        {
            this.Id = int.Parse(id);
            this.Number = int.Parse(number);
            this.Surface = float.Parse(surface);
            this.ExtraNightPrice = float.Parse(extraNightPrice);
            this.Type = campingPlaceType;
            this.Location = this.GetLocation();
        }
        

        public string GetLocation()
        {
            return $"{this.Type.Accommodation.Prefix}-{this.Number}";
        }

    }
}
