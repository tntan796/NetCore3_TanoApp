var WholePriceProduct = function () {
    this.initializer = function () {
        registerEvent();
    }

    function registerEvent() {
        $("body").on("click", ".btn-whole-price", function (e) {
            e.preventDefault();
            $("#modalWholePrice").modal("show");
            var id = $(this).data("id");
            $("#hideId").val(id);
            $.ajax({
                type: "get",
                url: '/admin/product/GetWholePrice',
                data: {
                    productId: id
                },
                dataType: "json",
                success: function (response) {
                    var template = $("#template-table-whole-price").html();
                    var render = '';
                    $.each(response, function (i, item) {
                        render += Mustache.render(template, {
                            Id: item.id,
                            From: item.fromQuantity,
                            To: item.toQuantity,
                            Price: item.price
                        });
                    });
                    $("#table-whole-price-content").html(render);
                },
                error: function (error) {
                    console.log('Get error: ', error);
                }
            })
        })

        $("body").on("click", "#btn-add-whole-price", function (e) {
            e.preventDefault();
            var template = $("#template-table-whole-price").html();
            var render = '';
            render += Mustache.render(template, {
                Id: 0,
                From: 0,
                To: 0,
                Price: 0
            });
            $("#table-whole-price-content").append(render);
        });

        $("body").on("click", ".btn-delete-whole-price", function (e) {
            e.preventDefault();
            $(this).closest("tr").remove();
        })

        $("body").on("click", "#btnSaveWholePrice", function (e) {
            e.preventDefault();
            var wholePriceList = [];
            $.each($("#table-whole-price-content").find("tr"), function (i, item) {
                wholePriceList.push({
                    Id: $(item).data("id"),
                    ProductId: $("#hideId").val(),
                    ToQuantity: $(item).find('input.txtTo').first().val(),
                    FromQuantity: $(item).find('input.txtFrom').first().val(),
                    Price: $(item).find('input.txtPrice').first().val()
                });
            });
            $.ajax({
                type: "post",
                dataType: "json",
                url: "/admin/product/AddWholePrice",
                data: {
                    productId: $("#hideId").val(),
                    wholePrices: wholePriceList
                },
                success: function (response) {
                    customNotify("Save Successful", types.success);
                    $("#modalWholePrice").show("hide");
                },
                error: function (error) {
                    customNotify("Save Fail", types.danger);
                    console.log("Error when click btnSaveWholePrice: ", error);
                }
            })
        })
    }
}