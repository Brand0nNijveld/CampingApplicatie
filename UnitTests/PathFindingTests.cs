using CampingApplication.Business.PathService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class PathFindingTests
    {
        [Test]
        public void FindShortestPath()
        {
            // Arrange
            Graph graph = new();
            var startNode = new Node(0, 0, 0);
            var node1 = new Node(1, 0, 1);
            var node2 = new Node(2, -1, 1);
            var node3 = new Node(3, 1, 1);
            var node4 = new Node(4, 1, 3);
            var endNode = new Node(5, -1, 2);

            graph.ConnectNodes(startNode, node1);
            graph.ConnectNodes(node1, node2);
            graph.ConnectNodes(node1, node3);
            graph.ConnectNodes(node3, node4);
            graph.ConnectNodes(node2, endNode);

            // Act
            Route shortestRoute = Dijkstra.FindShortestPath(graph, startNode, endNode);

            // Assert
            var expectedNodes = new List<Node> { startNode, node1, node2, endNode };
            Assert.Multiple(() =>
            {
                Assert.That(shortestRoute.TotalDistance, Is.EqualTo(3));
                Assert.That(shortestRoute.Nodes, Is.EqualTo(expectedNodes).Using<Node>((a, b) => a.ID == b.ID ? 0 : 1));
            });
        }
    }
}
