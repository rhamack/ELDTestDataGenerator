using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELDTestDataGenerator
{
    public class GPSCalculator
    {
        public static GPSResult CalcPosition(double startingLatitude, double startingLongitude, int compassBearing, int MPHSpeed, int durationSeconds)
        {
            GPSResult r = new GPSResult();

            // calc the distance traveled.\
            r.MilesTraveled = durationSeconds * MPHSpeed / 3600;

            double startingLatitudeRadians = DegreesToRadians(startingLatitude);
            double startingLongitudeRadians = DegreesToRadians(startingLongitude);
            double CompassBearingRadians = DegreesToRadians(compassBearing);

            // latitude 
            // =DEGREES(
            //    ASIN(SIN(H9) 
            //  * COS(D10/$P$4) 
            //  + COS(H9) 
            //  * SIN(D10/$P$4) 
            //  * COS(J10)))

            // longitude
            // =DEGREES(
            //    prevLongRadians 
            //    + ATAN2(
            //      COS(D10/$P$4)
            //      -SIN(H9)
            //      *SIN(H10)
            //      ,
            //      SIN(J10)
            //      *SIN(D10/$P$4) 
            //      * COS(H9)))
            // NOTE: need toinvert the parms from excel for ATAN2

            double newLatitudeRadians = Math.Asin(Math.Sin(startingLatitudeRadians) 
                * Math.Cos(r.MilesTraveled / Models.Constants.EARTH_RADIUS)
                + Math.Cos(startingLatitudeRadians) 
                * Math.Sin(r.MilesTraveled / Models.Constants.EARTH_RADIUS) 
                * Math.Cos(CompassBearingRadians));

            double x = Math.Cos(r.MilesTraveled / Models.Constants.EARTH_RADIUS)
                    - Math.Sin(startingLatitudeRadians)
                    * Math.Sin(newLatitudeRadians);

            double y =
                    Math.Sin(CompassBearingRadians)
                    * Math.Sin(r.MilesTraveled / Models.Constants.EARTH_RADIUS)
                    * Math.Cos(startingLatitudeRadians);

            double newLongitudeRadians =
                startingLongitudeRadians
                + Math.Atan2(y, x);

            r.EndingLatitude = RadiansToDegrees(newLatitudeRadians);
            r.EndingLongitude = RadiansToDegrees(newLongitudeRadians);
            

            return r;
        }


        private static double DegreesToRadians(double degreeValue) {
            return degreeValue * (Math.PI / 180);
        }

        private static double RadiansToDegrees(double radianValue) {
            return radianValue * 180 / Math.PI;
        }

    }

}