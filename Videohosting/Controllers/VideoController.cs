using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Videohosting.CustomResults;
using Videohosting.Models;

namespace Videohosting.Controllers
{
    public class VideoController : Controller
    {
        // GET: Video/   
        [Authorize(Roles = "User")]
        public ActionResult Index()
        {
            return View(new ApplicationDbContext().Videos.ToList());
        }

        // GET: Video/video  
        [Authorize(Roles = "User")]
        public ActionResult ShowVideo(string filePath)
        {
            return new VideoResult(filePath);
        }

        
        public ActionResult Display(string name)
        {
            return View(new ApplicationDbContext().Videos.First(video => video.FilePath == name));
        }

        // GET: Video/video 
        [Authorize(Roles = "User")]
        public ActionResult Upload()
        {
            return View();
        }

        // POST: Video/video
        [HttpPost]
        [Authorize(Roles = "User")]
        public ActionResult Upload(Video video, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid && upload != null)
            {                              
                var fileName = Path.GetFileName(upload.FileName);
                upload.SaveAs(Server.MapPath("~/App_Data/Videos/" + fileName));

                var db = new ApplicationDbContext();

                var user = db.Users.FirstOrDefault(usr => usr.UserName == System.Web.HttpContext.Current.User.Identity.Name);
                video.User = user ?? throw new NotImplementedException();
                video.FilePath = fileName;
                db.Entry(video).State = EntityState.Added;

                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}