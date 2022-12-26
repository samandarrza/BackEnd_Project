using System.ComponentModel.DataAnnotations;

namespace Quarter.Models
{
    public class Broker
    {
        public int Id { get; set; }
        [MaxLength(30)]
        public string Fullname { get; set; }
        public string? Image { get; set; }
        public string? Desc { get; set; }

    }
}
