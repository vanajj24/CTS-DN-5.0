using System.Collections.Generic;

namespace RetailInventory.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        // Navigation property marked virtual for Lazy Loading (Lab 10)
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
