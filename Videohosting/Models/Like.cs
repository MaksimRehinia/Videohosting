using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Videohosting.Models
{
    public class Like
    {
        public int Id { get; set; }
        public virtual ApplicationUser User { get; set; }
        public bool LikeValue { get; set; }
        public virtual Video Video { get; set; }

    }
}