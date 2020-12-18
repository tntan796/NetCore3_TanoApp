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
            loadFunctionList(fillPermission);
            $("#modalAssignPermission").modal("show");
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
                beforeSend: function () {
                    tano.startLoading();
                },
                success: function (response) {
                    var template = '';
                    $.each(response, function (i, item) {
                        template += '< tr data-id="'+ item.id +'">';
                        template += '<td>'+ item.name +'</td>';
                        template += '<td>';
                        template += '<label>';
                        template += '<input type="checkbox" value="'+ item.id +'" class="ckView">';
                        template += '<span class="text">Allow</span>';
                        template += '</label>';
                        template += '</td>';
                        template += '<td>';
                        template += '<label>';
                        template += '<input type="checkbox" value="' + item.id +'" class="ckAdd">';
                        template += '<span class="text">Allow</span>';
                        template += '</label>';
                        template += '</td>';
                        template += '<td>';
                        template += '<label>';
                        template += '<input type="checkbox" value="' + item.id +'" class="ckEdit">';
                        template += '<span class="text">Allow</span>';
                        template += '</label>';
                        template += '</td>';
                        template += '<td>';
                        template += '<label>';
                        template += '<input type="checkbox" value="'+ item.id +'" class="ckDelete">';
                        template += '<span class="text">Allow</span>';
                        template += '</label>';
                        template += '</td>';
                        template += '</tr>';
                    });
                    $("#lst-data-function").html(template);
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

                    // Checked on CheckAllView when click into lasted CkView
                    $(".ckView").on("click", function () {
                        $("#ckCheckAllView").prop("checked", $(".ckView:checked").length === response.length);
                    })
                    $(".ckAdd").on("click", function () {
                        $("#ckCheckAllCreate").prop("checked", $(".ckAdd:checked").length === response.length);
                    })
                    $(".ckEdit").on("click", function () {
                        $("#ckCheckAllEdit").prop("checked", $(".ckEdit:checked").length === response.length);
                    })
                    $(".ckDelete").on("click", function () {
                        $("#ckCheckAllDelete").prop("checked", $(".ckDelete:checked").length === response.length);
                    })
                    fillPermission($("#hideId").val());
                }
            })
        }

        function fillPermission(roleId) {
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
                    $.each($("#tblFunction tbody tr", function (i, item) {
                        $.each(listPermission, function (j, jtem) {
                            if (jtem.FunctionId == $(item).data('id')) {
                                $(item).find(".ckView").first().prop("checked", jtem.CanRead);
                                $(item).find(".ckAdd").first().prop("checked", jtem.CanCreate);
                                $(item).find(".ckEdit").first().prop("checked", jtem.CanUpdate);
                                $(item).find(".ckDelete").first().prop("checked", jtem.CanDelete);
                            }
                        })
                    }));
                    var checkAllView = $(".ckView:checked").length == $("#tblFunction tbody tr .ckAdd").length;
                    $("#ckCheckAllView").prop("checked", checkAllView);
                    var checkAllAdd = $(".ckAdd:checked").length == $("#tblFunction tbody tr .ckAdd").length;
                    $("#ckCheckAllCreate").prop("checked", checkAllAdd);
                    var checkAllEdit = $(".ckEdit:checked").length == $("#tblFunction tbody tr .ckEdit").length;
                    $("#ckCheckAllEdit").prop("checked", checkAllEdit);
                    var checkAllDelete = $(".ckDelete:checked").length == $("#tblFunction tbody tr .ckDelete").length;
                    $("#ckCheckAllDelete").prop("checked", checkAllDelete);
                },
                error: function (error) {
                    console.log('error:', error);
                }
            })
        }
    }
}