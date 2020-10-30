var loginControllerJS = function () {
    this.initialize = function () {
        registerEvents();
    }

    var registerEvents = function () {
        $('#btnLogin').on('click', function (e) {
            e.preventDefault();
            var user = $('#txtUserName').val();
            var pass = $('#txtPassword').val();
            var rememberMe = $('#rememberMe').prop("checked") == true ? true : false;
            login(user, pass, rememberMe);
        })
    }

    var login = function (user, pass, rememberMe) {
        $.ajax({
            type: 'POST',
            data: {
                UserName: user,
                Password: pass,
                Remember: rememberMe
            },
            dataType: 'json',
            url: '/admin/login/authen',
            success: function (res) {
                if (res.success) {
                    customNotify("Đăng nhập thành công", types.success);
                } else {
                    customNotify("Đăng nhập không đúng", types.danger);
                }
            },
            error: function (err) {
                console.log(err);
            }
        })
    }
}