using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public string ToSerializedJson() {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(this);
            return json;
        }

        public string ToKML() {
            // create a KML file for map plotting
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.AppendLine("<kml xmlns=\"http://www.opengis.net/kml/2.2\">");

            // draw a line?
            /*
            sb.AppendLine("<Placemark>");
            sb.AppendLine("<name>ELD Travel Path</name>");
            sb.AppendLine("<description>Approximate travel path to demonstrate ELD reporting</description>");
            sb.AppendLine("<styleUrl>#yellowLineGreenPoly</styleUrl>");
            sb.AppendLine("<LineString>");
            sb.AppendLine("<extrude>1</extrude>");
            sb.AppendLine("<tesselate>1</tesselate>");


            sb.AppendLine("</LineString>");
            sb.AppendLine("</Placemark>");
            */

            // placemarks
            foreach (var p in eventObjects) {
                sb.AppendLine("<Placemark>");
                sb.AppendLine("<Point>");
                sb.AppendFormat("<coordinates>{0},{1},0</coordinates>", p.transientOBDReadings.Latitude, p.transientOBDReadings.Longitude);
                sb.AppendLine("</Point>");
                sb.AppendLine("</Placemark>");

            }


            sb.AppendLine("</kml>");
            return sb.ToString();
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

        public int DurationSeconds { get; set; }

    }
}