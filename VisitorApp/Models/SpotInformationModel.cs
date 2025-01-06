using CampingApplication.Business;
using CampingApplication.Business.PathService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.VisitorApp.Models
{
    public class SpotInformationModel(CampingSpot spot) : BaseModel
    {
        private CampingSpot campingSpot = spot;
        public CampingSpot CampingSpot
        {
            get => campingSpot;
            set
            {
                campingSpot = value;
                OnPropertyChanged(nameof(CampingSpot));
            }
        }

        private List<FacilityRouteModel> routes = [];
        public List<FacilityRouteModel> Routes
        {
            get => routes;
            set
            {
                routes = value;
                OnPropertyChanged(nameof(Routes));
            }
        }

        private FacilityRouteModel? hoveredFacilityRoute;
        public FacilityRouteModel? HoveredFacilityRoute
        {
            get => hoveredFacilityRoute;
            set
            {
                hoveredFacilityRoute = value;
                OnPropertyChanged(nameof(HoveredFacilityRoute));
            }
        }
    }
}
