using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.Business
{
    public abstract class MapEntity(int ID, double x, double y)
    {
        public int ID { get; private set; } = ID;
        public double XCoordinate { get; private set; } = x;
        public double YCoordinate { get; private set; } = y;
    }
}
