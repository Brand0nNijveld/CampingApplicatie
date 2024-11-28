using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CampingApplication.VisitorApp.Views.Components
{
    public enum ButtonState
    {
        Enabled,
        Disabled,
        Loading,
    }

    public partial class CustomButton : Button
    {
        public static readonly DependencyProperty ButtonStateProperty =
            DependencyProperty.Register(
                "ButtonState",
                typeof(ButtonState),
                typeof(CustomButton),
                new PropertyMetadata(ButtonState.Enabled, OnButtonStateChanged));

        public ButtonState ButtonState
        {
            get { return (ButtonState)GetValue(ButtonStateProperty); }
            set { SetValue(ButtonStateProperty, value); }
        }

        private static void OnButtonStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var button = (CustomButton)d;
            button.UpdateVisualState();
        }

        private void UpdateVisualState()
        {
            VisualStateManager.GoToState(this, ButtonState.ToString(), true);

            if (ButtonState == ButtonState.Disabled || ButtonState == ButtonState.Loading)
            {
                this.IsEnabled = false;
            }
            else
            {
                this.IsEnabled = true;
            }
        }
    }
}
