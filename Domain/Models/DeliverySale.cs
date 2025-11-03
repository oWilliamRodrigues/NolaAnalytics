namespace Domain.Models
{
    public class DeliverySale : BaseEntity
    {
        public int SaleId { get; set; }
        public string CourierId { get; set; }
        public string CourierName { get; set; }
        public string CourierPhone { get; set; }
        public string CourierType { get; set; }
        public string DeliveredBy { get; set; }
        public string DeliveryType { get; set; }
        public string Status { get; set; }
        public float DeliveryFee { get; set; }
        public float CourierFee { get; set; }
        public string Timing { get; set; }
        public string Mode { get; set; }

        public Sale Sale { get; set; }
        public ICollection<DeliveryAddress> DeliveryAddresses { get; set; }
    }
}
