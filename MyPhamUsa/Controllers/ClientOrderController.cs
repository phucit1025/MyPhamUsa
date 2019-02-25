﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
using MyPhamUsa.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using MyPhamUsa.Models.ViewModels;

namespace MyPhamUsa.Controllers
{
    [Route("api/Order/[action]")]
    public class ClientOrderController : Controller
    {
        private readonly IOrderService _orderService;

        public ClientOrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult CreateOrder([FromBody] OrderCreateViewModel newOrder)
        {
            var orderId = _orderService.CreateOrder(newOrder);
            if (orderId != 0)
            {
                return StatusCode(200, new { orderId = orderId });
            }
            return StatusCode(400);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin, Staff")]
        public IActionResult DeleteOrder(int orderId)
        {
            var result = _orderService.DeleteOrder(orderId);
            if (result) return StatusCode(200);
            return StatusCode(400);
        }
    }
}