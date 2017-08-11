using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELDTestDataGenerator.Models
{
    public class CommonParms
    {

        public string EventId { get; private set; }
        public byte EventTypeCode { get; private set; }
        public byte EventRecordStatusCode { get; private set; }
        public byte EventRecordOriginCode { get; private set; }
        public byte EventCode { get; private set; }
        public DateTimeOffset EventTimestamp { get; private set; }
        public int EventSequenceNum { get; private set; }
        public string EventDataCheckValue { get; private set; }

        public string CommentAnnotation { get; private set; }
        public string DriversLocationDesc { get; private set; }

        public DateTime? DateOfCertifiedRecord { get; private set; }

        public string RelatedEventId { get; private set; }

    }
}