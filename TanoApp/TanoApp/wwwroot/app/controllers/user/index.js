var UserController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }

    function registerEvents() {
        //Init validation
        $("#frmMaintainance").validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtFullName: { required: true },
                txtUserName: { required: true },
                txtPassword: {
                    required: true,
                    minlength: 6
                },
                txtConfirmPassword: {
                    equalTo: "#txtPassword"
                },
                txtEmail: {
                    required: true,
                    email: true
                }
            }
        });

        $("#txt-search-keyword").keypress(function (e) {
            if (e.which === 13) {
                e.preventDefault();
                loadData();
            }
        })

        $("#btn-search").on("click", function () {
            loadData();
        })

        $("#ddl-show-page").on("change", function () {
            tano.configs.pageSize = $(this).val();
            tano.configs.pageIndex = 1;
            loadData(true);
        })


        $("#btn-create").on("click", function () {
            resetFormMaintainance();
            initRoleList();
            $("#modal-add-edit").modal("show");
        })

        $("body").on("click", ".btn-edit", function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/admin/user/getbyid",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    tano.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $("#hideId").val(data.Id);
                    $("#txtFullName").val(data.FullName);
                    $("#txtUserName").val(data.UserName);
                    $("#txtEmail").val(data.Email);
                    $("#txtPhoneNumber").val(data.Email);
                    $("#txtPhoneNumber").val(data.PhoneNumber);
                    $("#ckStatus").prop("checked", data.Status === 1);
                    initRoleList(data.Roles);
                    disbaleFieldEdit(true);
                    customNotify("Update successful!", types.sucess);
                    $("#modal-add-edit").modal("show");
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
                var fullName = $("#txtFullName").val();
                var userName = $("#txtUserName").val();
                var password = $("#txtPassword").val();
                var email = $("#txtEmail").val();
                var roles = [];
                $.each($('input[name="ckRoles"]'), function (i, item) {
                    if ($(item).prop('checked') === true) {
                        roles.push($(item).prop('value'));
                    }
                });
                var status = $("#ckStatus").prop("checked") == true ? 1 : 0;
                $.ajax({
                    type: "post",
                    url: "/admin/user/saveentity",
                    data: {
                        Id: id,
                        FullName: fullName,
                        UserName: userName,
                        Password: password,
                        Email: email,
                        PhoneNumber: phoneNumber,
                        Status: status,
                        Roles: roles
                    },
                    dataType: "json",
                    beforeSend: function () {
                        tano.startLoading();
                    },
                    sucess: function () {
                        customNotify("Update successful", types.sucess);
                        $("#modal-add-edit").modal("hide");
                        resetFormMaintainance();
                        tano.stopLoading();
                        loadData(true);
                    },
                    error: function () {
                        customNotify("Update fail", types.danger);
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
                    type: "Post",
                    url: "/admin/user/delete",
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

        function disableFieldEdit(disabled) {
            $("#txtUserName").prop("disabled", disabled);
            $("#txtPassword").prop("disabled", disabled);
            $("#txtConfirmPassword").prop("disabled", disabled);
        }

        function resetFormMaintainance() {
            disableFieldEdit(false);
            $("#hideId").val("");
            $("#txtFullName").val("");
            $("#txtUserName").val("");
            $("#txtPassword").val("");
            $("#txtConfirmPassword").val("");
            $("#input[name='ckRoles']").removeAttr('checked');
            $("#txtPhoneNumber").val("");
            $("#ckStatus").prop("checked", true);
        }

        function initRoleList(selectedRoles) {
            $.ajax({
                url: '/admin/role/getall',
                type: 'get',
                dataType: 'json',
                async: false,
                success: function (reponse) {
                    var template = $("#role-template").html();
                    var data = reponse;
                    var render = '';
                    $.each(data, function (i, item) {
                        var checked = '';
                        if (selectedRoles !== undefined && selectedRoles.indexOf(item.Name) !== -1) {
                            checked = 'checked';
                        }
                        render += Mustache.render(template, {
                            Name: item.Name,
                            Description: item.Description,
                            Checked: checked
                        });
                    });
                    $("#list-roles").html(render);
                }
            })
        }

        function loadData(isPageChanged) {
            $.ajax({
                type: 'GET',
                url: '/admin/user/getallpaging',
                data: {
                    categoryId: $("#ddl-category-serach").val(),
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
                    if (reponse.RowCount > 0) {
                        $.each(response.Results, function (i, item) {
                            render += Mustable.render(template, {
                                FullName: item.FullName,
                                Id: item.Id,
                                userName: item.UserName,
                                Avatar: item.Avatar === undefined ? '<img src = "/admin-side/images/user.png"/>' : '',
                                DateCreated: tano.dateTimeFormatJson(item.DateCreated),
                                Status: tano.getStatus(item.Status)
                            });
                        });
                        $("#lbl-total-records").text(response.RowCount);
                        if (render !== undefined) {
                            $("#tbl-content").html(render);
                        }
                        wrapPing(response.RowCount, function () {
                            loadData();
                        }, isPageChanged);
                    }
                    else {
                        $("tbl-content").html("");
                    }
                    tano.stopLoading();
                },
                error: function (status) {
                    console.log(status);
                    tano.stopLoading();
                }
            })
        }

        function wrapPaging(recordCount, callBack, changePageSize) {
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

    }
}