$('#btnSubscribe').click(function () {
    var regex = /^[\w\-\.\+]+\@[a-zA-Z0-9\.\-]+\.[a-zA-z0-9]{2,4}$/;
    var emailId = $('#txtUserEmail').val();
    if (emailId == '' || emailId == undefined) {
        $('#userAlertMsgTitle').html("Invalid");
        $('#userAlertMsgBody').html("Please enter your email id and then try!");
        $('#userAlertMsg').show();
        return;
    }
    else {
        $('#userAlertMsgTitle').html("In Process");
        $('#userAlertMsgBody').html("Please wait, processing your request...");
        $('#userAlertMsg').show();
    }

    if (regex.test(emailId)) {
        $("#div_txtUserEmail").attr("class", "");
        $("#span_txtUserEmail").attr("display", "none");

        $.ajax({
            url: "/SubscribeUser",
            type: "POST",
            data: { "email": emailId },
            dataType: "json",
            contenttype: "application/json; charset=utf-8",
            success: function (result) {
                if (result.Status) {
                    if (result.Code == 'OK' || result.Code == 'UNSUBSCRIBED_OK') {
                        $('#userAlertMsgTitle').html("Success");
                        $('#txtUserEmail').val('');
                    }
                    else if (result.Code == 'EXIST')
                        $('#userAlertMsgTitle').html("Email Exists");
                    else if (result.Code == 'ERR')
                        $('#userAlertMsgTitle').html("Failed");

                    $('#userAlertMsgBody').html(result.Message);
                    $('#userAlertMsg').show();
                }
            },
            error: function () {
                $('#userAlertMsgTitle').html("Error");
                $('#userAlertMsgBody').html("Failed to process. Please try again!");
                $('#userAlertMsg').show();
            }
        });
    }
    else {
        $("#div_txtUserEmail").attr("class", "has-error");
        $("#span_txtUserEmail").attr("class", "glyphicon glyphicon-error form-control-feedback");
        $('#userAlertMsgTitle').html("Invalid");
        $('#userAlertMsgBody').html("Seems entered email id is not valid. Please enter valid email id and then try!");
        $('#userAlertMsg').show();
    }
});