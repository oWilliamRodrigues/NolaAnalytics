namespace Domain.Models
{
    public class Item : BaseEntity
    {
        public int BrandId { get; set; }
        public int? SubBrandId { get; set; }
        public int? CategoryId { get; set; }
        public string Name { get; set; }
        public string PosUuid { get; set; }

        public Category Category { get; set; }
    }
}
