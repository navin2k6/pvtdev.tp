$(document).ready(function () {
    $('#liSearch').attr('class', '');
    $('#liMyProf').attr('class', 'active');
    $('#liMyResume').attr('class', '');
    $('#liUser').attr('class', '');
});


// Personal info update:
$("#btnProfile").click(function () {
    var ctrls = [['txtFirstName', 'text', 'first name'], ['txtLastName', 'text', 'last name'], ['Gender', 'list', 'gender'], ['txtDoB', 'text', 'date of birth'],
    ['MaritalStatus', 'list', 'marital status'], ['txtPhone1', 'text', 'phone number'], ['txtCity', 'text', 'city name currently residing'], ['Country', 'list', 'country'],
    ['txtLanguages', 'text', 'atleast one language you speak']];
    var success = true;
    for (var i = 0; i < ctrls.length; i++) {
        success = ValidateControl(ctrls[i][0], ctrls[i][1]);
        if (!success)
            break;
    }

    if (!success) {
        $("#msgAlert").attr("class", "alert alert-danger alert-dismissible");
        $("#msgAlert").show();
        $("#msgAlert").html('Please provide ' + ctrls[i][2]);
        $('#' + ctrls[i][0]).focus();
        return;
    }

    if ($("#VisaStatus").val() == "2") {
        if ($("#VisaCountry").val() == "0") {
            $("#VisaCountry").attr('style', 'border-color:#a94442;');
            $("#VisaCountry").focus();

            $("#msgAlert").attr("class", "alert alert-danger alert-dismissible");
            $("#msgAlert").show();
            $("#msgAlert").html('Please select visa country');
            return;
        } else {
            $("#VisaCountry").attr('style', 'border-color:#DDDDDD;');
        }
    }

    $("#btnProfile").hide();
    $("#imgProcess").show();

    $.ajax({
        url: '../Profile/ManageProfile',
        type: 'POST',
        data: {
            FirstName: $("#txtFirstName").val(), LastName: $("#txtLastName").val().trim(), Gender: $("#Gender").val(), DoB: $("#txtDoB").val(), Phone1: $("#txtPhone1").val(),
            Phone2: $("#txtPhone2").val(), Address: $("#txtAddress").val(), City: $("#txtCity").val(), PostalCode: $("#txtPostalCode").val(), Nationality: $("#Country").val(),
            LangKnown: $("#txtLanguages").val(), MaritalStatus: $("#MaritalStatus").val(), PassportStatus: $("#PassportStatus").val(), VisaStatus: $("#VisaStatus").val(),
            VisaCountry: $("#VisaCountry").val()
        },

        datatype: "json",
        contenttype: "application/json; charset=utf-8",

        processdata: false,
        success: function (result) {
            if (result == 'OK') {
                $("#msgAlert").attr("class", "alert alert-success alert-dismissible");
                $("#msgAlert").html('Personal information saved successfully.');
            }
            else {
                $("#msgAlert").attr("class", "alert alert-danger alert-dismissible");
                $("#msgAlert").html('Sorry! Failed to save personal information. Please try again!');
            }
            $("#msgAlert").show();
        },
        error: function () {

        }
    });

    $("#btnProfile").show();
    $("#imgProcess").hide();
});


