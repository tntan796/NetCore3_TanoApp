var productCategory = function () {
    this.initialize = function () {
        loadData();
        registerEvent();
    }

    function loadData() {
        $.ajax({
            method: 'get',
            url: '/admin/productcategory/getall',
            dataType: 'json',
            success: function (response) {
                var data = [];
                response.forEach(item => {
                    data.push({
                        id: item.id,
                        text: item.name,
                        parentId: item.parentId,
                        sortOrder: item.sortOrder
                    });
                });
                var treeArr = tano.unflattern(data);
                treeArr.sort(function (a, b) {
                    return a.sortOrder - b.sortOrder;
                });

                $('#treeProductCategory').tree({
                    data: treeArr,
                    dnd: true,
                    onContextMenu: function (e, node) {
                        e.preventDefault();
                        $("#hideIdMenu").val(node.id);
                        $("#contextMenu").menu('show',
                            {
                                left: e.pageX,
                                top: e.pageY
                            });
                    },
                    onDrop: function (target, source, point) {
                        var targetNode = $(this).tree('getNode', target);
                        if (point === 'append') {
                            var children = [];
                            $.each(targetNode.children, function (i, item) {
                                children.push({
                                    key: item.id,
                                    value: i
                                });
                            });
                            // Update to database
                            $.ajax({
                                url: '/admin/productcategory/updateParentId',
                                type: 'post',
                                data: {
                                    sourceId: source.id,
                                    targetId: targetNode.id,
                                    items: children
                                },
                                success: function (response) {
                                    customNotify("Update parent Id faile", types.success);
                                    loadData();
                                },
                                error: function (error) {
                                    customNotify("Update parent Id faile", types.danger);
                                    console.log(error);
                                }
                            });

                        } else if (point == 'top' || point == 'bottom') {
                            $.ajax({
                                url: '/admin/productcategory/reorder',
                                type: 'post',
                                data: {
                                    sourceId: source.id,
                                    targetId: targetNode.id
                                },
                                success: function (response) {
                                    customNotify("Order successful", types.success);
                                    loadData();
                                },
                                error: function (error) {
                                    customNotify("Re order fail", types.danger);
                                    console.log(error);
                                }
                            });
                        }
                    },

                });
            },
            error: function (error) {

            }
        })
    }

    function registerEvent() {
        $("#frmMaintainance").validate({
            errorClass: 'text-danger',
            ignore: [],
            lang: 'vi',
            rules: {
                txtNameM: { required: true },
                txtOrderM: { number: true },
                txtHomeOrderM: {number: true}
            },
            messages: {

            }
        });
        $("#btnCreate").off('click').on('click', function () {
            resetFormMaintainance();
            initTreeDropCategory();
            $("#modalAddEdit").modal("show");
        })
        $("body").on('click', '#btnEdit', function (e) {
            var id = $("#hideIdMenu").val();
            $.ajax({
                type: 'GET',
                url: '/admin/productcategory/getbyid',
                data: { id: id },
                dataType: 'json',
                beforeSend: function () {
                    tano.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $("#hideIdMenu").val(data.id);
                    $("#txtNameM").val(data.name);
                    initTreeDropCategory(data.categoryId);
                    $("#txtDescM").val(data.description);
                    $("#txtImageM").val(data.thumbnailImage);
                    $("#txtSeoKeywordM").val(data.seoKeywords);
                    $("#txtSeDescriptionM").val(data.seoDescription);
                    $("#txtSeoPageTitleM").val(data.seoPageTitle);
                    $("#txtSeoAliasM").val(data.seoAlias);
                    $("#ckStatusM").prop('checked', data.state == 1);
                    $("#ckShowHomeM").prop('checked', data.homeFlag)
                    $("#txtHomeOrderM").val(data.homeOrder);
                    $("#txtOrderM").val(data.sortOrder);

                    $("#modalAddEdit").modal('show');
                    tano.stopLoading();
                },
                error: function (status) {
                    tano.stopLoading();
                }
            })
        })

        $("body").on('click', '#btnDelete', function (e) {
            var id = $("#hideIdMenu").val();
            if (confirm("Bạn có chắc chắn muốn xóa?")) {
                $.ajax({
                    type: 'post',
                    url: '/admin/productcategory/delete',
                    data: { id: id },
                    beforeSend: function () {
                        tano.startLoading();
                    },
                    success: function (response) {
                        alert("xóa thành công");
                        tano.stopLoading();
                        loadData();
                    },
                    error: function (status) {
                        alert("xóa thất bại");
                        tano.stopLoading();
                    }
                })
            }
        })

        $("body").on("click", "#btnSave", function (e) {
            e.preventDefault();
            if ($("#frmMaintainance").valid()) {
                let id = $("#hideIdMenu").val();
                let name = $("#txtNameM").val();
                let parentId = $("#ddlCategoryIdM").combotree("getValue");
                let description = $("txtDescM").val();
                let image = $("#txtImageM").val();
                let order = $("#txtOrderM").val();
                let homeOrder = $("#txtHomeOrderM").val();
                let seoKeyWord = $("#txtSeoKeywordM").val();
                let seoMetaDescription = $("#txtSeoDescriptionM").val();
                let seoPageTitle = $("#txtSeoPageTitleM").val();
                let seoAlias = $("#txtSeoAliasM").val();
                let status = $("#ckStatusM").prop("checked") == true ? 1 : 0;
                let showHome = $("#ckShowHomeM").prop("checked");
                $.ajax({
                    type: "post",
                    url: "/admin/productcategory/saveentity",
                    data: {
                        Id: id,
                        Name: name,
                        Description: description,
                        ParentId: parentId,
                        HomeOrder: homeOrder,
                        Image: image,
                        HomeFlag: showHome,
                        SortOrder: order,
                        Status: status,
                        SeoPageTitle: seoPageTitle,
                        SeoAlias: seoAlias,
                        SeoKeywords: seoKeyWord,
                        SeoDescription: seoMetaDescription
                    },
                    dataType: 'json',
                    beforeSend: function () {
                        tano.startLoading();
                    },
                    success: function (response) {
                        alert("Thanh  cong");
                        $("#modalAddEdit").modal("hide");
                        resetFormMaintainance();
                        tano.stopLoading();
                        loadData(true);
                    },
                    error: function () {
                        alert("Cap nhat danh muc loi");
                        tano.stopLoading();
                    }
                });
            }
            return false;
        });

        $("#btnSelectImg").on("click", function () {
            $("#fileInputImage").click();
        })

        $("#fileInputImage").on("change", function () {
            var fileUpload = $(this).get(0);
            var files = fileUpload.files;
            var data = new FormData();
            for (var i = 0; i < files.length; i++) {
                data.append(files[i].name, files[i]);
            }
            $.ajax({
                type: "POST",
                url: "/admin/upload/uploadimage",
                contentType: false,
                processData: false,
                data: data,
                success: function (path) {
                    $("#txtImage").val(path);
                    customNotify("Upload image successful!", types.sucess);
                },
                error: function () {
                    customNotify("Upload image fail!", types.danger);
                }
            })
        })
    }

    function resetFormMaintainance() {
        $("#hideIdMenu").val(0);
        $("#txtNameM").val('');
        initTreeDropCategory('');
        $("#txtDescM").val('');
        $("#txtImageM").val('');
        $("#txtSeoKeywordM").val('');
        $("#txtSeDescriptionM").val('');
        $("#txtSeoPageTitleM").val('');
        $("#txtSeoAliasM").val('');
        $("#ckStatusM").prop('checked', true);
        $("#ckShowHomeM").prop('checked', false)
        $("#txtHomeOrderM").val('');
    }
    function initTreeDropCategory(selectedId) {
        $.ajax({
            url: "/admin/productcategory/getall",
            type: "get",
            dataType: "json",
            async: false,
            success: function (response) {
                var data = [];
                $.each(response, function (i, item) {
                    data.push({
                        id: item.id,
                        text: item.name,
                        parentId: item.parentId,
                        sortOrder: item.sortOrder
                    });
                });
                var arr = tano.unflattern(data);
                $("#ddlCategoryIdM").combotree({
                    data: arr
                });
                if (selectedId != undefined) {
                    $("#ddlCategoryIdM").combotree('setValue', selectedId);
                }
                $(".textbox.combo").css('width', '100%');
            }
        })
    }
}