var UserController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
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
                if (response.rowCount > 0) {
                    $.each(response.results, function (i, item) {
                        render += Mustache.render(template, {
                            Account: item.account,
                            FullName: item.fullName,
                            Id: item.id,
                            UserName: item.userName,
                            Avatar: item.avatar === null ? '<img src="https://lh3.googleusercontent.com/proxy/xgail7fMYaoDLNjH4HWCq1c21sbtCqZ6J23JHwmdP5DdQUaxF460pEz5y3h-WQqhm6vb_bH2eWli2QtAs-wLrBbvcIVTXqbo0DCpGpb9Uqf6PWc3s2Y" width=25 />' : '<img src="' + item.avatar + '" width=25 />',
                            DateCreated: tano.dateTimeFormatJson(item.dateCreated),
                            Status: tano.getStatus(item.status)
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
            },
            messages: {
                txtFullName: { required: "Full Name is required!"},
                txtUserName: { required: "User Name is required!"},
                txtPassword: {
                    required: "Password is required!",
                    minlength: "Min length is 6 character!"
                },
                txtConfirmPassword: {
                    equalTo: "Confirm Password must same the password!"
                },
                txtEmail: {
                    required: "Email is required!",
                    email: "Not type of Email!"
                }
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
            initRoleList();
            $("#modalAddEdit").modal("show");
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
                    $("#hideId").val(data.id);
                    $("#txtFullName").val(data.fullName);
                    $("#txtUserName").val(data.userName);
                    $("#txtEmail").val(data.email);
                    $("#txtPhoneNumber").val(data.phoneNumber);
                    $("#ckStatus").prop("checked", data.status === 1);
                    initRoleList(data.Roles);
                    disableFieldEdit(true);
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
                var fullName = $("#txtFullName").val();
                var userName = $("#txtUserName").val();
                var password = $("#txtPassword").val();
                var phoneNumber = $("#txtPhoneNumber").val();
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
                    url: "/admin/user/SaveEntity",
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
                            ID: item.id,
                            Name: item.name,
                            Description: item.description,
                            Checked: checked
                        });
                    });
                    $("#list-roles").html(render);
                }
            })
        }
    }
}