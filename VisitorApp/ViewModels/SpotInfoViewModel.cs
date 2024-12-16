using CampingApplication.Business.CampingSpotService;
using CampingApplication.Business;
using CampingApplication.VisitorApp.Views.Information;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CampingApplication.VisitorApp.Models;
using System.Diagnostics;

namespace CampingApplication.VisitorApp.ViewModels
{
    public class SpotInfoViewModel : INotifyPropertyChanged
    {
        private readonly CampingSpotService _campingSpotService;

        private int id;
        public int ID
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged(nameof(ID));
            }
        }

        private string description;
        public string Description
        {
            get => description;
            set
            {
                description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        //private double size;
        //public double Size
        //{
        //    get => size;
        //    set
        //    {
        //        size = value;
        //        OnPropertyChanged(nameof(Size));
        //    }
        //}

        private double pricePerNight;
        public double PricePerNight
        {
            get => pricePerNight;
            set
            {
                pricePerNight = value;
                OnPropertyChanged(nameof(PricePerNight));
            }
        }

        private bool pets;
        public bool Pets
        {
            get => pets;
            set
            {
                pets = value;
                OnPropertyChanged(nameof(Pets));
            }
        }

        private bool electricity;
        public bool Electricity
        {
            get => electricity;
            set
            {
                electricity = value;
                OnPropertyChanged(nameof(Electricity));
            }
        }

        private string type;
        public string Type
        {
            get => type;
            set
            {
                type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        
        public SpotInfoViewModel(int id)
        {
            ID = id;
            _campingSpotService = ServiceProvider.Current.Resolve<CampingSpotService>();

        }

        
        public async Task LoadCampingSpotInfoAsync()
        {
            try
            {
               
                var spot = await _campingSpotService.GetCampingSpotInfoAsync(ID);

                if (spot != null)
                {
                    Debug.WriteLine($"camping info found for ID: {ID}");
                    PricePerNight = spot.PricePerNight;
                    Pets = spot.Pets;
                    Electricity = spot.Electricity;
                }
                else
                {
                    Debug.WriteLine($"No camping info found for ID: {ID}");
                    //PricePerNight = 50.0;
                    //Pets = false;
                    //Electricity = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading camping info for ID {ID}: {ex.Message}");
            }
        }



        
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

       
        public event PropertyChangedEventHandler? PropertyChanged;


    }
}
