var RoleController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }

    function loadData(isPageChanged) {
        $.ajax({
            type: 'GET',
            url: '/admin/role/getallpaging',
            data: {
                keyword: $('#txt-search-keyword').val(),
                page: tano.configs.pageIndex,
                pageSize: tano.configs.pageSize
            },
            dataType: 'json',
            beforeSend: function () {
                tano.startLoading();
            },
            success: function (response) {
                var template = $("#table-template").html();
                var render = "";
                if (response.rowCount > 0) {
                    $.each(response.results, function (i, item) {
                        render += Mustache.render(template, {
                            Id: item.id,
                            Name: item.name,
                            Description: item.description
                        });
                    });
                    $("#lbl-total-records").text(response.rowCount);
                    if (render !== undefined) {
                        $("#tbl-body").html(render);
                    }
                    wrapPaging(response.rowCount, function () {
                        loadData();
                    }, isPageChanged);
                }
                else {
                    $("tbl-body").html("");
                }
                tano.stopLoading();
            },
            error: function (status) {
                console.log(status);
                tano.stopLoading();
            }
        })
    }

    function wrapPaging(recordCount, callback, changePageSize) {
        var totalSize = Math.ceil(recordCount / tano.configs.pageSize);
        // Unbinde pagination if it exist or click change pagesize
        if ($('#paginationUL a').length === 0 || changePageSize === true) {
            $('#paginationUL').empty();
            $('#paginationUL').removeData('twbs-pagination');
            $('#paginationUL').unbind('page');
        }
        //Bind pagination event
        $('#paginationUL').twbsPagination({
            totalPages: totalSize,
            visiblePages: 7,
            first: '<i class="fa fa-angle-double-left" aria-hidden="true"></i>',
            prev: '<i class="fa fa-angle-left" aria-hidden="true"></i>',
            next: '<i class="fa fa-angle-right" aria-hidden="true"></i>',
            last: '<i class="fa fa-angle-double-right" aria-hidden="true"></i>',
            onPageClick: function (event, p) {
                tano.configs.pageIndex = p;
                setTimeout(callback(), 200);
            }
        })
    }

    function registerEvents() {
        //Init validation
        $("#frmMaintainance").validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtName: { required: true },
                txtDescription: { required: true}
            },
            messages: {
                txtName: { required: "Name is required!" },
                txtDescription: { required: "Description is required!" }
            }
        });

        $("#txtKeyword").keypress(function (e) {
            if (e.which === 13) {
                e.preventDefault();
                loadData();
            }
        })

        $("#btnSearch").on("click", function () {
            loadData();
        })

        $("#ddl-show-page").on("change", function () {
            tano.configs.pageSize = $(this).val();
            tano.configs.pageIndex = 1;
            loadData(true);
        })


        $("#btnCreate").on("click", function () {
            resetFormMaintainance();
            $("#modalAddEdit").modal("show");
        })

        $("body").on("click", ".btn-assign-permission", function (e) {
            $("#hideId").val($(this).data('id'));
            $("#modalAssignPermission").modal("show");
            loadFunctionList(fillPermission);
        });

        $("body").on("click", ".btn-edit", function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/admin/role/getbyid",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    tano.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $("#hideId").val(data.id);
                    $("#txtName").val(data.name);
                    $("#txtDescription").val(data.description);
                    $("#modalAddEdit").modal("show");
                    tano.stopLoading();
                },
                error: function () {
                    customNotify("Update fail!", types.danger);
                }
            })
        })

        $('#btnSave').on('click', function (e) {
            if ($("#frmMaintainance").valid()) {
                e.preventDefault();
                var id = $("#hideId").val();
                var name = $("#txtName").val();
                var description = $("#txtDescription").val();
                $.ajax({
                    type: "post",
                    url: "/admin/role/saveentity",
                    data: {
                        Id: id,
                        Name: name,
                        Description: description
                    },
                    dataType: "json",
                    beforeSend: function () {
                        tano.startLoading();
                    },
                    success: function () {
                        if (id) {
                            customNotify("Update successful", types.sucess);
                        } else {
                            customNotify("Add successful", types.sucess);
                        }
                        $("#modalAddEdit").modal("hide");
                        resetFormMaintainance();
                        tano.stopLoading();
                        loadData(true);
                    },
                    error: function () {
                        if (id) {
                            customNotify("Update fail", types.danger);
                        } else {
                            customNotify("Add fail", types.danger);
                        }
                        tano.stopLoading();
                    }
                })
            }
        })

        $("body").on("click", ".btn-delete", function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            if (confirm("Are you want to delete?")) {
                $.ajax({
                    type: "delete",
                    url: "/admin/role/delete",
                    data: { id: that },
                    beforeSend: function () {
                        tano.startLoading();
                    },
                    success: function () {
                        customNotify("Delete Success!", types.success);
                        tano.stopLoading();
                        loadData();
                    },
                    error: function () {
                        customNotify("Delete Fail!", types.danger);
                        tano.stopLoading();
                    }
                })
            }
        })

        $("#btnSavePermission").off("click").on("click", function (e) {
            e.preventDefault();
            var listPermission = [];
            var trElement = $("#tblFunction tbody tr");
            for (var i = 0; i < trElement.length; i++) {
                var item = trElement[i];
                listPermission.push({
                    RoleId: $("#hideId").val(),
                    FunctionId: $(item).data("id"),
                    CanRead: $(item).find('.ckView').first().prop("checked"),
                    CanCreate: $(item).find('.ckAdd').first().prop("checked"),
                    CanUpdate: $(item).find('.ckEdit').first().prop("checked"),
                    CanDelete: $(item).find('.ckDelete').first().prop("checked")
                });
            }
            $.ajax({
                type: "post",
                url: "/admin/role/SavePermission",
                data: {
                    listPermission: listPermission,
                    roleId: $('#hideId').val()
                },
                beforeSend: function () {
                    tano.startLoading();
                },
                success: function (response) {
                    customNotify("Save Successful!", types.sucess);
                    $("#modalAssignPermission").modal("hide");
                    tano.stopLoading();
                },
                error: function (err) {
                    customNotify("Save Fail!", types.danger);
                    console.log("error:", err);
                    tano.stopLoading();
                }
            })

        });

        function resetFormMaintainance() {
            $("#hideId").val("");
            $("#txtName").val("");
            $("#txtDescription").val("");
        }

        function loadFunctionList(fillPermission) {
            return $.ajax({
                type: "GET",
                url: '/admin/function/getall',
                dataType: 'json',
                success: function (response) {
                    //response = buildTree(response);
                    bindingTemplate(response);
                    handleClickCheckAll();
                    handleCheckAllItemInParentMenu(response.length);
                    fillPermission($("#hideId").val(), response.length);
                }
            })
        }

        function bindingTemplate(response) {
            var template = '';
            response.forEach((item, i) => {
                if (item.parentId !== null) {
                    template += '<tr data-id="' + item.id + '">';
                    template += '<td>' + item.name + '</td>';
                } else {
                    template += '<tr class="menu-parent" data-id="' + item.id + '">';
                    template += '<td><b>' + item.name + '<b/></td>';
                }
                template += '<td>';
                template += '<label>';
                template += '<input type="checkbox" id="ckView-' + item.id + '" value="' + item.id + '" class="ckView ' + item.parentId + ' ">';
                template += '</label>';
                template += '</td>';
                template += '<td>';
                template += '<label>';
                template += '<input type="checkbox" id="ckAdd-' + item.id + '" value="' + item.id + '" class="ckAdd ' + item.parentId + '">';
                template += '</label>';
                template += '</td>';
                template += '<td>';
                template += '<label>';
                template += '<input type="checkbox" id="ckEdit-' + item.id + '" value="' + item.id + '" class="ckEdit ' + item.parentId + '">';
                template += '</label>';
                template += '</td>';
                template += '<td>';
                template += '<label>';
                template += '<input type="checkbox" id="ckDelete-' + item.id + '" value="' + item.id + '" class="ckDelete ' + item.parentId + '">';
                template += '</label>';
                template += '</td>';
                template += '</tr>';
            });
            $("#lst-data-function").html(template);
        }

        function handleClickCheckAll() {
            // Handle action when user click check all checkbox
            $("#ckCheckAllView").on("click", function () {
                $(".ckView").prop('checked', $(this).prop('checked'));
            })
            $("#ckCheckAllCreate").on("click", function () {
                $(".ckAdd").prop('checked', $(this).prop('checked'));
            })
            $("#ckCheckAllEdit").on("click", function () {
                $(".ckEdit").prop('checked', $(this).prop('checked'));
            })
            $("#ckCheckAllDelete").on("click", function () {
                $(".ckDelete").prop('checked', $(this).prop('checked'));
            })
        }

        function handleCheckAllItemInParentMenu(totalItem) {
            fillCheckAll("ckView", "#ckCheckAllView", totalItem);
            fillCheckAll("ckAdd", "#ckCheckAllCreate", totalItem);
            fillCheckAll("ckEdit", "#ckCheckAllEdit", totalItem);
            fillCheckAll("ckDelete", "#ckCheckAllDelete", totalItem);
        }

        function fillCheckAll(checkBoxName, checkAllName, totalItem) {
            $("." + checkBoxName).on("click", function () {
                // Checked all item in menu parent when checked in menu parent
                var parentNode = $(this).closest("tr .menu-parent").prev()[0];
                //if (parentNode.classList.contains("menu-parent")) {
                //    $("." + checkBoxName + "." + parentNode.dataset.id).prop('checked', $(this).prop("checked"));
                //}

                //// Check menu parent when click all sub menu
                //var parentClass = "#" + checkBoxName + "-" + parentNode.dataset.id;
                //var isCheckMenuParent = $(parentClass + ":checked").length == $(parentClass).length;
                //var cbMenuParentElm = $(parentClass);
                //cbMenuParentElm.prop("checked", isCheckMenuParent);

                $(checkAllName).prop("checked", $("." + checkBoxName + ":checked").length === totalItem);
            })
        }

        function buildTree(functions) {
            var result = [];
            const parentIds = functions.filter(t => t.parentId == null);
            parentIds.forEach(item => {
                item.childs = [];
                result.push(item);
                buildNestedTree(functions, item);
            })
            return result;
        }

        function buildNestedTree(functions, parent) {
            functions.forEach(item => {
                if (item.parentId == parent.id) {
                    item.childs = [];
                    parent.childs.push(item);
                    const nestedNode = functions.filter(t => t.parentId == item.id);
                    if (nestedNode.length > 0) {
                        buildNestedTree(functions, item);
                    }
                }
            })
        }

        function buildTreeTemplate(datas) {
            var template = '';
            template += '<div class="panel-group" id="accordion">';
            template += '<div class="panel panel-default" id="panel1">';
            template += '<div class="panel-heading">';
            template += '<h4 class="panel-title">';
            template += '<div class="row">';
            template += '<div class="col-sm-6">' + datas.name + '</div>';
            template += '<div class="col-sm-6 text-right">';
            template += '<a data-toggle="collapse" data-target="#collapse' + datas.id + '" href="#collapse' + datas.id + '"><i class="fa fa-minus" aria-hidden="true"></i></a>';
            template += '<i class="fa fa-minus" aria-hidden="true"></i>';
            template += '</div>';
            template += '</div>';
            template += '</h4>';
            template += '</div>';
            template += '<div id="collapse' + datas.id +'" class="panel-collapse collapse in">';
            if (datas.childs.length > 0) {
            template += '<div class="panel-body">';
            datas.childs.forEach(item => {
            template += buildTreeTemplate(item);
            });
            template += '</div>';
            }
            template += '</div>';
            template += '</div>';
            template += '</div>';
            template += '</div>';
            return template;
        }

        function fillPermission(roleId, totalItem) {
            $.ajax({
                type: "Post",
                url: "/admin/role/listAllFunction",
                data: {
                    roleId: roleId
                },
                dataType: "json",
                beforeSend: function () {
                    
                },
                success: function (listPermission) {
                    var trElm = $("#tblFunction tbody tr");
                    if (trElm.length <= 0) {
                        return;
                    }
                    for (let i = 0; i < trElm.length; i++) {
                        listPermission.forEach((jtem, j) => {
                            let item = trElm[i];
                            if (jtem.functionId == $(item).data('id')) {
                                $(item).find(".ckView").first().prop("checked", jtem.canRead);
                                $(item).find(".ckAdd").first().prop("checked", jtem.canCreate);
                                $(item).find(".ckEdit").first().prop("checked", jtem.canUpdate);
                                $(item).find(".ckDelete").first().prop("checked", jtem.canDelete);
                            }
                        })
                    }
                    var checkAllView = $(".ckView:checked").length == totalItem;
                    $("#ckCheckAllView").prop("checked", checkAllView);
                    var checkAllAdd = $(".ckAdd:checked").length == totalItem;
                    $("#ckCheckAllCreate").prop("checked", checkAllAdd);
                    var checkAllEdit = $(".ckEdit:checked").length == totalItem;
                    $("#ckCheckAllEdit").prop("checked", checkAllEdit);
                    var checkAllDelete = $(".ckDelete:checked").length == totalItem;
                    $("#ckCheckAllDelete").prop("checked", checkAllDelete);
                },
                error: function (error) {
                    console.log('error:', error);
                }
            })
        }
    }
}