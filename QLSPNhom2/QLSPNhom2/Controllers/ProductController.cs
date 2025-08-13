using Microsoft.AspNetCore.Mvc;
using QLSPNhom2.Models;
using QLSPNhom2.ViewModel;

namespace QLSPNhom2.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var model = new ProductViewModel();
            model.Categories = _context.Categories.Select(e => new DTO.CategoryDTO
            {
                Id = e.Id,
                Name = e.Name
            }).ToList();
            model.Products = _context.Products.Select(e => new DTO.ProductDTO
            {
                Id = e.Id,
                Name = e.Name,
                Price = e.Price,
                Quantity = e.Quantity,
                Avatar = e.Avatar,
                IdCategory = e.IdCategory
            }).ToList();
            return View(model);
        }
    }
}
