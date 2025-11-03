namespace Domain.Models
{
    public class CouponSale : BaseEntity
    {
        public int SaleId { get; set; }
        public int CouponId { get; set; }
        public float Value { get; set; }
        public string Target { get; set; }
        public string Sponsorship { get; set; }

        public Sale Sale { get; set; }
        public Coupon Coupon { get; set; }
    }
}
