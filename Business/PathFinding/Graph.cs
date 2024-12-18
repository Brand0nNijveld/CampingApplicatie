using System;
using System.Collections.Generic;
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
