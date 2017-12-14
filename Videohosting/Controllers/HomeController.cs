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
                return PartialView("_DisplayVideos", GetItemsPage());
            }

            return View("Index", GetItemsPage());
        }

        private List<Video> GetItemsPage()
        {
            var itemsToSkip = currentPage * PageSize;
            currentPage++;
            using (var db = new ApplicationDbContext())
            {
                var videos = db.Videos.OrderBy(t => t.Id).Skip(itemsToSkip).Take(PageSize).ToList();
                if (videos?.Count == 0)
                {
                    currentPage--;
                }

                return videos;
            }
        }
    }
}