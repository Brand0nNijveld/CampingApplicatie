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
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace CampingApplication.VisitorApp.Views.Map
{
    public class PathView
    {
        private Canvas canvas;

        public PathViewModel ViewModel { get; private set; }
        public Path MainPath { get; private set; } = new();
        private List<Path> routePaths = [];

        public PathView(Canvas canvas, PathViewModel viewModel)
        {
            this.canvas = canvas;
            ViewModel = viewModel;

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.Routes))
            {
                DrawRoutes();
            }
        }

        private void DrawRoutes()
        {
            foreach (var route in routePaths)
            {
                canvas.Children.Remove(route);
            }

            routePaths = [];

            SolidColorBrush[] colors = { Brushes.Blue, Brushes.Yellow, Brushes.Red, Brushes.Purple };
            for (int i = 0; i < ViewModel.Routes.Count; i++)
            {
                SolidColorBrush color = colors[i];
                var route = ViewModel.Routes[i];

                Path path = DrawRoute(route, color);
                Canvas.SetZIndex(path, 0);
                canvas.Children.Add(path);
                routePaths.Add(path);
            }
        }


        public void DrawMainPath()
        {
            Graph mainGraph = ViewModel.MainGraph;
            MainPath = DrawPath(mainGraph, Brushes.Black, 70, 0.2);
            canvas.Children.Add(MainPath);

            // DEBUG TO CHECK NODES
            //DrawNodes(mainGraph);
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

            List<PathFigure> pathFigures = GenerateMainPath(startNode);

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

        private List<PathFigure> GenerateMainPath(Node node, HashSet<Tuple<Node, Node>>? edgesDrawn = null, List<PathFigure>? paths = null, PathFigure? currentPath = null)
        {
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

            Point nodePoint = NodeToPoint(node);
            Point neighborPoint = NodeToPoint(neighbor);

            BezierSegment bezierSegment = GetBezierSegment(nodePoint, neighborPoint);

            if (currentPath == null)
            {
                currentPath = new PathFigure { StartPoint = nodePoint };
                paths.Add(currentPath);
            }

            currentPath.Segments.Add(bezierSegment);

            // Further draw current path
            GenerateMainPath(neighbor, edgesDrawn, paths, currentPath);

            // Start new branches if there are more neighbors
            for (int i = 1; i < node.Neighbors.Count; i++)
            {
                // Leave out current path so it starts a new path
                GenerateMainPath(node.Neighbors[i], edgesDrawn, paths);
            }

            return paths;
        }

        private Path DrawRoute(Route route, SolidColorBrush color)
        {
            List<Node> nodes = route.Nodes;
            Node startNode = nodes[0];
            PathGeometry pathGeometry = new();
            PathFigure path = new() { StartPoint = NodeToPoint(startNode) };

            for (int i = 0; i < nodes.Count; i++)
            {
                Node node = nodes[i];
                if (i + 1 >= nodes.Count)
                {
                    break;
                }

                Node next = nodes[i + 1];
                Point nodePoint = NodeToPoint(node);
                Point nextPoint = NodeToPoint(next);

                BezierSegment bezierSegment = GetBezierSegment(nodePoint, nextPoint);

                path.Segments.Add(bezierSegment);
            }

            pathGeometry.Figures.Add(path);

            return new Path
            {
                Data = pathGeometry,
                Stroke = color,
                Opacity = 1,
                StrokeThickness = 15,
                StrokeLineJoin = PenLineJoin.Round,
                Fill = Brushes.Transparent
            };
        }

        private void DrawNodes(List<Node> nodes)
        {
            double pixelsPerMeter = CampingMapViewModel.PIXELS_PER_METER;

            foreach (var node in nodes)
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

        private void DrawNodes(Graph graph)
        {
            List<Node> nodes = [.. graph.AdjacencyList.Keys];
            DrawNodes(nodes);
        }

        /// <summary>
        /// Converting a nodes real world positions to a point on the screen, for a path.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private Point NodeToPoint(Node node)
        {
            double pixelsPerMeter = CampingMapViewModel.PIXELS_PER_METER;
            double x = MapConversionHelper.MetersToPixels(node.X, pixelsPerMeter);
            double y = MapConversionHelper.MetersToPixels(node.Y, pixelsPerMeter);
            return new Point(x, y);
        }

        /// <summary>
        /// Method for creating a smooth join on the intersections
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private BezierSegment GetBezierSegment(Point from, Point to)
        {
            double controlX1 = from.X;
            double controlY1 = from.Y;
            double controlX2 = from.X;
            double controlY2 = from.Y;

            BezierSegment bezierSegment = new BezierSegment(
                new Point(controlX1, controlY1),  // Control point 1
                new Point(controlX2, controlY2),  // Control point 2
                to,  // Endpoint (neighbor)
                true  // IsStroked (to draw the path)
            );
            bezierSegment.IsSmoothJoin = true;

            return bezierSegment;
        }
    }
}
