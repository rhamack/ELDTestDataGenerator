using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELDTestDataGenerator.Models
{
    public class TravelProfile
    {
        public string TravelProfileId { get; set; }

        public double startingLatitude { get; set; }

        public double startingLongitude { get; set; }

        public int startingCompassBearing { get; set; }

        public TravelProfile() {
            startingLatitude = 48.724511;
            startingLongitude = -122.4715;
            startingCompassBearing = 180;
            TravelProfileId = "default travel profile";
        }

        public int GetBearing(double lat, double lng, int currentBearing)
        {
            // plot a course roughly around the US

            return 180;
        }


    }
}