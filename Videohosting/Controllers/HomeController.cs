using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Videohosting.CustomResults;
using Videohosting.Models;

namespace Videohosting.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new ApplicationDbContext().Videos.ToList());
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