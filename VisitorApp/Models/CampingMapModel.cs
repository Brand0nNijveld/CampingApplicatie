using CampingApplication.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.VisitorApp.Models
{
    public class CampingMapModel : BaseModel
    {
        private Dictionary<int, CampingSpot> campingSpots = [];
        public Dictionary<int, CampingSpot> CampingSpots
        {
            get => campingSpots; set
            {
                campingSpots = value;
                OnPropertyChanged(nameof(CampingSpots));
            }
        }

        private List<Facility> facilities = [];
        public List<Facility> Facilities
        {
            get => facilities; set
            {
                facilities = value;
                OnPropertyChanged(nameof(Facilities));
            }
        }

        private SpotInformationModel? selectedCampingSpot;
        public SpotInformationModel? SelectedCampingSpot
        {
            get => selectedCampingSpot;
            set
            {
                if (value != selectedCampingSpot)
                {
                    selectedCampingSpot = value;
                    OnPropertyChanged(nameof(SelectedCampingSpot));
                }
            }
        }

        private Dictionary<int, CampingSpot>? availableCampingSpots;
        public Dictionary<int, CampingSpot>? AvailableCampingSpots
        {
            get => availableCampingSpots;
            set
            {
                availableCampingSpots = value;
                OnPropertyChanged(nameof(AvailableCampingSpots));
            }
        }

        private DateTime startDate;
        public DateTime StartDate
        {
            get => startDate;
            set
            {
                startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        private DateTime endDate;
        public DateTime EndDate
        {
            get => endDate;
            set
            {
                endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }
    }
}
