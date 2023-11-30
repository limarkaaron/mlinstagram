using MLInstagram.Data;

namespace MLInstagram.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public DateTime DatePosted { get; set; }
        public string? Caption { get; set; }
        public string S3Url { get; set; }

		public string UploaderId { get; set; }
	}
}