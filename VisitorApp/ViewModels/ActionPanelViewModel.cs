using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CampingApplication.VisitorApp.ViewModels
{
    public class ActionPanelViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private int currentView;
        public int CurrentView
        {
            get => currentView;
            set
            {
                currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public List<UserControl> Views { get; private set; } = [];

        public void SetSteps(List<UserControl> steps, int startWith = 0)
        {
            this.Views = steps;
            CurrentView = startWith;
        }

        public void ClearAndHide()
        {
            Views = [];
            CurrentView = 0;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
