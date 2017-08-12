using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ELDTestDataGenerator.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Test() {

            // default
            return View(getDefaultEventSet());
        }

        public ActionResult KML() {

            return this.Content(getDefaultEventSet().ToKML(), "application/vnd.google-earth.kml+xml");
        }


        public ActionResult Index()
        {

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private Models.EventSet getDefaultEventSet() {
            Models.TestProfile p = new Models.TestProfile();
            p.LoadTripSegments();
            p.profileSegments[0].DurationSeconds = 122 * 3600; // 122 hours of driving at 60MPH will take us all the the way around...
            p.PollingIntervalSeconds = 3600; // each hour
            Models.EventSet es = EventGenerator.GenerateEvents(p);

            return es;
        }

    }
}