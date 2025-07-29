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
        public IActionResult Index(string keyWord = "", int pageIndex = 1, int pageSize = 2)
        {
            var pagingInfo = new PagingInfo
            {
                PageSize = pageSize,
                PageIndex = pageIndex,
            };
            CategoryViewModel model = new CategoryViewModel();
            model.KeyWord = keyWord;
            if (string.IsNullOrEmpty(keyWord))
            {
                var ls = _context.Categories.
                    Select(e => new CategoryDTO { Id = e.Id, Name = e.Name });

                pagingInfo.Total = ls.Count();
                model.Categories = ls.Skip(pagingInfo.PageSize * (pagingInfo.PageIndex - 1))
                                     .Take(pagingInfo.PageSize)
                                     .ToList();
            }
            else
            {
                var ls = _context.Categories.Where(e => e.Name.Contains(keyWord)).
                    Select(e => new CategoryDTO { Id = e.Id, Name = e.Name });
                pagingInfo.Total = ls.Count();
                model.Categories = ls.ToList();
            }
            model.PagingInfo = pagingInfo;
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
            var category = _context.Categories.Where(e => e.Name == model.Request.Name).FirstOrDefault();
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
        [HttpGet]
        public IActionResult Delete(long id)
        {
            var category = _context.Categories.Where(e => e.Id == id).Select(e => new CategoryDTO
            {
                Id = e.Id,
                Name = e.Name
            }).FirstOrDefault();
            var model = new CategoryViewModel();
            model.Response = category;
            return View(model);
        }
        [HttpPost]
        public IActionResult Delete(CategoryViewModel model)
        {
            var category = _context.Categories.Where(e => e.Id == model.Request.Id).FirstOrDefault();
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
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
        public PagingInfo PagingInfo { get; set; }
    }
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class PagingInfo
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public int Total { get; set; }
        public int PageCount
        {
            get
            {
                return Total / PageSize + (Total % PageSize != 0 ? 1 : 0);
            }
        }

    }
}
