namespace GameZoneApp.Models
{
    public class Game:BaseEntity
    {

        [MaxLength(2500)]
        public string? Description { get; set; }
        [MaxLength(500)]
        public string? Cover { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = default!;

        public ICollection<GameDevice> Devices { get; set; } = new List<GameDevice>();
    }
}
