using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CampingApplication.VisitorApp.ViewModels
{
    public class ActionPanelViewModel : BaseViewModel
    {
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

        public void Next()
        {
            if (currentView + 1 >= Views.Count)
            {
                Debug.WriteLine("ACTIONPANEL: Trying to go next, but current is the last view.");
                return;
            }

            CurrentView++;
        }

        public void Previous()
        {
            if (currentView - 1 < 0)
            {
                Debug.WriteLine("ACTIONPANEL: Trying to go back, but current is first view (clearing and hiding panel).");
                ClearAndHide();
                return;
            }

            CurrentView--;
        }

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
    }
}
