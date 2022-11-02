using Explorer.Domain.Entity;
using Explorer.Models;
using Explorer.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Explorer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITypeFileService _typeFileService;

        public HomeController(ITypeFileService typeFileService)
        {
            _typeFileService = typeFileService;
        }

        [HttpGet]
        public async Task<IActionResult> ModalDeleteType()
        {
            var response = await _typeFileService.GetTypesFiles();
            Console.WriteLine("ZZZZ");
            Console.WriteLine(response.Data);
            Console.WriteLine(response.StatusCode);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {

                return PartialView("ModalDeleteType", response.Data);
            }
            return RedirectToAction("Error");

            
        }


        public async Task<IActionResult> ModalAddFile()
        {
                return PartialView("ModalAddFile");

        }


        public async Task<IActionResult> ModalAddFolder()
        {

            return PartialView("ModalAddFolder");

        }


        public async Task<IActionResult> ModalAddType()
        {

            return PartialView("ModalAddType");

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}