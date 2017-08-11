using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELDTestDataGenerator.Models
{
    public class DataDiagnosticState
    {
        public List<Diagnostic> Diagnostics { get; set; }

        public DataDiagnosticState() {
            Diagnostics = new List<Diagnostic>();
        }

        public bool HasCurrentDiagnostic(string deviceId) {
            int dcount = Diagnostics.Count(x => x.AssociatedDeviceId == deviceId);
            return dcount == 0 ? false : true;
        }
    }

    public class Diagnostic
    {
        public string DiagnosticCode { get; set; }

        public string AssociatedDeviceId { get; set; }

        public DateTimeOffset DiagnosticNotedDateTime { get; set; }

        public double TimeSinceDiagnosticStarted { get {

                TimeSpan ts = new TimeSpan(DateTimeOffset.Now.Ticks - DiagnosticNotedDateTime.Ticks);
                return ts.TotalSeconds;

            } }

    }
}
