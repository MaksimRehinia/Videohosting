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
        private static int _currentPage = 0;
        private IComparer<Video> currentComparer = new ViewCountComparer();

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
            _currentPage = 1;
            return View(new ApplicationDbContext().Channels.First(channel =>
                channel.User.UserName == System.Web.HttpContext.Current.User.Identity.Name));
            //return ViewMore(System.Web.HttpContext.Current.User.Identity.Name);            
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
                return PartialView("_DisplayVideos", GetItemsPage(userName, currentComparer).Videos.ToList());
            }

            return View("Index", GetItemsPage(userName, currentComparer));
        }

        private static Channel GetItemsPage(string userName, IComparer<Video> comparer)
        {
            var itemsToSkip = _currentPage * PageSize;
            _currentPage++;
            var db = new ApplicationDbContext();
            var channel = db.Channels.FirstOrDefault(t => t.User.UserName == userName);
            channel.Videos = channel.Videos.ToList();
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
            currentComparer = new ViewCountComparer();
            return PartialView("_DisplayVideos", GetItemsPage(userName, currentComparer).Videos.ToList());
        }

        public ActionResult SortByLikes(string userName)
        {
            _currentPage = 0;
            currentComparer = new LikeComparer();
            return PartialView("_DisplayVideos", GetItemsPage(userName, currentComparer).Videos.ToList());
        }

        private class ViewCountComparer : IComparer<Video>
        {
            public int Compare(Video x, Video y)
            {
                if (x == null || y == null)
                {
                    throw new ArgumentNullException();
                }

                return x.ViewsCount > y.ViewsCount ? -1 : x.ViewsCount == y.ViewsCount ? 0 : 1;
            }
        }

        private class LikeComparer : IComparer<Video>
        {
            public int Compare(Video x, Video y)
            {
                if (x == null || y == null)
                {
                    throw new ArgumentNullException();
                }

                return x.LikesCount > y.LikesCount ? -1 : x.LikesCount == y.LikesCount ? 0 : 1;
            }
        }
    }
}