/* Personal info update:*/
$("#btnSalary").click(function () {
    var msg = '';
    var isValid = true;
    if ($("#ctcList").val() == "" || $("#ctcList").val() == undefined) {
        msg = 'Please select current CTC. If you are not working or fresher, select to Not Applicable.';
        isValid = false;
    }
    else if ($("#currencyCTC").val() == "" || $("#currencyCTC").val() == undefined) {
        msg = 'Please select CTC currency type.';
        isValid = false;
    }
    else if ($("#ectcList").val() == "" || $("#ectcList").val() == undefined) {
        msg = 'Please select expected CTC.';
        isValid = false;
    }
    else if ($("#currencyECTC").val() == "" || $("#currencyECTC").val() == undefined) {
        msg = 'Please select expected CTC currency type.';
        isValid = false;
    }

    if (!isValid) {
        $("#msgSalAlert").attr("class", "alert alert-danger alert-dismissible");
        $("#msgSalAlert").html(msg);
        $("#msgSalAlert").show();
        return;
    }

    $("#btnSalary").hide();
    $("#imgSalProcess").show();

    $.ajax({
        url: '../Profile/ManageSalary',
        type: 'POST',
        data: { ctc: $("#ctcList").val(), ctcCurType: $("#currencyCTC").val(), ectc: $("#ectcList").val(), ectcCurType: $("#currencyECTC").val() },

        datatype: "json",
        contenttype: "application/json; charset=utf-8",

        processdata: false,
        success: function (result) {
            if (result.Status == 'OK') {
                $("#msgSalAlert").attr("class", "alert alert-success alert-dismissible");
            }
            else {
                $("#msgSalAlert").attr("class", "alert alert-danger alert-dismissible");
                //$("#msgSalAlert").html("Highlighted information are invalid. Please provide the valid information and then try again!");
            }
            $("#msgSalAlert").html(result.Message);
            $("#msgSalAlert").show();
        },
        error: function () {
            //alert("Failed to load..");
        }
    });

    $("#btnSalary").show();
    $("#imgSalProcess").hide();
});


// Load salary info:
function LoadSalaryInfo() {
    //alert('Hello');
    $.ajax({
        url: '../Profile/LoadSalaryInfo',
        type: 'POST',
        data: {},

        datatype: "json",
        contenttype: "application/json; charset=utf-8",

        processdata: false,
        success: function (result) {
            //alert(JSON.stringify(result["CTC"]));
            $("#ctcList option[value=" + result["CTC"] + "]").attr('selected', 'selected');
            $("#currencyCTC option[value=" + result["CTCCurrencyType"] + "]").attr('selected', 'selected');
            $("#ectcList option[value=" + result["ECTC"] + "]").attr('selected', 'selected');
            $("#currencyECTC option[value=" + result["ECTCCurrencyType"] + "]").attr('selected', 'selected');
        },
        error: function () {
            //alert("Failed to load..");
        }
    });
}


// Skills set update:
$("#btnSkills").click(function () {
    $.ajax({
        url: '../Profile/ManageSkillsSet',
        type: 'POST',
        data: { primarySkills: $("#txtPriSkills").val(), secondarySkills: $("#txtSecSkills").val() },

        datatype: "json",
        contenttype: "application/json; charset=utf-8",

        processdata: false,
        success: function (result) {
            if (result.Status == 'OK') {
                $("#msgSkillsAlert").attr("class", "alert alert-success alert-dismissible");
                $("#msgSkillsAlert").html(result.Message);

                if (result.Url != '')
                    window.location.href = '../jobseeker/' + result.Url + '?user=profile';
            }
            else {
                $("#msgSkillsAlert").attr("class", "alert alert-danger alert-dismissible");
                $("#msgSkillsAlert").html(result.Message);
            }
            $("#msgSkillsAlert").show();
        },
        error: function () {
            //alert("Failed to load..");
        }
    });
});


// Skills set update:
$("#btnSummary").click(function () {
    $.ajax({
        url: '../Profile/ManageSummaryProfile',
        type: 'POST',
        data: { profileSummary: $("#txtSummaryProfile").val(), expYear: $("#txtYears").val(), expMonth: $("#txtMonths").val() },

        datatype: "json",
        contenttype: "application/json; charset=utf-8",

        processdata: false,
        success: function (result) {
            if (result.Status == 'OK') {
                $("#msgSummaryAlert").attr("class", "alert alert-success alert-dismissible");
            }
            else {
                $("#msgSummaryAlert").attr("class", "alert alert-danger alert-dismissible");
            }

            $("#msgSummaryAlert").html(result.Message);
            $("#msgSummaryAlert").show();
        },
        error: function () {
        }
    });
});


function ResetControl() {
    $("#btnProjects").attr('projVal', '');
    $("#btnProjects").attr('projAction', "ADD");
}

