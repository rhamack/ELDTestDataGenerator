using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELDTestDataGenerator.Models
{
    public class ProfileSegment
    {
        public string ProfileId { get; set; }

        public int ElementSeqNum { get; set; }

        public int MPH { get; set; }

        public int DurationSeconds { get; set; }

        public int CompassBearing { get; set; }

        public string Action { get; set; }

    }
}