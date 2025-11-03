namespace Domain.Models
{
    public class ItemProductSale : BaseEntity
    {
        public int ProductSaleId { get; set; }
        public int ItemId { get; set; }
        public int? OptionGroupId { get; set; }

        public float Quantity { get; set; }
        public float AdditionalPrice { get; set; }
        public float Price { get; set; }
        public float Amount { get; set; }
        public string Observations { get; set; }

        public ProductSale ProductSale { get; set; }
        public Item Item { get; set; }
        public OptionGroup OptionGroup { get; set; }

        public ICollection<ItemItemProductSale> ItemItemProductSales { get; set; }
    }
}
