using System.Collections.Generic;

namespace Videohosting.Models
{
    public class Video
    {
        public int Id { get; set; }
                
        public string Title { get; set; }

        public string Description { get; set; }

        public byte[] ContentBytes { get; set; }

        public string FilePath { get; set; }        
        
        public int LikesCount { get; set; }

        public int DislikesCount { get; set; }

        public int ViewsCount { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}