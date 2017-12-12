using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Videohosting.Models
{
    public class Chanel
    {   
        public int Id { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Video> Videos { get; set; }
    }
}