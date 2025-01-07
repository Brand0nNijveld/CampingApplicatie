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
        public BookingDetailsView()
        {
            InitializeComponent();
        }

        private float _totalPrice { get; set; }

        public void SetDetails(int id, DateTime startDate, DateTime endDate, int amountOfNights, float pricePerNight, float totalPrice)
        {
            _totalPrice = totalPrice;

            ID.Text = id.ToString();
            StartDate.Text = startDate.ToShortDateString();
            EndDate.Text = endDate.ToShortDateString();
            AmountOfNights.Text = amountOfNights.ToString();
            PricePerNight.Text = FormatPrice(pricePerNight);
            TotalPrice.Text = FormatPrice(_totalPrice);
        }

        public void PriceChangePets(bool pets)
        {
            if (pets) { _totalPrice += 60; }
            if (!pets) { _totalPrice -= 60; }

            TotalPrice.Text = FormatPrice(_totalPrice);
        }

        public void PriceChangeElectricity(bool electricity)
        {
            if (electricity) { _totalPrice += 60; }
            if (!electricity) { _totalPrice -= 60; }

            TotalPrice.Text = FormatPrice(_totalPrice);
        }

        private static string FormatPrice(float priceInEuros)
        {
            var cultureInfo = new CultureInfo("nl-NL");
            return priceInEuros.ToString("C", cultureInfo);
        }
    }
}
