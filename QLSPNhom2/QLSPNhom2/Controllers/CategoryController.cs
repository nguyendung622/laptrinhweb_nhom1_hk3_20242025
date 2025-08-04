using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index(string keyWord="")
        {
            if (string.IsNullOrEmpty(keyWord))
            {
                var ls = _context.Categories.ToList();
                var model = new CategoryViewModel();
                model.Categories = ls;
                return View(model);
            }
            else
            {
                var ls = _context.Categories.Where(e=>e.Name.Contains(keyWord)).ToList();
                var model = new CategoryViewModel();
                model.Categories = ls;
                return View(model);
            }
        }
    }
}
