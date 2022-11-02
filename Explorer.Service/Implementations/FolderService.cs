using Explorer.DAL.Interfaces;
using Explorer.Domain.Entity;
using Explorer.Domain.Enum;
using Explorer.Domain.Response;
using Explorer.Domain.ViewModels;
using Explorer.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Service.Implementations
{
    public class FolderService : IFolderService
    {
        private readonly IFolderRepository _folderRepository;

        public FolderService(IFolderRepository folderRepository)
        {
            _folderRepository = folderRepository;
        }
       
             
       public async Task<IBaseResponse<FolderViewModel>> CreateFolder(FolderViewModel folderViewModel)
       {
            Console.WriteLine("Serv");
            Console.WriteLine(folderViewModel.NameFolder);
            Console.WriteLine(folderViewModel.IdParentFolder);
            var baseResponse = new BaseResponse<FolderViewModel>();
           try
           {
               var folder = new Folder()
               {

                   NameFolder = folderViewModel.NameFolder,
                   IdParentFolder = folderViewModel.IdParentFolder

               };

               await _folderRepository.Create(folder);
           }
           catch (Exception ex)
           {
               return new BaseResponse<FolderViewModel>()
               {
                   Description = $"[CreateFolder] : {ex.Message}",
                   StatusCode = StatusCode.InternalServerError
               };
           }
           return baseResponse;
       }
       

      public async Task<IBaseResponse<bool>> DeleteFolder(int id)
      {
          var baseResponse = new BaseResponse<bool>()
          {
              Data = true
          };
          try
          {
              var Folder = await _folderRepository.GetById(id);
              if (Folder == null)
              {
                  baseResponse.Description = "Folder not found";
                  baseResponse.StatusCode = StatusCode.FolderNotFound;
                  baseResponse.Data = false;

                  return baseResponse;
              }

              await _folderRepository.Delete(Folder);

              return baseResponse;
          }
          catch (Exception ex)
          {
              return new BaseResponse<bool>()
              {
                  Description = $"[DeleteFolder] : {ex.Message}",
                  StatusCode = StatusCode.InternalServerError
              };
          }
      }



      public async Task<IBaseResponse<Folder>> EditFolder(int id, FolderViewModel model)
      {
          var baseResponse = new BaseResponse<Folder>();
          try
          {
              var folder = await _folderRepository.GetById(id);
              if (folder == null)
              {
                  baseResponse.StatusCode = StatusCode.FolderNotFound;
                  baseResponse.Description = "Folder not found";
                  return baseResponse;
              }

   
              folder.NameFolder = model.NameFolder;
              folder.IdParentFolder = model.IdParentFolder;
                
           

              await _folderRepository.Update(folder);


              return baseResponse;
          

          }
          catch (Exception ex)
          {
              return new BaseResponse<Folder>()
              {
                  Description = $"[EditFolder] : {ex.Message}",
                  StatusCode = StatusCode.InternalServerError
              };
          }
      }



        public async Task<IBaseResponse<FolderViewModel>> GetFolderById(int id) // получение по id
        {
            var baseResponse = new BaseResponse<FolderViewModel>();
            try
            {
                var folder = await _folderRepository.GetById(id);
                if (folder == null)
                {
                    baseResponse.Description = "Folder not found";
                    baseResponse.StatusCode = StatusCode.FolderNotFound;
                    return baseResponse;
                }

                var data = new FolderViewModel()
                {
   
                    NameFolder = folder.NameFolder,
                    IdParentFolder = folder.IdParentFolder

                };

                baseResponse.StatusCode = StatusCode.OK;
                baseResponse.Data = data;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<FolderViewModel>()
                {
                    Description = $"[GetFolder] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }


        public async Task<IBaseResponse<Folder>> GetFolderByName(string name)
        {
            var baseResponse = new BaseResponse<Folder>();
            try
            {
                var folder = await _folderRepository.GetByName(name);
                if (folder == null)
                {
                    baseResponse.Description = "Folder not found";
                    baseResponse.StatusCode = StatusCode.FolderNotFound;
                    return baseResponse;
                }

                baseResponse.Data = folder;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<Folder>()
                {
                    Description = $"[GetFolderByName] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<Folder>>> GetFolders()
        {
            var baseResponse = new BaseResponse<IEnumerable<Folder>>();
            try
            {
                var folders = await _folderRepository.Select();
                if (folders.Count == 0)
                {
                    baseResponse.Description = "Найдено 0 элементов";
                    baseResponse.StatusCode = StatusCode.FolderNotFound;
                    return baseResponse;
                }

                baseResponse.Data = folders;
                baseResponse.StatusCode = StatusCode.OK;

                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Folder>>()
                {
                    Description = $"[GetFolders] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
