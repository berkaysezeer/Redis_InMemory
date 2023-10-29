using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StackExchangeApi.Web.Services;

namespace StackExchangeApi.Web.Controllers
{
    public class HashTypeController : BaseController
    {
        private string cacheKey { get; set; } = "sozluk";

        public HashTypeController(RedisService redisService) : base(redisService)
        {
        }

        public IActionResult Index()
        {
            Dictionary<string, string> list = new();

            if (db.KeyExists(cacheKey))
            {
                db.HashGetAll(cacheKey).ToList().ForEach(x =>
                {
                    list.Add(x.Name, x.Value);
                });
            }

            return View(list);
        }

        [HttpPost]
        public IActionResult Add(string name, string value)
        {
            db.HashSet(cacheKey, name, value);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(string key)
        {
            await db.HashDeleteAsync(cacheKey, key);
            return RedirectToAction("Index");
        }

    }
}
