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
            var db = new ApplicationDbContext();
            var video = db.Videos.FirstOrDefault(vid => vid.Id == id);
            if (video != null)
            {
                video.ViewsCount++;
                db.SaveChanges();
            }
            return PartialView(video);
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
            if (ModelState.IsValid && file != null && file.InputStream.Length <= 2 * 1024 * 1024 * 1024L - 1)
            {                              
                var fileName = Path.GetFileName(file.FileName);                

                using (var db = new ApplicationDbContext())
                {
                    var channel = db.Channels.FirstOrDefault(temp =>
                        temp.User.UserName == System.Web.HttpContext.Current.User.Identity.Name);
                    
                    video.Channel = channel;
                    video.ContentBytes = file.InputStream.ReadAsBytes();
                    video.FilePath = fileName;
                    db.Entry(video).State = EntityState.Added;
                    channel.Videos.Add(video);
                    db.SaveChanges();
                }

                return RedirectToAction("Index", "Channel");
            }

            ViewBag.ErrorMessage = "Video size must be less then 2 Gb.";
            return View();            
        }

        [Authorize]
        public ActionResult Like(int videoId)
        {
            Video video = null;
            double percentage = 0;
            using (var db = new ApplicationDbContext())
            {
                video = db.Videos.FirstOrDefault(temp => temp.Id == videoId);
                var like = db.Likes.Where(temp => temp.User.UserName == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault(temp => temp.Video.Id == videoId);
                if (like == null)
                {
                    db.Likes.Add(new Like
                    {
                        User = db.Users.First(
                            temp => temp.UserName == System.Web.HttpContext.Current.User.Identity.Name),
                        LikeValue = true,
                        Video = video
                    });
                    video.LikesCount++;
                }
                else
                {
                    if (!like.LikeValue)
                    {
                        like.LikeValue = true;
                        video.LikesCount++;
                        video.DislikesCount--;
                    }
                }
                db.SaveChanges();
                percentage = video.LikesCount;
                percentage = percentage / (video.LikesCount + video.DislikesCount);
                percentage = percentage * 100 % 101;
            }
            return PartialView((int)percentage);
        }

        [Authorize]
        public ActionResult Dislike(int videoId)
        {
            Video video = null;
            double percentage = 0;
            using (var db = new ApplicationDbContext())
            {
                video = db.Videos.FirstOrDefault(temp => temp.Id == videoId);
                var like = db.Likes.Where(temp => temp.User.UserName == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault(temp => temp.Video.Id == videoId);
                if (like == null)
                {

                    db.Likes.Add(new Like
                    {
                        User = db.Users.First(
                            temp => temp.UserName == System.Web.HttpContext.Current.User.Identity.Name),
                        LikeValue = false,
                        Video = video

                    });
                    video.DislikesCount++;
                }
                else
                {
                    if (like.LikeValue)
                    {
                        like.LikeValue = false;
                        video.DislikesCount++;
                        video.LikesCount--;
                    }
                }
                db.SaveChanges();
                percentage = video.LikesCount;
                percentage = percentage / (video.LikesCount + video.DislikesCount);
                percentage = percentage * 100 % 101;

            }
            return PartialView("Like", (int)percentage);

        }
    }
}