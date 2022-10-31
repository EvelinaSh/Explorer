using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Domain.ViewModels
{
    public class TypeFileViewModel
    {
        public int IdTypeFile { get; set; }
        public string NameType { get; set; }
        public byte[] Icon { get; set; }
    }
}
