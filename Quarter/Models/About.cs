using Quarter.Attributes.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quarter.Models
{
    public class About
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
        [MaxLength(300)]
        public string Desc { get; set; }
        [MaxLength(300)]
        public string Text { get; set; }
        [MaxLength(100)]
        public string BtnUrl { get; set; }
        [MaxLength(50)]
        public string BtnText { get; set; }
        [MaxLength(100)]
        public string? Image { get; set; }
        [MaxLength(100)]
        public string? VideoImage { get; set; }
        [MaxLength(100)]
        public string VideoUrl { get; set; }
        [NotMapped]
        [MaxFileSize(2048)]
        [AllowedFileTypes("image/jpeg", "image/png")]
        public IFormFile? ImageFile { get; set; }
        [NotMapped]
        [MaxFileSize(2048)]
        [AllowedFileTypes("image/jpeg", "image/png")]
        public IFormFile? VideoImageFile { get; set; }
    }
}
