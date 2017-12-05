using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Videohosting.CustomResults
{
    public class VideoResult : ActionResult
    {
        private readonly string filePath;

        public VideoResult(string filePath)
        {
            this.filePath = filePath;
        }


        /// <summary> 
        /// The below method will respond with the Video file 
        /// </summary> 
        /// <param name="context"></param> 
        public override void ExecuteResult(ControllerContext context)
        {
            var videoFilePath = HostingEnvironment.MapPath("~/App_Data/Videos/" + filePath);
            context.HttpContext.Response.AddHeader("Content-Disposition", "attachment; filename=" + filePath);
            var file = new FileInfo(videoFilePath);
            if (file.Exists)
            {
                var stream = file.OpenRead();
                var bytesinfile = new byte[stream.Length];
                stream.Read(bytesinfile, 0, (int)file.Length);
                context.HttpContext.Response.BinaryWrite(bytesinfile);
            }
        }
    }
}