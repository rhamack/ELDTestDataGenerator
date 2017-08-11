using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELDTestDataGenerator
{
    public class EventGenerator
    {
        public static Models.EventSet GenerateEvents(string profileId)
        {
            Models.EventSet es = new Models.EventSet();


            // default
            Models.TestProfile p = new Models.TestProfile();
            p.LoadTrip();

            double lat = p.startingLatitude;
            double lng = p.startingLongitude;

            Models.EventObjects eo = new Models.EventObjects();
            es.eventObjects.Add(eo);

            // startup events...
            Models.TransientOBDReadings obd = new Models.TransientOBDReadings();
            obd.Latitude = lat;
            obd.Longitude = lng;
            obd.SpeedometerReading = 0;
            eo.transientOBDReadings = obd;

            DateTimeOffset currDT = p.StartingDateTime;

            // set the current states
            Models.CurrentLocationState currentLocationState = new Models.CurrentLocationState(); // done
            Models.CurrentVehicleState currentVehicleState = new Models.CurrentVehicleState();    // done
            Models.DataDiagnosticState dataDiagnosticState = new Models.DataDiagnosticState();    // TODO:
            Models.DeviceMalfunctionState deviceMalfunctionState = new Models.DeviceMalfunctionState(); //TODO:
            Models.CurrentTripState currentTripState = new Models.CurrentTripState();             // TODO:
            Models.CommonParms commonParms = new Models.CommonParms();

            // set up the current Vehicle state
            currentVehicleState.CurrentEventCode = 1; // off duty
            currentVehicleState.CurrentSpecialDrivingCategoryCode = 0; // none
            currentVehicleState.SpeedometerReading = 0;
            currentVehicleState.VehicleIsMoving = false;






            // set the current and previous locations
            Models.Location PrevLocation = new Models.Location();
            Models.Location currLocation = new Models.Location();

            currLocation.EngineHours = p.startingEngineHours;
            currLocation.Latitude = lat;
            currLocation.LocationRecordedDateTime = p.StartingDateTime;
            currLocation.OdometerReading = p.startingOdometer;
            currLocation.ReducedGPSPrecision = false;

            PrevLocation.EngineHours = p.startingEngineHours;
            PrevLocation.Latitude = lat;
            PrevLocation.LocationRecordedDateTime = p.StartingDateTime;
            PrevLocation.OdometerReading = p.startingOdometer;
            PrevLocation.ReducedGPSPrecision = false;

            currentLocationState.CurrentLocation = currLocation;
            currentLocationState.PreviousLocation = PrevLocation;



            int intermediateInterval = 300; // 5 minutes?
            int currentBearing = 180;

            foreach (var pseg in p.profileSegments)
            {
                // start a clock, increment per 5 minutes...
                DateTimeOffset segEnd = currDT.AddSeconds(pseg.DurationSeconds);
                do
                {

                    // calc the new position and create the event objects
                    Models.EventObjects eox = new Models.EventObjects();
                    es.eventObjects.Add(eox);

                    currentBearing = GetBearing(lat, lng, currentBearing);

                    var r = GPSCalculator.CalcPosition(lat, lng, currentBearing, pseg.MPH, intermediateInterval);
                    var obdx = new Models.TransientOBDReadings() { Latitude = r.EndingLatitude, Longitude = r.EndingLongitude, SpeedometerReading = (byte)pseg.MPH };
                    eox.transientOBDReadings = obdx;

                    // set the location state
                    currentLocationState.SetNewLocation(obdx);


                    // clone the vehicle state
                    currentVehicleState.SpeedometerReading = (byte)pseg.MPH;
                    if (pseg.MPH > 0)
                    {
                        currentVehicleState.VehicleIsMoving = true;
                    }

                    switch (pseg.Action)
                    {
                        case "PreDrivingMovement":
                            currentVehicleState.CurrentEventCode = 4; // on duty
                            currentVehicleState.CurrentSpecialDrivingCategoryCode = 0;
                            break;
                        case "Driving":
                            currentVehicleState.CurrentEventCode = 3; // driving
                            currentVehicleState.CurrentSpecialDrivingCategoryCode = 0;
                            break;
                        case "DrivingPersonalUse":
                            currentVehicleState.CurrentEventCode = 3; // driving
                            currentVehicleState.CurrentSpecialDrivingCategoryCode = 1;
                            break;
                        case "DrivingYardMoves":
                            currentVehicleState.CurrentEventCode = 3; // driving
                            currentVehicleState.CurrentSpecialDrivingCategoryCode = 2;
                            break;

                    }


                    // clone the current states into the eventobjects
                    eox.currentVehicleState = currentVehicleState.CloneFromCurrent();
                    eox.currentLocationState = currentLocationState.CloneFromCurrent();

                    var c = currentVehicleState.Clone();
                    var cc = currentLocationState.Clone();

                    // set the common parms
                    eox.commonParms = commonParms;




                    lat = r.EndingLatitude;
                    lng = r.EndingLongitude;

                    currDT = currDT.AddSeconds(intermediateInterval); // interval for intermediate events...


                } while (currDT <= segEnd);
            }
            return es;

        }

        private static int GetBearing(double lat, double lng, int currentBearing)
        {
            // plot a course roughly around the US

            return 180;
        }


    }
}