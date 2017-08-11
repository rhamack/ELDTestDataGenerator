using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELDTestDataGenerator.Models
{
    public class CurrentDriverState 
    {
        public string DriverId { get; set; }
        public string DeviceId { get; set; }

        public string DriverFirstName { get; set; }
        public string DriverLastName { get; set; }

        public string DriverLicenseNumber { get; set; }

        public string DriverLicenseIssuingStateCode { get; set; }



        public byte DriverDayStartHour { get; set; }

        public string TimeZoneId { get; set; }

        public string CarrierMultiDayBasis { get; set; }
 
        public bool DriverIsExempt { get; set; }

        public bool PersonalUseOfCMVAllowed { get; set; }

        public bool YardMovesAllowed { get; set; }

        public bool PersonalUseOfCMVInEffect { get; set; }

        public bool IsUnidentifiedDriver { get; set; }

        public CurrentDriverState() {
        }

        public CurrentDriverState(bool setDefaults)
        {
            if (setDefaults)
                SetDefaults();
        }

        public CurrentDriverState Clone()
        {
            CurrentDriverState clone = new CurrentDriverState();
            clone = (CurrentDriverState)this.MemberwiseClone();
            return clone;
        }

        public void SetDefaults() {
            this.CarrierMultiDayBasis = "7";
            this.DeviceId = "testdevice";
            this.DriverDayStartHour = 0;
            this.DriverFirstName = "Test";
            this.DriverId = "testdriverid";
            this.DriverIsExempt = false;
            this.DriverLastName = "Driver";
            this.DriverLicenseIssuingStateCode = "AZ";
            this.DriverLicenseNumber = "AZ123456";
            this.IsUnidentifiedDriver = false;
            this.PersonalUseOfCMVAllowed = false;
            this.PersonalUseOfCMVInEffect = false;
            this.TimeZoneId = "Mountain Standard Time";
            this.YardMovesAllowed = false;
            
        }

    }
}
