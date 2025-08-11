using QLSP.Controllers;
using QLSP.DTO;

namespace QLSP.ViewModel
{
    public class ProductViewModel
    {
        public List<CategoryDTO> Categories { get; set; }
        public List<ProductDTO> Products { get; set; }
        public ProductDTO Request { get; set; }
        public ProductDTO Response { get; set; }
    }
}
