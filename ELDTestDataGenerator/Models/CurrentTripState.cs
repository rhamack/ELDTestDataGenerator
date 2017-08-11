using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELDTestDataGenerator.Models
{
    public class CurrentTripState
    {
        public string VIN { get; set; }

        public string CMVUnitNumber { get; set; }

        public string CMVTrailerNumbers { get; set; }

        public string ShippingDocumentNumber { get; set; }

        public string CarrierUSDOTNumber { get; set; }

        public string CurrentDriverId { get; set; }

        public List<CurrentDriverState> Drivers { get; set; }

        public string BluetoothId { get; set; }

        public CurrentTripState() {
            Drivers = new List<CurrentDriverState>();
        }

        public CurrentTripState(bool setDefaults) {
            Drivers = new List<CurrentDriverState>();
            if (setDefaults)
                SetDefaults();
        }

        public CurrentDriverState CurrentDriver {
            get
            {
                CurrentDriverState cds = null;
                var ds = Drivers.SingleOrDefault(x => x.DriverId == this.CurrentDriverId);
                if (ds != null)
                    cds = ds;

                return cds;
            }
}
        public CurrentTripState Clone() {
            CurrentTripState ts = new CurrentTripState();
            ts = (CurrentTripState) this.MemberwiseClone();
            ts.Drivers = new List<CurrentDriverState>();
            for(int n=0; n< this.Drivers.Count; n++)
            {
                CurrentDriverState cds = this.Drivers[n].Clone();
                ts.Drivers.Add(cds);
            }
            return ts;
        }

        public void SetDefaults() {
            CurrentDriverState driver = new CurrentDriverState(true);
            this.Drivers.Add(driver);

            this.BluetoothId = "testbluetoothid";
            this.CarrierUSDOTNumber = "C123456789";
            this.CMVTrailerNumbers = "AZ9876";
            this.CMVUnitNumber = "CAR54";
            this.CurrentDriverId = driver.DriverId;
            this.ShippingDocumentNumber = "MANIFEST123";
            this.VIN = "ABCD1234EFGH56789";
        }
    }
}
