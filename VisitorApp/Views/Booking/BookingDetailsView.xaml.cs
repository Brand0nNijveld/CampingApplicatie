using CampingApplication.Business.BookingService;
using CampingApplication.VisitorApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace CampingApplication.VisitorApp.Views.Booking
{
    /// <summary>
    /// Interaction logic for BookingDetailsView.xaml
    /// </summary>
    public partial class BookingDetailsView : UserControl
    {
        private CampingMapModel? mapModel;

        public BookingDetailsView()
        {
            InitializeComponent();
        }

        public void SetDetails(int id, CampingMapModel mapModel)
        {
            ID.Text = id.ToString();
            this.mapModel = mapModel;
            mapModel.PropertyChanged += MapModel_PropertyChanged;
        }

        private void MapModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(mapModel.StartDate) || e.PropertyName == nameof(mapModel.EndDate))
            {
                SetData();
            }
        }

        private void SetData()
        {
            if (mapModel == null)
            {
                return;
            }

            DateTime startDate = mapModel.StartDate;
            DateTime endDate = mapModel.EndDate;

            int amountOfNights = BookingService.CalculateAmountOfNights(startDate, endDate);
            float totalPrice = BookingService.CalculateTotalPrice(amountOfNights, 60);

            StartDate.Text = startDate.ToShortDateString();
            EndDate.Text = endDate.ToShortDateString();
            AmountOfNights.Text = amountOfNights.ToString();
            PricePerNight.Text = FormatPrice(60);
            TotalPrice.Text = FormatPrice(totalPrice);
        }

        private static string FormatPrice(float priceInEuros)
        {
            var cultureInfo = new CultureInfo("nl-NL");
            return priceInEuros.ToString("C", cultureInfo);
        }
    }
}
