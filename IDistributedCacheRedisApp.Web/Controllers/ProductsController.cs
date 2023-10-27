using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

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

            _distributedCache.SetString("FirstStringCache", "Berkay Sezer", entryOptions);
            await _distributedCache.SetStringAsync("AsyncCache", "Berkay", entryOptions);

            return View();
        }

        public IActionResult Show()
        {
            ViewBag.Cache = _distributedCache.GetString("FirstStringCache");
            return View();
        }

        public IActionResult Remove()
        {
            _distributedCache.Remove("FirstStringCache");
            return View();
        }
    }
}
