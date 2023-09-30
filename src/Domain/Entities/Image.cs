namespace Domain.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Image
    {
#nullable disable
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public byte[] ImageData { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public string MimeType { get; set; }

        public User User { get; set; }
    }
}