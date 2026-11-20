namespace E_commerce_System.DTOs.OrderDTOs
{
    public class CreateOrderRequest
    {
        public DateTime Date { get; set; }
        public int CustomerId { get; set; }
    }

    public class UpdateOrderRequest
    {
        public DateTime Date { get; set; }
        public int CustomerId { get; set; }
    }


}
