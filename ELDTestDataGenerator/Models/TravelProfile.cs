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
            int newBearing = currentBearing;
            // plot a course roughly around the US
            switch (currentBearing) {

                case 180:
                    // look for a latitude below 38.469 (around santa rosa) and change...
                    if (lat <= 38.469)
                        newBearing = 135;
                    break;
                case 135:
                    // look for a latitude below 34.283 (around los angeles) 
                    if (lat <= 34.283)
                        newBearing = 110;
                    break;
                case 110: 
                    // change around tucson...
                    if (lat <= 34.243 && lng >= -110.985)
                        newBearing = 90;
                    break;
                case 90: // headed due east
                    // change around savannah...
                    if (lng >= -82.5)
                        newBearing = 30;
                    break;
                case 30: // headed northeast
                    // change around new york...
                    if (lat >= 40.746)
                        newBearing = 270;
                    break;
                case 270: // headed northeast
                    // change around chicago...
                    if (lng <= -87.660)
                        newBearing = 288;
                    break;

            }



            return newBearing;
        }


    }
}