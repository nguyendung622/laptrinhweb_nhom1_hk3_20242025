using QLSPNhom2.DTO;
using QLSPNhom2.Models;

namespace QLSPNhom2.ViewModel
{
    public class CategoryViewModel
    {
        public CategoryDTO Request { get; set; }
        public CategoryDTO Response { get;set; }
        public List<CategoryDTO> Categories { get; set; }
        public int PageSize { get; set; } = 5;
        public int PageIndex { get; set; } = 1;
        public int PageCount
        {
            get
            {
                return TotalItem / PageSize + (TotalItem % PageSize > 0 ? 1 : 0);
            }
        }
        public int TotalItem { get; set; }
    }
}
