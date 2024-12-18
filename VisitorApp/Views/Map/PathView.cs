using CampingApplication.Business.PathFinding;
using CampingApplication.Client.Shared.Helpers;
using CampingApplication.VisitorApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CampingApplication.VisitorApp.Views.Map
{
    public class PathView
    {
        private Canvas canvas;

        public Path MainPath { get; private set; } = new();

        public PathView(Canvas canvas)
        {
            this.canvas = canvas;
        }

        public void DrawMainPath()
        {
            HashSet<Tuple<Node, Node>> edgesDrawn = [];

            // Sample graph with nodes and edges
            Graph graph = new();
            var startNode = new Node(0, 33.95, 50);
            var node1 = new Node(1, 33.95, 37.2);
            var node2 = new Node(2, 15.3, 37.2);
            var node3 = new Node(3, 15.3, 5.3);
            var node4 = new Node(4, 41.6, 37.2);
            var node5 = new Node(5, 41.6, 44.97);
            var node6 = new Node(6, 41.6, 5.3);
            var node7 = new Node(7, 29.8, 37.2);
            var node8 = new Node(8, 29.8, 20.4);
            var node9 = new Node(9, 50, 5.3);
            graph.ConnectNodes(startNode, node1);
            graph.ConnectNodes(node1, node2);
            graph.ConnectNodes(node2, node3);
            graph.ConnectNodes(node2, node4);
            graph.ConnectNodes(node4, node5);
            graph.ConnectNodes(node4, node6);
            graph.ConnectNodes(node1, node7);
            graph.ConnectNodes(node7, node8);
            graph.ConnectNodes(node6, node9);


            double pixelsPerMeter = CampingMapViewModel.PIXELS_PER_METER;

            int startX = MapConversionHelper.MetersToPixels(startNode.X, pixelsPerMeter);
            int startY = MapConversionHelper.MetersToPixels(startNode.Y, pixelsPerMeter);

            PathGeometry pathGeometry = new();
            List<PathFigure> pathFigures = DrawPath(startNode, edgesDrawn);

            foreach ((var node, var neighbors) in graph.AdjacencyList)
            {
                // Convert node position to pixels
                double nodeX = MapConversionHelper.MetersToPixels(node.X, pixelsPerMeter);
                double nodeY = MapConversionHelper.MetersToPixels(node.Y, pixelsPerMeter);
                // Draw node (Ellipse)
                Border nodeEllipse = new Border
                {
                    Width = 30,
                    Height = 30,
                    CornerRadius = new CornerRadius(100),
                    Background = new SolidColorBrush(Colors.Red)
                };

                // Create and position a TextBlock for the node's ID
                TextBlock idText = new TextBlock
                {
                    Text = node.ID.ToString(),  // Display the node's ID
                    Foreground = Brushes.Black,
                    FontSize = 20,
                    FontWeight = FontWeights.Bold,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                nodeEllipse.Child = idText;

                Canvas.SetLeft(nodeEllipse, nodeX - 15);
                Canvas.SetTop(nodeEllipse, nodeY - 15);
                canvas.Children.Add(nodeEllipse);
            }

            foreach (var path in pathFigures)
            {
                pathGeometry.Figures.Add(path);
            }

            // Create the Path with the geometry and add it to the canvas
            MainPath = new Path
            {
                Data = pathGeometry,
                Stroke = Brushes.Black,  // Thick road color
                Opacity = 0.2,
                StrokeThickness = 70,  // Set a thick road width here
                StrokeLineJoin = PenLineJoin.Round,
                Fill = Brushes.Transparent
            };

            canvas.Children.Add(MainPath);
        }

        private List<PathFigure> DrawPath(Node node, HashSet<Tuple<Node, Node>> edgesDrawn, List<PathFigure>? paths = null, PathFigure? currentPath = null)
        {
            double pixelsPerMeter = CampingMapViewModel.PIXELS_PER_METER;
            // Convert node position to pixels
            double nodeX = MapConversionHelper.MetersToPixels(node.X, pixelsPerMeter);
            double nodeY = MapConversionHelper.MetersToPixels(node.Y, pixelsPerMeter);

            if (paths == null)
            {
                paths = [];
            }

            if (node.Neighbors.Count == 0)
                return paths;

            // Get first neighbor
            var neighbor = node.Neighbors.FirstOrDefault((n) =>
            {
                // Only add unvisited nodes
                return !edgesDrawn.Contains(Tuple.Create(n, node)) && !edgesDrawn.Contains(Tuple.Create(node, n));
            });

            if (neighbor == null)
            {
                return paths;
            }

            Debug.WriteLine($"Drawing edge from {node.ID} to {neighbor.ID}");

            edgesDrawn.Add(Tuple.Create(node, neighbor));

            double neighborX = MapConversionHelper.MetersToPixels(neighbor.X, pixelsPerMeter);
            double neighborY = MapConversionHelper.MetersToPixels(neighbor.Y, pixelsPerMeter);
            // Define control points for the curve at the turn
            double controlX1 = nodeX + (neighborX - nodeX) / 2;
            double controlY1 = nodeY;  // Adjust this value to make the curve smoother or sharper
            double controlX2 = nodeX + (neighborX - nodeX) / 2;
            double controlY2 = nodeY;  // Adjust this value to make the curve smoother or sharper

            BezierSegment bezierSegment = new BezierSegment(
                new Point(controlX1, controlY1),  // Control point 1
                new Point(controlX2, controlY2),  // Control point 2
                new Point(neighborX, neighborY),  // Endpoint (neighbor)
                true  // IsStroked (to draw the path)
            );
            bezierSegment.IsSmoothJoin = true;

            if (currentPath == null)
            {
                currentPath = new PathFigure { StartPoint = new Point(nodeX, nodeY) };
                paths.Add(currentPath);
            }

            currentPath.Segments.Add(bezierSegment);

            // Further draw current path
            DrawPath(neighbor, edgesDrawn, paths, currentPath);

            // Start new branches if there are more neighbors
            for (int i = 1; i < node.Neighbors.Count; i++)
            {
                // Leave out current path so it starts a new path
                DrawPath(node.Neighbors[i], edgesDrawn, paths);
            }

            return paths;
        }
    }
}
