using Microsoft.AspNetCore.Mvc;

namespace HelloGroup2.Controllers
{
    public class MathController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Add(long a, long b)
        {
            long c = a + b;
            MathResult mathResult = new MathResult
            {
                A = a,
                B = b,
                C = c,
                Message = $"Tổng của {a} và {b} là {c}"
            };
            return View(mathResult);
        }
    }
    public class MathResult
    {
        public long A { get; set; }
        public long B { get; set; }
        public long C { get; set; }
        public string Message { get; set; }
    }
}
