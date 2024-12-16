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
        
        public bool Pets { get; private set; }
        public bool Electricity { get; private set; }
        //public string Type { get; private set; }


        public CampingSpotInfo(int ID, double PricePerNight, bool Pets, bool Electricity)
        {
            this.ID = ID;
            this.PricePerNight = PricePerNight;
            this.Pets = Pets;
            this.Electricity = Electricity;
            //this.Type = Type;
        }
    }
}
