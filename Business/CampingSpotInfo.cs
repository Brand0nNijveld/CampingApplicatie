using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.Business
{
    public class CampingSpotInfo
    {

        public int ID { get; private set; }

        public double PricePerNight { get; private set; }
        public double Size { get; private set; }

        public CampingSpotInfo(int ID, double PricePerNight, double Size) {
            this.ID = ID;
            this.PricePerNight = PricePerNight;
            this.Size = Size;
        }
    }
}
