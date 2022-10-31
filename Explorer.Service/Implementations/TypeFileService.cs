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
    public class TypeFileService : ITypeFileService
    {
        private readonly ITypeFileRepository _typeFileRepository;

        public TypeFileService(ITypeFileRepository TypeFileRepository)
        {
            _typeFileRepository = TypeFileRepository;
        }
       
             
       public async Task<IBaseResponse<TypeFileViewModel>> CreateTypeFile(TypeFileViewModel typeFileViewModel)
       {
           var baseResponse = new BaseResponse<TypeFileViewModel>();
           try
           {
               var typeFile = new Domain.Entity.TypeFile()
               {
                
                   NameType = typeFileViewModel.NameType,
                   Icon = typeFileViewModel.Icon
           
               };

               await _typeFileRepository.Create(typeFile);
           }
           catch (Exception ex)
           {
               return new BaseResponse<TypeFileViewModel>()
               {
                   Description = $"[CreateTypeFile] : {ex.Message}",
                   StatusCode = StatusCode.InternalServerError
               };
           }
           return baseResponse;
       }
       

      public async Task<IBaseResponse<bool>> DeleteTypeFile(int id)
      {
          var baseResponse = new BaseResponse<bool>()
          {
              Data = true
          };
          try
          {
              var typeFile = await _typeFileRepository.GetById(id);
              if (typeFile == null)
              {
                  baseResponse.Description = "TypeFile not found";
                  baseResponse.StatusCode = StatusCode.TypeFileNotFound;
                  baseResponse.Data = false;

                  return baseResponse;
              }

              await _typeFileRepository.Delete(typeFile);

              return baseResponse;
          }
          catch (Exception ex)
          {
              return new BaseResponse<bool>()
              {
                  Description = $"[DeleteTypeFile] : {ex.Message}",
                  StatusCode = StatusCode.InternalServerError
              };
          }
      }



      public async Task<IBaseResponse<Domain.Entity.TypeFile>> EditTypeFile(int id, TypeFileViewModel model)
      {
          var baseResponse = new BaseResponse<Domain.Entity.TypeFile>();
          try
          {
              var typeFile = await _typeFileRepository.GetById(id);
              if (typeFile == null)
              {
                  baseResponse.StatusCode = StatusCode.TypeFileNotFound;
                  baseResponse.Description = "TypeFile not found";
                  return baseResponse;
              }


              typeFile.NameType = model.NameType;
              typeFile.Icon = model.Icon;
               

              await _typeFileRepository.Update(typeFile);


              return baseResponse;
          

          }
          catch (Exception ex)
          {
              return new BaseResponse<Domain.Entity.TypeFile>()
              {
                  Description = $"[Edit] : {ex.Message}",
                  StatusCode = StatusCode.InternalServerError
              };
          }
      }


        public async Task<IBaseResponse<TypeFileViewModel>> GetTypeFile(int id) 
        {
            var baseResponse = new BaseResponse<TypeFileViewModel>();
            try
            {
                var typeFile = await _typeFileRepository.GetById(id);
                if (typeFile == null)
                {
                    baseResponse.Description = "TypeFile not found";
                    baseResponse.StatusCode = StatusCode.TypeFileNotFound;
                    return baseResponse;
                }

                var data = new TypeFileViewModel()
                {
                    NameType = typeFile.NameType,
                    Icon = typeFile.Icon
                };

                baseResponse.StatusCode = StatusCode.OK;
                baseResponse.Data = data;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<TypeFileViewModel>()
                {
                    Description = $"[GetTypeFile] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }


        public async Task<IBaseResponse<Domain.Entity.TypeFile>> GetTypeFileByName(string name)
        {
            var baseResponse = new BaseResponse<Domain.Entity.TypeFile>();
            try
            {
                var typeFile = await _typeFileRepository.GetByName(name);
                if (typeFile == null)
                {
                    baseResponse.Description = "TypeFile not found";
                    baseResponse.StatusCode = StatusCode.TypeFileNotFound;
                    return baseResponse;
                }

                baseResponse.Data = typeFile;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<Domain.Entity.TypeFile>()
                {
                    Description = $"[GetTypeFileByName] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<Domain.Entity.TypeFile>>> GetTypesFiles()
        {
            var baseResponse = new BaseResponse<IEnumerable<Domain.Entity.TypeFile>>();
            try
            {
                var typeFiles = await _typeFileRepository.Select();
                if (typeFiles.Count == 0)
                {
                    baseResponse.Description = "Найдено 0 элементов";
                    baseResponse.StatusCode = StatusCode.TypeFileNotFound;
                    return baseResponse;
                }

                baseResponse.Data = typeFiles;
                baseResponse.StatusCode = StatusCode.OK;

                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Domain.Entity.TypeFile>>()
                {
                    Description = $"[GetTypeFiles] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
