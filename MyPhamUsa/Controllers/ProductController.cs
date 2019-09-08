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

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetProduct(int id)
        {
            var result = _productService.GetProduct(id);
            if (result != null) return StatusCode(200, result);
            return StatusCode(400);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetProducts(int pageSize = 20, int pageIndex = 0)
        {
            var result = _productService.GetProducts(pageSize, pageIndex);
            return StatusCode(200, result);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetProductsByCategory(int categoryId, int pageSize = 20, int pageIndex = 20)
        {
            var result = _productService.GetProducts(categoryId, pageSize, pageIndex);
            return StatusCode(200, result);
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

        [HttpPost]
        public IActionResult IsAvailableCode([FromBody] ProductCodeValidViewModel model)
        {
            if (_productService.IsAvailableCode(model)) return StatusCode(200);
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