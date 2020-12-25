var billController = function () {
    var cacheObj = {
        products: [],
        colors: [],
        sizes: [],
        paymentMethods: [],
        billStatus: []
    };
    this.initialize = function () {
        $.when(
            loadPaymentMethod(),
            loadColors(),
            loadSizes(),
            loadStatus(),
            loadProducts()
        ).then(() => {
                loadData()
            }
        );
        registerEvent();
    }

    function loadStatus() {
        return $.ajax({
            method: 'get',
            url: '/admin/bill/getBillStatus',
            dataType: 'json',
            success: function (response) {
                cacheObj.billStatus = response;
                console.log('cacheObj:', cacheObj);
                var render = "";
                response.forEach(item => {
                    render += "<option value='" + item.value + "'>" + item.name + "</option>";
                });
                $("#ddlBillStatus").html(render);
            },
            error: function (error) {

            }
        });
    }

    function loadPaymentMethod() {
        return $.ajax({
            method: 'get',
            url: '/admin/bill/getPaymentMethod',
            dataType: 'json',
            success: function (response) {
                cacheObj.paymentMethods = response;
                console.log('cacheObj:', cacheObj);

                var render = "";
                response.forEach(item => {
                    render += "<option value='" + item.value + "'>" + item.name + "</option>";
                });
                $("#ddlPaymentMethod").html(render);
            },
            error: function (error) {

            }
        })
    }

    function loadColors() {
        return $.ajax({
            method: 'get',
            url: '/admin/bill/getcolors',
            dataType: 'json',
            success: function (response) {
                cacheObj.colors = response;
            },
            error: function (error) {

            }
        })
    }

    function loadSizes() {
        return $.ajax({
            method: 'get',
            url: '/admin/bill/getsizes',
            dataType: 'json',
            success: function (response) {
                cacheObj.sizes = response;
            },
            error: function (error) {

            }
        })
    }

    function loadProducts() {
        return $.ajax({
            method: 'get',
            url: '/admin/product/getall',
            dataType: 'json',
            success: function (response) {
                cacheObj.products = response;
            },
            error: function (error) {

            }
        })
    }

    function loadData(isPageChanged) {
        $.ajax({
            method: 'get',
            url: '/admin/bill/getAllPaging',
            dataType: 'json',
            data: {
                startDate: $('txtFromDate').val(),
                endDate: $('txtEndDate').val(),
                keyWord: $("txtKeyWord").val(),
                pageIndex: tano.configs.pageIndex,
                pageSize: tano.configs.pageSize
            },
            success: function (response) {
                var template = $("#table-template").html();
                var render = "";
                if (response.rowCount > 0) {
                    $.each(response.results, function (i, item) {
                        render += Mustache.render(template, {
                            CustomerName: item.customerName,
                            Id: item.id,
                            Description: item.description,
                            PaymentMethod: getPaymentMethodName(item.paymentMethod),
                            DateCreated: tano.dateTimeFormatJson(item.dateCreated),
                            BillStatus: getBillStatusName(item.billStatus)
                        });
                    });
                    $("#lblTotalRecords").text(response.RowCount);
                    if (render != undefined) {
                        $("#tbl-content").html(render);
                    }
                    wrapPaging(response.rowCount, function () {
                        loadData();
                    }, isPageChanged);
                } else {
                    $("#tbl-context").html("");
                }
            },
            error: function (error) {

            }
        })
    }

    function getProductOptions(selectedId) {
        var products = "<select class='form-control ddlProductId'>";
        $.each(cacheObj.products, function (i, product) {
            if (selectedId === product.id) {
                products += "<option value='" + product.id + "' selected='selected'>" + product.name + "</option>";
            } else {
                products += "<option value='" + product.id + "'>" + product.name + "</option>";
            }
        })
        products += "</select>";
        return products;
    }

    function getColorOptions(selectedId) {
        var colors = "<select class='form-control ddlColorId'>";
        $.each(cacheObj.colors, function (i, color) {
            if (selectedId === color.id) {
                colors += "<option value='" + color.id + "' selected='selected'>" + color.name + "</option>";
            } else {
                colors += "<option value='" + color.id + "'>" + color.name + "</option>";
            }
        })
        colors += "</select>";
        return colors;
    }

    function getSizeOptions(selectedId) {
        var sizes = "<select class='form-control ddlSizeId'>";
        $.each(cacheObj.sizes, function (i, size) {
            if (selectedId === color.id) {
                sizes += "<option value='" + size.id + "' selected='selected'>" + size.name + "</option>";
            } else {
                sizes += "<option value='" + size.id + "'>" + size.name + "</option>";
            }
        })
        sizes += "</select>";
        return sizes;
    }

    function registerEvent() {
        $('.datepicker').datepicker({
            format: 'mm/dd/yyyy',
            autoclose: true,
            todayHighlight: true,
            orientation: 'auto bottom'
        });

        $("#frmMaintainance").validate({
            errorClass: 'text-danger',
            ignore: [],
            lang: 'vi',
            rules: {
                txtName: { required: true },
                txtAddress: { required: true },
                txtPhone: { required: true },
                txtMessage: { required: true },
                ddlBillStatus: { required: true }
            }           
        });

        $("#btnCreate").off('click').on('click', function () {
            resetFormMaintainance();
            $("#billDetailModal").modal("show");
        })

        $("#txtKeyword").keypress(function (e) {
            if (e.which === 13) {
                e.preventDefault();
                loadData();
            }
        })

        $("#ddlShowPage").on("change", function () {
            tano.configs.pageSize = $(this).val();
            tano.configs.pageIndex = 1;
            loadData(true);
        })

        $("#btnSearch").on("click", function () {
            loadData();
        })

        $("body").on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var id = $(this).data("id");
            $.ajax({
                type: 'GET',
                url: '/admin/bill/getbyid',
                data: { id: id },
                dataType: 'json',
                beforeSend: function () {
                    tano.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $("#hideId").val(data.id);
                    $("#txtName").val(data.customerName);
                    $("#txtAddress").val(data.customerAddress);
                    $("#txtPhone").val(data.customerMobile);
                    $("#txtMessage").val(data.customerMessage);
                    $("#ddlPaymentMethod").val(data.paymentMethod);
                    $("#ddlBillStatus").val(data.billStatus);
                    $("#billDetailModal").modal('show');
                    var billDetails = data.billDetails;
                    if (data.billDetails !== null && data.billDetails.length > 0) {
                        var render = '';
                        var templateDetails = $("#template-table-bill-details").html();
                        billDetails.forEach(item => {
                            var products = getProductOptions(item.productId);
                            var colors = getColorOptions(item.colorId);
                            var sizes = getSizeOptions(item.sizeId);
                            render += Mustache.render(templateDetails, {
                                Id: item.id,
                                Products: products,
                                Colors: colors,
                                Sizes: sizes,
                                Quantity: item.quantity
                            });
                            $("#tblBillDetail").html(render);
                        })
                    }
                    tano.stopLoading();
                },
                error: function (status) {
                    tano.stopLoading();
                }
            })
        })

        $("body").on('click', '.btnDeleteDetail', function (e) {
            $(this).parent().parent().remove();
        })

        $("body").on("click", "#btnSave", function (e) {
            e.preventDefault();
            if ($("#frmMaintainance").valid()) {
                let id = $("#hideId").val();
                let name = $("#txtName").val();
                let address = $("#txtAddress").val();
                let phone = $("#txtPhone").val();
                let customerId = $("#ddlCustomerId").val();
                let message = $("#txtMessage").val();
                let paymentMethod = $("#ddlPaymentMethod").val();
                let billStatus = $("#ddlBillStatus").val();
                var billDetails = [];
                var detailsElm = $('#tblBillDetail tr');


                $.each(detailsElm, function (i, item) {
                    billDetails.push({
                        Id: $(item).data("id"),
                        ProductId: $(item).find("select.ddlProductId").first().val(),
                        Quantity: $(item).find("input.txtQuantity").first().val(),
                        ColorId: $(item).find("select.ddlColorId").first().val(),
                        SizeId: $(item).find("select.ddlSizeId").first().val(),
                        BillId: id
                    });
                })
                $.ajax({
                    type: "post",
                    url: "/admin/bill/saveentity",
                    data: {
                        Id: id,
                        BillStatus: billStatus,
                        CustomerAddress: address,
                        CustomerId: customerId,
                        CustomerMessage: message,
                        CustomerMobile: phone,
                        CustomerName: name,
                        PaymentMethod: paymentMethod,
                        Status: 1,
                        BillDetails: billDetails
                    },
                    dataType: 'json',
                    beforeSend: function () {
                        tano.startLoading();
                    },
                    success: function (response) {
                        alert("Thanh  cong");
                        $("#billDetailModal").modal("hide");
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

        $("#btnExport").on("click", function () {
            var id = $("#hideId").val();
            $.ajax({
                type: "post",
                url: "/admin/bill/exportExcel",
                data: { billId: id },
                success: function (response) {
                    window.location.ref = response;
                },
                error: function (error) {
                    console.log('error:', error);
                }
            })
        })

        $('#btnAddDetail').on("click", function () {
            var template = $("#template-table-bill-details").html();
            var products = getProductOptions(null);
            var colors = getColorOptions(null);
            var sizes = getSizeOptions(null);
            var render = Mustache.render(template, {
                Id: 0,
                Products: products,
                Colors: colors,
                Sizes: sizes,
                Quantity: 0,
                Total: 0
            });
            $("#tblBillDetail").append(render);
        })
    }

    function resetFormMaintainance() {
        $("#hideId").val(0);
        $("#txtName").val('');
        $("#txtAddress").val('');
        $("#txtPhone").val('');
        $("#txtMessage").val('');
    }

    function getPaymentMethodName(paymentMethod) {
        var method = $.grep(cacheObj.paymentMethods, function (element, index) {
            return element.value == paymentMethod;
        });
        if (method && method.length > 0)
            return method[0].name;
        else return '';
    }
    function getBillStatusName(status) {
        var status = $.grep(cacheObj.billStatus, function (element, index) {
            return element.value == status;
        });
        if (status && status.length > 0)
            return status[0].name;
        else return '';
    }

    function wrapPaging(recordCount, callBack, changePageSize) {
        var totalsize = Math.ceil(recordCount / tano.configs.pageSize);
        //Unbind pagination if it existed or click change pagesize
        if ($('#paginationUL a').length === 0 || changePageSize === true) {
            $('#paginationUL').empty();
            $('#paginationUL').removeData("twbs-pagination");
            $('#paginationUL').unbind("page");
        }
        //Bind Pagination Event
        $('#paginationUL').twbsPagination({
            totalPages: totalsize,
            visiblePages: 7,
            first: 'Đầu',
            prev: 'Trước',
            next: 'Tiếp',
            last: 'Cuối',
            onPageClick: function (event, p) {
                tano.configs.pageIndex = p;
                setTimeout(callBack(), 200);
            }
        });
    }
   
}