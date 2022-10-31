using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Domain.ViewModels
{
    public class FolderViewModel
    {
        public int IdFolder { get; set; }
        public string NameFolder { get; set; }

        public int IdParentFolder { get; set; }
    }
}
