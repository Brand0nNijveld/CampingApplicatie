using System.Text;
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

namespace datapicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BeginDatePicker.BlackoutDates.AddDatesInPast();
            EndDatePicker.BlackoutDates.AddDatesInPast();
        }
        private void BeginDatePicker_SelectedDate(object sender, SelectionChangedEventArgs e)
        {
            ValidateDates();
        }
        private void EndDatePicker_SelectedDate(object sender, SelectionChangedEventArgs e)
        {
            ValidateDates();
        }
        
        private void ValidateDates()
        {
            if (BeginDatePicker.SelectedDate.HasValue && EndDatePicker.SelectedDate.HasValue)
            {
                DateTime beginDate = BeginDatePicker.SelectedDate.Value;
                DateTime endDate = EndDatePicker.SelectedDate.Value;
                DateValidationResult result = DateValidationService.ValidateDates(beginDate, endDate);

                if (result == DateValidationResult.EndBeforeBegin )
                {
                    ResultTextBlock.Text = "De einddatum mag niet eerder zijn dan de begindatum.";
                }else if (result == DateValidationResult.EndIsBegin)
                {
                    ResultTextBlock.Text = "De einddatum en begindatum mogen niet gelijk zijn.";
                }
                else
                {
                    ResultTextBlock.Text = "De datums zijn geldig.";
                }
            }
            else
            {
                ResultTextBlock.Text = "Selecteer beide datums.";
            }
        }
    }
}