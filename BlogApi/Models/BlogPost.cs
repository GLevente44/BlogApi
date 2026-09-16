namespace BlogApi.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Context { get; set; }
        public DateTime? postTime { get; set; }
        public DateTime? updateTime { get; set; }
        public int blogId { get; set; }
    }
}
