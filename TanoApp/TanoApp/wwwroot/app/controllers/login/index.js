$(function () {
    $("#loginForm").validate({
        rules: {
            // The key name on the left side is the name attribute
            // of an input field. Validation rules are defined
            // on the right side
            username: "required",
            password: {
                required: true,
                minlength: 5
            }
        },
        messages: {
            username: "Please enter your firstname",
            password: {
                required: "Please provide a password",
                minlength: "Your password must be at least 5 characters long"
            }
        },
        // Make sure the form is submitted to the destination defined
        // in the "action" attribute of the form when valid
        submitHandler: function (form) {
            form.submit();
        }
    });
});


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