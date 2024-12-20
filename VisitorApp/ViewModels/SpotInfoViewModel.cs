using CampingApplication.Business;
using CampingApplication.Business.CampingSpotService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

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

        private double width;
        public double Width
        {
            get => width;
            set
            {
                width = value;
                OnPropertyChanged(nameof(Width));
            }
        }

        private double length;
        public double Length
        {
            get => length;
            set
            {
                length = value;
                OnPropertyChanged(nameof(Length));
            }
        }

        private int numberOfNights;
        public int NumberOfNights
        {
            get => numberOfNights;
            set
            {
                numberOfNights = value;
                OnPropertyChanged(nameof(NumberOfNights));
            }
        }

        private List<CampingSpotImage> images = new List<CampingSpotImage>();
        public List<CampingSpotImage> Images
        {
            get => images;
            set
            {
                images = value;
                OnPropertyChanged(nameof(Images));
            }
        }

        //    public List<CampingSpotImage> Images { get; set; } = new List<CampingSpotImage>
        //{
        //    new CampingSpotImage { FilePath = "Images/1.jpg" } // Set the correct path
        //};

        public SpotInfoViewModel(int id, int numberOfNights)
        {
            ID = id;
            NumberOfNights = numberOfNights;
            _campingSpotService = ServiceProvider.Current.Resolve<CampingSpotService>();
        }

        public async Task LoadCampingSpotInfoAsync()
        {
            try
            {
                var spot = await _campingSpotService.GetCampingSpotInfoAsync(ID);

                if (spot != null)
                {
                    PricePerNight = spot.PricePerNight;
                    Pets = spot.Pets;
                    Electricity = spot.Electricity;
                    Length = spot.Length;
                    Width = spot.Width;
                    Type = spot.Type;

                    // Fetch the images
                    var spotImages = await _campingSpotService.GetCampingSpotImagesAsync(ID);
                    Images = new List<CampingSpotImage>(spotImages);
                }
                else
                {
                    Debug.WriteLine($"No camping info found for ID: {ID}");
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
