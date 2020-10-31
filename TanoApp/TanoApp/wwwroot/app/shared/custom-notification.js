/*--------------------------------------
        Notifications & Dialogs
    ---------------------------------------*/
/*
    * Notifications
    */
var types = {
    success: 'success',
    warning: 'warning',
    info: 'info',
    inverse: 'inverse',
    danger: 'danger'
};
var customNotify = function (message = '', type = types.success) {
    $.growl({
        icon: undefined,
        title: undefined,
        message: message,
        url: ''
    },
    {
        element: 'body',
        type: type,
        allow_dismiss: true,
        placement: {
            from: undefined,
            align: undefined
        },
        offset: {
            x: 30,
            y: 30
        },
        spacing: 10,
        z_index: 999999,
        delay: 2500,
        timer: 1000,
        url_target: '_blank',
        mouse_over: false,
        animate: {
            enter: undefined,
            exit: undefined
        },
        icon_type: 'class',
        template: '<div data-growl="container" class="alert" role="alert">' +
            '<button type="button" class="close" data-growl="dismiss">' +
            '<span aria-hidden="true">&times;</span>' +
            '<span class="sr-only">Close</span>' +
            '</button>' +
            '<span data-growl="icon"></span>' +
            '<span data-growl="title"></span>' +
            '<span data-growl="message"></span>' +
            '<a href="#" data-growl="url"></a>' +
            '</div>'
    });
};