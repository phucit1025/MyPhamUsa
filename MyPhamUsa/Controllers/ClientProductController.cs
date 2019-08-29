using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
using MyPhamUsa.Services.Interfaces;
using System.Collections.Generic;

namespace MyPhamUsa.Controllers
{
    [Route("api/Client/Product/[action]")]
    public class ClientProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ClientProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            var results = _productService.GetClientProducts();
            return StatusCode(200, results);
        }

        [HttpGet]
        public IActionResult GetClientProduct(int id)
        {
            var result = _productService.GetClientProduct(id);
            if (result != null) return StatusCode(200, result);
            return StatusCode(400);
        }

        [HttpGet]
        public IActionResult GetClientProducts(List<int> ids)
        {
            var results = _productService.GetClientProducts(ids);
            return StatusCode(200, results);
        }

        [HttpGet]
        public IActionResult GetProductsByCategory(int categoryId)
        {
            var results = _productService.GetClientProducts(categoryId);
            return StatusCode(200, results);
        }

        [HttpGet]
        public IActionResult SearchProducts(string name)
        {
            var results = _productService.SearchClientProducts(name);
            return StatusCode(200, results);
        }


    }
}
