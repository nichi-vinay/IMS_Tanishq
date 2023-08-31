
$(function () {   
   
    $('#loginForm').validate({
        rules: {
            Email: {
                required: true,
                email: true,
            },
            Password: {
                required: true, 
                minlength: 8
            }
        },
        messages: {
            Email: {
                required: "Please enter a email address",
                email: "Please enter a vaild email address"
            },
            Password: {
                required: "Please provide a password",
                minlength: "Your password must be at least 8 characters long"
            }
        },
        errorElement: 'span',
        errorPlacement: function (error, element) {
            error.addClass('invalid-feedback');
            element.closest('.input-group').append(error);
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass('is-invalid');
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass('is-invalid');
        }
    });
    loginFormSubmit();
});
function loaderDiv() {
    return '<div class="overlay">' +
        '<i class="fas fa-2x fa-sync-alt fa-spin"></i>' +
        '</div>';
}
function loginFormSubmit() {
    $("#loginForm").submit(function (e) {
        if (!($('#Email').hasClass('is-invalid') || $('#Password').hasClass('is-invalid'))) {
            var model = {};
            model.Email = $('#Email').val();
            model.Password = $('#Password').val();
            e.preventDefault();
            $('#loginCard').append(loaderDiv());
            $.ajax({
                url: '/Login/LoginValidation',
                type: "POST",
                data: '{model:' + JSON.stringify(model) + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result.data == 'Redirect') {
                        window.location = result.url;
                    }
                    else {
                        toastr.error("Invalid user credentials. Please try again.");
                        $("#loginForm")[0].reset();
                        $('#loginCard')[0].removeChild($('#loginCard')[0].lastChild);
                    }
                },
                error: function () {
                    $('#loginCard')[0].removeChild($('#loginCard')[0].lastChild);
                }

            });
        }
    });
}