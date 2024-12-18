using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.Client.Shared.Helpers
{
    public static class MapConversionHelper
    {
        public static double PixelsToMeters(double pixels, int pixelsPerMeter)
        {
            return pixels / pixelsPerMeter;
        }

        public static int MetersToPixels(double meters, int pixelsPerMeter)
        {
            return (int)Math.Round(meters * pixelsPerMeter);
        }
    }
}
