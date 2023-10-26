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
            //Cahce kontrol etmek için 1.yol
            if (String.IsNullOrEmpty(_memoryCache.Get<string>("Zaman")))
                _memoryCache.Set<string>("Zaman", DateTime.Now.ToString());

            //Cahce kontrol etmek için 2.yol
            //eğer true dönerse zamanCache'e değeri atar
            if (!_memoryCache.TryGetValue("Zaman", out string zamanCache))
                _memoryCache.Set<string>("Zaman", DateTime.Now.ToString());


            return View();
        }

        public IActionResult MemoryGet()
        {
            //cache silmek için kullanılır
            _memoryCache.Remove("Zaman");

            //Get ile değeri almaya çalışır, eğer yok ise oluşturur
            //var mı yok mu kontrol etmeye gerek yok
            _memoryCache.GetOrCreate<string>("Zaman", enrty =>
            {
                return DateTime.Now.ToString();
            });

            ViewBag.Zaman = _memoryCache.Get<string>("Zaman");
            return View();
        }
    }
}
