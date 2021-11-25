using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CampingPlace
    {
        public int Id { get; private set; }
        public int Number { get; private set; }
        public float Surface { get; private set; }
        public float ExtraNightPrice { get; private set; }
        
        public CampingPlaceType Type { get; private set; }

        public CampingPlace(int id, int number, float surface, float extraNightPrice, CampingPlaceType campingPlaceType)
        {
            this.Id = id;
            this.Number = number;
            this.Surface = surface;
            this.ExtraNightPrice = extraNightPrice;
            this.Type = campingPlaceType;
        }
        
    }
}
