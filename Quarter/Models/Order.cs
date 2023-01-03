using Quarter.Enums;
using System.ComponentModel.DataAnnotations;

namespace Quarter.Models
{
    public class Order : BaseEntity
    {
        public string? AppUserId { get; set; }
        [MaxLength(25)]
        public string Fullname { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(500)]
        public string? Note { get; set; }

        public AppUser? AppUser { get; set; }
        public List<OrderItem>? OrderItems { get; set; }
        public OrderStatus Status { get; set; }
    }
}
