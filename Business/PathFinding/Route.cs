using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.Business.PathFinding
{
    public class Route
    {
        public List<Node> Nodes { get; set; }
        public double TotalDistance { get; set; }

        public Route(List<Node> nodes)
        {
            Nodes = nodes;
        }
    }
}
