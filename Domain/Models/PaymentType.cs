namespace Domain.Models
{
    public class PaymentType : BaseEntity
    {
        public int? BrandId { get; set; }
        public string Description { get; set; }
    }
}
