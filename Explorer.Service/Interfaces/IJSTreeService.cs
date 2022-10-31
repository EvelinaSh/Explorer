
using Explorer.Domain.Response;
using Explorer.Domain.ViewModels;



namespace Explorer.Service.Interfaces
{
    public interface IJSTreeService
    {
        Task<IBaseResponse<List<JsTreeViewModel>>> GetTree(); // коллекция корневых элементов
       
        Task<IBaseResponse<List<JsTreeViewModel>>> GetTree(string id);


    }
}
