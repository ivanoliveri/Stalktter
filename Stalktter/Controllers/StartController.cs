using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Stalktter.Controllers
{
    public class StartController : Controller
    {

        public ActionResult Index()
        {
            return View(new StartViewModel());
        }

        public ActionResult Stalk(StartViewModel startViewModel)
        {
            return RedirectToAction("Index", "Home", new RouteValueDictionary(startViewModel));
        }

    }
}
