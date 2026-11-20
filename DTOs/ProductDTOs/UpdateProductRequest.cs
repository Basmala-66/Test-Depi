namespace E_commerce_System.DTOs.ProductDTOs
{
    public class UpdateProductRequest
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
    }
}
