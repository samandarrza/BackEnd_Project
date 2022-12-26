namespace Quarter.Models
{
    public class Review
    {
        public int HouseId { get; set; }
        public int AppUserId { get; set; }
        public byte Rate { get; set; }
        public string Text { get; set; }
        public House House { get; set; }
        public AppUser AppUser { get; set; }
    }
}
