using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.Business.PathFinding
{

    public class Graph
    {
        public readonly Dictionary<Node, List<(Node, double)>> AdjacencyList;

        public Graph()
        {
            AdjacencyList = [];
        }

        public void AddNode(Node node)
        {
            if (!AdjacencyList.ContainsKey(node))
            {
                AdjacencyList[node] = [];
            }
        }

        public void ConnectNodes(Node from, Node to)
        {
            double distance = from.GetDistanceTo(to);
            ConnectNodes(from, to, distance);
        }

        public void ConnectNodes(Node from, Node to, double distance)
        {
            if (!AdjacencyList.ContainsKey(from))
                AddNode(from);
            if (!AdjacencyList.ContainsKey(to))
                AddNode(to);

            AdjacencyList[from].Add((to, distance));
            AdjacencyList[to].Add((from, distance));
            from.AddNeighbor(to);
        }

        public void ConnectNodeToClosestEdge(Node from)
        {
            var (startEdge, endEdge) = FindClosestEdge(from);
            if (startEdge != null && endEdge != null)
            {
                AddNodeOnEdge(startEdge, endEdge, from);
            }
            else
            {
                Debug.WriteLine("Did not find the closest edge from this node!");
            }
        }

        /// <summary>
        /// Adds new node on the closest position on the edge from the "from" node. 
        /// This from node gets connected to the graph.
        /// </summary>
        /// <param name="newNode"></param>
        /// <param name="edgeStart"></param>
        /// <param name="edgeEnd"></param>
        /// <param name="from"></param>
        public void AddNodeOnEdge(Node edgeStart, Node edgeEnd, Node from)
        {
            double dxLine = edgeEnd.X - edgeStart.X;
            double dyLine = edgeEnd.Y - edgeStart.Y;

            double dxFrom = from.X - edgeStart.X;
            double dyFrom = from.Y - edgeStart.Y;

            double lineLengthSquared = dxLine * dxLine + dyLine * dyLine;
            if (lineLengthSquared == 0) return;

            double t = (dxFrom * dxLine + dyFrom * dyLine) / lineLengthSquared;
            t = Math.Max(0, Math.Min(1, t));

            // Calculate the closest point on the segment to the 'from' node
            double closestX = edgeStart.X + t * dxLine;
            double closestY = edgeStart.Y + t * dyLine;

            Node newNode = new(AdjacencyList.Count, closestX, closestY);

            // Remove old edge
            AdjacencyList[edgeStart].RemoveAll(e => e.Item1 == edgeEnd);
            AdjacencyList[edgeEnd].RemoveAll(e => e.Item1 == edgeStart);

            // Add the new node and connect to edgeStart and edgeEnd (ends up with 2 edges and 3 nodes).
            ConnectNodes(newNode, from);
            ConnectNodes(edgeStart, newNode, edgeStart.GetDistanceTo(newNode));
            ConnectNodes(newNode, edgeEnd, newNode.GetDistanceTo(edgeEnd));

            Debug.WriteLine($"Added node {newNode.ID} at position ({newNode.X}, {newNode.Y}) on the edge between {edgeStart.ID} and {edgeEnd.ID}");
        }

        public (Node?, Node?) FindClosestEdge(Node point)
        {
            double minDistance = double.MaxValue;
            Node? closestStart = null, closestEnd = null;

            foreach (var from in AdjacencyList.Keys)
            {
                foreach (var (to, _) in AdjacencyList[from])
                {
                    double distance = point.GetDistanceToEdge(from, to);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestStart = from;
                        closestEnd = to;
                    }
                }
            }

            return (closestStart, closestEnd);
        }

        public void PrintConnections()
        {
            foreach (var node in AdjacencyList)
            {
                Console.Write(node.Key.ID + " is connected to: ");
                foreach (var connection in node.Value)
                {
                    Console.Write($"{connection.Item1.ID} (Distance: {connection.Item2} meters) ");
                }
                Console.WriteLine();
            }
        }
    }
}
