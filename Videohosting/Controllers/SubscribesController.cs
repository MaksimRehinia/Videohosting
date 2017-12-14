using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Videohosting.Models;

namespace Videohosting.Controllers
{
    public class SubscribesController : Controller
    {
        // GET: Subscribes
        public ActionResult Index()
        {   
            return View(new ApplicationDbContext().Subscriptions.FirstOrDefault(sub => sub.Subscriber.User.UserName == System.Web.HttpContext.Current.User.Identity.Name).Channels.ToList());
        }
    }
}