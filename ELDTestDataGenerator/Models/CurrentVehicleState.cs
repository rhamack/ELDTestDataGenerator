using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELDTestDataGenerator.Models
{
    public class CurrentVehicleState
    {
        private object thisLock = new object();


        private int LastEventSequenceNumber;

        public bool VehicleIsMoving { get; set; }

        public byte CurrentEventCode { get; set; } // driving, off-duty, sleeper berth, on-duty

        public byte CurrentSpecialDrivingCategoryCode { get; set; }

        public byte SpeedometerReading { get; set; }


        public int GetNextEventSequenceNumber()
        {
            // USE this method rather than the property
            lock (thisLock)
            {
                this.LastEventSequenceNumber++;
                return LastEventSequenceNumber;
            }
        }

        public void SetLastEventSequenceNumber(int lastEventSequenceNumber)
        {
            lock (thisLock)
            {
                this.LastEventSequenceNumber = lastEventSequenceNumber;
            }
        }

        public CurrentVehicleState Clone()
        {
            CurrentVehicleState cvs = new CurrentVehicleState();
            cvs = (CurrentVehicleState)this.MemberwiseClone();
            return cvs;

        }

        public CurrentVehicleState() { }

        public CurrentVehicleState(Models.TestProfile p) {
                SetDefaults();
        }


        public void SetDefaults() {
            this.CurrentEventCode = 1; // off duty
            this.CurrentSpecialDrivingCategoryCode = 0; // none
            this.SpeedometerReading = 0;
            this.VehicleIsMoving = false;

        }
    }
}
