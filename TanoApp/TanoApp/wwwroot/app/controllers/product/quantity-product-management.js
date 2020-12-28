var QuantityProductManagerment = function () {
    var self = this;

    var cachedObj = {
        colors: [],
        sizes: []
    };

    this.initializer = function () {
        loadColors();
        loadSizes();
        registerEvent();
    }

    function registerEvent() {
        $("body").on("click", ".btn-quantity-management", function (e) {
            e.preventDefault();
            var id = $(this).data("id");
            $("#hideId").val(id);
            loadQuantities();
            $("#modalQuantityProductManagerment").modal("show");
        });

        $("body").on("click", ".btn-delete-quantity", function (e) {
            e.preventDefault();
            $(this).closest("tr").remove();
        });


        $("#btn-add-quantity").on("click", function () {
            var template = $("#template-table-quantity").html();
            var render = Mustache.render(template, {
                Id: 0,
                Colors: getColorOptions(null),
                Sizes: getSizeOptions(null),
                Quantity: 0
            });
            $("#table-quantity-content").append(render);
        });

        $("#btnSaveQuantity").on("click", function () {
            var quantityList = [];
            $.each($("#table-quantity-content").find("tr"), function (i, item) {
                quantityList.push({
                    Id: $(item).data("id"),
                    ProductId: $("#hideId").val(),
                    Quantity: $(item).find('input.txtQuantity').first().val(),
                    SizeId: $(item).find('select.ddlSizeId').first().val(),
                    ColorId: $(item).find('select.ddlColorId').first().val()
                });
            });
            $.ajax({
                url: "/admin/product/SaveQuantities",
                data: {
                    productId: $("#hideId").val(),
                    quantities: quantityList
                },
                type: 'post',
                dataType: 'json',
                success: function (response) {
                    $("#modalQuantityProductManagerment").modal("hide");
                    $("#table-quantity-content").html("");
                }
            })
        })
    }

    function loadQuantities() {
        $.ajax({
            url: '/admin/product/getquantities',
            data: {
                productId: $("#hideId").val()
            },
            type: "get",
            dataType: "json",
            success: function (response) {
                var render = '';
                var template = $("#template-table-quantity").html();
                $.each(response, function (i, item) {
                    render += Mustache.render(template, {
                        Id: item.id,
                        Colors: getColorOptions(item.colorId),
                        Sizes: getSizeOptions(item.sizeId),
                        Quantity: item.quantity
                    });
                });
                $("#table-quantity-content").html(render);
            }
        })
    }

    function loadColors() {
        return $.ajax({
            type: "get",
            url: "/admin/bill/getcolors",
            dataType: "json",
            success: function (response) {
                cachedObj.colors = response;
            },
            error: function (err) {
                tano.notify("Load colors error");
                console.log("Loading color:", err);
            }
        })
    }

    function loadSizes() {
        return $.ajax({
            type: 'get',
            url: '/admin/bill/getsizes',
            dataType: 'json',
            success: function (response) {
                cachedObj.sizes = response;
            },
            error: function (err) {
                tano.notify("Loading size fail");
                console.log("Loading size fail: ", err);
            }
        })
    }

    function getColorOptions(selectedId) {
        var colors = "<select class='form-control ddlColorId'>";
        $.each(cachedObj.colors, function (i, color) {
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
        $.each(cachedObj.sizes, function (i, size) {
            if (selectedId === color.id) {
                sizes += "<option value='" + size.id + "' selected='selected'>" + size.name + "</option>";
            } else {
                sizes += "<option value='" + size.id + "'>" + size.name + "</option>";
            }
        })
        sizes += "</select>";
        return sizes;
    }

}
