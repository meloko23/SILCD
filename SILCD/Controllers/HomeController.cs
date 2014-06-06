using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SILCD.Controllers {
    public class HomeController : BaseController {
        public ActionResult Index() {
            return View();
        }

        public ActionResult LandingPage() {
            return View();
        }

        public ActionResult Sobre() {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact() {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}