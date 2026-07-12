using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RetailInventory.Models
{
    public class Product
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; } = null!;
        
        public decimal Price { get; set; }
        
        public int CategoryId { get; set; }
        
        // Navigation property marked virtual for Lazy Loading (Lab 10)
        public virtual Category Category { get; set; } = null!;

        // Added in Lab 8 for schema updates
        public int StockQuantity { get; set; }

        // Added in Lab 11 for Many-to-Many relationships
        public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();

        // Added in Lab 11 for One-to-One relationships
        public virtual ProductDetail? ProductDetail { get; set; }

        // Added in Lab 15 for concurrency detection
        [Timestamp]
        public byte[] RowVersion { get; set; } = null!;
    }
}
