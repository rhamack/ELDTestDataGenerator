using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELDTestDataGenerator.Models
{
    public class GPSLocation
    {
        public double? Latitude { get; private set; }
        public double? Longitude { get; private set; }

        public bool ReducedGPSPrecision { get; private set; }

        public string DriversLocationDesc { get; private set; }

        protected GPSLocation(double? latitude, double? longitude, bool reducedGPSPrecision)
        {
            Latitude = latitude;
            Longitude = longitude;
            ReducedGPSPrecision = reducedGPSPrecision;

            // make sure that we are only storing to one decimal place if reduced precision is expected
            if (ReducedGPSPrecision)
            {
                if (Latitude != null)
                {
                    Latitude = Math.Round(Latitude.Value, 1);
                }
                if (Longitude != null)
                {
                    Longitude = Math.Round(Longitude.Value, 1);
                }
            }



        }

        public static GPSLocation Create(double? latitude, double? longitude, bool reducedGPSPrecision)
        {
            // business rules to validate the position
            if (latitude != null)
            {
                if (latitude > 90 || latitude < -90)
                {
                    throw new ArgumentException("Latitude cannot be greater than 90 or less than -90 degrees", "latitude");
                }
            }

            if (longitude != null)
            {
                if (longitude > 180 || longitude < -180)
                {
                    throw new ArgumentException("Longitude cannot be greater than 180 or less than -180 degrees", "longitude");
                }
            }


            GPSLocation evt = new GPSLocation(latitude, longitude, reducedGPSPrecision);


            return evt;

        }


    }
}
