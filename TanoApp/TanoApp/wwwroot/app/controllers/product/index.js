var productController = function () {
    this.initializer = function () {
        loadCategories();
        loadData();
        registerEvent();
        registerControls();
    }

    function registerControls() {
        CKEDITOR.replace('txtContent', {});
        $.fn.modal.Constructor.prototype.enforceFocus = function () {
            $(document)
                .off('focusin.bs.modal')// Guard agains infinity focus loop 
                .on('focusin.bs.modal', $.proxy(function (e) {
                    if (this.$element[0] !== e.target && !this.$element.has(e.target).length
                        // CKEditor compatibility fix start
                        && !$(e.target).closest('.cke_dialog, .cke').length
                    ) {
                        this.$element.trigger('focus');
                    }
                }, this));
        };
    }

    function registerEvent() {
        $("#frmMaintainance").validate({
            rules: {
                // The key name on the left side is the name attribute
                // of an input field. Validation rules are defined
                // on the right side
                txtNameM: "required",
                ddlCategoryIdM: {
                    required: true,
                },
                txtPriceM: {
                    required: true,
                    number: true
                }
            },
            messages: {
                txtNameM: "Vui lòng nhập tên",
                ddlCategoryIdM: {
                    required: "Vui lòng chọn Danh mục",
                },
                txtPriceM: {
                    required: "Vui lòng nhập giá",
                    number: "Vui lòng nhập giá là số"
                }
            },
            // Make sure the form is submitted to the destination defined
            // in the "action" attribute of the form when valid
            submitHandler: function (form) {
                form.submit();
            }
        });

        $("#ddlShowPage").on('change', function () {
            tano.configs.pageSize = $(this).val();
            tano.configs.pageIndex = 1;
            loadData(true);
        });

        $("#ddlCategorySearch").on('change', function () {
            loadData(true);
        });

        $("#btnSearch").on('click', function () {
            loadData(true);
        });
        $("#txtKeyword").on('keypress', function (e) {
            if (e.which == 13) {
                loadData(true);
            }
        });
        $("#btnCreate").on("click", function () {
            resetFormMaintainance();
            initTreeDropCategory();
            $("#modalAddEdit").modal("show");
        })

        $("body").on("click", ".btn-edit", function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/admin/product/getById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    tano.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $("#hidIdM").val(data.id);
                    $("#txtNameM").val(data.name);
                    initTreeDropCategory(data.categoryId);
                    $("#txtDescM").val(data.description);
                    $("#txtUnitM").val(data.unit);
                    $("#txtPriceM").val(data.price);
                    $("#txtOriginalPriceM").val(data.originalPrice);
                    $("#txtPromotionPriceM").val(data.promotionPrice);
                    //$("#txtImageM").val(data.thumbailImage);
                    $("#txtTagM").val(data.tags);
                    $("#txtMetakeywordM").val(data.seoKeywords);
                    $("#txtMetaDescriptionM").val(data.seoDescription);
                    $("#txtSeoPageTitleM").val(data.seoPageTitle);
                    $("#txtSeoAliasM").val(data.seoAlias);
                    CKEDITOR.instances.txtContent.setData('');
                    $("#ckStatusM").prop("checked", data.status == 1);
                    $("#ckHotM").prop("checked", data.hotFlag);
                    $("#ckShowHomeM").prop("checked", data.homeFlag);
                    $("#modalAddEdit").modal("show");
                    tano.stopLoading();
                },
                error(error) {
                    tano.stopLoading();
                    console.log("Get product id faild:", error);
                }
            })
        })

        $("body").on("click", ".btn-delete", function (e) {
            e.preventDefault();
            var that = $(this).data("id");
            if (confirm("Bạn có chắc chắn muốn xóa không?")) {
                $.ajax({
                    type: "delete",
                    url: "/admin/product/delete",
                    data: { id: that },
                    dataType: "json",
                    beforeSend: function () {
                        tano.startLoading();
                    },
                    success: function (response) {
                        alert("Xóa thành công!");
                        tano.stopLoading();
                        loadData();
                    },
                    error: function (error) {
                        alert("Xóa thất bại");
                        tano.stopLoading();
                    }
                })
            }
        })

        $("#btnSave").on("click", function (e) {
            if ($("#frmMaintainance").valid()) {
                e.preventDefault();
                var id = $("#hidIdM").val();
                var name = $("#txtNameM").val();
                var categoryId = $("#ddlCategoryIdM").combotree('getValue');
                var description = $("#txtDescM").val();
                var unit = $("#txtUnitM").val();
                var price = $("#txtPriceM").val();
                var originalPrice = $("#txtOriginalPriceM").val();
                var promotionPrice = $("#txtPromotionPriceM").val();
                var image = $("#txtImageM").val();
                var tags = $("#txtTagM").val();
                var seoKeyword = $("#txtMetakeywordM").val();
                var seoMetaDescription = $("#txtMetaDescriptionM").val();
                var seoPageTitle = $("#txtSeoPageTitleM").val();
                var seoAlias = $("#txtSeoAliasM").val();
                var content = CKEDITOR.instances.txtContent.getData();
                var status = $("#ckStatusM").prop("checked") == true ? 1 : 0;
                var hot = $("#ckHotM").prop("checked");
                var showHome = $("#ckShowHomeM").prop("checked");
                $.ajax({
                    type: "POST",
                    url: "/admin/product/saveEntity",
                    data: {
                        Id: id,
                        CategoryId: categoryId,
                        Name: name,
                        Image: '',
                        Price: price,
                        OriginalPrice: originalPrice,
                        PromotionPrice: promotionPrice,
                        Description: description,
                        Content: content,
                        HomeFlag: showHome,
                        HotFlag: hot,
                        Tags: tags,
                        Unit: unit,
                        Status: status,
                        SeoPageTitle: seoPageTitle,
                        SeoAlias: seoAlias,
                        SeoKeywords: seoKeyword,
                        SeoDescription: seoMetaDescription
                    },
                    dataType: "JSON",
                    beforeSend: function () {
                        tano.startLoading();
                    },
                    success: function (response) {
                        alert('Cập nhật thành công');
                        resetFormMaintainance();
                        tano.stopLoading();
                        loadData(true);
                        $('#modal-add-edit').modal('hide');
                    },
                    error: function (e) {
                        console.log('Cập nhật thất bại: ', e);
                        alert("Cập nhật thất bại");
                        tano.stopLoading();
                    }
                })
            }
        })
    }

    function resetFormMaintainance() {
        $("#hidIdM").val(0);
        $("#txtNameM").val('');
        initTreeDropCategory('');
        $("#txtDescM").val('');
        $("#txtUnitM").val('');
        $("#txtPriceM").val(0);
        $("#txtOriginalPriceM").val('');
        $("#txtPromotionPriceM").val('');
        //$("#txtImageM").val('');
        $("#txtTagM").val('');
        $("#txtMetakeywordM").val('');
        $("#txtMetaDescriptionM").val('');
        $("#txtSeoPageTitleM").val('');
        $("#txtSeoAliasM").val('');
        CKEDITOR.instances.txtContent.setData('');
        $("#ckStatusM").prop("checked", true);
        $("#ckHotM").prop("checked", false);
        $("#ckShowHomeM").prop("checked", false);
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

    function loadCategories() {
        $.ajax({
            type: "get",
            url: "/admin/product/getallcategories",
            datatype: "json",
            success: function (result) {
                var render = "<option value=''>--Chọn danh mục --</option>";
                result.forEach(item => {
                    render += "<option value='" + item.id + "'>" + item.name + "</option>";
                });
                $("#ddlCategorySearch").html(render);
            },
            error: function (error) {
                customNotify("Get data fail categories", types.danger);
                console.log(error);
            }
        });
    }

    function loadData(isPageChanged = false) {
        var template = $("#table-template").html();
        $.ajax({
            type: "get",
            data: {
                categoryId: $('#ddlCategorySearch').val(),
                keyword: $('#txtKeyword').val(),
                page: tano.configs.pageIndex,
                pageSize: tano.configs.pageSize
            },
            url: "/admin/product/getallpaging",
            datatype: "json",
            success: function (result) {
                $("#tbl-body").html('');
                Mustache.parse(template);
                result.results.forEach((item) => {
                    item.isActive = item.status == 1;
                    item.status = item.status == 1 ? 'Hoạt động' : 'Không hoạt động';
                    let rendered = Mustache.render(template, item);
                    $("#tbl-body").append(rendered);
                });
                $("#lblTotalRecords").text(result.rowCount);
                wrapPaging(result.rowCount, function () {
                    loadData();
                }, isPageChanged);
            },
            error: function (error) {
                customNotify("Get data fail", types.danger);
                console.log(error);
            }
        });
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
}