namespace Domain.Models
{
    public class ItemItemProductSale : BaseEntity
    {
        public int ItemProductSaleId { get; set; }
        public int ItemId { get; set; }
        public int? OptionGroupId { get; set; }

        public float Quantity { get; set; }
        public float AdditionalPrice { get; set; }
        public float Price { get; set; }
        public float Amount { get; set; }

        public ItemProductSale ItemProductSale { get; set; }
        public Item Item { get; set; }
        public OptionGroup OptionGroup { get; set; }
    }
}
