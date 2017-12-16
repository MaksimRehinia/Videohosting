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
        private const int PageSize = 3;
        private static int _currentPage;

        [Authorize]
        public ActionResult Index()
        {
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
            _currentPage = 0;
            return ViewMore(System.Web.HttpContext.Current.User.Identity.Name);            
        }

        public ActionResult Channel(string userName)
        {
            if (userName == User.Identity.Name)
            {
                return RedirectToAction("Index");
            }
            var db = new ApplicationDbContext();
            return View(db.Channels.First(channel =>
                channel.User.UserName == userName));
        }

        [Authorize]
        public ActionResult Subscribe(string userName, int channelId)
        {
            ViewBag.Button = false;
            var db = new ApplicationDbContext();
            var subs = db.Subscriptions.FirstOrDefault(subscription =>
                subscription.Subscriber.User.UserName == System.Web.HttpContext.Current.User.Identity.Name);
            if (subs == null)
            {
                db.Subscriptions.Add(new Subscription()
                {
                    //Добавление нового профиля подписки
                    Subscriber = db.Channels.First(channel => channel.User.UserName == System.Web.HttpContext.Current.User.Identity.Name),
                    Channels = new List<Channel>() { db.Channels.First(channel => channel.Id == channelId) }
                });
                db.SaveChanges();
                return PartialView(db.Channels.First(channel => channel.Id == channelId));
            }
            var sub = subs.Channels.FirstOrDefault(channel => channel.Id == channelId);
            if (sub == null)
            {
                //Добавление подписки к уже созданному профилю
                subs.Channels.Add(db.Channels.First(channel => channel.Id == channelId));
            }
            db.SaveChanges();
            return PartialView(db.Channels.First(channel => channel.Id == channelId));
        }

        [Authorize]
        public ActionResult Unsubscribe(string userName, int channelId)
        {
            ViewBag.Button = true;
            var db = new ApplicationDbContext();
            var subs = db.Subscriptions.FirstOrDefault(subscription =>
                subscription.Subscriber.User.UserName == System.Web.HttpContext.Current.User.Identity.Name);
            if (subs == null)
            {
                return PartialView("Subscribe", db.Channels.First(channel => channel.Id == channelId));
                //Нет профиля подписок =>
            }
            var sub = subs.Channels.FirstOrDefault(channel => channel.Id == channelId);
            if (sub == null)
            {
                //В профиле нет подписок
            }
            else
            {
                //Есть подписка => удалить подписку
                subs.Channels.Remove(sub);
            }

            db.SaveChanges();
            return PartialView("Subscribe", db.Channels.First(channel => channel.Id == channelId));
        }

        [Authorize]
        public ActionResult ViewMore(string userName)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("_DisplayVideos", GetItemsPage(userName));
            }

            return View("Index", GetItemsPage(userName));
        }

        private static List<Video> GetItemsPage(string userName)
        {
            var itemsToSkip = _currentPage * PageSize;
            _currentPage++;
            var db = new ApplicationDbContext();
            var videos = db.Videos
                .Where(video => video.Channel.User.UserName == userName)
                .OrderBy(t => t.Id)
                .Skip(itemsToSkip)
                .Take(PageSize)
                .ToList();

            if (videos?.Count == 0)
            {
                _currentPage--;
            }

            return videos;
        }
    }
}