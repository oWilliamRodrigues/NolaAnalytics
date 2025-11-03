namespace Domain.Models
{
    public class Store : BaseEntity
    {
        public int BrandId { get; set; }
        public int? SubBrandId { get; set; }

        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string AddressStreet { get; set; }
        public int? AddressNumber { get; set; }
        public string Zipcode { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        public bool IsActive { get; set; }
        public bool IsOwn { get; set; }
        public bool IsHolding { get; set; }
        public DateTime? CreationDate { get; set; }

        public Brand Brand { get; set; }
        public SubBrand SubBrand { get; set; }
    }
}
