
//start jQuery
$(document).ready(() => {
    $(document).ajaxStart(() => {
        $('#loading-gif').addClass('show');
    }).ajaxStop(() => {
        $('#loading-gif').removeClass('show');
    });

    //===================login into system======================//
    $('#loginForm').submit((e) => {

        e.preventDefault();

        var response = validateLogin();

        if (response === false) {
            return false;
        }
        else {

            const data = {
                Email: $('#email').val(),
                Password: $('#password').val()
            };

            $.ajax({
                url: '/Auth/LogIn',
                type: 'POST',
                data: JSON.stringify(data),
                contentType: "application/json;charset=UTF-8",
                processData: false,
                dataType: "JSON",
                success: (result) => {
                    if (result.success) {

                        toastr.success(result.message, "Sever Response !");

                        // Optionally redirect to another page
                        window.location.href = "/Home/Index";

                    } else {
                        toastr.error(result.message, "Sever Response !");
                    }
                },
                error: (err) => {
                    toastr.error(err.message, "Sever Response !");
                }
            });
        }
    });

    //===================validate control login======================//
    const validateLogin = () => {

        var isValid = true;

        if ($('#email').val() === "") {
            toastr.error("Please Input the Email Address", "Server Resonse");
            $('#email').css('border-color', 'red');
            $('#email').focus();
            isValid = false;
        } else {
            $('#email').css('border-color', '#cccccc');
            if ($('#password').val() === "") {
                toastr.error("Please Input the Password", "Server Resonse");
                $('#password').css('border-color', 'red');
                $('#password').focus();
                isValid = false;
            } else {
                $('#password').css('border-color', '#cccccc');

            }
        }
        return isValid;
    }

    //===================register user account======================//
    $('#registerForm').submit(function (e) {

        e.preventDefault();

        var response = validateRegister();

        if (response === false) {
            return false;
        }
        else {
            var formData = new FormData(this);

            $.ajax({
                url: '/Auth/Register',
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                success: (result) => {
                    if (result.success) {
                        toastr.success(result.message, "Sever Response !");
                        // Optionally redirect to another page
                        //window.location.href = "/Auth/LogIn";
                        clear();
                    } else {
                        toastr.error(result.message, "Sever Response !");
                    }
                },
                error: (err) => {
                    toastr.error(err.message, "Sever Response !");
                }
            });
        }
    });

    //===================clear control==============================//
    const clear = () => {
        $("#username").val("");
        $("#email").val("");
        $("#gender").val("Male");
        $("#phone").val("");
        $("#password").val("");
        $("#address").val("");
        $("#imageFile").val("");
        $("#image").attr('src', '../Images/blank-image.png');
    }

    //================validate control register====================//
    const validateRegister = () => {
        let isValid = true;
        if ($('#username').val() === "") {
            toastr.info("Please Input the Username", "Server Resonse");
            $('#username').css('border-color', 'red');
            $('#username').focus();
            isValid = false;
        } else {
            $('#username').css('border-color', '#cccccc');
            if ($('#email').val() === "") {
                toastr.error("Please Input the Email Address", "Server Resonse");
                $('#email').css('border-color', 'red');
                $('#email').focus();
                isValid = false;
            } else {
                $('#email').css('border-color', '#cccccc');
                if ($('#password').val() === "") {
                    toastr.error("Please Input the Password", "Server Resonse");
                    $('#password').css('border-color', 'red');
                    $('#password').focus();
                    isValid = false;
                } else {
                    $('#password').css('border-color', '#cccccc');

                }
            }
        }
        return isValid;
    }

    //end jQuery
});

