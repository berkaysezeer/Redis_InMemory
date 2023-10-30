using Microsoft.AspNetCore.Mvc;
using RedisExampleApp.Api.Models;
using RedisExampleApp.Api.Repositories;
using RedisExampleApp.Api.Services;
using RedisExampleApp.Cache;
using StackExchange.Redis;

namespace RedisExampleApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        //Controller --> Service --> Repository
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _productService.GetListAsync());
        }

        [HttpGet("{id}")] //www.api.com./products/1
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _productService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            return Created(string.Empty, await _productService.CreateAsync(product));
        }
    }
}
