using Microsoft.AspNetCore.Mvc;
using QLSP.DTO;
using QLSP.Models;
using QLSP.ViewModel;
using System.Drawing.Printing;

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
        public IActionResult Index(int pageIndex = 1, int pageSize = 5)
        {
            var pagingInfo = new PagingInfo
            {
                PageSize = pageSize,
                PageIndex = pageIndex,
                Total = _context.Products.Count()
            };
            var products = _context.Products
                .Skip(pagingInfo.PageSize * (pagingInfo.PageIndex - 1))
                .Take(pagingInfo.PageSize)
                .Select(e => new ProductDTO
                {
                    Id = e.Id,
                    Name = e.Name,
                    Price = e.Price,
                    Avatar = e.Avatar,
                    CategoryId = e.CategoryId,
                    Quantity = e.Quantity
                }).ToList();

            var model = new ProductViewModel();
            model.Products = products;
            model.PagingInfo = pagingInfo;
            var cats = _context.Categories.Select(e => new CategoryDTO
            {
                Id = e.Id,
                Name = e.Name
            }).ToList();
            model.Categories = cats;

            return View(model);
        }

        public IActionResult LoadProduct(
            int pageIndex = 1, int pageSize = 5,
            int idCategory = 0, string keyWord = "")
        {
            var pagingInfo = new PagingInfo
            {
                PageSize = pageSize,
                PageIndex = pageIndex,
            };
            IQueryable<Product> products = _context.Products;
            if (idCategory != 0)
            {
                products = products.Where(e => e.CategoryId == idCategory);
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                products = products.Where(e => e.Name.Contains(keyWord));
            }
            pagingInfo.Total = products.Count();
            var ls = products
                .Skip(pagingInfo.PageSize * (pagingInfo.PageIndex - 1))
                .Take(pagingInfo.PageSize)
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
            model.Products = ls;
            model.PagingInfo = pagingInfo;
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
            return Ok(new { message = "Đã thêm mới thành công" });
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var model = new ProductViewModel();
            model.Response = _context.Products.Where(e => e.Id == id).Select(e => new ProductDTO
            {
                Id = e.Id,
                Name = e.Name,
                Price = e.Price,
                Avatar = e.Avatar,
                CategoryId = e.CategoryId,
                Quantity = e.Quantity
            }).First();
            var cats = _context.Categories.Select(e => new CategoryDTO
            {
                Id = e.Id,
                Name = e.Name
            }).ToList();
            model.Categories = cats;
            return PartialView("_Update",model);
        }
    }
}
