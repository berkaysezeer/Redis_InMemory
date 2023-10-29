using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StackExchangeApi.Web.Services;

namespace StackExchangeApi.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;

        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(0);
        }

        public IActionResult Index()
        {
            //db.KeyDelete("name");
            db.StringSet("name", "Berkay Sezer");
            db.StringSet("ziyaretci", 100);

            return View();
        }

        public IActionResult Show()
        {
            var name = db.StringGet("name");
            var nameLen = db.StringLength("name");

            if (name.HasValue) ViewBag.Name = $"{name} ({nameLen})";

            var ziyaretci = db.StringIncrement("ziyaretci");
            ViewBag.Ziyaretci = ziyaretci.ToString();

            return View();
        }
    }
}
