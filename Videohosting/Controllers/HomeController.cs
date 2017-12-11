using System.Linq;
using System.Web.Mvc;
using Videohosting.Models;

namespace Videohosting.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return PartialView(new ApplicationDbContext().Videos.ToList());
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
            return PartialView(new ApplicationDbContext().Videos.ToList());
        }
    }
}