using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyPhamUsa.Services.Interfaces;

namespace MyPhamUsa.Controllers
{
    [Route("api/Order/[action]")]
    [ApiController]
    [Authorize(Roles = "Admin, Staff")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public IActionResult GetOrders(DateTime? time, int pageSize, int pageIndex = 1)
        {
            var orders = _orderService.GetOrders(time);
            var totalRecord = orders.Count;
            var totalPage = Math.Ceiling((double)totalRecord / (double)pageSize);
            #region Paging
            orders = orders.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            #endregion
            return StatusCode(200, new { orders, pageIndex, totalPage, total = totalRecord });
        }
    }
}