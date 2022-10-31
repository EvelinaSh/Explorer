using Explorer.DAL.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Explorer.Service.Interfaces;
using Explorer.Domain.ViewModels;
using Explorer.Service.Implementations;

namespace Explorer.Controllers
{
    public class TypeFileController : Controller
    {
        private readonly ITypeFileService _typeFileService;

        public TypeFileController(ITypeFileService typeFileService)
        {
            _typeFileService = typeFileService;
        }


        // GET: TypeFileController
        [HttpGet]
        public async Task<IActionResult> GetTypesFiles()
        {
            var response = await _typeFileService.GetTypesFiles();
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data.ToList());
            }
            return RedirectToAction("Error");
        }

        [HttpGet]
        public async Task<IActionResult> GetTypeFile(int id)
        {
            var response = await _typeFileService.GetTypeFile(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return RedirectToAction("Error");
        }


        public async Task<IActionResult> DeleteTypeFile(int id)
        {
            var response = await _typeFileService.DeleteTypeFile(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return RedirectToAction("GetTypesFiles");
            }
            return RedirectToAction("Error");
        }

        [HttpGet]
        public async Task<IActionResult> SaveTypeFile(int id)
        {
            if (id == 0)
            {
                return View();
            }

            var response = await _typeFileService.GetTypeFile(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }

            return RedirectToAction("Error");
        }

        [HttpPost]
        public async Task<IActionResult> SaveTypeFile(TypeFileViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.IdTypeFile == 0)
                {
                    await _typeFileService.CreateTypeFile(model); 
                }
                else
                {
                    await _typeFileService.EditTypeFile(model.IdTypeFile, model); 
                }
            }

            return RedirectToAction("GetTypesFiles");
        }
    }
}
