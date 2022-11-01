    function getFolderContextMenu($node, tree) {
            return {
                  "Folder": {
                  "seperator_before": false,
                  "seperator_after": false,
                  "label": "Добавить папку",
                   action: function (obj) {
                       $('#modal').modal('show');
                       $('#ContentModalAddFile').css('display', 'none');
                       $('#ContentModalAddType').css('display', 'none');
                       $('#ContentModalDeleteType').css('display', 'none');
                       $('#alertAddFolder').css('display', 'none');
                       $('#ContentModalAddFolder').css('display', 'block');
                       let idParent = parseInt($node.id);
                       $('#addFolderBtn').on('click', function () {
                           let NewNameFolder = $('input#NewNameFolder').val();
                           let model = {
                               IdFolder: 0,
                               NameFolder: NewNameFolder,
                               IdParentFolder: idParent
                           }
                           $.ajax({
                               url: "/Folder/SaveFolder",
                               type: 'POST',
                               data: model,
                               success: function (result) {
                                   console.log("Успешно")
                               }
                           });
                           $node = tree.create_node($node.id, { text: NewNameFolder, a_attr: { type: 'folder' } });
                           tree.deselect_all();
                           tree.select_node($node);
                       });

                       
                          }
                  },
                  "File": {
                  "seperator_before": false,
                  "seperator_after": false,
                  "label": "Загрузить файл",
                  action: function (obj) {
                      let IdFolder = parseInt($node.id);
                      $('#ContentModalAddFile').css('display', 'block');
                      $('#ContentModalAddType').css('display', 'none');
                      $('#ContentModalDeleteType').css('display', 'none');
                      $('#ContentModalAddFolder').css('display', 'none');
                      $('#modal').modal('show');
                      $('#alertAddFile').css('display', 'none');

                      $('#addFileBtn').on('click', function () {
                          console.log("Меню")
                                var input = document.getElementById('files');
                                var files = input.files;
                                var formData = new FormData();

                                for (var i = 0; i != files.length; i++) {
                                        formData.append("files", files[i]);
                                }

                          let DescriptionFile = $('input#DescriptionFile').val();
                                                
                                formData.append("DescriptionFile", DescriptionFile);
                              formData.append("IdFolder", IdFolder);
                             
                                $.ajax({
                                    url: '/File/CreateFile',
                                    type: 'POST',
                                    data: formData,
                                    processData: false,  // tell jQuery not to process the data
                                    contentType: false,  // tell jQuery not to set contentType
                                    success: function (result) {
                                    
                                        tree.create_node($node.id, { text: result.nameFile, icon: result.icon, a_attr: { type: "file", title: result.descriptionFile } });
                                        tree.deselect_all();
                                    }
                                });


                      });
                    
                       
                  }
                  },
                  "Rename": {
                  "separator_before": false,
                  "separator_after": false,
                  "label": "Переименовать",
                             "action": function (obj) {
                                     tree.edit($node);
                                     tree.rename_node($node);
                             }
                  },
                  "Remove": {
                  "separator_before": false,
                  "separator_after": false,
                  "label": "Удалить",
                  "action": function (obj) {
                            tree.delete_node($node);
                  }
                  }
            }
    }

    function getFileContextMenu($node, tree) {
            return {
                "Rename": {
                "separator_before": false,
                "separator_after": false,
                "label": "Переименовать",
                "action": function (obj) {
                    tree.edit($node);
                    tree.rename_node($node);
                    }
                },
                "Remove": {
                "separator_before": false,
                "separator_after": false,
                "label": "Удалить",
                "action": function (obj) {
                        tree.delete_node($node);
                }
                },
                "Download": {
                "separator_before": false,
                "separator_after": false,
                "label": "Скачать",
                "action": function (obj) {
                    let IdFile = parseInt($node.id);
                    console.log(IdFile)
                         $.ajax({
                             url: "/File/DownloadFile",
                             type: 'GET',
                             data: { id: IdFile },
                             success: function (result) {
                                 console.log("Скачано")
                            }
                         });
                        }
                }
            }
    }

    $('#treeview').jstree({
        "plugins": ["unique", "contextmenu", "types", "themes", "crrm", "html_data", "ui"],
    'core': {
        "check_callback": true,
    'themes': {'ellipsis': true, 'url': true, 'dots': true, 'icons': true, 'dblclick-toggle': true, 'name':'proton', 'responsive': true },
    'data': {
        'url': function (node) {
                        return node.id === '#' ? "/TreeView/GetRoot" : "/TreeView/GetChildren/" + node.id;
                    },
    'data': function (node) {
                        
                        return {'id': node.id };
                    }
                }
            },
    'contextmenu': {
        "items": function ($node) {
                    var tree = $("#treeview").jstree(true);
                    if ($node.a_attr.type === 'file')
                    return getFileContextMenu($node, tree);
                    else
                    return getFolderContextMenu($node, tree);
                }
        }
          
    });



    $('#treeview').on("rename_node.jstree", function (e, data) {

            if (data.node.a_attr.type === "file"){
                 let IdFile = parseInt(data.node.id);
                 let attr = data.node.text.split('.');
                 let model = {
                    IdFile: IdFile,
                    NameFile: attr[0]
                 }

             $.ajax({
                url: "/File/UpdateFile",
                type: 'POST',
                data: model,
                success: function (result) {
                    console.log("Успех")
                }
            });
    }
    else {
        let idParent = parseInt(data.node.parent);
        let idFolder = parseInt(data.node.id);

        let model = {
            IdFolder: idFolder,
            NameFolder: data.node.text,
            IdParentFolder: idParent
        }

        $.ajax({
            url: "/Folder/SaveFolder",
        type: 'POST',
        data: model,
        success: function (result) {
            console.log("Успех")
        }
        });
    }
    });

    $('#treeview').on("delete_node.jstree", function (e, data) {

        if (data.node.a_attr.type === "file") {
        let IdFile = parseInt(data.node.id);

        $.ajax({
            url: "/File/DeleteFile",
            type: 'POST',
            data: {id: IdFile },
            success: function (result) {
                console.log("Успех")
            }
        });
        }
        else {
        let idFolder = parseInt(data.node.id);

         $.ajax({
            url: "/Folder/DeleteFolder",
            type: 'POST',
            data: {id: idFolder },
            success: function (result) {
                console.log("Успех")
            }
         });
        }
    });


    $('#treeview').on('dblclick', '.jstree-anchor', function (e, data) {
        let instance = $.jstree.reference(this)
        let node = instance.get_node(this);

        if (node.id.includes("file")) {
        let idNode = parseInt(node.id);

        $.ajax({
            url: '/File/GetFile/' + idNode,
            success: function (response) {
            let file = JSON.parse(JSON.stringify(response));

        $('#myTab').append(
        ' <li class="nav-item" style="margin-right: 0.5em;" id="' + file.NameFile + '-nav-tab" role="presentation"><div class="nav-link" id="' + file.NameFile + '-tab" data-bs-toggle="tab" data-bs-target="#' + file.NameFile + '-tab-pane" type="button" role="tab" aria-controls="' + file.NameFile + '-tab-pane" aria-selected="false">' + file.NameFile + '<button type="button" id="' + file.NameFile + '-btn-close" class="btn-close" aria-label="Close"></button></div></li>'
        );

        $('#myTabContent').append(
        '<div class="tab-pane fade" style="margin-top: 0.5em;" id="' + file.NameFile + '-tab-pane" role="tabpanel" aria-labelledby="' + file.NameFile + '-tab" tabindex="0" >' + file.ContentFile + '</div>'
        );
        }
        });
        }
                       
    });


    $('#myTab').on('click', '.btn-close', function(e, data){
        let idEl = $(this).attr("id").split('-')[0];
    
        $("#" + idEl + "-nav-tab").remove();
        $("#" + idEl + "-tab-pane").remove();
    });

