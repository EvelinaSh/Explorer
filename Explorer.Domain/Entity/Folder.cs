using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Domain.Entity
{
    public class Folder
    {
        [Key] public int IdFolder { get; set; }

        public string NameFolder { get; set; }

        public int IdParentFolder { get; set; }

        public List<File> Files { get; set; }

    }
}
