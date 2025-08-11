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
        public IActionResult Create(CategoryViewModel model)
        {
            var cat = new Category();
            cat.Name = model.Request.Name;
            _context.Categories.Add(cat);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var cat = _context.Categories.Where(e => e.Id == id).Select(e => new CategoryDTO
            {
                Id = e.Id,
                Name = e.Name,
            }).FirstOrDefault();
            var categoryViewModel = new CategoryViewModel();
            categoryViewModel.Response = cat;
            return View(categoryViewModel);
        }
        [HttpPost]
        public IActionResult Update(CategoryViewModel model)
        {
            var cat = _context.Categories.Where(e => e.Id == model.Request.Id).FirstOrDefault();
            if (cat != null)
            {
                cat.Name = model.Request.Name;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var cat = _context.Categories.Where(e => e.Id == id).Select(e => new CategoryDTO
            {
                Id = e.Id,
                Name = e.Name,
            }).FirstOrDefault();
            var categoryViewModel = new CategoryViewModel();
            categoryViewModel.Response = cat;
            return View(categoryViewModel);
        }
        [HttpPost]
        public IActionResult Delete(CategoryViewModel model)
        {
            var cat = _context.Categories.Where(e => e.Id == model.Request.Id).FirstOrDefault();
            if (cat != null)
            {
                var products = _context.Products.Where(e => e.IdCategory == model.Request.Id).ToList();
                if (products.Count > 0)
                {
                    _context.Products.RemoveRange(products);
                }
                _context.Categories.Remove(cat);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
