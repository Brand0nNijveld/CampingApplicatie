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
        Pool,
        Unknown,
    }

    public class Facility : MapEntity
    {
        public FacilityType Type { get; private set; }

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
                case FacilityType.Pool:
                    return "Zwembad";
                default:
                    return "?";
            }
        }

        private FacilityType ParseFacilityType(string type)
        {
            switch (type.ToLower())
            {
                case "reception":
                    return FacilityType.Reception;
                case "restroom":
                    return FacilityType.Restroom;
                case "shower":
                    return FacilityType.Shower;
                case "playground":
                    return FacilityType.Playground;
                case "pool":
                    return FacilityType.Pool;
                default:
                    Debug.WriteLine($"Assigned unknown type to facility! ({type})");
                    return FacilityType.Unknown;
            }
        }

        public Facility(int ID, double posX, double posY, string type) : base(ID, posX, posY)
        {
            this.Type = ParseFacilityType(type);
        }
    }
}
