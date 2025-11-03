using Domain.Enums;

namespace Domain.Models
{
    public class Channel : BaseEntity
    {
        public int BrandId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ChannelType Type { get; set; }

        public Brand Brand { get; set; }
    }
}
