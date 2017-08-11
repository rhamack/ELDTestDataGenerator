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


        public DateTimeOffset StartingDateTime { get; set; }

        public List<ProfileSegment> profileSegments { get; set; }

        public double startingEngineHours { get; set; }

        public int startingOdometer { get; set; }

        public TravelProfile travelProfile { get; set; }

        public TestProfile() {
            startingEngineHours = 1200;
            startingOdometer = 55000;

            // start at 8AM today
            StartingDateTime = new DateTimeOffset(DateTime.Today, new TimeSpan(-7, 0, 0)).AddHours(8);

            profileSegments = new List<ProfileSegment>();
            travelProfile = new TravelProfile();
        }



        public void LoadTripSegments() {
            // driving for 8 hours

            ProfileSegment pseg = new ProfileSegment();
            pseg.Action = "Driving";
            pseg.CompassBearing = 180;
            pseg.MPH = 60;
            pseg.ElementSeqNum = 10;
            pseg.DurationSeconds = 8 * 3600; // 8 hours
            this.profileSegments.Add(pseg);
        }




    }
}