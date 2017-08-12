using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ELDTestDataGenerator.Data;

namespace ELDTestDataGenerator.Controllers
{
    public class TestProfilesController : Controller
    {
        private ELDTestDataEntities db = new ELDTestDataEntities();

        // GET: TestProfiles
        public ActionResult Index()
        {
            return View(db.TestProfiles.ToList());
        }

        // GET: TestProfiles/Create
        public ActionResult Create()
        {
            ViewBag.CarrierMultiDayBasisList = getMultiDaySelectList();
            ViewBag.TimeZonesList = new SelectList(db.TimeZones, "TimeZoneId", "TimeZoneId");
            ViewBag.StartingHourList = getStartingHourList();

            return View();
        }

        // POST: TestProfiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProfileId,ProfileName,PollingIntervalSeconds,StartingDateTime,StartingEngineHours,StartingOdometer,CarrierUSDOTNumber,BluetoothId,CMVUnitNumber, CMVTrailerNumbers,ShippingDocumentNumber,VIN,CarrierMultiDayBasis,DeviceId,CurrentDriverId,Driver1Id,Driver1FirstName,Driver1LastName,Driver1IsExempt,Driver1DayStartHour,Driver1DriverLicenseIssuingStateCode,Driver1DriverLicenseNumber,Driver1PersonalUseOfCMVAllowed,Driver1YardMovesAllowed,Driver1TimeZoneId,Driver2Id,Driver2FirstName,Driver2LastName,Driver2IsExempt,Driver2DayStartHour,Driver2DriverLicenseIssuingStateCode,Driver2DriverLicenseNumber,Driver2YardMovesAllowed,Driver2PersonalUseOfCMVAllowed,Driver2TimeZoneId")] TestProfile testProfile)
        {
            if (ModelState.IsValid)
            {
                db.TestProfiles.Add(testProfile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CarrierMultiDayBasisList = getMultiDaySelectList();
            ViewBag.TimeZonesList = new SelectList(db.TimeZones, "TimeZoneId", "TimeZoneId");
            ViewBag.StartingHourList = getStartingHourList();
            return View(testProfile);
        }

        // GET: TestProfiles/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestProfile testProfile = db.TestProfiles.Find(id);
            if (testProfile == null)
            {
                return HttpNotFound();
            }
            ViewBag.CarrierMultiDayBasisList = getMultiDaySelectList();
            ViewBag.TimeZonesList = new SelectList(db.TimeZones, "TimeZoneId", "TimeZoneId");
            ViewBag.StartingHourList = getStartingHourList();
            return View(testProfile);
        }

        // POST: TestProfiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProfileId,ProfileName,PollingIntervalSeconds,StartingDateTime,StartingEngineHours,StartingOdometer,CarrierUSDOTNumber,BluetoothId,CMVUnitNumber,CMVTrailerNumbers,ShippingDocumentNumber,VIN,CarrierMultiDayBasis,DeviceId,CurrentDriverId,Driver1Id,Driver1FirstName,Driver1LastName,Driver1IsExempt,Driver1DayStartHour,Driver1DriverLicenseIssuingStateCode,Driver1DriverLicenseNumber,Driver1PersonalUseOfCMVAllowed,Driver1YardMovesAllowed,Driver1TimeZoneId,Driver2Id,Driver2FirstName,Driver2LastName,Driver2IsExempt,Driver2DayStartHour,Driver2DriverLicenseIssuingStateCode,Driver2DriverLicenseNumber,Driver2YardMovesAllowed,Driver2PersonalUseOfCMVAllowed,Driver2TimeZoneId")] TestProfile testProfile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(testProfile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CarrierMultiDayBasisList = getMultiDaySelectList();
            ViewBag.TimeZonesList = new SelectList(db.TimeZones, "TimeZoneId", "TimeZoneId");
            ViewBag.StartingHourList = getStartingHourList();
            return View(testProfile);
        }

        // GET: TestProfiles/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestProfile testProfile = db.TestProfiles.Find(id);
            if (testProfile == null)
            {
                return HttpNotFound();
            }
            return View(testProfile);
        }

