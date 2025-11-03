namespace Domain.Models
{
    public class DeliveryAddress : BaseEntity
    {
        public int SaleId { get; set; }
        public int? DeliverySaleId { get; set; }

        public string Street { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }
        public string FormattedAddress { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Reference { get; set; }

        public float? Latitude { get; set; }
        public float? Longitude { get; set; }

        public Sale Sale { get; set; }
        public DeliverySale DeliverySale { get; set; }
    }
}
