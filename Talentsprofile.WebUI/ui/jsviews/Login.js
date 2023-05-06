$("#login").submit(function () {
    if ($("#email").val() == "") {
        $("#email").attr('style', 'border-color:#DDDDDD;');
        $("#msgAlert").attr("class", "alert alert-danger alert-dismissible");
        $("#msgAlert").html("Please enter email id and then try again!");
        $("#msgAlert").show();
        return false;
    }

    if ($("#password").val() == "") {
        $("#password").attr('style', 'border-color:#DDDDDD;');
        $("#msgAlert").attr("class", "alert alert-danger alert-dismissible");
        $("#msgAlert").html("Please enter password and then try again!");
        $("#msgAlert").show();
        return false;
    }
    return true;
});

$("#email").click(function () {
    ClearControl();
});

$("#password").click(function () {
    ClearControl();
});

function ClearControl() {
    $("#msgAlert").hide();
    $("#msgAlert").attr("class", "");
    $("#msgAlert").html("");
}



//$(document).ready(function () {

//});



//$("#signIn").live('click', (function () {
//    alert('Wait..');
//    LoginToTM();
//}));


//function LoginToTM() {
//    $.ajax({
//        url: "/Home/Login",
//        data: { 'usrName': 'navin' },
//        type: "post",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        processdata: false,
//        success: function (result) {
//            alert(result);
//        },
//        error: function () {
//            alert("Failed to load..");
//        }
//    });
//}
