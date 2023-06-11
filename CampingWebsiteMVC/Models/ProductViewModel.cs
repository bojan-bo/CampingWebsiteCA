namespace CampingWebsiteMVC.Models
{
    public class ProductViewModel
    {
        public string Id { get; set; }
        public string CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsOnSale { get; set; }
        public double OriginalPrice { get; set; }
    }
}