$('#ButtonAddFolder').on('click', function (e, data) {
    $('#ContentModalDeleteType').remove();
    $('#ContentModalAddFile').css('display', 'none');
    $('#ContentModalAddType').css('display', 'none');
    $('#ContentModalDeleteType').css('display', 'none');
    $('#ContentModalAddFolder').css('display', 'block');
    $('#modal').modal('show');
    $('#alertAddFolder').css('display', 'block');

    $('#addFolderBtn').on('click', function () {
    let NameFolder = $('input#NewNameFolder').val();
    console.log(NameFolder)
    let model = {
        IdFolder: 0,
        NameFolder: NameFolder,
        IdParentFolder: 1
    }
    $.ajax({
        url: "/Folder/SaveFolder",
        type: 'POST',
        data: model,
        success: function (result) {
            console.log("Успешно")
        }
    });
        $('#treeview').jstree(true).create_node("#", { text: NameFolder, a_attr: { type: 'folder' } });
    });
});

$('#ButtonAddFile').on('click', function (e, data) {
    $('#ContentModalDeleteType').remove();
    $('#ContentModalAddFile').css('display', 'block');
    $('#ContentModalAddType').css('display', 'none');
    $('#ContentModalDeleteType').css('display', 'none');
    $('#ContentModalAddFolder').css('display', 'none');
    $('#alertAddFile').css('display', 'block');

    $('#modal').modal('show');
    $('#addFileBtn').on('click', function () {
        console.log("Кнопка")
        let IdFolder = 1;
        var input = document.getElementById('files');
        var files = input.files;
        var formData = new FormData();

        for (var i = 0; i != files.length; i++) {
            formData.append("files", files[i]);
        }

        let DescriptionFile = $('input#DescriptionFile').val();
        formData.append("DescriptionFile", DescriptionFile);
        formData.append("IdFolder", IdFolder);

        $.ajax({
            url: '/File/CreateFile',
            type: 'POST',
            data: formData,
            processData: false,  // tell jQuery not to process the data
            contentType: false,  // tell jQuery not to set contentType
            success: function (result) {
                console.log(result.nameFile)
                console.log(result.descriptionFile)
                console.log(result.icon)
                $('#treeview').jstree(true).create_node("#", { text: result.nameFile, icon: result.icon, a_attr: { type: "file", title: result.descriptionFile } });
                $('#treeview').deselect_all();
            }
        });
        

     

    });
});


