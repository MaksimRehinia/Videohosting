namespace Videohosting.Models
{
    public class Video
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }
        // Id of the user that uploaded the video.
        public int UserId { get; set; }
        public int LikesCount { get; set; }
        public int DislikesCount { get; set; }
        public int ViewsCount { get; set; }
        public int CommentsCount { get; set; }
    }
}