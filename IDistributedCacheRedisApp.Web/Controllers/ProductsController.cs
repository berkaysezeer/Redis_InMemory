using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IDistributedCache _distributedCache;

        public ProductsController(IDistributedCache distributedCache) => _distributedCache = distributedCache;

        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions entryOptions = new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(2)
            };

            //_distributedCache.SetString("FirstStringCache", "Berkay Sezer", entryOptions);
            //await _distributedCache.SetStringAsync("AsyncCache", "Berkay", entryOptions);

            Product product = new() { Id = 1, Name = "Kalem" };
            string jsonProduct = JsonSerializer.Serialize(product);
            await _distributedCache.SetStringAsync("product:1", jsonProduct, entryOptions);


            Product product2 = new() { Id = 2, Name = "Silgi" };
            string jsonProduct2 = JsonSerializer.Serialize(product2);
            Byte[] byteProduct = Encoding.UTF8.GetBytes(jsonProduct2);

            await _distributedCache.SetAsync("product:2", byteProduct, entryOptions);
            return View();
        }

        public async Task<IActionResult> ByteCache()
        {
            DistributedCacheEntryOptions entryOptions = new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(5)
            };

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/code.jpg");
            byte[] byteImage = System.IO.File.ReadAllBytes(path);

            await _distributedCache.SetAsync("image:1", byteImage, entryOptions);

            return View();
        }

        public IActionResult ImageUrl()
        {
            byte[] imageByte = _distributedCache.Get("image:1");
            var image = File(imageByte, "image/jpg");

            return image;
        }


        public IActionResult Show()
        {
            //ViewBag.Cache = _distributedCache.GetString("FirstStringCache");
            string jsonPorduct = _distributedCache.GetString("product:1");
            var product = JsonSerializer.Deserialize<Product>(jsonPorduct);
            ViewBag.Cache = jsonPorduct;

            Byte[] byteProduct2 = _distributedCache.Get("product:2");
            string product2 = Encoding.UTF8.GetString(byteProduct2);
            return View();
        }

        public IActionResult Remove()
        {
            //_distributedCache.Remove("FirstStringCache");
            _distributedCache.Remove("product:1");
            return View();
        }
    }
}
