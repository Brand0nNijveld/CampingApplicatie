using CampingApplication.Business.PathFinding;
using CampingApplication.Client.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.VisitorApp.ViewModels
{
    public class PathViewModel : INotifyPropertyChanged
    {
        private CampingMapViewModel? campingMapViewModel;
        public CampingMapViewModel? CampingMapViewModel
        {
            get => campingMapViewModel;
            set
            {
                campingMapViewModel = value;
                if (campingMapViewModel != null)
                {
                    campingMapViewModel.SelectedCampingSpotChanged += CampingMapViewModel_SelectedCampingSpotChanged;
                }
            }
        }

        private Graph mainGraph;
        public Graph MainGraph
        {
            get => mainGraph;
            set
            {
                mainGraph = value;
                OnPropertyChanged(nameof(MainGraph));
            }
        }

        private List<Graph> routeGraphs = [];
        public List<Graph> RouteGraphs
        {
            get => routeGraphs;
            set
            {
                routeGraphs = value;
                OnPropertyChanged(nameof(RouteGraphs));
            }
        }

        public PathViewModel()
        {
            mainGraph = new();
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
            var node10 = new Node(10, 50, 20);
            MainGraph.ConnectNodes(startNode, node1);
            MainGraph.ConnectNodes(node1, node2);
            MainGraph.ConnectNodes(node2, node3);
            MainGraph.ConnectNodes(node2, node4);
            MainGraph.ConnectNodes(node4, node5);
            MainGraph.ConnectNodes(node4, node6);
            MainGraph.ConnectNodes(node1, node7);
            MainGraph.ConnectNodes(node7, node8);
            MainGraph.ConnectNodes(node6, node9);
            MainGraph.ConnectNodeToClosestEdge(node10);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this.PropertyChanged, new PropertyChangedEventArgs(propertyName));
        }

        private void CampingMapViewModel_SelectedCampingSpotChanged(CampingSpotViewModel? campingSpot)
        {
            if (campingMapViewModel == null || campingSpot == null)
                return;
            var pixelsPerMeter = CampingMapViewModel.PIXELS_PER_METER;
            var (xStart, yStart) = MapConversionHelper.PixelsToMeters(campingSpot.PositionX + campingSpot.Width / 2, campingSpot.PositionY + campingSpot.Height / 2, pixelsPerMeter);

            List<Graph> routesToFacilities = [];
            Node startNode = new(-1, xStart, yStart);

            var facilities = campingMapViewModel.FacilityData;
            foreach (var facility in facilities)
            {
                Graph route = mainGraph.DeepCopyGraph();
                route.StartNode = startNode;
                Node endNode = new(route.AdjacencyList.Count, facility.XCoordinate, facility.YCoordinate);

                route.ConnectNodeToClosestEdge(startNode);
                route.ConnectNodeToClosestEdge(endNode);

                routesToFacilities.Add(route);
                Debug.WriteLine("Creating graph for specific facility!");
            }

            RouteGraphs = routesToFacilities;
        }
    }
}
