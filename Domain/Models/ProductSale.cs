namespace Domain.Models
{
    public class ProductSale : BaseEntity
    {
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public float Quantity { get; set; }
        public float BasePrice { get; set; }
        public float TotalPrice { get; set; }
        public string Observations { get; set; }

        public Sale Sale { get; set; }
        public Product Product { get; set; }
        public ICollection<ItemProductSale> ItemProductSales { get; set; }
    }
}
