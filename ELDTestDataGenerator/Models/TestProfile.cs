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

        public List<TestProfileSegment> profileSegments { get; set; }

        public double startingEngineHours { get; set; }

        public int startingOdometer { get; set; }

        public TravelProfile travelProfile { get; set; }

        public int PollingIntervalSeconds { get; set; }

        public CurrentTripState DefaultCurrentTripState { get; set; }

        public TestProfile() {
            startingEngineHours = 1200;
            startingOdometer = 55000;

            // start at 8AM today
            StartingDateTime = new DateTimeOffset(DateTime.Today, new TimeSpan(-7, 0, 0)).AddHours(8);
            PollingIntervalSeconds = 300;

            profileSegments = new List<TestProfileSegment>();
            travelProfile = new TravelProfile();
            DefaultCurrentTripState = new CurrentTripState();

        }



        public void LoadTripSegments() {
            // driving for 8 hours

            TestProfileSegment pseg = new TestProfileSegment();
            pseg.ActionId = "Driving";
            pseg.MPH = 60;
            pseg.SegmentSeqNum = 10;
            pseg.DurationSeconds = 8 * 3600; // 8 hour default

            this.profileSegments.Add(pseg);
        }




    }
}