using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Domain.Entity
{
    public class File
    {
        [Key]
       public int IdFile { get; set; }

        public string NameFile { get; set; }

        public string DescriptionFile { get; set; }

        [ForeignKey("TypeFile")] public int? IdType { get; set; }

        public TypeFile TypeFile { get; set; }

        [ForeignKey("Folder")] public int? IdFolder { get; set; }

        public Folder Folder { get; set; }

        public string? ContentFile { get; set; }


    }

}
