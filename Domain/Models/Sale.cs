namespace Domain.Models
{
    public class Sale : BaseEntity
    {
        public int StoreId { get; set; }
        public int? SubBrandId { get; set; }
        public int? CustomerId { get; set; }
        public int ChannelId { get; set; }

        public string CodSale1 { get; set; }
        public string CodSale2 { get; set; }
        public string CustomerName { get; set; }
        public string SaleStatusDesc { get; set; }

        public decimal TotalAmountItems { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalIncrease { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal ServiceTaxFee { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ValuePaid { get; set; }

        public int? ProductionSeconds { get; set; }
        public int? DeliverySeconds { get; set; }
        public int? PeopleQuantity { get; set; }

        public string DiscountReason { get; set; }
        public string IncreaseReason { get; set; }
        public string Origin { get; set; }

        public Store Store { get; set; }
        public SubBrand SubBrand { get; set; }
        public Customer Customer { get; set; }
        public Channel Channel { get; set; }

        public ICollection<ProductSale> ProductSales { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public ICollection<CouponSale> CouponSales { get; set; }

        public DeliverySale DeliverySale { get; set; }
    }
}
