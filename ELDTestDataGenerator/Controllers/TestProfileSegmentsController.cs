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
    public class TestProfileSegmentsController : Controller
    {
        private ELDTestDataEntities db = new ELDTestDataEntities();

        // GET: TestProfileSegments
        public ActionResult Index(string profileId)
        {
            var profile = db.TestProfiles.SingleOrDefault(x => x.ProfileId == profileId);
            if (profile != null) {

                var testProfileSegments = db.TestProfileSegments.Include(t => t.TestProfile);
                ViewBag.ProfileId = profileId;
                return View(testProfileSegments.Where(x=>x.ProfileId == profileId).OrderBy(x=>x.SegmentSeqNum).ToList());

            }

            return View("Empty");
        }


        // GET: TestProfileSegments/Create
        public ActionResult Create(string profileId)
        {
            //ViewBag.ProfileId = new SelectList(db.TestProfiles, "ProfileId", "ProfileName");

            ViewBag.ActionsList = new SelectList(db.Actions, "ActionId", "ActionDesc");

            ViewBag.ProfileId = profileId;
            return View();
        }

        // POST: TestProfileSegments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProfileId,SegmentSeqNum,ActionId,CommentAnnotation,DateOfCertifiedRecord,DriversLocationDesc,DurationSeconds,MPH")] TestProfileSegment testProfileSegment)
        {
            if (ModelState.IsValid)
            {
                db.TestProfileSegments.Add(testProfileSegment);
                db.SaveChanges();
                return RedirectToAction("Index", new { profileId = testProfileSegment.ProfileId});
            }

            ViewBag.ActionsList = new SelectList(db.Actions, "ActionId", "ActionDesc");
            ViewBag.ProfileId = testProfileSegment.ProfileId;
            return View(testProfileSegment);
        }

        // GET: TestProfileSegments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestProfileSegment testProfileSegment = db.TestProfileSegments.Find(id);
            if (testProfileSegment == null)
            {
                return HttpNotFound();
            }
            ViewBag.ActionsList = new SelectList(db.Actions, "ActionId", "ActionDesc");
            return View(testProfileSegment);
        }

        // POST: TestProfileSegments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProfileId,SegmentSeqNum,ActionId,CommentAnnotation,DateOfCertifiedRecord,DriversLocationDesc,DurationSeconds,MPH")] TestProfileSegment testProfileSegment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(testProfileSegment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { profileId = testProfileSegment.ProfileId });
            }
            ViewBag.ActionsList = new SelectList(db.Actions, "ActionId", "ActionDesc");
            return View(testProfileSegment);
        }

        // GET: TestProfileSegments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestProfileSegment testProfileSegment = db.TestProfileSegments.Find(id);
            if (testProfileSegment == null)
            {
                return HttpNotFound();
            }
            return View(testProfileSegment);
        }

        // POST: TestProfileSegments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            TestProfileSegment testProfileSegment = db.TestProfileSegments.Find(id);
            string profileId = testProfileSegment.ProfileId;
            db.TestProfileSegments.Remove(testProfileSegment);
            db.SaveChanges();
            return RedirectToAction("Index", new { profileId = profileId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
