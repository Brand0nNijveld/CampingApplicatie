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

    public class Facility : MapEntity
    {
        public FacilityType Type { get; private set; }

        public Facility(int ID, double posX, double posY, FacilityType type) : base(ID, posX, posY)
        {
            this.Type = type;
        }

        public static string TypeToString(FacilityType type)
        {
            switch (type)
            {
                case FacilityType.Reception:
                    return "Receptie";
                case FacilityType.Restroom:
                    return "Toilet";
                case FacilityType.Shower:
                    return "Douches";
                case FacilityType.Playground:
                    return "Speeltuin";
                default:
                    return "?";
            }
        }

        public Facility(int ID, int posX, int posY, string type) : base(ID, posX, posY)
        {
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
