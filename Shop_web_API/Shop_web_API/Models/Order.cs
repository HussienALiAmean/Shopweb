namespace Shop_web_API.Models
{
    public class Order
    {
            public int Id { get; set; }
            public string UserId { get; set; }
            public ApplicationUser User { get; set; }
            public DateTime OrderDate { get; set; }

            public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
