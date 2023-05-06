var isValid = false;
var user = '';

function ValidateCtrl(ctrlId, placeholder) {
    var type = $("#" + ctrlId + "").attr('type');
    var regex = '';
    switch (type) {
        case "email":
            regex = /^[\w\-\.\+]+\@[a-zA-Z0-9\.\-]+\.[a-zA-z0-9]{2,4}$/;
            break;
        case "text":
            regex = /^[a-zA-Z.& ]+$/;
            break;
        case "phone":
            regex = /^[\d\s]+$/;
            break;
        case "uri":
            regex = /^[\d\s]+$/;
            break;
        case "password":
            regex = /^[\d\s]+$/;
            break;
        case "captcha":
            regex = /^[a-zA-Z0-9]+$/;
            break;
        default:
            isValid = true;
            break;
    }

    if (regex != '') {
        if (regex.test($("#" + ctrlId + "").val()))
            isValid = true;
        else {
            $("#" + ctrlId + "").val('');
            if (placeholder == '' || placeholder == null || placeholder == undefined)
                $("#" + ctrlId + "").attr('placeholder', 'Please enter valid ' + ctrlId);
            else
                $("#" + ctrlId + "").attr('placeholder', 'Please enter valid ' + placeholder);
            isValid = false;
            $("#" + ctrlId + "").focus();
        }
    }
    else
        isValid = true;

    //if (ctrlId == "retypepassword") {
    //    if ($("#retypepassword").val() == "") {
    //        $("#" + ctrlId + "").attr('placeholder', 'Re-type password');
    //        isValid = false;
    //    }
    //    else {
    //        if ($("#password").val() != $("#retypepassword").val()) {
    //            $("#retypepassword").val('');
    //            $("#" + ctrlId + "").attr('placeholder', 'Password and re-type password must be same');
    //            isValid = false;
    //        }
    //        else
    //            isValid = true;
    //    }
    //}

    NotifyErrors(ctrlId);
}


function NotifyErrors(ctrlId) {
    ctrlId = ctrlId.toLowerCase();
    if (!isValid) {
        if (ctrlId == "name" || ctrlId == "surname" || ctrlId == "captcha")
            $("#div_" + ctrlId + "").attr("class", "col-sm-4 has-error has-feedback");
        else if (ctrlId == "companyname")
            $("#div_" + ctrlId + "").attr("class", "col-sm-8 has-error has-feedback");
        else
            $("#div_" + ctrlId + "").attr("class", "col-sm-8 has-error has-feedback");

        $("#span_" + ctrlId + "").attr("class", "glyphicon glyphicon-error form-control-feedback");
        $("#span_" + ctrlId + "").show();
    }
    else {
        if (ctrlId == "name" || ctrlId == "surname" || ctrlId == "captcha")
            $("#div_" + ctrlId + "").attr("class", "col-sm-4 has-success has-feedback");
        else
            $("#div_" + ctrlId + "").attr("class", "col-sm-8 has-success has-feedback");

        $("#span_" + ctrlId + "").attr("class", "glyphicon glyphicon-ok form-control-feedback");
        $("#span_" + ctrlId + "").show();
    }
}

function ValidateLists(ctrlId) {
    if ($("#" + ctrlId + "").val() == "0") {
        $("#" + ctrlId + "").attr('style', 'border-color:#a94442;');
        isValid = false;
    }
    else {
        $("#" + ctrlId + "").attr('style', 'border-color:#DDDDDD;');
        isValid = true;
    }
}


function VerifyForm() {
    if ($("#name").val() == "") {
        ValidateCtrl("name");
    }

    if ($("#surname").val() == "") {
        ValidateCtrl("surname");
    }

    if ($("#emailId").val() == "") {
        ValidateCtrl("emailId");
    }

    if ($("#Gender").val() == "0") {
        ValidateLists("Gender");
    }

    if ($("#Country").val() == "0") {
        ValidateLists("Country");
    }

    if (user == 'employer') {
        if ($("#companyname").val() == "") {
            ValidateCtrl("companyname");
        }

        if ($("#designation").val() == "") {
            ValidateCtrl("designation");
        }

        //if ($("#retypepassword").val() == "") {
        //    ValidateCtrl("retypepassword");
        //}
    }

    if ($("#captcha").val() == "") {
        ValidateCtrl("captcha");
    }

    if (!isValid) {
        $("[name='msgStatus']").hide();
        $("#msgAlertSignUp").attr("class", "alert alert-danger alert-dismissible");
        $("#msgAlertSignUp").html("Highlighted information are invalid. Please provide the valid information and then try again!");
        $("#msgAlertSignUp").show();
    }
}