$('#ButtonAddType').on('click', function (e, data) {
    $('#ContentModalDeleteType').remove();
    $('#ContentModalAddFile').css('display', 'none');
    $('#ContentModalAddType').css('display', 'block');
    $('#ContentModalDeleteType').css('display', 'none');
    $('#ContentModalAddFolder').css('display', 'none');
    $('#modal').modal('show');
    $('#addTypeBtn').on('click', function () {
        console.log("ICON1")
        var input = document.getElementById('icons');
        console.log(input)
        var icons = input.files;
        var formData = new FormData();
        console.log(icons)
         for (var i = 0; i != icons.length; i++) {
            formData.append("icons", icons[i]);
        }

        let NameType = $('input#NameType').val();
        formData.append("NameType", NameType);
        console.log(NameType)

        $.ajax({
            url: '/TypeFile/SaveType',
            type: 'POST',
            data: formData,
            processData: false,  // tell jQuery not to process the data
            contentType: false,  // tell jQuery not to set contentType
            success: function (result) {
                console.log("ICON")
            }
        });

    });

});

$('#ButtonDeleteType').on('click', function (e, data) {
    $('#ContentModalAddFile').css('display', 'none');
    $('#ContentModalAddType').css('display', 'none');
    $('#ContentModalAddFolder').css('display', 'none');
    $('#modal').modal('show')
    $.ajax({
        type: 'GET',
        url: '/Home/ModalDeleteType',
        success: function (response) {
            $('#ModelContentForPartial').append(response);
            $('#modal').modal('show')
        },
        failure: function () {
            $('#modal').modal('hide')
        },
        error: function (response) {
            alert(response.responseText);
        }
    });

});


