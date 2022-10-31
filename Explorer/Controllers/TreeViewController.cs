using Explorer.Domain.ViewModels;
using Explorer.Models;
using Explorer.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.Unicode;
using Controller = Microsoft.AspNetCore.Mvc.Controller;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;


namespace Explorer.Controllers
{
    public class TreeViewController : Controller
    {
        private readonly IJSTreeService _jsTreeService;

        public TreeViewController(IJSTreeService jsTreeService)
        {
            _jsTreeService = jsTreeService;
        }
       
        public async Task<IActionResult> GetRoot()
        {
            var response = await _jsTreeService.GetTree();
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                var items = response.Data.ToList();
                var opts = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                    WriteIndented = true
                };
                return Json(items, opts);

            }
            return RedirectToAction("Error");

        }

        public async Task<IActionResult> GetChildren(string id)
        {
            var response = await _jsTreeService.GetTree(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                var items = response.Data.ToList();
                var opts = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                    WriteIndented = true
                };
                return Json(items, opts);
            }
            return RedirectToAction("Error");
        }
    }
}
