namespace Shop_web_API.Models.DTOs
{
    public class OrderProductDTO
    {
        public int? OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalePrice { get; set; }
    }
}
