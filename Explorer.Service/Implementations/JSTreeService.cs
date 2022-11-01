using Explorer.DAL.Interfaces;
using Explorer.DAL.Repositories;
using Explorer.Domain.Enum;
using Explorer.Domain.Response;
using Explorer.Domain.ViewModels;
using Explorer.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Explorer.Service.Implementations
{
    public class JSTreeService : IJSTreeService
    {
        private readonly IFolderRepository _folderRepository;

        public JSTreeService(IFolderRepository folderRepository)
        {
            _folderRepository = folderRepository;
        }

        public async Task<IBaseResponse<List<JsTreeViewModel>>> GetTree()
        {

            var baseResponse = new BaseResponse<List<JsTreeViewModel>>();
            try
            {
                var folders = await _folderRepository.Select();
                if (folders.Count == 0)
                {
                    baseResponse.Description = "Найдено 0 элементов";
                    baseResponse.StatusCode = StatusCode.FolderNotFound;
                    return baseResponse;
                }

                baseResponse.Data = new List<JsTreeViewModel>() { };


                foreach (var folder in folders)
                {
                    foreach(var file in folder.Files)
                    {
                        if ((file.IdFolder).Equals(1))
                        {
                            baseResponse.Data.Add(
                           new JsTreeViewModel()
                           {
                               id = file.IdFile + "file",
                               parent = "#",
                               text = file.NameFile + "." + file.TypeFile.NameType,
                               children = false,
                               a_attr = new Attr { type = "file", title = file.DescriptionFile },
                               icon = "data:image/png;base64," + Convert.ToBase64String(file.TypeFile.Icon)
                           }); ;
                        }
                         
                    }
                    
                    if (folder.IdParentFolder.Equals(1))
                    {
                        var childs = folders.Where(x => x.IdParentFolder == folder.IdFolder);
                        var child = childs.Count() != 0 ? true : false;

                        baseResponse.Data.Add(
                            new JsTreeViewModel()
                            {
                                id = folder.IdFolder + "folder",
                                parent = "#",
                                text = folder.NameFolder,
                                children = child,
                                a_attr = new Attr { type = "folder", title = "Папка " + folder.NameFolder }
                            });
                    }
                  

                }
                

                baseResponse.StatusCode = StatusCode.OK;

                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<JsTreeViewModel>>()
                {
                    Description = $"[GetTree] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }



        public async Task<IBaseResponse<List<JsTreeViewModel>>> GetTree(string id)
        {
            var baseResponse = new BaseResponse<List<JsTreeViewModel>>();
            try
            {
                var folders = await _folderRepository.Select();
                if (folders.Count == 0)
                {
                    baseResponse.Description = "Найдено 0 элементов";
                    baseResponse.StatusCode = StatusCode.FolderNotFound;
                    return baseResponse;
                }
                baseResponse.Data = new List<JsTreeViewModel>() { };
                foreach (var folder in folders)
                {

                    foreach (var file in folder.Files)
                    {
                        if ((file.IdFolder + "folder").Equals(id))
                        {
                            baseResponse.Data.Add(
                                new JsTreeViewModel()
                                {
                                    id = file.IdFile + "file",
                                    parent = file.IdFolder + "folder",
                                    text = file.NameFile + "." + file.TypeFile.NameType,
                                    children = false,
                                    a_attr = new Attr { type = "file", title = file.DescriptionFile },
                                    icon = "data:image/png;base64," + Convert.ToBase64String(file.TypeFile.Icon)
                                });
                        }
                    }

                    
                    if ((folder.IdParentFolder + "folder").Equals(id))
                    {
                        var childs = folders.Where(x => x.IdParentFolder == folder.IdFolder);
                        var child = childs.Count() != 0 ? true : false;

                            baseResponse.Data.Add(
                                    new JsTreeViewModel()
                                    {
                                        id = folder.IdFolder + "folder",
                                        parent = folder.IdParentFolder + "folder",
                                        text = folder.NameFolder,
                                        children = child,
                                        a_attr = new Attr { type = "folder", title = "Папка " + folder.NameFolder }
                                    });
                       

          
                    }

                       
                }


                baseResponse.StatusCode = StatusCode.OK;

                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<JsTreeViewModel>>()
                {
                    Description = $"[GetTree(string)] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }


    }
}
