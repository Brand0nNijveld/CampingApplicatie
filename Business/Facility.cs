using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.Business
{
    public enum FacilityType
    {
        Reception,
        Restroom,
        Shower,
        Playground,
        Unknown,
    }

    public class Facility
    {
        public int ID { get; private set; }
        public FacilityType Type { get; private set; }
        public int PositionX { get; private set; }
        public int PositionY { get; private set; }

        public Facility(int ID, FacilityType type, int posX, int posY)
        {
            this.ID = ID;
            this.Type = type;
            this.PositionX = posX;
            this.PositionY = posY;
        }

        public Facility(int ID, string type, int posX, int posY)
        {
            this.ID = ID;
            this.PositionX = posX;
            this.PositionY = posY;

            switch (type.ToLower())
            {
                case "reception":
                    this.Type = FacilityType.Reception;
                    break;
                case "restroom":
                    this.Type = FacilityType.Restroom;
                    break;
                case "shower":
                    this.Type = FacilityType.Shower;
                    break;
                case "playground":
                    this.Type = FacilityType.Playground;
                    break;
                default:
                    this.Type = FacilityType.Unknown;
                    Debug.WriteLine($"Assigned unknown type to facility! ({type})");
                    break;
            };
        }
    }
}
