using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Videohosting.CustomResults;

namespace Videohosting.Controllers
{
    public class VideoController : Controller
    {
        // GET: Video/video
        [Authorize(Roles = "User")]
        public ActionResult Index(string filePath)
        {
            return new VideoResult(filePath);
        }
    }
}