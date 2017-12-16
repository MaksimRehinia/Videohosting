using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Videohosting.Models;

namespace Videohosting.Controllers
{
    public class HomeController : Controller
    {
        private const int PageSize = 3;
        private static int currentPage;
        private IComparer<Video> currentComparer = new ViewCountComparer();

        public ActionResult Index()
        {
            currentPage = 0;
            return ViewMore();
        }

        public ActionResult About()
        {            
            return View();
        }

        public ActionResult Contact()
        {            
            return View();
        }

        public ActionResult ViewMore()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("_DisplayVideos", GetItemsPage(new ViewCountComparer()));
            }

            return View("Index", GetItemsPage(new ViewCountComparer()));
        }

        private List<Video> GetItemsPage(IComparer<Video> comparer)
        {
            var itemsToSkip = currentPage * PageSize;
            currentPage++;
            var db = new ApplicationDbContext();
            var videos = db.Videos.ToList();
            videos = videos.OrderBy(t => t, comparer).Skip(itemsToSkip).Take(PageSize).ToList();
            if (videos?.Count == 0)
            {
                currentPage--;
            }
            return videos;

        }

        public ActionResult SortByViews()
        {
            currentPage = 0;
            currentComparer = new ViewCountComparer();
            return PartialView("_DisplayVideos", GetItemsPage(currentComparer));
        }

        public ActionResult SortByLikes()
        {
            currentPage = 0;
            currentComparer = new LikeComparer();
            return PartialView("_DisplayVideos", GetItemsPage(currentComparer));
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

                return x.LikesCount > y.LikesCount? -1 : x.LikesCount == y.LikesCount ? 0 : 1;
            }
        }
    }


   
}