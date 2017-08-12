using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELDTestDataGenerator.Models
{
    public class TestProfileSegment
    {
        public int SegmentSeqNum { get; set; }

        public string ProfileId { get; set; }

        public int MPH { get; set; }

        public int DurationSeconds { get; set; }

        public string ActionId { get; set; }

        // collected for some events...
        public string CommentAnnotation { get;  set; }
        public string DriversLocationDesc { get;  set; }
        public DateTime? DateOfCertifiedRecord { get;  set; }

    }
}