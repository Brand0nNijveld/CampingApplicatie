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
using CampingApplication.Business;
using CampingApplication.VisitorApp.ViewModels;

namespace CampingApplication.VisitorApp.Views
{
    /// <summary>
    /// Interaction logic for DatePicker.xaml
    /// </summary>
    public partial class DateRangePicker : UserControl
    {
        public MainViewModel? mainViewModel;

        public DateRangePicker()
        {
            InitializeComponent();
            BeginDatePicker.BlackoutDates.AddDatesInPast();
            EndDatePicker.BlackoutDates.AddDatesInPast();
        }

        public DateTime? BeginDate { get; private set; }
        public DateTime? EndDate { get; private set; }

        private void BeginDatePicker_SelectedDate(object sender, SelectionChangedEventArgs e)
        {
            BeginDate = BeginDatePicker.SelectedDate;
            ValidateDates();
        }

        private void EndDatePicker_SelectedDate(object sender, SelectionChangedEventArgs e)
        {
            EndDate = EndDatePicker.SelectedDate;
            ValidateDates();
        }
        private void ValidateDates()
        {
            if (BeginDatePicker.SelectedDate.HasValue && EndDatePicker.SelectedDate.HasValue)
            {
                DateTime beginDate = BeginDatePicker.SelectedDate.Value;
                DateTime endDate = EndDatePicker.SelectedDate.Value;
                DateValidationResult result = DateValidationService.ValidateDates(beginDate, endDate);

                if (result == DateValidationResult.ValidDates)
                {
                    ResultTextBlock.Text = "";

                    // Geef de datums door aan de CampingMapViewModel
                    mainViewModel?.CampingMapViewModel.UpdateNumberOfNights(beginDate, endDate);

                    // Haal de beschikbare plekken op
                    mainViewModel?.CheckAvailableSpots(beginDate, endDate);
                    return;
                }

                if (result == DateValidationResult.EndBeforeBegin)
                {
                    ResultTextBlock.Text = "De einddatum mag niet eerder zijn dan de begindatum.";
                }
                else if (result == DateValidationResult.EndIsBegin)
                {
                    ResultTextBlock.Text = "De einddatum en begindatum mogen niet gelijk zijn.";
                }

                mainViewModel?.CampingMapViewModel.ClearAvailability();
            }
            else
            {
                ResultTextBlock.Text = "Selecteer beide datums.";
            }
        }


    }
}
