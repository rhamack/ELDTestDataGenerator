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

            Models.EventSet es = EventGenerator.GenerateEvents("test");

            return View(es);
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
    }
}