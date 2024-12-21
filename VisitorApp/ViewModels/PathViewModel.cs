using CampingApplication.Business.PathFinding;
using CampingApplication.Client.Shared.Helpers;
using CampingApplication.VisitorApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace CampingApplication.VisitorApp.ViewModels
{
    public class PathViewModel : BaseViewModel
    {
        private CampingMapModel campingMapModel;

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

        public List<FacilityRouteModel> Routes
        {
            get
            {
                if (campingMapModel.SelectedCampingSpot != null)
                {
                    return campingMapModel.SelectedCampingSpot.Routes;
                }
                else
                {
                    return [];
                }
            }
            set
            {
                if (campingMapModel.SelectedCampingSpot != null)
                {
                    campingMapModel.SelectedCampingSpot.Routes = value;
                }

                OnPropertyChanged(nameof(Routes));
            }
        }

        public FacilityRouteModel? ShownRoute
        {
            get
            {
                if (campingMapModel.SelectedCampingSpot != null)
                {
                    return campingMapModel.SelectedCampingSpot.HoveredFacilityRoute;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (campingMapModel.SelectedCampingSpot != null)
                {
                    campingMapModel.SelectedCampingSpot.HoveredFacilityRoute = value;
                }

                OnPropertyChanged(nameof(ShownRoute));
            }
        }

        public PathViewModel(CampingMapModel campingMapModel)
        {
            this.campingMapModel = campingMapModel;
            campingMapModel.PropertyChanged += CampingMapModel_PropertyChanged;

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
            MainGraph.ConnectNodes(startNode, node1);
            MainGraph.ConnectNodes(node1, node2);
            MainGraph.ConnectNodes(node2, node3);
            MainGraph.ConnectNodes(node4, node5);
            MainGraph.ConnectNodes(node4, node1);
            MainGraph.ConnectNodes(node4, node6);
            MainGraph.ConnectNodes(node1, node7);
            MainGraph.ConnectNodes(node7, node8);
            MainGraph.ConnectNodes(node6, node9);
        }

        private async void CampingMapModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(campingMapModel.SelectedCampingSpot))
            {
                Routes = [];

                if (campingMapModel.SelectedCampingSpot != null)
                {
                    var selectedSpot = campingMapModel.SelectedCampingSpot;
                    selectedSpot.PropertyChanged += SelectedSpot_PropertyChanged;

                    await Task.Delay(500);
                    var (x, y) = selectedSpot.CampingSpot.GetCenterPoint();
                    await CreateRoutes(new Node(-1, x, y));
                }
            }
        }

        private void SelectedSpot_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(campingMapModel.SelectedCampingSpot.HoveredFacilityRoute))
            {
                OnPropertyChanged(nameof(ShownRoute));
            }
        }

        public async Task CreateRoutes(Node from)
        {
            if (campingMapModel.SelectedCampingSpot == null)
                return;

            List<FacilityRouteModel> routesToFacilities = [];

            foreach (var facility in campingMapModel.Facilities)
            {
                Graph graph = mainGraph.DeepCopyGraph();
                graph.StartNode = from;
                Node endNode = new(graph.AdjacencyList.Count, facility.XCoordinate, facility.YCoordinate);

                graph.ConnectNodeToClosestEdge(from);
                graph.ConnectNodeToClosestEdge(endNode);

                Route route = Dijkstra.FindShortestPath(graph, from, endNode);
                var facilityRouteModel = new FacilityRouteModel(facility, route);

                routesToFacilities.Add(facilityRouteModel);
            }

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                Routes = routesToFacilities;
            });
        }
    }
}