// Manage projects done:
$("#btnProjects").click(function () {
    $.ajax({
        url: '../Profile/ManageProjects',
        type: 'POST',
        data: { title: $("#txtTitle").val(), toolsUsed: $("#txtTools").val(), role: $("#txtRoles").val(), synopsis: $("#txtSynopsis").val(), val: $("#btnProjects").attr('btnProjVal'), action: $("#btnProjects").attr('btnProjAction') },

        datatype: "json",
        contenttype: "application/json; charset=utf-8",

        processdata: false,
        success: function (result) {
            if (result.Success) {
                LoadProjects(null, '');
                $("#txtMsg").attr("class", "alert alert-success alert-dismissible");
                $("#txtMsg").html(result.Message);
                ResetControl();
                $("#editDtl").hide();
            }
            else {
                $("#txtMsg").attr("class", "alert alert-danger alert-dismissible");
                $("#txtMsg").html(result.Message);
            }
            $("#txtMsg").show();
        },
        error: function () {
        }
    });
});


$("[name=VisaStatus]").change(function () {
    ($("#VisaStatus").val() == "2") ? $("#divVisaSec").show() : $("#divVisaSec").hide()
});


function LoadCoverLetter() {
    $.ajax({
        url: '../Profile/ManageCoverLetter',
        type: 'POST',
        data: { desc: '', action: 'get' },

        datatype: "json",
        contenttype: "application/json; charset=utf-8",

        processdata: false,
        success: function (result) {
            if (result.Status == 'OK') {
                $("#txtLetterDesc").val(result.Desc);
                $("#msgSummaryAlert").hide();
            }
            else {
                $("#msgSummaryAlert").attr("class", "alert alert-danger alert-dismissible");
                $("#msgSummaryAlert").html("Sorry! Failed to load cover letter. Please try again!");
                $("#msgSummaryAlert").show();
            }
        },
        error: function () {
            //alert("Failed to load..");
        }
    });
}


$("#btnSaveLetter").click(function () {
    $.ajax({
        url: '../Profile/ManageCoverLetter',
        type: 'POST',
        data: { desc: $("#txtLetterDesc").val(), action: 'post' },

        datatype: "json",
        contenttype: "application/json; charset=utf-8",

        processdata: false,
        success: function (result) {
            if (result.Status == 'OK') {
                $("#msgSummaryAlert").attr("class", "alert alert-success alert-dismissible");
                $("#msgSummaryAlert").html("Cover letter saved successfully.");
            }
            else {
                $("#msgSummaryAlert").attr("class", "alert alert-danger alert-dismissible");
                $("#msgSummaryAlert").html("Sorry! Failed to save cover letter. Please try again!");
            }
            $("#msgSummaryAlert").show();
        },
        error: function () {
            //alert("Failed to load..");
        }
    });
});

$("#liPersonalProfile").click(function () {
    $("#divPersonalProfile").show();
    $("#liPersonalProfile").attr("class", "active");

    $("#divSalaryDtl").hide();
    $("#liSalaryDtl").attr("class", "");

    $("#divPhoto").hide();
    $("#liPhoto").attr("class", "");
});

$("#liSalaryDtl").click(function () {
    $("#divSalaryDtl").show();
    $("#liSalaryDtl").attr("class", "active");

    $("#divPersonalProfile").hide();
    $("#liPersonalProfile").attr("class", "");

    $("#divPhoto").hide();
    $("#liPhoto").attr("class", "");
    LoadSalaryInfo();
});

$("#liPhoto").click(function () {
    $("#divPhoto").show();
    $("#liPhoto").attr("class", "active");
    $("#photo").attr('src', $('#userProfImg').val());

    $("#divSalaryDtl").hide();
    $("#liSalaryDtl").attr("class", "");

    $("#divPersonalProfile").hide();
    $("#liPersonalProfile").attr("class", "");
});


$("#Upload").click(function () {
    $('#uploader').show(); $('#uploader').attr('src', '../../assets/images/loader.gif');
    var formData = new FormData();
    var totalFiles = document.getElementById("FileUpload").files.length;
    var file = document.getElementById("FileUpload").files[0];
    formData.append("FileUpload", file);
    //formData.append("Title", $('#txtResumeTitle').val());
    $.ajax({
        type: 'POST',
        url: '../Profile/UploadFiles',
        data: formData,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (result) {
            //var _info = (JSON.stringify(result));
            $('#photo').attr('src', result.fileName);
            $('#uploader').hide();
            $('#uploader').attr('src', '');
        },
        error: function (error) {
            $('#uploader').hide();
            $('#uploader').attr('src', '');
        }
    });
});
