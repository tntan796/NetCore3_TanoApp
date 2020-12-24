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
            loadProducts()
        ).then(() => {
                loadData()
                registerEvent();
            }
        );
    }

    function loadPaymentMethod() {
        $.ajax({
            method: 'get',
            url: '/admin/bill/getall',
            dataType: 'json',
            success: function (response) {

            },
            error: function (error) {

            }
        })
    }

    function loadColors() {
        $.ajax({
            method: 'get',
            url: '/admin/bill/getall',
            dataType: 'json',
            success: function (response) {

            },
            error: function (error) {

            }
        })
    }

    function loadSizes() {
        $.ajax({
            method: 'get',
            url: '/admin/bill/getall',
            dataType: 'json',
            success: function (response) {

            },
            error: function (error) {

            }
        })
    }

    function loadProducts() {
        $.ajax({
            method: 'get',
            url: '/admin/bill/getall',
            dataType: 'json',
            success: function (response) {

            },
            error: function (error) {

            }
        })
    }

    function loadData() {
        $.ajax({
            method: 'get',
            url: '/admin/bill/getall',
            dataType: 'json',
            success: function (response) {
              
            },
            error: function (error) {

            }
        })
    }

    function registerEvent() {
        $("")

        $("#frmMaintainance").validate({
            errorClass: 'text-danger',
            ignore: [],
            lang: 'vi',
            rules: {
                txtNameM: { required: true },
                txtOrderM: { number: true },
                txtHomeOrderM: { number: true }
            },
            messages: {

            }
        });
        $("#btnCreate").off('click').on('click', function () {
            resetFormMaintainance();
            $("#billDetailModal").modal("show");
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

                    $("#billDetailModal").modal('show');
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
   
}