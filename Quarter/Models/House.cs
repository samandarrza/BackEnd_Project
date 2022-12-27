using Quarter.Attributes.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quarter.Models
{
    public class House:BaseEntity
    {
        [MaxLength(30)]
        public string Name { get; set; }
        [MaxLength(3000)]
        public string? Desc { get; set; }
        public byte RoomsCount { get; set; }
        public byte BedRoomCount { get; set; }
        public byte BathRoomCount { get; set; }
        public int BuiltYear { get; set; }
        public int SquareFt { get; set; }
        public int CarParking { get; set; }
        public bool Status { get; set; }
        public int SalePrice { get; set; }
        public int CostPrice { get; set; }
        public int DiscountPercent { get; set; }
        public int BrokerId { get; set; }
        public int CategoryId { get; set; }
        public int CityId { get; set; }
        public Broker? Broker { get; set; }
        public Category? Category { get; set; }
        public City? City { get; set; }
        [NotMapped]
        public List<int>? AminityIds { get; set; } = new List<int>();
        [NotMapped]
        public List<int>? HouseImageIds { get; set; } = new List<int>();
        [NotMapped]
        [MaxFileSize(2048)]
        [AllowedFileTypes("image/jpeg", "image/png")]
        public IFormFile? PosterFile { get; set; }

        [NotMapped]
        [MaxFileSize(2048)]
        [AllowedFileTypes("image/jpeg", "image/png")]
        public List<IFormFile>? ImageFiles { get; set; }

        public List<HouseAmenity>? HouseAmenities { get; set; }
        public List<HouseImage>? HouseImages { get; set; }
    }
}
