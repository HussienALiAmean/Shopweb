namespace Shop_web_API.Models.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public ICollection<OrderProductDTO> OrderProducts { get; set; }
    }
}
