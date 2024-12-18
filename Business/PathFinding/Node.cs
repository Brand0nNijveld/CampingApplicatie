using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.Business.PathFinding
{
    public class Node(int id, double x, double y)
    {
        public int ID { get; set; } = id;
        public double X { get; private set; } = x;
        public double Y { get; private set; } = y;

        public List<Node> Neighbors { get; set; } = [];

        public void AddNeighbor(Node neighbor)
        {
            if (!Neighbors.Contains(neighbor))
            {
                Neighbors.Add(neighbor);
                neighbor.AddNeighbor(this);
            }
        }

        public double GetDistanceTo(Node other)
        {
            return Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));
        }
    }
}
