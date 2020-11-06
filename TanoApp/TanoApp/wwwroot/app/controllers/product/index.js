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