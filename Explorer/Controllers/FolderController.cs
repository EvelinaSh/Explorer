using Explorer.DAL.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Explorer.Service.Interfaces;
using Explorer.Domain.ViewModels;
using Explorer.Service.Implementations;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.Unicode;

namespace Explorer.Controllers
{
    public class FolderController : Controller
    {
        private readonly IFolderService _folderService;

        public FolderController(IFolderService folderService)
        {
            _folderService = folderService;
        }


        [HttpGet]
        public async Task<IActionResult> GetFolders()
        {
            var response = await _folderService.GetFolders();
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
        public async Task<IActionResult> GetFolder(int id)
        {
            var response = await _folderService.GetFolderById(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                var items = response.Data;
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


        public async Task<IActionResult> DeleteFolder(int id)
        {
            var response = await _folderService.DeleteFolder(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return RedirectToAction("GetFolders");
            }
            return RedirectToAction("Error");
        }

        [HttpPost]
        public async Task<IActionResult> SaveFolder(FolderViewModel model)
        {

            if (model.IdFolder == 0)
                {
                    await _folderService.CreateFolder(model);
                }
                else
                {
                    await _folderService.EditFolder(model.IdFolder, model);
                }
          
            return RedirectToAction("GetFolders");
        }
    }
}
