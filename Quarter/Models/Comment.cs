namespace Quarter.Models
{
    public class Comment:BaseEntity
    {
        public int HouseId { get; set; }
        public string AppUserId { get; set; }
        public string Text { get; set; }

        public House House { get; set; }
        public AppUser AppUser { get; set; }
    }
}
