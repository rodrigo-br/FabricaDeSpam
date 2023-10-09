namespace WebApp.Models
{
    public class ImageViewModel
    {
        public byte[] ImageData { get; set; }
        public List<string> Topics { get; set; }
        public string FileName { get; set; }
        public string UserId { get; set; }
        public string MimeType { get; set; }
    }
}
