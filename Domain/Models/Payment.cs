namespace Domain.Models
{
    public class Payment : BaseEntity
    {
        public int SaleId { get; set; }
        public int? PaymentTypeId { get; set; }
        public decimal Value { get; set; }
        public bool IsOnline { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }

        public Sale Sale { get; set; }
        public PaymentType PaymentType { get; set; }
    }
}
