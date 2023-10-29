using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StackExchangeApi.Web.Services;

namespace StackExchangeApi.Web.Controllers
{
    public class SortedSetTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        private string listKey = "sortedsetnames";

        public SortedSetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(3);
        }

        public IActionResult Index()
        {
            HashSet<string> nameList = new HashSet<string>();
            if (db.KeyExists(listKey))
            {
                //db.SortedSetRangeByValue(listKey).ToList().ForEach(x =>
                //{
                //    nameList.Add(x.ToString());
                //});

                //sıralayabilmek için order: Order.Descending
                db.SortedSetRangeByRank(listKey, order: Order.Descending).ToList().ForEach(x =>
                {
                    nameList.Add(x.ToString());
                });
            }

            return View(nameList);
        }

        [HttpPost]
        public IActionResult Add(string name, int score)
        {
            //if (!db.KeyExists(listKey))
            db.KeyExpire(listKey, DateTime.Now.AddMinutes(5));

            db.SortedSetAdd(listKey, name, score);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(string name)
        {
            await db.SortedSetRemoveAsync(listKey, name);
            return RedirectToAction("Index");
        }
    }
}
