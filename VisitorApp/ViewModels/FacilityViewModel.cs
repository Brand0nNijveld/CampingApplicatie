using CampingApplication.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.VisitorApp.ViewModels
{
    public class FacilityViewModel : INotifyPropertyChanged
    {
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

        private int positionX;
        public int PositionX
        {
            get => positionX;
            set
            {
                positionX = value;
                OnPropertyChanged(nameof(PositionX));
            }
        }

        private int positionY;
        public int PositionY
        {
            get => positionY;
            set
            {
                positionY = value;
                OnPropertyChanged(nameof(PositionY));
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

        public FacilityViewModel(Facility facility)
        {
            ID = facility.ID;
            positionX = facility.PositionX;
            positionY = facility.PositionY;
            type = facility.Type.ToString();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this.PropertyChanged, new PropertyChangedEventArgs(propertyName));
        }
    }
}
