namespace MLInstagram.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string? FileName { get; set; }
        public DateTime DatePosted { get; set; }
        public string? Caption { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public float? Duration { get; set; }
    }
}
