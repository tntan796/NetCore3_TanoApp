var productController = function () {
    this.initializer = function () {
        loadData();
        registerEvent();
    }

    function registerEvent() {
        $("#ddlShowPage").on('change', function () {
            tano.configs.pageSize = $(this).val();
            tano.configs.pageIndex = 1;
            loadData(true);
        });
    }

    function loadData(isPageChanged = false) {
        var template = $("#table-template").html();
        $.ajax({
            type: "get",
            data: {
                categoryId: null,
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