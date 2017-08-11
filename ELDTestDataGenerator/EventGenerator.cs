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
            p.LoadTripSegments();

            DateTimeOffset currDT = p.StartingDateTime;

            bool setDefaults = true;

            // set the current states
            Models.CurrentLocationState currentLocationState = new Models.CurrentLocationState(p); // done
            Models.CurrentVehicleState currentVehicleState = new Models.CurrentVehicleState(p);    // done
            Models.DataDiagnosticState dataDiagnosticState = new Models.DataDiagnosticState();    // TODO:
            Models.DeviceMalfunctionState deviceMalfunctionState = new Models.DeviceMalfunctionState(); //TODO:
            Models.CurrentTripState currentTripState = new Models.CurrentTripState(setDefaults);
            Models.CommonParms commonParms = new Models.CommonParms();

            Models.TransientOBDReadings obd = new Models.TransientOBDReadings(p);
            obd.VIN = currentTripState.VIN;

            int intermediateInterval = 300; // 5 minutes?
            int currentBearing = 180;

            double lat = p.travelProfile.startingLatitude;
            double lng = p.travelProfile.startingLongitude;
            double engineHours = p.startingEngineHours;
            int odometer = p.startingOdometer;

            foreach (var pseg in p.profileSegments)
            {
                // start a clock, increment per 5 minutes...
                DateTimeOffset segEnd = currDT.AddSeconds(pseg.DurationSeconds);
                do
                {

                    // calc the new position and create the event objects
                    Models.EventObjects eox = new Models.EventObjects();
                    eox.IntendedEventName = pseg.Action;
                    es.eventObjects.Add(eox);

                    // calculate the new position
                    currentBearing = p.travelProfile.GetBearing(lat, lng, currentBearing);
                    if (pseg.MPH > 0)
                    {
                        // moving, so calc new position
                        var r = GPSCalculator.CalcPosition(lat, lng, currentBearing, pseg.MPH, intermediateInterval);

                        // accumulate the trip values
                        odometer += (int)r.MilesTraveled;

                        lat = r.EndingLatitude;
                        lng = r.EndingLongitude;

                    }
                    // add the interval to the engine hours
                    engineHours += (double)((double)intermediateInterval / (double)3600);

                    var obdx = new Models.TransientOBDReadings() { Latitude = lat, Longitude = lng, SpeedometerReading = (byte)pseg.MPH, VIN=currentTripState.VIN , OdometerReading = odometer, EngineHours = engineHours};
                    eox.transientOBDReadings = obdx;

                    // set the location state
                    currentLocationState.SetNewLocation(obdx);

                    // modify vehicle state
                    int seqnum = currentVehicleState.GetNextEventSequenceNumber();
                    eox.SeqNum = seqnum;

                    currentVehicleState.SpeedometerReading = (byte)pseg.MPH;
                    if (pseg.MPH > 0)
                        currentVehicleState.VehicleIsMoving = true;

                    switch (pseg.Action)
                    {
                        case "DriverLogin":
                            currentVehicleState.CurrentEventCode = 4; // on duty not driving
                            currentVehicleState.CurrentSpecialDrivingCategoryCode = 0;
                            currentTripState.CurrentDriver.PersonalUseOfCMVInEffect = false;
                            break;
                        case "EngineStart":
                            currentVehicleState.CurrentEventCode = 4; // on duty not driving
                            currentVehicleState.CurrentSpecialDrivingCategoryCode = 0;
                            currentTripState.CurrentDriver.PersonalUseOfCMVInEffect = false;
                            break;

                        case "PreDrivingMovement":
                            //currentVehicleState.CurrentEventCode = 4; // whatever it was before
                            currentVehicleState.CurrentSpecialDrivingCategoryCode = 0;
                            currentTripState.CurrentDriver.PersonalUseOfCMVInEffect = false;
                            break;
                        case "Driving":
                            currentVehicleState.CurrentEventCode = 3; // driving
                            currentVehicleState.CurrentSpecialDrivingCategoryCode = 0;
                            currentTripState.CurrentDriver.PersonalUseOfCMVInEffect = false;
                            break;
                        case "DrivingPersonalUse":
                            currentVehicleState.CurrentEventCode = 3; // driving
                            currentVehicleState.CurrentSpecialDrivingCategoryCode = 1;
                            currentTripState.CurrentDriver.PersonalUseOfCMVInEffect = true;
                            break;
                        case "DrivingYardMoves":
                            currentVehicleState.CurrentEventCode = 3; // driving
                            currentVehicleState.CurrentSpecialDrivingCategoryCode = 2;
                            currentTripState.CurrentDriver.PersonalUseOfCMVInEffect = false;
                            break;
                        case "StopMoving":
                            //currentVehicleState.CurrentEventCode = 3; // whatever it was before
                            currentVehicleState.CurrentSpecialDrivingCategoryCode = 0;
                            currentTripState.CurrentDriver.PersonalUseOfCMVInEffect = false;
                            break;

                    }


                    // clone the current states into the eventobjects
                    // so they can be recreated
                    eox.currentVehicleState = currentVehicleState.Clone();
                    eox.currentLocationState = currentLocationState.Clone();
                    eox.currentTripState = currentTripState.Clone();
                    eox.dataDiagnosticState = dataDiagnosticState.Clone();
                    eox.deviceMalfunctionState = deviceMalfunctionState.Clone();



                    // set the common parms
                    eox.commonParms = commonParms;

                    currDT = currDT.AddSeconds(intermediateInterval); // interval for intermediate events...


                } while (currDT <= segEnd);
            }
            return es;

        }


    }
}