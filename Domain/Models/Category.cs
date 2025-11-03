using Domain.Enums;

namespace Domain.Models
{
    public class Category : BaseEntity
    {
        public int BrandId { get; set; }
        public int? SubBrandId { get; set; }
        public string Name { get; set; }
        public CategoryType Type { get; set; }
        public string PosUuid { get; set; }

        public SubBrand SubBrand { get; set; }
    }
}
