using System.Linq;
using System.Web.Mvc;
using Videohosting.Models;

namespace Videohosting.CustomResults
{
    public class VideoResult : ActionResult
    {
        private readonly int _id;

        public VideoResult(int id)
        {
            _id = id;
        }

        /// <summary> 
        /// The below method will respond with the Video file.
        /// </summary> 
        /// <param name="context"></param> 
        public override void ExecuteResult(ControllerContext context)
        {
            using (var db = new ApplicationDbContext())
            {
                var video = db.Videos.FirstOrDefault(item => item.Id == _id);
                if (!ReferenceEquals(video, null))
                {
                    context.HttpContext.Response.Clear();
                    context.HttpContext.Response.ContentType = "video/mp4";
                    context.HttpContext.Response.AppendHeader("Content-Disposition", "filename=" + video.FilePath);
                    context.HttpContext.Response.BinaryWrite(video.ContentBytes);
                    context.HttpContext.Response.End();
                }
            }            
        }
    }
}