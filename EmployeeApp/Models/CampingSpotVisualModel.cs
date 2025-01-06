using CampingApplication.Client.Shared.Helpers;
using CampingApplication.EmployeeApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.EmployeeApp.Models
{
    public class CampingSpotVisualModel
    {
        public int ID { get; private set; }
        public int PositionX { get; private set; }
        public int PositionY { get; private set; }
        public bool Available { get; set; }

        public CampingSpotVisualModel(int id, double positionX, double positionY)
        {
            this.ID = id;
            int pixelsPerMeter = CampingMapViewModel.PIXELS_PER_METER;
            var (x, y) = MapConversionHelper.MetersToPixels(positionX, positionY, pixelsPerMeter);
            this.PositionX = x;
            this.PositionY = y;
        }
    }
}
