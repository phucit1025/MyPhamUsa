using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPhamUsa.Models.ViewModels;
using MyPhamUsa.Services.Interfaces;
using System;

namespace MyPhamUsa.Controllers
{
    [Route("api/Product/[action]")]
    [ApiController]
    [Authorize(Roles = "Admin, Staff")]
    public partial class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
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
        [AllowAnonymous]
        public IActionResult GetProducts()
        {
            var result = _productService.GetProducts();
            return StatusCode(200, result);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteProduct(int productId)
        {
            var result = _productService.DeleteProduct(productId);
            if (result) return StatusCode(200);
            return StatusCode(400);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateProduct(ProductUpdateViewModel newProduct)
        {
            var result = _productService.UpdateProduct(newProduct);
            if (result) return StatusCode(200);
            return StatusCode(400);
        }

        [HttpGet]
        public IActionResult GetProduct(int id)
        {
            var result = _productService.GetProduct(id);
            if (result != null) return StatusCode(200, result);
            return StatusCode(400);
        }

        #region Staff
        [HttpGet]
        public IActionResult GetProductsByStaff()
        {
            var result = _productService.GetProductsByStaff();
            return StatusCode(200, result);
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        public IActionResult UpdateStaffProduct(ProductStaffUpdateViewModel newProduct)
        {
            if (_productService.UpdateProduct(newProduct)) return StatusCode(200);
            return StatusCode(400);
        }
        #endregion

    }
}