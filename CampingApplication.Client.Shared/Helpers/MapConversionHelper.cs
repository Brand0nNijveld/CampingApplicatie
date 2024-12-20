using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.Client.Shared.Helpers
{
    public static class MapConversionHelper
    {
        public static double PixelsToMeters(double pixels, double pixelsPerMeter)
        {
            return pixels / pixelsPerMeter;
        }

        public static int MetersToPixels(double meters, double pixelsPerMeter)
        {
            return (int)Math.Round(meters * pixelsPerMeter);
        }

        public static (double, double) PixelsToMeters(double pixelsX, double pixelsY, double pixelsPerMeter)
        {
            return (pixelsX / pixelsPerMeter, pixelsY / pixelsPerMeter);
        }

        public static (int, int) MetersToPixels(double metersX, double metersY, double pixelsPerMeter)
        {
            return (MetersToPixels(metersX, pixelsPerMeter), MetersToPixels(metersY, pixelsPerMeter));
        }
    }
}
