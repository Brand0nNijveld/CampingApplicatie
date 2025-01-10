using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.Business.PathService
{
    public class Node(int id, double x, double y)
    {
        public int ID { get; set; } = id;
        public double X { get; set; } = x;
        public double Y { get; set; } = y;

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

        public double GetDistanceToEdge(Node first, Node second)
        {
            // Vector from first to second (line segment direction)
            double dxLine = second.X - first.X;
            double dyLine = second.Y - first.Y;

            // Vector from first to the point (this node)
            double dxPoint = this.X - first.X;
            double dyPoint = this.Y - first.Y;

            // Compute the dot product of the point vector and the line vector
            double lineLengthSquared = dxLine * dxLine + dyLine * dyLine;

            if (lineLengthSquared == 0) return GetDistanceTo(first);  // If both points are the same, return distance to first

            // Projection scalar of point onto line segment
            double t = (dxPoint * dxLine + dyPoint * dyLine) / lineLengthSquared;

            // Clamp t to the segment [0, 1] (ensures we stay on the segment)
            t = Math.Max(0, Math.Min(1, t));

            // Calculate the closest point on the segment
            double closestX = first.X + t * dxLine;
            double closestY = first.Y + t * dyLine;

            // Return the distance from the point to the closest point on the segment
            return GetDistanceTo(new Node(-1, closestX, closestY));
        }

    }
}