function ValidateForm() {
    isValid = true;
    VerifyForm();

    if (isValid) {
        if (!$("#agreed").prop('checked')) {
            $("[name='msgStatus']").hide();
            $("#msgAlertSignUp").show();
            $("#msgAlertSignUp").attr("class", "alert alert-danger alert-dismissible");
            $("#msgAlertSignUp").html("Please accept the terms and condition to register.");

            isValid = false;
        }
    }
}


$("#formSignUp").submit(function () {
    user = 'talent';
    ValidateForm();

    if (isValid) {

    }
    else
        return false;
});


$("#formRegister").submit(function () {
    user = 'employer';
    ValidateForm();

    if (isValid) {

    }
    else
        return false;
});


function LoginToProfile() {
    $('#msgAlert').html('');
    $('#msgAlert').hide();

    $.ajax({
        url: '/Login',
        type: 'POST',
        data: { "email": $("#email").val(), "pwd": $("#password").val(), rememberMe: $('[name="rememberMe"]').prop('checked') },
        datatype: "json",
        contenttype: "application/json; charset=utf-8",
        success: function (result) {
            if (result.Status) {
                sessionStorage.setItem("talentsprofileUser", JSON.stringify(result.Token));
                window.location = document.location.origin + "/" + result.Url;
            }
            else {
                $('#msgAlert').html(result.Message);
                $('#msgAlert').show();
                $('#password').val('');
            }
        },
        error: function () {
        }
    });
}

function ValidateEmail() {
    isValid = false;
    regex = /^[\w\-\.\+]+\@[a-zA-Z0-9\.\-]+\.[a-zA-z0-9]{2,4}$/;
    if (regex != '') {
        if (regex.test($("#pwdemail").val())) {
            isValid = true;
            $("#div_pwdemail").attr("class", "col-sm-8 has-success has-feedback");
            $("#span_pwdemail").attr("class", "glyphicon glyphicon-ok form-control-feedback");
            $("#span_pwdemail").show();
        }
        else {
            isValid = false;
            $("#pwdemail").val('');
            $("#pwdemail").attr('placeholder', 'Please enter valid email');
            $("#pwdemail").focus();

            $("#div_pwdemail").attr("class", "col-sm-8 has-error has-feedback");
            $("#span_pwdemail").attr("class", "glyphicon glyphicon-error form-control-feedback");
            $("#span_pwdemail").show();
        }
    }
    else
        isValid = false;
}

$("#btnForgotPassword").click(function () {
    //ValidateEmail();
    if (isValid) {
        $.ajax({
            url: '/ForgotPassword',
            type: 'POST',
            data: { "email": $("#pwdemail").val() },
            datatype: "json",
            contenttype: "application/json; charset=utf-8",
            success: function (result) {
                if (result.Status) {
                    //$('#fpBody').html('');
                    $('#divUsrMsg').attr('class', 'alert alert-success');
                    $('#divUsrMsg').html(result.Message);

                }
                else {
                    $('#divUsrMsg').attr('class', 'alert alert-warning');
                    $('#divUsrMsg').html(result.Message);
                }

                $('#divUsrMsg').show();
            },
            error: function () {
            }
        });
    }
    else
        return false;
});

$("#formSetPassword").submit(function () {
    isValid = false;
    var regex = /^[\d\s]+$/;
    $('#uac').val(window.location.search);

    if ($("#setpassword").val() == "") {
        isValid = false;
        $("#msgAlertSetPwd").html("Please choose a password.");
    }
    else {
        if (regex.test($("#setpassword").val())) {
            isValid = true;
        }
    }

    if ($("#retypepassword").val() == "") {
        isValid = false;
        $("#msgAlertSetPwd").html("Please re-type password.");
    }
    else {
        if ($("#setpassword").val() != $("#retypepassword").val()) {
            //$("#retypepassword").val('');
            //$("#" + ctrlId + "").attr('placeholder', 'Password and re-type password must be same');
            $("#msgAlertSetPwd").html("Password and re-type password must be same.");
            isValid = false;
        }
        else
            isValid = true;
    }

    if (isValid) {
    }
    else {
        $("#msgAlertSetPwd").show();
        $("#msgAlertSetPwd").attr("class", "alert alert-danger alert-dismissible");
        return false;
    }
});

