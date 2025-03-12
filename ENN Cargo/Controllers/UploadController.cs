﻿using Microsoft.AspNetCore.Mvc;
using ENN_Cargo.Core;
namespace ENN_Cargo.Controllers
{
    [Route("Upload")]
    public class UploadController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly CloudinaryService _cloudinaryService;
            public UploadController(CloudinaryService cloudinaryService)
            {
                _cloudinaryService = cloudinaryService;
            }
            [HttpGet]
            public IActionResult Upload()
            {
                return View();
            }
            [HttpPost]
            public async Task<IActionResult> UploadFile(IFormFile file)
            {
                if (file == null || file.Length == 0)
                {
                    ViewBag.Error = "Моля, изберете файл.";
                    return View("Upload");
                }
                var imageUrl = await _cloudinaryService.UploadImageAsync(file);
                if (imageUrl == null)
                {
                    ViewBag.Error = "Грешка при качването!";
                    return View("Upload");
                }
                ViewBag.ImageUrl = imageUrl;
                return View("Upload");
        }
    }
}