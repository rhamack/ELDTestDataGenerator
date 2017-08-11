using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELDTestDataGenerator.Models
{
    public class DeviceMalfunctionState
    {
        public DeviceMalfunctionState()
        {
            Malfunctions = new List<DeviceMalfunction>();
        }

        public List<DeviceMalfunction> Malfunctions { get; set; }

        public bool HasCurrentMalfunction { get { return Malfunctions.Count > 0 ? true : false; } }

        public DeviceMalfunctionState Clone()
        {
            DeviceMalfunctionState clone = new DeviceMalfunctionState();
            clone = (DeviceMalfunctionState)this.MemberwiseClone();
            clone.Malfunctions.Clear();
            foreach (var d in this.Malfunctions)
            {
                DeviceMalfunction dm = (DeviceMalfunction)d.Clone();
                clone.Malfunctions.Add(dm);
            }
            return clone;
        }
    }

    public class DeviceMalfunction
    {
        public string MalfunctionCode { get; set; }
        public DateTimeOffset MalfunctionNotedDateTime { get; set; }

        public double TimeSinceMalfunctionStarted
        {
            get
            {
                TimeSpan ts = new TimeSpan(DateTimeOffset.Now.Ticks - MalfunctionNotedDateTime.Ticks);
                return ts.TotalSeconds;
            }
        }

        public DeviceMalfunction Clone()
        {
            DeviceMalfunction clone = new DeviceMalfunction();
            clone = (DeviceMalfunction)this.MemberwiseClone();
            return clone;
        }
    }
}
