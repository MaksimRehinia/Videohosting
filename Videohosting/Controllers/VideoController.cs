using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using RestSharp.Extensions;
using Videohosting.CustomResults;
using Videohosting.Models;

namespace Videohosting.Controllers
{
    public class VideoController : Controller
    {
        // GET: Video/video   
        public ActionResult ShowVideo(int id)
        {
            return new VideoResult(id);
        }
        
        public ActionResult Display(int id)
        {
            return PartialView(new ApplicationDbContext().Videos.FirstOrDefault(video => video.Id == id));
        }

        [Authorize(Roles = "User")]
        public ActionResult AddComment(string message, string name)
        {
            Comment comment;
            using (var db = new ApplicationDbContext())
            {
                comment = new Comment
                {
                    Message = message,
                    Video = db.Videos.First(video => video.Title == name),
                    User = db.Users.First(usr => usr.UserName == System.Web.HttpContext.Current.User.Identity.Name)
                };
                db.Comments.Add(comment);
                db.SaveChanges();
            }

            return PartialView(comment);
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
        public ActionResult Upload(Video video, HttpPostedFileBase file)
        {
            if (ModelState.IsValid && file != null)
            {                              
                var fileName = Path.GetFileName(file.FileName);

                var stream = file.InputStream;

                using (var db = new ApplicationDbContext())
                {
                    var chanel = db.Chanels.FirstOrDefault(temp =>
                        temp.User.UserName == System.Web.HttpContext.Current.User.Identity.Name);
                    video.Chanel = chanel;
                    video.ContentBytes = stream.ReadAsBytes();
                    video.FilePath = fileName;
                    db.Entry(video).State = EntityState.Added;

                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index", "Chanel");
        }
    }
}