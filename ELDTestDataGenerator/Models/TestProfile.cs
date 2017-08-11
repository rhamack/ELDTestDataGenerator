using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELDTestDataGenerator.Models
{
    public class TestProfile
    {

        public string ProfileId { get; set; } 
        public string ProfileName { get; set; }

        public double startingLatitude { get; set; } 

        public double startingLongitude { get; set; }

        public int startingCompassBearing { get; set; }

        public DateTimeOffset StartingDateTime { get; set; }

        public List<ProfileSegment> profileSegments { get; set; }

        public double startingEngineHours { get; set; }

        public int startingOdometer { get; set; }


        public TestProfile() {
            startingLatitude = 48.724511;
            startingLongitude = -122.4715;
            startingCompassBearing = 180;

            startingEngineHours = 1200;


            // start at 8AM today

            StartingDateTime = new DateTimeOffset(DateTime.Today, new TimeSpan(-7, 0, 0)).AddHours(8);

            profileSegments = new List<ProfileSegment>();
        }



        public void LoadTrip() {
            // driving for 8 hours

            ProfileSegment pseg = new ProfileSegment();
            pseg.Action = "Driving";
            pseg.CompassBearing = 180;
            pseg.MPH = 60;
            pseg.ElementSeqNum = 10;
            pseg.DurationSeconds = 8 * 3600;
            this.profileSegments.Add(pseg);
        }




    }
}