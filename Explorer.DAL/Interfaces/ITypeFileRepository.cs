using System;
using Explorer.Domain.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.DAL.Interfaces
{
    public interface ITypeFileRepository : IBaseRepository<TypeFile>
    {
        Task<TypeFile> GetByName(string name);
    }
}
