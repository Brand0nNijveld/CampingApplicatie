using CampingApplication.Business.PathFinding;
using CampingApplication.Client.Shared.Helpers;
using CampingApplication.VisitorApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public PathViewModel ViewModel { get; private set; }
        public Path MainPath { get; private set; } = new();

        public PathView(Canvas canvas)
        {
            this.canvas = canvas;
            ViewModel = new();

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.RouteGraphs))
            {
                for (int i = 0; i < ViewModel.RouteGraphs.Count; i++)
                {
                    SolidColorBrush color = new();
                    var routeGraph = ViewModel.RouteGraphs[i];
                    if (i == 1)
                    {
                        color = Brushes.Blue;
                    }
                    else if (i == 2)
                    {
                        color = Brushes.Yellow;
                    }

                    var route = DrawPath(routeGraph, color, 15);
                    canvas.Children.Add(route);

                    //DrawNodes(routeGraph);
                }
            }
        }

        public void DrawMainPath()
        {
            Graph mainGraph = ViewModel.MainGraph;
            MainPath = DrawPath(mainGraph, Brushes.Black, 70, 0.2);
            canvas.Children.Add(MainPath);

            // DEBUG TO CHECK NODES
            DrawNodes(mainGraph);
        }

        public void AddClosestConnection(double x, double y)
        {
            Node from = new(-1, x, y);
            ViewModel.MainGraph.ConnectNodeToClosestEdge(from);
            if (MainPath != null)
            {
                canvas.Children.Remove(MainPath);
            }

            DrawMainPath();
        }

        private Path DrawPath(Graph graph, SolidColorBrush color, int strokeThickness = 30, double opacity = 1)
        {
            PathGeometry pathGeometry = new();
            Node? startNode;
            if (graph.StartNode != null)
            {
                startNode = graph.StartNode;
            }
            else
            {
                startNode = graph.AdjacencyList.First().Key;
            }

            List<PathFigure> pathFigures = GeneratePathList(startNode);

            foreach (var path in pathFigures)
            {
                pathGeometry.Figures.Add(path);
            }

            return new Path
            {
                Data = pathGeometry,
                Stroke = color,
                Opacity = opacity,
                StrokeThickness = strokeThickness,
                StrokeLineJoin = PenLineJoin.Round,
                Fill = Brushes.Transparent
            };
        }

        private List<PathFigure> GeneratePathList(Node node, HashSet<Tuple<Node, Node>>? edgesDrawn = null, List<PathFigure>? paths = null, PathFigure? currentPath = null)
        {
            double pixelsPerMeter = CampingMapViewModel.PIXELS_PER_METER;
            // Convert node position to pixels
            double nodeX = MapConversionHelper.MetersToPixels(node.X, pixelsPerMeter);
            double nodeY = MapConversionHelper.MetersToPixels(node.Y, pixelsPerMeter);

            paths ??= [];
            edgesDrawn ??= [];

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
            GeneratePathList(neighbor, edgesDrawn, paths, currentPath);

            // Start new branches if there are more neighbors
            for (int i = 1; i < node.Neighbors.Count; i++)
            {
                // Leave out current path so it starts a new path
                GeneratePathList(node.Neighbors[i], edgesDrawn, paths);
            }

            return paths;
        }

        private void DrawNodes(Graph graph)
        {
            double pixelsPerMeter = CampingMapViewModel.PIXELS_PER_METER;

            foreach ((var node, var neighbors) in graph.AdjacencyList)
            {
                double nodeX = MapConversionHelper.MetersToPixels(node.X, pixelsPerMeter);
                double nodeY = MapConversionHelper.MetersToPixels(node.Y, pixelsPerMeter);
                Border nodeEllipse = new()
                {
                    Width = 30,
                    Height = 30,
                    CornerRadius = new CornerRadius(100),
                    Background = new SolidColorBrush(Colors.Red)
                };

                TextBlock idText = new TextBlock
                {
                    Text = node.ID.ToString(),
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
        }

    }
}
