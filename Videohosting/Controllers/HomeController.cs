using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Videohosting.Controllers.Comparers;
using Videohosting.Models;

namespace Videohosting.Controllers
{
    public class HomeController : Controller
    {
        private static readonly int PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"]);
        private static int _currentPage;
        private IComparer<Video> _currentComparer = new ViewCountComparer();

        public ActionResult Index()
        {
            _currentPage = 0;
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

        private static List<Video> GetItemsPage(IComparer<Video> comparer)
        {
            var itemsToSkip = _currentPage * PageSize;
            _currentPage++;
            var db = new ApplicationDbContext();
            var videos = db.Videos.ToList();
            videos = videos.OrderBy(t => t, comparer).Skip(itemsToSkip).Take(PageSize).ToList();
            if (videos?.Count == 0)
            {
                _currentPage--;
            }

            return videos;
        }

        public ActionResult SortByViews()
        {
            _currentPage = 0;
            _currentComparer = new ViewCountComparer();
            return PartialView("_DisplayVideos", GetItemsPage(_currentComparer));
        }

        public ActionResult SortByLikes()
        {
            _currentPage = 0;
            _currentComparer = new LikeComparer();
            return PartialView("_DisplayVideos", GetItemsPage(_currentComparer));
        }                
    }   
}