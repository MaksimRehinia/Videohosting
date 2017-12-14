using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Videohosting.Models;

namespace Videohosting.Controllers
{
    public class ChannelController : Controller
    {   
        [Authorize]
        public ActionResult Index()
        {
            return View(new ApplicationDbContext().Videos.Where(video=>
                video.Channel.User.UserName == System.Web.HttpContext.Current.User.Identity.Name).ToList());
        }

        public ActionResult Channel(string userName)
        {
            if (userName == User.Identity.Name)
            {
                return RedirectToAction("Index");
            }

            using (var db = new ApplicationDbContext())
            {
                return View(db.Videos.Where(temp =>
                    temp.Channel.User.UserName == userName).ToList());
            }
        }
    }
}