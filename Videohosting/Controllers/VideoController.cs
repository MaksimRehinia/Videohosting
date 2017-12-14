﻿using System.Data.Entity;
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
            if (ModelState.IsValid && file != null && file.InputStream.Length <= 2 * 1024 * 1024 * 1024L - 1)
            {                              
                var fileName = Path.GetFileName(file.FileName);                

                using (var db = new ApplicationDbContext())
                {
                    var chanel = db.Chanels.FirstOrDefault(temp =>
                        temp.User.UserName == System.Web.HttpContext.Current.User.Identity.Name);
                    video.Chanel = chanel;
                    video.ContentBytes = file.InputStream.ReadAsBytes();
                    video.FilePath = fileName;
                    db.Entry(video).State = EntityState.Added;

                    db.SaveChanges();
                }

                return RedirectToAction("Index", "Chanel");
            }

            ViewBag.ErrorMessage = "Video size must be less then 2 Gb.";
            return View();            
        }

        [Authorize]
        public ActionResult Like(int videoId)
        {
            Video video = null;
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
            }
            return PartialView(video);
        }

        [Authorize]
        public ActionResult Dislike(int videoId)
        {
            Video video = null;
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
                    video.LikesCount++;
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
            }
            return PartialView("Like",video);

        }
    }
}