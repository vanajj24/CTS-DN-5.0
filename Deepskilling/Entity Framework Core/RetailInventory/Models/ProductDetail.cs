namespace RetailInventory.Models
{
    public class ProductDetail
    {
        public int ProductDetailId { get; set; }
        public string WarrantyInfo { get; set; } = null!;
        public int ProductId { get; set; }
        
        // Navigation property marked virtual for Lazy Loading (Lab 10)
        public virtual Product Product { get; set; } = null!;
    }
}
