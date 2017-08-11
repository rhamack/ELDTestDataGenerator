using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELDTestDataGenerator
{
    public class GPSResult
    {
        public double EndingLatitude { get; set; }

        public double EndingLongitude { get; set; }
        
        public double MilesTraveled { get; set; }

        public string MapCoordinates { get {return string.Format("{0},{1}", this.EndingLatitude, this.EndingLongitude);} }
    }
}