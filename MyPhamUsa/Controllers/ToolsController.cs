﻿using Microsoft.AspNetCore.Mvc;
using MyPhamUsa.Services.Interfaces;

namespace MyPhamUsa.Controllers
{
    [Route("api/Tools/[action]")]
    [ApiController]
    public class ToolsController : ControllerBase
    {
        private readonly IToolService _toolService;

        public ToolsController(IToolService toolService)
        {
            _toolService = toolService;
        }

        [HttpGet]
        public void DeleteImages()
        {
            _toolService.DeleteImages();
        }

        [HttpGet]
        public void DeleteCategoryMappings()
        {
            _toolService.DeleteCategoryMappings();
        }
    }
}