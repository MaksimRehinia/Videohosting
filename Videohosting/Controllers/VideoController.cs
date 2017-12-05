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
        public ActionResult Index(string filePath = "1.mp4")
        {
            return new VideoResult(filePath);
        }
    }
}