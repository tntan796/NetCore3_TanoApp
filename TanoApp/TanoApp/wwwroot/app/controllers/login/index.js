var loginController = function () {
    this.initialize = function () {

    }

    var registerEvents = function () {
        $('#btnLogin').on('click', function (e) {
            e.preventDefault();
            var user = $('#txtUserName');
            var pass = $('#txtPassword');

        })
    }

    var login = function (user, pass) {
        $.ajax({
            type: 'POST',
            data: {
                userName: user,
                passWord: pass
            },
            dataType: 'json',
            url: '/admin/login/authen',
            sucess: function (res) {
                if (res.sucess) {
                    window.location.href = "/Admin/Home/Index";
                } else {
                    alert("Đăng nhập không đúng");
                }
            }
        })
    }
}