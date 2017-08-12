using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELDTestDataGenerator
{
    public class EventGenerator
    {
        public static Models.EventSet GenerateEvents(Models.TestProfile p)
        {
            Models.EventSet es = new Models.EventSet();
            es.ProfileId = p.ProfileId;

            DateTimeOffset currDT = p.StartingDateTime;

            // set the initial current states
            Models.CurrentLocationState currentLocationState = new Models.CurrentLocationState(p); // done
            Models.CurrentVehicleState currentVehicleState = new Models.CurrentVehicleState(p);    // done
            Models.DataDiagnosticState dataDiagnosticState = new Models.DataDiagnosticState();    // TODO:
            Models.DeviceMalfunctionState deviceMalfunctionState = new Models.DeviceMalfunctionState(); //TODO:
            Models.CurrentTripState currentTripState = new Models.CurrentTripState(true);

            Models.TransientOBDReadings obd = new Models.TransientOBDReadings(p);
            obd.VIN = currentTripState.VIN;

            int polling_interval = p.PollingIntervalSeconds; // 5 minutes?

            int currentBearing = p.travelProfile.startingCompassBearing;
            double lat = p.travelProfile.startingLatitude;
            double lng = p.travelProfile.startingLongitude;
            double engineHours = p.startingEngineHours;
            int odometer = p.startingOdometer;



            foreach (var pseg in p.profileSegments)
            {
                // start a clock, increment per 5 minutes...
                DateTimeOffset segEnd = currDT.AddSeconds(pseg.DurationSeconds);

                // we always add a record for a segment, but only add intermediate records
                // when the vehicle is moving.

                bool firstEntry = true;

                do
                {

                    // calc the new position and create the event objects
                    Models.EventObjects eox = new Models.EventObjects();
                    eox.IntendedEventName = pseg.ActionId;
                    eox.DurationSeconds = pseg.DurationSeconds;

                    // calculate the new position
                    if (pseg.MPH > 0)
                    {
                        currentBearing = p.travelProfile.GetBearing(lat, lng, currentBearing); // get the bearing from the travel profile

                        // moving, so calc new position
                        var r = GPSCalculator.CalcPosition(lat, lng, currentBearing, pseg.MPH, polling_interval);

                        // accumulate the trip values
                        odometer += (int)r.MilesTraveled;

                        lat = r.EndingLatitude;
                        lng = r.EndingLongitude;

                    }

                    if (firstEntry || pseg.MPH > 0) 
                        es.eventObjects.Add(eox);


                    // add the interval to the engine hours
                    engineHours += (double)((double)polling_interval / (double)3600);

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

                    currDT = currDT.AddSeconds(polling_interval); // interval for intermediate events...


                    // create the common parameters in support of the events
                    Models.CommonParms commonParms = new Models.CommonParms();
                    commonParms.CommentAnnotation = pseg.CommentAnnotation;
                    commonParms.DateOfCertifiedRecord = pseg.DateOfCertifiedRecord;
                    commonParms.DriversLocationDesc = pseg.DriversLocationDesc;
                    //commonParms.EventCode = ? // below
                    commonParms.EventDataCheckValue = ""; // let the model do this
                    commonParms.EventId = System.Guid.NewGuid().ToString();
                    commonParms.RelatedEventId = commonParms.EventId;
                    commonParms.EventRecordOriginCode = 1; // recorded by ELD
                    commonParms.EventRecordStatusCode = 1; // active
                    commonParms.EventSequenceNum = eox.SeqNum;
                    commonParms.EventTimestamp = currDT;
                    //commonParms.EventTypeCode = ?; // below

                    /* the only event requiring code and type code is driver status change
                     *  , EventEnums.DriverStatusChangeEventCode eventCode // 1 = off duty, 2 = sleeper berth, 3 = driving, 4 = on-duty not driving
                        , EventEnums.EventRecordOrigin eventRecordOriginCode // 1 = automatically created by ELD, 2 = created by driver, 3 = requested by other, 4 = assumed from unidentified
                     */



                    switch (pseg.ActionId)
                    {
                        case "DriverLogin":
                            commonParms.EventTypeCode = 5;
                            currentVehicleState.CurrentEventCode = 4; // on duty not driving
                            currentVehicleState.CurrentSpecialDrivingCategoryCode = 0;
                            currentTripState.CurrentDriver.PersonalUseOfCMVInEffect = false;
                            break;
                        case "EngineStart":
                            commonParms.EventTypeCode = 6;
                            currentVehicleState.CurrentEventCode = 4; // on duty not driving
                            currentVehicleState.CurrentSpecialDrivingCategoryCode = 0;
                            currentTripState.CurrentDriver.PersonalUseOfCMVInEffect = false;
                            break;
                        case "EngineShutdown":
                            commonParms.EventTypeCode = 6;
                            currentVehicleState.CurrentEventCode = 4; // on duty not driving
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
                        case "OnDutyNotDriving":
                            currentVehicleState.CurrentEventCode = 4; // on duty
                            currentVehicleState.CurrentSpecialDrivingCategoryCode = 0;
                            currentTripState.CurrentDriver.PersonalUseOfCMVInEffect = false;
                            break;
                        case "OffDuty":
                            currentVehicleState.CurrentEventCode = 1; // off duty
                            currentVehicleState.CurrentSpecialDrivingCategoryCode = 0;
                            currentTripState.CurrentDriver.PersonalUseOfCMVInEffect = false;
                            break;
                        case "SleeperBerth":
                            currentVehicleState.CurrentEventCode = 2; // sleeper berth
                            currentVehicleState.CurrentSpecialDrivingCategoryCode = 0;
                            currentTripState.CurrentDriver.PersonalUseOfCMVInEffect = false;
                            break;


                    }

                    if (firstEntry)
                        commonParms.EventTypeCode = currentVehicleState.CurrentEventCode;
                    else
                        commonParms.EventTypeCode = 2; // intermediate event



                    // clone the current states into the eventobjects
                   // so they can be recreated
                    eox.currentVehicleState = currentVehicleState.Clone();
                    eox.currentLocationState = currentLocationState.Clone();
                    eox.currentTripState = currentTripState.Clone();
                    eox.dataDiagnosticState = dataDiagnosticState.Clone();
                    eox.deviceMalfunctionState = deviceMalfunctionState.Clone();

                    // set the common parms
                    eox.commonParms = commonParms; 


                    firstEntry = false;

                } while (currDT <= segEnd);
            }
            return es;

        }


    }
}