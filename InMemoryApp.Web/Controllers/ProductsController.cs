using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private IMemoryCache _memoryCache;

        public ProductsController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            _memoryCache.Set<string>("Zaman", DateTime.Now.ToString());
            return View();
        }

        public IActionResult MemoryGet()
        {
            ViewBag.Zaman = _memoryCache.Get<string>("Zaman");
            return View();
        }
    }
}
