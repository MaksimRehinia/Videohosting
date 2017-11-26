namespace Videohosting.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public int VideosCount { get; set; }                
    }
}