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
    }
}
