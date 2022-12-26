using System.ComponentModel.DataAnnotations;

namespace Quarter.Models
{
    public class Aminity
    {
        public int Id { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        [MaxLength(30)]
        public string Icon { get; set; }
    }
}
