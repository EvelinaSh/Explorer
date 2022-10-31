using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Domain.Enum
{
    public enum StatusCode
    {
        FileNotFound = 0,

        FolderNotFound = 10,

        TypeFileNotFound = 20,

        OK = 200,
        InternalServerError = 500
    }
}
