using Microsoft.AspNetCore.Mvc;
using QLSP.DTO;
using QLSP.Models;
using QLSP.ViewModel;

namespace QLSP.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
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
        [HttpPost]
        public async Task<IActionResult> CreateAsync(ProductViewModel model)
        {
            var dto = model.Request;
            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                Quantity = dto.Quantity,
                CategoryId = dto.CategoryId,
                Avatar = ""
            };
            if (dto.FormFileAvatar != null && dto.FormFileAvatar.Length > 0)
            { 
                var folder = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(folder);
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.FormFileAvatar.FileName);
                var filePath = Path.Combine(folder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.FormFileAvatar.CopyToAsync(fileStream);
                }
                product.Avatar = $"/uploads/{fileName}";
            }

            _context.Products.Add(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> CreateAjax(ProductViewModel model)
        {
            var dto = model.Request;
            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                Quantity = dto.Quantity,
                CategoryId = dto.CategoryId,
                Avatar = ""
            };
            if (dto.FormFileAvatar != null && dto.FormFileAvatar.Length > 0)
            {
                var folder = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(folder);
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.FormFileAvatar.FileName);
                var filePath = Path.Combine(folder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.FormFileAvatar.CopyToAsync(fileStream);
                }
                product.Avatar = $"/uploads/{fileName}";
            }

            _context.Products.Add(product);
            _context.SaveChanges();
            return Ok(new {message = "Đã thêm mới thành công"});
        }
    }
}
