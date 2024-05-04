namespace FoodShareNetAPI.DTO.Order
{
    public class OrderDetailsDTO
    {
        public int Id { get; set; }
        public int BeneficiaryId { get; set; }
        public int DonationId { get; set; }
        public int CourierId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public int OrderStatusId { get; set; }
        public int Quantity {  get; set; }

    }
}
