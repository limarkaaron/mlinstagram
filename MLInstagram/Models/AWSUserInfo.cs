namespace MLInstagram.Models
{
	public class AWSUserInfo
	{
		public const string SectionName = "AWS";
		public string? AccessKey { get; set; }
		public string? SecretKey { get; set; }
		public string? S3Bucket { get; set; }
	}
}
