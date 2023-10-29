using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StackExchangeApi.Web.Services;

namespace StackExchangeApi.Web.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        private string listKey = "sortedsetnames";

        public ListTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(1);

        }

        public IActionResult Index()
        {
            List<string> nameList = new List<string>();

            if (db.KeyExists(listKey))
            {
                db.ListRange(listKey).ToList().ForEach(x =>
                {
                    nameList.Add(x.ToString());
                });
            }

            return View(nameList);
        }

        public IActionResult Show()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(string name)
        {
            db.ListRightPush(listKey, name); //listenin sonuna ekler
            return RedirectToAction("Index");
        }

        public IActionResult Remove(string name)
        {
            //herhangi bir geri dönüş değeri beklemediğimiz için Wait() kullanıyoruz. Bu sayede async task await kullanmamıza gerek kalmıyor
            db.ListRemoveAsync(listKey, name).Wait();
            return RedirectToAction("Index");
        }

        public IActionResult RemoveFirstName()
        {
            if (db.KeyRefCount(listKey) > 0)
                db.ListLeftPop(listKey);

            return RedirectToAction("Index");
        }
    }
}
