using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Domain.ViewModels
{
    public class FileViewModel
    {
        public int IdFile { get; set; }
        public string NameFile { get; set; }

        public string DescriptionFile { get; set; }

        public int IdType { get; set; }

        public int IdFolder { get; set; }

        public string? ContentFile { get; set; }

    }


    public class DownloadedFileViewModel
    {
        public IEnumerable<IFormFile> Files { get; set; }

        public string DescriptionFile { get; set; }

        public int IdFolder { get; set; }



    }




}
