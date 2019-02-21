using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
using MyPhamUsa.Services.Interfaces;

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
            return StatusCode(200,results);
        }
    }
}
