using System;
using System.Collections.Generic;

namespace CampingApplication.Business.PathService
{
    public class Dijkstra
    {
        public static Route FindShortestPath(Graph graph, Node startNode, Node endNode)
        {
            graph.PrintConnections();
            // Distance dictionary: maps nodes to their current shortest distance from the startNode
            var distances = new Dictionary<Node, double>();
            var previousNodes = new Dictionary<Node, Node?>(); // To reconstruct the path
            var priorityQueue = new SortedSet<(double Distance, Node Node)>(Comparer<(double Distance, Node Node)>.Create((a, b) =>
            {
                int compare = a.Distance.CompareTo(b.Distance);
                if (compare == 0) return a.Node.ID.CompareTo(b.Node.ID); // Tie breaker for nodes with equal distances
                return compare;
            }));

            // Initialize distances and priority queue
            foreach (var node in graph.AdjacencyList.Keys)
            {
                distances[node] = double.MaxValue;
                previousNodes[node] = null;
            }
            distances[startNode] = 0;
            priorityQueue.Add((0, startNode));

            while (priorityQueue.Count > 0)
            {
                // Extract the node with the smallest distance
                var (currentDistance, currentNode) = priorityQueue.Min;
                priorityQueue.Remove(priorityQueue.Min);

                // Stop the algorithm once the endNode is reached
                if (currentNode == endNode)
                {
                    return ReconstructRoute(previousNodes, distances[endNode], startNode, endNode);
                }

                // Iterate through neighbors of the current node
                foreach (var (neighbor, weight) in graph.AdjacencyList[currentNode])
                {
                    double tentativeDistance = currentDistance + weight;

                    if (tentativeDistance < distances[neighbor])
                    {
                        // Update the priority queue
                        if (priorityQueue.Contains((distances[neighbor], neighbor)))
                        {
                            priorityQueue.Remove((distances[neighbor], neighbor));
                        }

                        // Update the shortest distance and previous node
                        distances[neighbor] = tentativeDistance;
                        previousNodes[neighbor] = currentNode;

                        // Add the neighbor with the updated distance
                        priorityQueue.Add((tentativeDistance, neighbor));
                    }
                }
            }

            // If we reach here, no path exists between startNode and endNode
            throw new Exception($"No path exists between Node {startNode.ID} and Node {endNode.ID}");
        }

        private static Route ReconstructRoute(Dictionary<Node, Node?> previousNodes, double totalDistance, Node startNode, Node endNode)
        {
            var pathNodes = new List<Node>();
            var currentNode = endNode;

            // Reconstruct the path from endNode to startNode
            while (currentNode != null)
            {
                pathNodes.Add(currentNode);
                currentNode = previousNodes[currentNode];
            }

            pathNodes.Reverse(); // Reverse the path to get start-to-end order

            return new Route(pathNodes) { TotalDistance = totalDistance };
        }
    }
}
