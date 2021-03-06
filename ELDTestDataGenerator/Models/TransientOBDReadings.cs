﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELDTestDataGenerator.Models
{
    public class TransientOBDReadings
    {
        public string VIN { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public Int32? OdometerReading { get; set; }

        public byte? SpeedometerReading { get; set; }

        public double? EngineHours { get; set; }

        public string MalfunctionCode { get; set; }

        public string DataDiagnosticCode { get; set; }

        public TransientOBDReadings() {
        }

        public TransientOBDReadings(TestProfile p) {
            this.Latitude = p.travelProfile.startingLatitude; 
            this.Longitude = p.travelProfile.startingLongitude ;
            this.SpeedometerReading = 0;
            this.EngineHours = p.startingEngineHours;
            this.OdometerReading = p.startingOdometer;
        }

    }
}
