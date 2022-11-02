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
using Explorer.Service.Implementations;
using System.Xml.Linq;

namespace Explorer.Controllers
{
    public class FileController : Controller
    {
        private readonly IFileService _fileService;
        private readonly ITypeFileService _typeFileService;

        public FileController(IFileService fileService, ITypeFileService typeFileService)
        {
            _fileService = fileService;
            _typeFileService = typeFileService;
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

        [HttpGet]
        public async Task<IActionResult> GetFileByName(string name)
        {
            var response = await _fileService.GetFileByName(name);

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
            var response = await _fileService.GetFile(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                Console.WriteLine(response.Data.ContentFile);
                var items = response.Data;
                /*
                FileInfo info = new FileInfo(items.NameFile + '.' + items.TypeFile.NameType);
                if (!info.Exists)
                {
                    using (StreamWriter writer = info.CreateText())
                    {
                        writer.WriteLine(items.ContentFile);

                    }
                }

                return File(info.OpenRead(), "text/plain");
                */

                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = items.NameFile + '.' + items.TypeFile.NameType,
                    Inline = true,
                };
                Response.Headers.Add("Content-Disposition", cd.ToString());

                return File(Encoding.UTF8.GetBytes(items.ContentFile), "application/octet-stream",
                items.NameFile + '.' + items.TypeFile.NameType, true);
                
            }
            return RedirectToAction("Error");
            

        }

        [HttpPost]
        public async Task<IActionResult> CreateFile([FromForm] DownloadedFileViewModel files)
        {
            FileViewModel mod = new FileViewModel();
            foreach (IFormFile file in files.Files)
            {
                string[] attr = file.FileName.Split('.');
                var responseType = await _typeFileService.GetTypeFileByName(attr[1]);
          
                if (responseType.Data == null)
                {
                    return RedirectToAction("Error");
                }
               
                    var result = new StringBuilder();
                    string res = "";
                    using (var reader = new StreamReader(file.OpenReadStream()))
                    {
                        res = reader.ReadToEnd().ToString();
                    }

                    mod = new FileViewModel()
                    {
                        NameFile = attr[0],
                        DescriptionFile = files.DescriptionFile,
                        IdType = responseType.Data.IdType,
                        IdFolder = files.IdFolder,
                        ContentFile = res

                    };
                    await _fileService.CreateFile(mod);
                    var responseFile = await _fileService.GetFileByName(attr[0]);
                    Console.WriteLine(responseFile.Data.TypeFile);
                    var icon = "data:image/png;base64," + Convert.ToBase64String(responseFile.Data.TypeFile.Icon);
                    FileViewModel objNameIcon = new FileViewModel()
                    {
                        NameFile = file.FileName,
                        Icon = icon,
                        DescriptionFile = files.DescriptionFile,

                    };
                    return Json(objNameIcon);
                

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
