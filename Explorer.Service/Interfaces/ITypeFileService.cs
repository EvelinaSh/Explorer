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
    public interface ITypeFileService
    {
        Task<IBaseResponse<IEnumerable<TypeFile>>> GetTypesFiles(); // коллекция всех элементов
        
        Task<IBaseResponse<TypeFileViewModel>> GetTypeFile(int id);

        Task<IBaseResponse<TypeFileViewModel>> CreateTypeFile(TypeFileViewModel typeFileViewModel);

        Task<IBaseResponse<bool>> DeleteTypeFile(int id);

        Task<IBaseResponse<TypeFile>> GetTypeFileByName(string name);

        Task<IBaseResponse<TypeFile>> EditTypeFile(int id, TypeFileViewModel model);
        
    }
}
