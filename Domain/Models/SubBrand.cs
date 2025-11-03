namespace Domain.Models
{
    public class SubBrand : BaseEntity
    {
        public int BrandId { get; set; }
        public string Name { get; set; }

        public Brand Brand { get; set; }
    }
}
