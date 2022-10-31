using Explorer.DAL.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Explorer.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Explorer.Domain.ViewModels;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;
using Microsoft.Net.Http.Headers;
using System.Text;
using Microsoft.JSInterop;
using static System.Net.Mime.MediaTypeNames;

namespace Explorer.Controllers
{
    public class FileController : Controller
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }


        // GET: FileController
        [HttpGet]
        public async Task<IActionResult> GetFiles()
        {
            var response = await _fileService.GetFiles();
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                var items = response.Data.ToList();
                var opts = new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                    WriteIndented = true
                };
                return Json(items, opts);
            }
            return RedirectToAction("Error");
        }

        [HttpGet]
        public async Task<IActionResult> GetFile(int id)
        {
            var response = await _fileService.GetFile(id);
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


        public async Task<IActionResult> DeleteFile(int id)
        {
            var response = await _fileService.DeleteFile(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return RedirectToAction("GetFiles");
            }
            return RedirectToAction("Error");
        }

      
        [HttpGet]
        public async Task<IActionResult> DownloadFile(int id)
        {
            Console.WriteLine("UHHHHHHHH");
            var response = await _fileService.GetFile(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                var items = response.Data;
                return File(Encoding.UTF8.GetBytes(items.ContentFile), "text/plain",
                items.NameFile + '.' + items.TypeFile.NameType, true);
            }
            return RedirectToAction("Error");
            

        }

        [HttpPost]
        public async Task<IActionResult> CreateFile([FromForm] DownloadedFileViewModel files)
        {

            foreach (IFormFile file in files.Files)
            {
                string[] attr = file.FileName.Split('.');
                string[] types = { "cs", "css", "docx", "html", "js", "sql", "txt", "xls", "xlsx", "xml" };
                var IdType = Array.IndexOf(types, attr[1]) + 1;
                var result = new StringBuilder();
                string res = "";
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                        res = reader.ReadToEnd().ToString();
                }
                Console.WriteLine(res);
                
                FileViewModel mod = new FileViewModel()
                {
                    NameFile = attr[0],
                    DescriptionFile = files.DescriptionFile,
                    IdType = IdType,
                    IdFolder = files.IdFolder,
                    ContentFile = res

                };
                var response = await _fileService.CreateFile(mod);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    return RedirectToAction("GetFiles");
                }
               
            }
            return RedirectToAction("Error");

        }



        public async Task<IActionResult> UpdateFile(FileViewModel model)
        {

            var response = await _fileService.EditFile(model.IdFile, model); 

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return RedirectToAction("GetFiles");
            }
            return RedirectToAction("Error");
        }
    }
}
