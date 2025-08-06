namespace RestaurantOps.Legacy.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        
        // Navigation properties for EF Core
        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }
} 