namespace Quarter.Models
{
    public class HouseAmenity
    {
        public int Id { get; set; }
        public int HouseId { get; set; }
        public int AmenityId { get; set; }

        public House House { get; set; }
        public Aminity Amenity { get; set; }
    }
}
