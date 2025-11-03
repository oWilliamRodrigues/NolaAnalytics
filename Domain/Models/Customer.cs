namespace Domain.Models
{
    public class Customer : BaseEntity
    {
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Cpf { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; }
        public int? StoreId { get; set; }
        public int? SubBrandId { get; set; }
        public string RegistrationOrigin { get; set; }

        public bool AgreeTerms { get; set; }
        public bool ReceivePromotionsEmail { get; set; }
        public bool ReceivePromotionsSms { get; set; }

        public Store Store { get; set; }
        public SubBrand SubBrand { get; set; }
    }
}
