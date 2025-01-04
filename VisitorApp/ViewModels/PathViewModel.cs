using CampingApplication.Business;
using CampingApplication.Business.PathService;
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
        private PathService pathService;
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

            pathService = ServiceProvider.Current.Resolve<PathService>();
            mainGraph = new();
            GetMainPath();
        }

        private async void GetMainPath()
        {
            MainGraph = await pathService.GetMainPath();
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
