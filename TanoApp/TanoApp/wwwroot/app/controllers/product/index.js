var productController = function () {
    this.initializer = function () {
        loadCategories();
        loadData();
        registerEvent();
    }

    function registerEvent() {
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
                    //CKEDITOR.instances.txtContentM.setData('');
                    $("#ckStatusM").prop("checked", data.status == 0);
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

        $("body").on("click", ".btn-edit", function (e) {
            e.preventDefault();
            var that = $(this).data("id");
            if (confirm("Ban co chac chan muon xoa")) {
                $.ajax({
                    type: "post",
                    url: "/admin/product/delete",
                    data: { id: that },
                    dataType: "json",
                    beforeSend: function () {
                        tano.startLoading();
                    },
                    success: function (response) {
                        alert("Xoa thanh cong");
                        tano.stopLoading();
                        loadData();
                    },
                    error: function (error) {
                        alert("Xoa that bai");
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
        //CKEDITOR.instances.txtContentM.setData('');
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