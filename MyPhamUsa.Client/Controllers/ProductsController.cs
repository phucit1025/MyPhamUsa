using Microsoft.AspNetCore.Mvc;
using MyPhamUsa.Client.Services;

namespace MyPhamUsa.Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("Search")]
        public IActionResult Search(int categoryId, string name, bool asTitle, int pageSize = 20, int pageIndex = 0)
        {
            var results = _productService.SearchProducts(categoryId, name, asTitle, pageSize, pageIndex);
            return StatusCode(200, results);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var results = _productService.GetProducts();
            return StatusCode(200, results);
        }

        [HttpGet("Category/{categoryId}")]
        public IActionResult GetByCategory(int categoryId)
        {
            var results = _productService.GetProducts(categoryId);
            return StatusCode(200, results);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var results = _productService.GetProduct(id);
            return StatusCode(200, results);
        }
    }
}