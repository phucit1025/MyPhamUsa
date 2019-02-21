using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPhamUsa.Models.ViewModels;
using MyPhamUsa.Services.Interfaces;

namespace MyPhamUsa.Controllers
{
    [Route("api/Product/[action]")]
    [ApiController]
    [Authorize(Roles = "Admin, Staff")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] ProductCreateViewModel newProduct)
        {
            var result = _productService.CreateProduct(newProduct);
            if (result)
            {
                return StatusCode(200);
            }
            else
            {
                return StatusCode(400);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetProducts()
        {
            var result = _productService.GetProducts();
            return StatusCode(200, result);
        }
        [HttpGet]
        public IActionResult GetProductsByStaff()
        {
            var result = _productService.GetProductsByStaff();
            return StatusCode(200, result);
        }

    }
}