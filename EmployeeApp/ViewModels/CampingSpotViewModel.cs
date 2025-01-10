using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.EmployeeApp.ViewModels
{
    public class CampingSpotViewModel : INotifyPropertyChanged
    {
        public int ID { get; private set; }
        public int PositionX { get; private set; }
        public int PositionY { get; private set; }
        private bool edited;
        public bool Edited
        {
            get => edited;
            set
            {
                edited = value;
                OnPropertyChanged(nameof(Edited));
            }
        }

        public CampingSpotViewModel(int id, int positionX, int positionY)
        {
            ID = id;
            PositionX = positionX;
            PositionY = positionY;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this.PropertyChanged, new PropertyChangedEventArgs(propertyName));
        }
    }
}
