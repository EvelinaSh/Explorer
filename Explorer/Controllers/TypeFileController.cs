using Explorer.DAL.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Explorer.Service.Interfaces;
using Explorer.Domain.ViewModels;
using Explorer.Service.Implementations;
using System.Text;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.Unicode;

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
                var items = response.Data.ToList();
                var opts = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                    WriteIndented = true
                };
                return Json(items, opts);
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

        [HttpGet]
        public async Task<IActionResult> GetTypeFileByName(string name)
        {
            Console.WriteLine(name);
            var response = await _typeFileService.GetTypeFileByName(name);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                Console.WriteLine(response.Data);
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


        [HttpPost]
        public async Task<IActionResult> SaveType([FromForm] UploadedTypeViewModel icons)
        {
            Console.WriteLine(icons.NameType);
            TypeFileViewModel mod = new TypeFileViewModel();
            foreach (IFormFile icon in icons.Icons)
            {
                if (icon != null)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(icon.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)icon.Length);
                    }

                    mod = new TypeFileViewModel()
                    {
                        NameType = icons.NameType,
                        Icon = imageData

                    };
                    var response = await _typeFileService.CreateTypeFile(mod);
                    if (response.StatusCode == Domain.Enum.StatusCode.OK)
                    {
                        return RedirectToAction("GetTypesFiles");
                    }
                    return RedirectToAction("Error");
                }


            }
            return RedirectToAction("Error");

        }
    }
}
