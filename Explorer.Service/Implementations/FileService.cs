using Explorer.DAL.Interfaces;
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
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;

        public FileService(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }
       
             
       public async Task<IBaseResponse<FileViewModel>> CreateFile(FileViewModel fileViewModel)
       {
           var baseResponse = new BaseResponse<FileViewModel>();
           try
           {
               var file = new Domain.Entity.File()
               {
                
                   NameFile = fileViewModel.NameFile,
                   DescriptionFile = fileViewModel.DescriptionFile,
                   IdType = fileViewModel.IdType,
                   IdFolder = fileViewModel.IdFolder,
                   ContentFile = fileViewModel.ContentFile
               };

               await _fileRepository.Create(file);
           }
           catch (Exception ex)
           {
               return new BaseResponse<FileViewModel>()
               {
                   Description = $"[CreateFile] : {ex.Message}",
                   StatusCode = StatusCode.InternalServerError
               };
           }
           return baseResponse;
       }
       

      public async Task<IBaseResponse<bool>> DeleteFile(int id)
      {
          var baseResponse = new BaseResponse<bool>()
          {
              Data = true
          };
          try
          {
              var file = await _fileRepository.GetById(id);
              if (file == null)
              {
                  baseResponse.Description = "File not found";
                  baseResponse.StatusCode = StatusCode.FileNotFound;
                  baseResponse.Data = false;

                  return baseResponse;
              }

              await _fileRepository.Delete(file);

              return baseResponse;
          }
          catch (Exception ex)
          {
              return new BaseResponse<bool>()
              {
                  Description = $"[DeleteFile] : {ex.Message}",
                  StatusCode = StatusCode.InternalServerError
              };
          }
      }



      public async Task<IBaseResponse<Domain.Entity.File>> EditFile(int id, FileViewModel model)
      {
          var baseResponse = new BaseResponse<Domain.Entity.File>();
          try
          {
              var file = await _fileRepository.GetById(id);
              if (file == null)
              {
                  baseResponse.StatusCode = StatusCode.FileNotFound;
                  baseResponse.Description = "File not found";
                  return baseResponse;
              }


              file.NameFile = model.NameFile;
        

              await _fileRepository.Update(file);


              return baseResponse;
          

          }
          catch (Exception ex)
          {
              return new BaseResponse<Domain.Entity.File>()
              {
                  Description = $"[EditFile] : {ex.Message}",
                  StatusCode = StatusCode.InternalServerError
              };
          }
      }



        public async Task<IBaseResponse<Domain.Entity.File>> GetFile(int id) // получение по id
        {
            var baseResponse = new BaseResponse<Domain.Entity.File>();
            try
            {
                var file = await _fileRepository.GetById(id);
                if (file == null)
                {
                    baseResponse.Description = "File not found";
                    baseResponse.StatusCode = StatusCode.FileNotFound;
                    return baseResponse;
                }

                baseResponse.Data = file;

                baseResponse.StatusCode = StatusCode.OK;
              
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<Domain.Entity.File>()
                {
                    Description = $"[GetFile] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }


        public async Task<IBaseResponse<Domain.Entity.File>> GetFileByName(string name)
        {
            var baseResponse = new BaseResponse<Domain.Entity.File>();
            try
            {
                var file = await _fileRepository.GetByName(name);

                if (file == null)
                {
                    baseResponse.Description = "File not found";
                    baseResponse.StatusCode = StatusCode.FileNotFound;
                    return baseResponse;
                }

                baseResponse.Data = file;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<Domain.Entity.File>()
                {
                    Description = $"[GetFileByName] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<Domain.Entity.File>>> GetFiles()
        {
            var baseResponse = new BaseResponse<IEnumerable<Domain.Entity.File>>();
            try
            {
                var files = await _fileRepository.Select();
                if (files.Count == 0)
                {
                    baseResponse.Description = "Найдено 0 элементов";
                    baseResponse.StatusCode = StatusCode.FileNotFound;
                    return baseResponse;
                }

                baseResponse.Data = files;
                baseResponse.StatusCode = StatusCode.OK;

                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Domain.Entity.File>>()
                {
                    Description = $"[GetFiles] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
