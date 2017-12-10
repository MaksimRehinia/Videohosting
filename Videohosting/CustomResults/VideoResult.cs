using System.IO;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Videohosting.CustomResults
{
    public class VideoResult : ActionResult
    {
        private readonly string _filePath;

        public VideoResult(string filePath)
        {
            _filePath = filePath;
        }

        /// <summary> 
        /// The below method will respond with the Video file.
        /// </summary> 
        /// <param name="context"></param> 
        public override void ExecuteResult(ControllerContext context)
        {
            var videoFilePath = HostingEnvironment.MapPath("~/App_Data/Videos/" + _filePath);
            if (File.Exists(videoFilePath))
            {
                context.HttpContext.Response.Clear();
                context.HttpContext.Response.ContentType = "application/octet-stream";
                context.HttpContext.Response.AppendHeader("Content-Disposition", "filename=" + _filePath);
                context.HttpContext.Response.TransmitFile(videoFilePath);
                context.HttpContext.Response.End();
            }
        }
    }
}