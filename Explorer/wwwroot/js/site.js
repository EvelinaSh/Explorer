    function getFolderContextMenu($node, tree) {
            return {
                  "Folder": {
                  "seperator_before": false,
                  "seperator_after": false,
                  "label": "Добавить папку",
                   action: function (obj) {
                       $("#ModalContentForPartial").empty();
                       let IdFolder = parseInt($node.id);
                       $.ajax({
                           type: 'GET',
                           url: '/Home/ModalAddFolder',
                           success: function (response) {
                               $('#ModalContentForPartial').append(response);

                               $('#ContentModalAddFolder').data("IdParent", IdFolder);
                               $('#alertAddFolder').css('display', 'none');

                               $('#modal').modal('show')
                           },
                           failure: function () {
                               $('#modal').modal('hide')

                           }
                       });


                       
                          }
                  },
                  "File": {
                  "seperator_before": false,
                  "seperator_after": false,
                  "label": "Загрузить файл",
                  action: function (obj) {
                      let IdFolder = parseInt($node.id);
                      $("#ModalContentForPartial").empty();

                      $.ajax({
                          type: 'GET',
                          url: '/Home/ModalAddFile',
                          success: function (response) {
                              $('#ModalContentForPartial').append(response);

                              $('#ContentModalAddFile').data("IdFolder", IdFolder);
                              $('#alertAddFile').css('display', 'none');

                              $('#modal').modal('show')
                          },
                          failure: function () {
                              $('#modal').modal('hide')

                          }
                      });

                  }
                  },
                  "Rename": {
                  "separator_before": false,
                  "separator_after": false,
                  "label": "Переименовать",
                             "action": function (obj) { 
                                 tree.edit($node);
  
                             }
                  },
                  "Remove": {
                  "separator_before": false,
                  "separator_after": false,
                  "label": "Удалить",
                  "action": function (obj) {
                      let idFolder = parseInt($node.id);
                      console.log(idFolder);
                      Node = $node;

                      let CycleDelete = function (Node) {
                          if (Node.children) {
                              $.ajax({
                                  url: "/TreeView/GetChildren/" + Node.id,
                                  success: function (result) {
                                      console.log(result);
                                      result.forEach(el => {
                                          console.log(el);
                                          CycleDelete(el)

                                      })
                                       
                                  }
                              });
                          }
                          if (Node.a_attr.type == "folder") {
                              console.log(Node.a_attr.type);
                              let Id = parseInt(Node.id);
                              $.ajax({
                                  url: "/Folder/DeleteFolder/" + Id,
                                  type: 'POST',
                                  success: function (result) {
                                      console.log("Успех")

                                  }
                              });
                          }
                          else {
                              console.log(Node.a_attr.type);
                              let Id = parseInt(Node.id);
                              $.ajax({
                                  url: "/File/DeleteFile/" + Id,
                                  type: 'POST',
                                  success: function (result) {
                                      console.log("Успех")

                                  }
                              });
                          }

                      } 

                      CycleDelete(Node);

                      $('#treeview').jstree(true).refresh();
                      
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
                                        
                    }
                },
                "Remove": {
                "separator_before": false,
                "separator_after": false,
                "label": "Удалить",
                "action": function (obj) {
                    tree.delete_node($node);
                    let IdFile = parseInt($node.id);

                    $.ajax({
                        url: "/File/DeleteFile",
                        type: 'POST',
                        data: { id: IdFile },
                        success: function (result) {
                            console.log("Успех")
                        }
                    });
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
                                 console.log(result)
                                 console.log("Скачано")
                                 window.location = '/File/DownloadFile/' + IdFile;
                            }
                         });
                        }
                }
            }
    }

    $('#treeview').jstree({
        "plugins": ["contextmenu", "types", "themes", "crrm", "html_data", "ui"],
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
          
    })
        .bind('rename_node.jstree', function (e, data) {
            if (data.node.a_attr.type == "folder") {
                let idParent = parseInt(data.node.parent);
                let idFolder = parseInt(data.node.id);

                console.log(idParent)
                console.log(idFolder)

                console.log(data.text);
                let model = {
                    IdFolder: idFolder,
                    NameFolder: data.text,
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
            else {
                let IdFile = parseInt(data.node.id);
                console.log(data.text);
                let attr = data.text.split('.');
                let model = {
                    IdFile: IdFile,
                    NameFile: attr[0]
                }
                console.log(IdFile)
                console.log(attr[0])
                $.ajax({
                    url: "/File/UpdateFile",
                    type: 'POST',
                    data: model,
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
                console.log(file.ContentFile[0])
        $('#myTab').append(
        ' <li class="nav-item" style="margin-right: 0.5em;" id="' + file.NameFile + '-nav-tab" role="presentation"><div class="nav-link" id="' + file.NameFile + '-tab" data-bs-toggle="tab" data-bs-target="#' + file.NameFile + '-tab-pane" type="button" role="tab" aria-controls="' + file.NameFile + '-tab-pane" aria-selected="false">' + file.NameFile + '<button type="button" id="' + file.NameFile + '-btn-close" class="btn-close" aria-label="Close"></button></div></li>'
        );

        $('#myTabContent').append(
            '<div class="tab-pane fade" style="margin-top: 0.5em; white-space: pre-line;" id="' + file.NameFile + '-tab-pane" role="tabpanel" aria-labelledby="' + file.NameFile + '-tab" tabindex="0" >' + file.ContentFile + '</div>'
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
    $("#ModalContentForPartial").empty();
    let IdFolder = 0;
    $.ajax({
        type: 'GET',
        url: '/Home/ModalAddFolder',
        success: function (response) {
            console.log(response)
            $('#ModalContentForPartial').append(response);

            $('#ContentModalAddFolder').data("IdParent", IdFolder);
            $('#alertAddFolder').css('display', 'block');
            $('#addFolderBtn').attr("id", "addRootFolderBtn");

            $('#modal').modal('show')
        },
        failure: function () {
            $('#modal').modal('hide')

        }
    });

});

$('#ButtonAddFile').on('click', function (e, data) {

    $("#ModalContentForPartial").empty();

    $.ajax({
        type: 'GET',
        url: '/Home/ModalAddFile',
        success: function (response) {
            $('#ModalContentForPartial').append(response);

            $('#ContentModalAddFile').data("IdFolder", 0);
            $('#alertAddFile').css('display', 'block');
            $('#addFileBtn').attr("id", "addRootFileBtn");

            $('#modal').modal('show')
        },
        failure: function () {
            $('#modal').modal('hide')

        }
    });

});


$('#ButtonAddType').on('click', function (e, data) {
    $("#ModalContentForPartial").empty();

    $.ajax({
        type: 'GET',
        url: '/Home/ModalAddType',
        success: function (response) {
            $('#ModalContentForPartial').append(response);
            $('#modal').modal('show')
        },
        failure: function () {
            $('#modal').modal('hide')
        }
    });

});

$('#ButtonDeleteType').on('click', function (e, data) {
    $("#ModalContentForPartial").empty();

    $('#modal').modal('show')
    $.ajax({
        type: 'GET',
        url: '/Home/ModalDeleteType',
        success: function (response) {
            $('#ModalContentForPartial').append(response);
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


