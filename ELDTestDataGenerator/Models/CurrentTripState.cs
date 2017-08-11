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
        public CurrentTripState MWClone() {
            CurrentTripState ts = (CurrentTripState) this.MemberwiseClone();
            return ts;
        }
    }
}
