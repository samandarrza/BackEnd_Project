using Quarter.Attributes.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quarter.Models
{
    public class Broker
    {
        public int Id { get; set; }
        [MaxLength(30)]
        public string Fullname { get; set; }
        public string? Image { get; set; }
        public string? Desc { get; set; }
        public List<House>? Houses { get; set; }

        [NotMapped]
        [MaxFileSize(2048)]
        [AllowedFileTypes("image/jpeg", "image/png")]
        public IFormFile? ProfilFile { get; set; }
    }
}
