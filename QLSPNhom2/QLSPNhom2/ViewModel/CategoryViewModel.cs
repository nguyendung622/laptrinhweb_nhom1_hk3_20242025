using QLSPNhom2.DTO;
using QLSPNhom2.Models;

namespace QLSPNhom2.ViewModel
{
    public class CategoryViewModel : Paging
    {
        public CategoryDTO Request { get; set; }
        public CategoryDTO Response { get; set; }
        public List<CategoryDTO> Categories { get; set; }

    }
}
