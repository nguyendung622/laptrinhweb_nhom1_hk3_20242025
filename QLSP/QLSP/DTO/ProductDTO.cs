namespace QLSP.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Avatar { get; set; }
        public int CategoryId { get; set; }
        public IFormFile FormFileAvatar { get; set; }
    }
}
