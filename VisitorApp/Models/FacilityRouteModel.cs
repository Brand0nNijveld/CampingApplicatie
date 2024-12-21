using CampingApplication.Business;
using CampingApplication.Business.PathFinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.VisitorApp.Models
{
    public class FacilityRouteModel : BaseModel
    {
        public Facility Facility { get; private set; }
        public Route Route { get; private set; }

        public string WalkDuration
        {
            get => FormatDuration();
            set
            {
                OnPropertyChanged(nameof(WalkDuration));
            }
        }

        public string Distance
        {
            get => FormatDistance();
            set
            {
                OnPropertyChanged(nameof(Distance));
            }
        }

        private string type = "";
        public string Type
        {
            get => type;
            set
            {
                type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        private string facilityName = "";
        public string FacilityName
        {
            get => facilityName;
            set
            {
                facilityName = value;
                OnPropertyChanged(nameof(FacilityName));
            }
        }

        public FacilityRouteModel(Facility facility, Route route)
        {
            this.Facility = facility;
            this.Route = route;
            Type = facility.Type.ToString();
            FacilityName = Facility.TypeToString(facility.Type);
        }

        private string FormatDistance()
        {
            int meters = (int)Math.Floor(Route.TotalDistance);
            return $"{meters}m";
        }

        private string FormatDuration()
        {
            double distanceInKilometers = Route.TotalDistance / 1000;
            double durationInHours = distanceInKilometers / 5;
            double durationInMinutes = durationInHours * 60;

            int roundedMin = (int)Math.Round(durationInMinutes);
            if (roundedMin < 1)
            {
                return $"<1 min";
            }

            return $"{roundedMin} min";
        }
    }
}
