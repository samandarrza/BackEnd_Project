using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Quarter.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public int OrderId { get; set; }
        public int? HouseId { get; set; }
        public int SalePrice { get; set; }
        public int DiscountPercent { get; set; }
        public int CostPrice { get; set; }

        public Order Order { get; set; }
        public House? House { get; set; }
    }
}
