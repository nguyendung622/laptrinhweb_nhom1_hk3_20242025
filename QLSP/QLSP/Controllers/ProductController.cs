using Microsoft.AspNetCore.Mvc;
using QLSP.DTO;
using QLSP.Models;
using QLSP.ViewModel;

namespace QLSP.Controllers
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
            var cats = _context.Categories.Select(e => new CategoryDTO
            {
                Id = e.Id,
                Name = e.Name
            }).ToList();
            model.Categories = cats;

            var products = _context.Products.Select(e => new ProductDTO
            {
                Id = e.Id,
                Name = e.Name,
                Price = e.Price,
                Avatar = e.Avatar,
                CategoryId = e.CategoryId,
                Quantity = e.Quantity
            }).ToList();
            model.Products = products;

            return View(model);
        }

        public IActionResult LoadProduct(int idCategory)
        {
            var products = _context.Products
                .Where(e => (idCategory == 0 ? true : e.CategoryId == idCategory))
                .Select(e => new ProductDTO
                {
                    Id = e.Id,
                    Name = e.Name,
                    Price = e.Price,
                    Quantity = e.Quantity,
                    Avatar = e.Avatar,
                    CategoryId = e.CategoryId
                }).ToList();
            var model = new ProductViewModel();
            model.Products = products;
            return PartialView("_List", model);
        }
    }
}
