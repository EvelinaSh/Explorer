using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using Explorer.Domain.Entity;
using Explorer.Domain.Response;
using Explorer.Domain.ViewModels;

namespace Explorer.Service.Interfaces
{
    public interface IFolderService
    {
        Task<IBaseResponse<IEnumerable<Folder>>> GetFolders(); // коллекция всех элементов
      
        Task<IBaseResponse<FolderViewModel>> GetFolderById(int id);

        Task<IBaseResponse<FolderViewModel>> CreateFolder(FolderViewModel folderViewModel);

        Task<IBaseResponse<bool>> DeleteFolder(int id);

        Task<IBaseResponse<Folder>> GetFolderByName(string name);

        Task<IBaseResponse<Folder>> EditFolder(int id, FolderViewModel model);
       
    }
}
