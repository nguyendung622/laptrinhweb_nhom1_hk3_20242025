using Microsoft.AspNetCore.Mvc;
using QLSP.Models;
using System.Net.WebSockets;

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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CategoryViewModel model)
        {
            var category = _context.Categories.Where(e=>e.Name == model.Request.Name).FirstOrDefault();
            if (category != null)
            {
                ViewData["name"] = model.Request.Name;   
                ViewData["message"] = "Tên danh mục này đã tồn tại";
                return View();
            }
            else
            {
                category = new Category
                {
                    Name = model.Request.Name
                };
                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult Update(long id)
        {
            CategoryViewModel model = new CategoryViewModel();
            var category = _context.Categories.Where(e => e.Id == id).Select(e => new CategoryDTO
            {
                Id = e.Id,
                Name = e.Name
            }).FirstOrDefault();
            model.Response = category;
            return View(model);
        }
        [HttpPost]
        public IActionResult Update(CategoryViewModel model)
        {
            var category = _context.Categories.Where(e => e.Id == model.Request.Id).FirstOrDefault();
            if (category != null)
            {
                category.Name = model.Request.Name;
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewData["message"] = "Không tìm thấy đối tượng";
                return View(model);
            }
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
        public CategoryDTO Request { get; set; }
        public CategoryDTO Response { get; set; }
    }
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
