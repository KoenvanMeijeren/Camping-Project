using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CampingField
    {
        public int LocationNumber { get; set; }
        public String BackgroundColor { get; set; }
        public String ImageResource { get; set; }

        public CampingField(int locationNumber, string backgroundColor, string imageResource)
        {
            this.LocationNumber = locationNumber;
            this.BackgroundColor = backgroundColor;
            this.ImageResource = imageResource;
        }
    }
}
