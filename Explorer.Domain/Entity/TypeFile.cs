using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Domain.Entity
{
    public class TypeFile
    {
        [Key] public int IdType { get; set; }

        public string NameType { get; set; }

        public byte[] Icon { get; set; }

        public List<File> Files { get; set; }
    }
}
