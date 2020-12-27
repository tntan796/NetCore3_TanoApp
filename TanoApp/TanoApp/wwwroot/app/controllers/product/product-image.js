var ImageProduct = function () {
    var self = this;

    var images = [];
    this.initializer = function () {
        registerEvent();
    }

    function registerEvent() {
        $("body").on("click", ".btn-image-management", function (e) {
            e.preventDefault();
            var id = $(this).data("id");
            $("#hideId").val(id);
            clearFileInput($("#fileImage"));
            loadImages();
            $("#product-image-modal").modal("show");
        });

        $("body").on("click", ".btn-delete-image", function (e) {
            e.preventDefault();
            $(this).closest("div").remove();
        })

        $("#fileImage").on("change", function () {
            var fileUpload = $(this).get(0);
            var files = fileUpload.files;
            var data = new FormData();
            for (var i = 0; i < files.length; i++) {
                data.append(files[i].name, files[i]);
            }
            $.ajax({
                type: "Post",
                url: "/admin/upload/uploadImage",
                contentType: false,
                processData: false,
                data: data,
                success: function (path) {
                    clearFileInput($("#fileImage"));
                    images.push(path);
                    $("#image-list").append("<div class='col-md-3'><img width='100' data-path='" + path + "' src='" + path + "'/></div>")
                    alert("thanh cong");
                },
                error: function (err) {
                    console.log(err);
                    alert("Loi roi");
                }
            })
        })

        $("#btnSaveImages").on("click", function () {
            var imageList = [];
            $.each($("#image-list").find("img"), function (i, item) {
                imageList.push($(this).data("path"));
            });
            $.ajax({
                url: '/admin/product/AddImages',
                data: {
                    productId: $("#hideId").val(),
                    images: images
                },
                type: 'post',
                dataType: 'json',
                success: function (respones) {
                    $("#product-image-modal").modal("hide");
                    $("#image-list").html("");
                    clearFileInput($("#fileImage"));
                }
            })
        })

        function loadImages() {
            $.ajax({
                url: '/admin/product/GetImages',
                data: {
                    productId: $("hideId").val()
                },
                type: 'get',
                dataType: 'json',
                success: function (response) {
                    var render = '';
                    $.each(response, function (i, item) {
                        render += '<div class="col-md-3"> <img width="100" src="' + item.path + '"/> <br/><a href="#" class="btn btn-delete-iamge btn-danger">Delete</a> </div>'
                    })
                }
            })
        }

        function clearFileInput(ctrl) {
            try {
                ctrl.value = null;
            } catch (ex) {
            }
            if (ctrl.value) {
                ctrl.parentNode.replaceChild(ctrl.cloneNode(true), ctrl);
            }
        }

    }
}