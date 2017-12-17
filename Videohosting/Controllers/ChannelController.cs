using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Videohosting.Controllers.Comparers;
using Videohosting.Models;

namespace Videohosting.Controllers
{
    public class ChannelController : Controller
    {
        private static readonly int PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"]);
        private static int _currentPage;
        private IComparer<Video> _currentComparer = new ViewCountComparer();

        [Authorize(Roles = "User")]
        public ActionResult Index()
        {
            _currentPage = 0;
            using (var db = new ApplicationDbContext())
            {
                if (db.Channels.FirstOrDefault(
                        channel => channel.User.UserName == db.Users.FirstOrDefault(usr =>
                                       usr.UserName == System.Web.HttpContext.Current.User.Identity.Name).UserName) == null)
                {
                    db.Channels.Add(new Channel
                    {
                        User = db.Users.First(usr => usr.UserName == System.Web.HttpContext.Current.User.Identity.Name),
                        Videos = new List<Video>(),
                        VideosCount = 0
                    });

                    db.SaveChanges();
                }                
            }
            
            return ViewMore(System.Web.HttpContext.Current.User.Identity.Name);
        }

        public ActionResult Channel(string userName)
        {
            _currentPage = 0;
            if (userName == User.Identity.Name)
            {
                return RedirectToAction("Index");
            }

            CheckSubscription(userName);
            return View(GetItemsPage(userName, _currentComparer));
        }
        
        private void CheckSubscription(string userName)
        {            
            var db = new ApplicationDbContext();
            var subs = db.Subscriptions.FirstOrDefault(subscription => subscription.Subscriber.User.UserName ==
                                                                       System.Web.HttpContext.Current.User.Identity.Name);
            if (subs == null)
            {
                ViewBag.IsSubscribed = false;
                return;
            }

            var sub = subs.Channels.FirstOrDefault(channel => channel.User.UserName == userName);
            if (sub == null)
            {
                ViewBag.IsSubscribed = false;
                return;
            }

            ViewBag.IsSubscribed = true;            
        }        

        public ActionResult Subscribe(string userName, int channelId)
        {
            var isAuthorized = CheckIfAuthorized();
            if (isAuthorized != null)
                return isAuthorized;

            ViewBag.IsSubscribed = true;
            var db = new ApplicationDbContext();
            var subs = db.Subscriptions.FirstOrDefault(subscription => subscription.Subscriber.User.UserName == userName);
            if (subs == null)
            {
                db.Subscriptions.Add(new Subscription
                {
                    // Add new subscription profile
                    Subscriber = db.Channels.First(channel => channel.User.UserName == userName),
                    Channels = new List<Channel> { db.Channels.First(channel => channel.Id == channelId) }
                });

                db.SaveChanges();                
                return RedirectToAction("Channel", "Channel", db.Channels.First(channel => channel.Id == channelId).User.UserName);
            }

            var sub = subs.Channels.FirstOrDefault(channel => channel.Id == channelId);
            if (sub == null)
            {
                // Adding subscription to already created profile
                subs.Channels.Add(db.Channels.First(channel => channel.Id == channelId));
            }

            db.SaveChanges();            
            return RedirectToAction("Channel", "Channel", db.Channels.First(channel => channel.Id == channelId).User.UserName);
        }
        
        public ActionResult Unsubscribe(string userName, int channelId)
        {
            var isAuthorized = CheckIfAuthorized();
            if (isAuthorized != null)
                return isAuthorized;

            ViewBag.IsSubscribed = false;
            var db = new ApplicationDbContext();
            var subs = db.Subscriptions.FirstOrDefault(subscription => subscription.Subscriber.User.UserName == userName);
            if (subs == null)
            {                                
                return RedirectToAction("Channel", "Channel", db.Channels.First(channel => channel.Id == channelId).User.UserName);
            }
            var sub = subs.Channels.FirstOrDefault(channel => channel.Id == channelId);
            if (sub != null)
            {                 
                subs.Channels.Remove(sub);
            }

            db.SaveChanges();            
            return RedirectToAction("Channel", "Channel", db.Channels.First(channel => channel.Id == channelId).User.UserName);
        }
        
        public ActionResult ViewMore(string userName)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("_DisplayVideos", GetItemsPage(userName, _currentComparer).Videos.ToList());
            }

            return View("Index", GetItemsPage(userName, _currentComparer));
        }

        private static Channel GetItemsPage(string userName, IComparer<Video> comparer)
        {
            var itemsToSkip = _currentPage * PageSize;
            _currentPage++;
            var db = new ApplicationDbContext();
            var channel = db.Channels.FirstOrDefault(t => t.User.UserName == userName);            
            channel.Videos = db.Videos.ToList()
                .Where(video => video.Channel.User.UserName == userName)
                .OrderBy(t => t, comparer)
                .Skip(itemsToSkip)
                .Take(PageSize)
                .ToList();

            if (channel.Videos?.Count == 0)
            {
                _currentPage--;
            }

            return channel;
        }

        public ActionResult SortByViews(string userName)
        {
            _currentPage = 0;
            _currentComparer = new ViewCountComparer();
            return PartialView("_DisplayVideos", GetItemsPage(userName, _currentComparer).Videos.ToList());
        }

        public ActionResult SortByLikes(string userName)
        {
            _currentPage = 0;
            _currentComparer = new LikeComparer();
            return PartialView("_DisplayVideos", GetItemsPage(userName, _currentComparer).Videos.ToList());
        }

        private ActionResult CheckIfAuthorized()
        {
            if (!System.Web.HttpContext.Current.User.IsInRole("User"))
            {
                return RedirectToAction("Login", "Account");
            }

            return null;
        }
    }
}