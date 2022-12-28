using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Quarter.Attributes.ValidationAttributes;

namespace Quarter.Models
{
    public class Slider
    {
        public int Id { get; set; }
        [MaxLength(30)]
        public string Title1 { get; set; }
        [MaxLength(30)]
        public string Title2 { get; set; }
        [MaxLength(30)]
        public string? SubTitleIcon { get; set; }
        [MaxLength(30)]
        public string? SubTitle { get; set; }
        [MaxLength(300)]
        public string Desc { get; set; }
        [MaxLength(60)]
        public string? Image { get; set; }
        [MaxLength(30)]
        public string BtnText { get; set; }
        [MaxLength(200)]
        public string RedirectUrl { get; set; }
        public int Order { get; set; }
        [NotMapped]
        [MaxFileSize(2048)]
        [AllowedFileTypes("image/jpeg", "image/png")]
        public IFormFile? ImageFile { get; set; }
    }
}
