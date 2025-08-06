using Microsoft.AspNetCore.Mvc;
using QLSPNhom2.DTO;
using QLSPNhom2.Models;
using QLSPNhom2.ViewModel;
using System.Net.WebSockets;

namespace QLSPNhom2.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(string keyWord = "", int pageSize = 5, int pageIndex = 1)
        {
            var model = new CategoryViewModel();
            model.PageSize = pageSize;
            model.PageIndex = pageIndex;
            IQueryable<Category> ls = _context.Categories;
            if (!string.IsNullOrEmpty(keyWord))
            {
                ls = ls.Where(e => e.Name.Contains(keyWord));
            }
            model.TotalItem = ls.Count();

            model.Categories = ls
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new CategoryDTO
            {
                Id = e.Id,
                Name = e.Name
            }).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(String tenDanhMuc)
        {
            var cat = new Category();
            cat.Name = tenDanhMuc;
            _context.Categories.Add(cat);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
