using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELDTestDataGenerator.Models
{
    public class EventSet
    {
        public string ProfileId { get; set; }

        public List<EventObjects> eventObjects { get; set; }

        public EventSet() {
            eventObjects = new List<EventObjects>();
        }

    }

    public class EventObjects {
        public int SeqNum { get; set; }

        public string IntendedEventName { get; set; }

        public CommonParms commonParms { get; set; }

        public TransientOBDReadings transientOBDReadings { get; set; }

        public CurrentTripState currentTripState { get; set; }

        public CurrentVehicleState currentVehicleState { get; set; }

        public CurrentLocationState currentLocationState { get; set; }

        public DeviceMalfunctionState deviceMalfunctionState { get; set; }

        public DataDiagnosticState dataDiagnosticState { get; set; }


    }
}