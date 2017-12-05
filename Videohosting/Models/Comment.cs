namespace Videohosting.Models
{
    public class Comment
    {        
        public int Id { get; set; }
        public string Message { get; }
        public virtual Video Video { get; set; }        
        public virtual ApplicationUser User { get; set; }
    }
}