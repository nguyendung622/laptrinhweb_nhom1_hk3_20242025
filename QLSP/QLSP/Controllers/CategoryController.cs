using Microsoft.AspNetCore.Mvc;
using QLSP.Models;

namespace QLSP.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(string keyWord="")
        {
            CategoryViewModel model = new CategoryViewModel();
            model.KeyWord = keyWord;
            if (string.IsNullOrEmpty(keyWord))
            {
                model.Categories = _context.Categories.
                    Select(e => new CategoryDTO { Id = e.Id, Name = e.Name }).ToList();
            }
            else
            {
                var ls = _context.Categories.Where(e => e.Name.Contains(keyWord)).
                    Select(e => new CategoryDTO { Id = e.Id, Name = e.Name }).ToList();

                model.Categories = ls;
            }
            return View(model);
        }
        List<CategoryDTO> genData()
        {
            var ls = new List<CategoryDTO>();
            ls.Add(new CategoryDTO { Id = 1, Name = "Mobile" });
            ls.Add(new CategoryDTO { Id = 2, Name = "Desktop" });
            ls.Add(new CategoryDTO { Id = 3, Name = "IPhone" });
            return ls;
        }
    }
    public class CategoryViewModel
    {
        public string KeyWord { get; set; }
        public List<CategoryDTO> Categories { get; set; }
    }
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