        // POST: TestProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TestProfile testProfile = db.TestProfiles.Find(id);
            db.TestProfiles.Remove(testProfile);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Plot(string id)
        {
            // get the profile and segments...
            Data.ELDTestDataEntities db = new ELDTestDataEntities();
            db.Configuration.LazyLoadingEnabled = false;
            var dbTestProfile = db.TestProfiles.Include("TestProfileSegments").SingleOrDefault(x => x.ProfileId == id);


            Models.EventSet es = new Models.EventSet();
            if (dbTestProfile != null) {
                // create the local current state objects
                Models.TestProfile p = new Models.TestProfile();
                p.DefaultCurrentTripState.BluetoothId = dbTestProfile.BluetoothId;
                p.DefaultCurrentTripState.CarrierUSDOTNumber = dbTestProfile.CarrierUSDOTNumber;
                p.DefaultCurrentTripState.CMVTrailerNumbers = dbTestProfile.CMVTrailerNumbers;
                p.DefaultCurrentTripState.CMVUnitNumber = dbTestProfile.CMVUnitNumber;
                p.PollingIntervalSeconds = dbTestProfile.PollingIntervalSeconds.Value;
                p.ProfileId = dbTestProfile.ProfileId;
                p.ProfileName = dbTestProfile.ProfileName;
                p.StartingDateTime = dbTestProfile.StartingDateTime.Value;
                p.startingEngineHours = dbTestProfile.StartingEngineHours.Value;
                p.startingOdometer = dbTestProfile.StartingOdometer.Value;
                p.travelProfile = new Models.TravelProfile();

                foreach (var seg in dbTestProfile.TestProfileSegments) {
                    Models.TestProfileSegment tps = new Models.TestProfileSegment();
                    tps.ActionId = seg.ActionId;
                    //tps.CommentAnnotation = seg.CommentAnnotation;
                    //tps.CompassBearing = seg.
                    tps.DateOfCertifiedRecord = seg.DateOfCertifiedRecord;
                    tps.DriversLocationDesc = seg.DriversLocationDesc;
                    tps.DurationSeconds = seg.DurationSeconds.HasValue ? seg.DurationSeconds.Value : 0;
                    tps.MPH = seg.MPH.HasValue ? seg.MPH.Value : 0;
                    tps.ProfileId = seg.ProfileId;
                    tps.SegmentSeqNum = seg.SegmentSeqNum;
                    
                    p.profileSegments.Add(tps);
                }

                p.DefaultCurrentTripState.CurrentDriverId = dbTestProfile.Driver1Id;


                Models.CurrentDriverState cds = new Models.CurrentDriverState();
                cds.CarrierMultiDayBasis = dbTestProfile.CarrierMultiDayBasis;
                cds.DeviceId = dbTestProfile.DeviceId;
                cds.DriverDayStartHour = (byte) (dbTestProfile.Driver1DayStartHour.HasValue ? dbTestProfile.Driver1DayStartHour.Value : 0);
                cds.DriverFirstName = dbTestProfile.Driver1FirstName;
                cds.DriverId = dbTestProfile.Driver1Id;
                cds.DriverIsExempt = dbTestProfile.Driver1IsExempt;
                cds.DriverLastName = dbTestProfile.Driver1LastName;
                cds.DriverLicenseIssuingStateCode = dbTestProfile.Driver1DriverLicenseIssuingStateCode;
                cds.DriverLicenseNumber = dbTestProfile.Driver1DriverLicenseNumber;
                cds.IsUnidentifiedDriver = false;
                cds.PersonalUseOfCMVAllowed = dbTestProfile.Driver1PersonalUseOfCMVAllowed;
                cds.PersonalUseOfCMVInEffect = false;
                cds.TimeZoneId = dbTestProfile.Driver1TimeZoneId;
                cds.YardMovesAllowed = dbTestProfile.Driver1YardMovesAllowed;


                p.DefaultCurrentTripState.Drivers.Add(cds);

                es = EventGenerator.GenerateEvents(p);
            }


            //Models.TestProfile p = new Models.TestProfile();
            //p.LoadTripSegments();
            //p.profileSegments[0].DurationSeconds = 40 * 3600; // 122 hours of driving at 60MPH will take us all the the way around...
            //p.PollingIntervalSeconds = 3600; // each hour
            //Models.EventSet es = EventGenerator.GenerateEvents(p);

            return View(es);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        private List<SelectListItem> getMultiDaySelectList()
        {
            List<SelectListItem> items = new List<SelectListItem>() { new SelectListItem() { Value = "7", Text = "7" }, new SelectListItem() { Value = "8", Text = "8" } };
            return items;
        }

        private List<SelectListItem> getStartingHourList()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            for (int n = 0; n < 24; n++)
            {
                string text = n.ToString("00");
                if (n == 0)
                    text = "Midnight";
                else
                {
                    if (n == 12)
                        text = "Noon";
                    else
                    {
                        if (n > 12)
                        {
                            text = (n - 12).ToString() + " PM";
                        }
                        else
                        {
                            text = n.ToString() + " AM";
                        }
                    }
                }

                var sli = new SelectListItem() { Value = n.ToString(), Text = text };
                items.Add(sli);
            }
            return items;
        }

    }
}
