using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.Business
{
    public class CampingSpotImage
    {
        public int PhotoID { get; set; }         // The ID of the photo in the database
        public string FilePath { get; set; }    // The full path of the photo
        public string Filename { get; set; }     // The filename of the photo

        // Constructor to initialize the object
        public CampingSpotImage(int photoID, string filePath, string filename)
        {
            PhotoID = photoID;
            FilePath = filePath;
            Filename = filename;
        }

        // Default constructor
        public CampingSpotImage() { }
    }
}
