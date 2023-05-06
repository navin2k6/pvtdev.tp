$("#btnSaveTalentType").click(function () {
    $("#msgAlert").hide();
    $("#msgAlert").text('');
    if ($("[name=talentType]:radio:checked").val() == 1) {
        if ($("#txtExpTitle").val() == '') {
            $("#txtExpTitle").focus();
            $("#msgAlert").show();
            $("#msgAlert").text('Please enter area of experience.');
            return;
        }
        else if ($("#txtYears").val() == '' || $("#txtMonths").val() == '') {
            $("#txtMonths").focus();
            $("#msgAlert").show();
            $("#msgAlert").text('Please select year/month of experience.');
            return;
        }
    }

    $.ajax({
        url: "../Talent/Profile/CreateProfile",
        data: { jobTitle: $("#txtExpTitle").val(), exprYr: $("#txtYears").val(), exprMonth: $("#txtMonths").val(), isExpr: ($("[name=talentType]:radio:checked").val() == 1) },
        type: "post",
        //contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: false,
        success: function (result) {
            if (result != undefined)
                window.location.href = '../jobseeker/' + result.Url + '?user=profile';    
        },
        error: function (result) {
            $("#msgAlert").show();
            $("#msgAlert").text('Failed to update your profile. Please try again!');
        }
    });
});


$("[name=talentType]").change(function () {
    if ($("[name=talentType]:radio:checked").val() == 0) {
        $("#divExpTitle").hide();
        ResetControl();
    } else {
        $("#divExpTitle").show();
    }
});

function ResetControl() {
    $("#txtExpTitle").val('');
    $("#txtYears").val('');
    $("#txtMonths").val('');
    $("#msgAlert").hide();
    $("#msgAlert").text('');
}


