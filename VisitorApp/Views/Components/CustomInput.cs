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
    public class CustomInput : TextBox
    {
        static CustomInput()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomInput),
                new FrameworkPropertyMetadata(typeof(CustomInput)));
        }

        // Dependency property for the error message
        public static readonly DependencyProperty ErrorMessageProperty =
            DependencyProperty.Register(nameof(ErrorMessage), typeof(string), typeof(CustomInput),
                new FrameworkPropertyMetadata(string.Empty, OnErrorMessageChanged));

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(Label), typeof(string), typeof(CustomInput),
                new FrameworkPropertyMetadata(string.Empty, OnErrorMessageChanged));

        public string ErrorMessage
        {
            get => (string)GetValue(ErrorMessageProperty);
            set => SetValue(ErrorMessageProperty, value);
        }

        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        private static void OnErrorMessageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (CustomInput)d;
            control.UpdateErrorVisual();
        }

        private void UpdateErrorVisual()
        {
            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                VisualStateManager.GoToState(this, "Error", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "Default", true);
            }
        }
    }
}
