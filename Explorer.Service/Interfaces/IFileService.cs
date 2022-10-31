using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using Explorer.Domain.Response;
using Explorer.Domain.ViewModels;

namespace Explorer.Service.Interfaces
{
    public interface IFileService
    {
        Task<IBaseResponse<IEnumerable<Domain.Entity.File>>> GetFiles(); // коллекция всех элементов
       
        Task<IBaseResponse<Domain.Entity.File>> GetFile(int id);

        Task<IBaseResponse<FileViewModel>> CreateFile(FileViewModel fileViewModel);

        Task<IBaseResponse<bool>> DeleteFile(int id);

        Task<IBaseResponse<Domain.Entity.File>> GetFileByName(string name);

        Task<IBaseResponse<Domain.Entity.File>> EditFile(int id, FileViewModel model);
       
    }
}
