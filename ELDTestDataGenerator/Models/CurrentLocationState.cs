using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELDTestDataGenerator.Models
{
    public class CurrentLocationState
    {
        public Location CurrentLocation { get; set; }

        public Location PreviousLocation { get; set; }


        public CurrentLocationState() {
            this.CurrentLocation = new Location();
            this.PreviousLocation = new Location();
        }

        public double TimeFromPreviousToCurrentLocation { get {
                if (PreviousLocation != null && CurrentLocation != null)
                {
                    TimeSpan ts = new TimeSpan(CurrentLocation.LocationRecordedDateTime.Ticks - PreviousLocation.LocationRecordedDateTime.Ticks);
                    return ts.TotalSeconds;
                }
                return 0;
            }
        }

        public double TimeSinceCurrentLocation { get {
                TimeSpan ts = new TimeSpan(DateTimeOffset.Now.Ticks - CurrentLocation.LocationRecordedDateTime.Ticks);
                return ts.TotalSeconds;
            }
        }

        public long MileageSinceLastLocation { get {
                long change = 0L;
                if (PreviousLocation != null && CurrentLocation != null)
                {
                    if (CurrentLocation.OdometerReading.HasValue && PreviousLocation.OdometerReading.HasValue)
                        change = CurrentLocation.OdometerReading.Value - PreviousLocation.OdometerReading.Value;
                }
                return change;
            } }

        public double EngineHoursSinceLastLocation
        {
            get
            {
                double change = 0D;
                if (PreviousLocation != null && CurrentLocation != null)
                {
                    if (CurrentLocation.EngineHours.HasValue && PreviousLocation.EngineHours.HasValue)
                        change = CurrentLocation.EngineHours.Value - PreviousLocation.EngineHours.Value;
                }
                return change;
            }
        }

        public void SetNewLocation(TransientOBDReadings readings) {
            this.PreviousLocation = this.CurrentLocation;
            this.CurrentLocation = new Location();
            this.CurrentLocation.EngineHours = readings.EngineHours;
            this.CurrentLocation.Latitude = readings.Latitude;
            this.CurrentLocation.LocationRecordedDateTime = DateTimeOffset.Now;
            this.CurrentLocation.Longitude = readings.Longitude;
            this.CurrentLocation.OdometerReading = readings.OdometerReading;
            this.CurrentLocation.ReducedGPSPrecision = PreviousLocation != null ? PreviousLocation.ReducedGPSPrecision : false;
        }

        public CurrentLocationState CloneFromCurrent() {
            CurrentLocationState cls = new CurrentLocationState();
            cls.CurrentLocation = this.CurrentLocation.CloneFromCurrent();
            cls.PreviousLocation = this.PreviousLocation.CloneFromCurrent();
            


            return cls;
        }

        public CurrentLocationState Clone()
        {
            CurrentLocationState cls = (CurrentLocationState)this.MemberwiseClone();
            cls.CurrentLocation = this.CurrentLocation.CloneFromCurrent();
            cls.PreviousLocation = this.PreviousLocation.CloneFromCurrent();



            return cls;
        }

    }



    public class Location {
        public double? Latitude { get; set;}
        public double? Longitude { get; set; }

        public DateTimeOffset LocationRecordedDateTime { get; set; }

        public Int32? OdometerReading { get; set; }

        public double? EngineHours { get; set; }

        public bool ReducedGPSPrecision { get; set; }

        public Location CloneFromCurrent() {
            Location loc = new Location();
            loc.EngineHours = this.EngineHours;
            loc.Latitude = this.Latitude;
            loc.LocationRecordedDateTime = this.LocationRecordedDateTime;
            loc.Longitude = this.Longitude;
            loc.OdometerReading = this.OdometerReading;
            loc.ReducedGPSPrecision = this.ReducedGPSPrecision;
            return loc;
        }

    }


}
