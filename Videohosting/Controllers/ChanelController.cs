using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Videohosting.Models;

namespace Videohosting.Controllers
{
    public class ChanelController : Controller
    {   
        [Authorize]
        public ActionResult Index()
        {
            return View(new ApplicationDbContext().Videos.Where(video=>
                video.Chanel.User.UserName == System.Web.HttpContext.Current.User.Identity.Name).ToList());
        }

        public ActionResult Chanel(string userName)
        {
            if (userName == User.Identity.Name)
            {
                return RedirectToAction("Index");
            }
            return View(new ApplicationDbContext().Videos.Where(temp =>
                temp.Chanel.User.UserName == userName).ToList());
        }
    }
}