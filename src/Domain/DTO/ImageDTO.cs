namespace Domain.DTO
{
    public class ImageDTO
    {
        public string UserId { get; set; }
        public byte[] ImageData { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }
    }
}
