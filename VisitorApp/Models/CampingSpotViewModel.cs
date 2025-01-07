using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.VisitorApp.Models
{
    public class CampingSpotViewModel
    {
        public int ID { get; private set; }
        public int PositionX { get; private set; }
        public int PositionY { get; private set; }
        public bool Available { get; set; }

        public CampingSpotViewModel(int id, int positionX, int positionY)
        {
            this.ID = id;
            this.PositionX = positionX;
            this.PositionY = positionY;
        }
    }
}
