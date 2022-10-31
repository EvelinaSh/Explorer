using Explorer.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.DAL.Interfaces
{
    public interface IFolderRepository : IBaseRepository<Folder>
    {
        Task<Folder> GetByName(string name);
    }
}
