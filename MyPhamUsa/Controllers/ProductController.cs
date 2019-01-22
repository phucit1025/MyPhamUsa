using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyPhamUsa.Services.Interfaces;
using MyPhamUsa.Models.ViewModels;

namespace MyPhamUsa.Controllers
{
    [Route("api/Product/[action]")]
    [ApiController]
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
        public IActionResult GetProducts()
        {
            var result = _productService.GetProducts();
            return StatusCode(200, result);
        }
    }
}