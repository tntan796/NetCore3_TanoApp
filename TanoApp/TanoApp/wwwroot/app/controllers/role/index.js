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
    }
}