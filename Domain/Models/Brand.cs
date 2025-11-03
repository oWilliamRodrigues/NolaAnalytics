namespace Domain.Models
{
    public class Brand : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<SubBrand> SubBrands { get; set; }
    }
}
