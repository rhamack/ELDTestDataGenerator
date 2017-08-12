using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELDTestDataGenerator.Models
{
    public class CommonParms 
    {

        public string EventId { get; set; }
        public byte EventTypeCode { get; set; }
        public byte EventRecordStatusCode { get; set; }
        public byte EventRecordOriginCode { get; set; }
        public byte EventCode { get; set; }
        public DateTimeOffset EventTimestamp { get; set; }
        public int EventSequenceNum { get; set; }
        public string EventDataCheckValue { get; set; }

        public string CommentAnnotation { get; set; }
        public string DriversLocationDesc { get; set; }

        public DateTime? DateOfCertifiedRecord { get; set; }

        public string RelatedEventId { get; set; }

    }
}