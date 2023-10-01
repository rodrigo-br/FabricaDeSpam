namespace Domain.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Image
    {
#nullable disable
        [Key]
        [Required]
        public Guid Id { get; set; }

        [ForeignKey("UserId")]
        [Required]
        public string UserId { get; set; }

        [Required]
        public byte[] ImageData { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public string MimeType { get; set; }

        public User User { get; set; }
    }
}