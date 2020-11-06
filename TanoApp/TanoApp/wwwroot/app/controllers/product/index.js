var productController = function () {
    this.initializer = function () {
        loadData();
    }

    function registerEvent() {

    }

    function loadData() {
        var template = $("#table-template").html();
        $.ajax({
            type: "get",
            url: "/admin/product/getall",
            datatype: "json",
            success: function (result) {
                Mustache.parse(template);
                result.forEach((item) => {
                    let rendered = Mustache.render(template, item);
                    $("#tbl-body").append(rendered);
                });
            },
            error: function (error) {
                customNotify("Get data fail", types.danger);
                console.log(error);
            }
        })
    }
}