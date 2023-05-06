/* Loads complete profile: */
function LoadProfile() {
    $.ajax({
        url: '../../Talent/Profile/GetProfile',
        type: 'POST',
        data: {},
        datatype: "json",
        contenttype: "application/json; charset=utf-8",
        processdata: false,
        success: function (profileInfo) {
            var result = profileInfo.PersonalDetail;

            if (result.ProfileCompleted == 0) {
                $(".progress").html('');
                $("#divProfileLoader").hide();
                $("#divProfileComplete").show();
            }

            else if (parseInt(result.ProfileCompleted) >= 20) {
                $(".progress").html('<div class="progress-bar" role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100" style="width: ' + result.ProfileCompleted + '%;">Profile completed ' + result.ProfileCompleted + '%</div>');
            }
            else {
                $(".progress").html('<div class="progress-bar" role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100" style="width: ' + result.ProfileCompleted + '%;"></div>&nbsp;Profile completed ' + result.ProfileCompleted + '%');
            }
            $('#resumeAttachment').text(result.ResumeAttached);
            $('#spanAttachment').attr('class', 'glyphicon glyphicon-paperclip');
            $("#name").text(result.Name.toUpperCase());
            var val = result.Designation + result.Education;
            if (val != "") {
                $("#designation").html((result.Designation == '') ? '' : result.Designation + "<br />" + result.Education);
                $("#divDesignation").show();
            }
            else
                $("#divDesignation").hide();

            if (result.ResumeTitle != "") {
                $("#resumeTitle").text(result.ResumeTitle);
                $("#divResumeTitle").show();
            }
            else
                $("#divResumeTitle").hide();

            $("#updatedOn").html('Last updated on ' + result.UpdatedOn);
            val = (result.Phone == undefined || result.AltNum == '') ? '' : "<span class='glyphicon glyphicon-phone'></span><strong> " + result.Phone + "</strong>";
            val = (result.AltNum == undefined || result.AltNum == '') ? val : val + ", <strong>" + result.AltNum + "</strong> &nbsp;&nbsp;";
            $("#contact").html(val + "<span class='glyphicon glyphicon-envelope'></span>&nbsp;<strong>" + result.Email + "</strong>");
            val = (result.City == undefined || result.City == '') ? result.Country : result.City + " (" + result.Country + ")";
            $("#location").html("<span class='glyphicon glyphicon-map-marker'></span> " + val);
            $("#photo").attr("src", result.Photo);
            $("#summaryProfile").html(result.SummaryProfile);
            $("#primarySkills").html('<b>Primary: </b>' + result.PrimarySkills);
            $("#secondarySkills").html('<b>Secondary: </b>' + result.SecondarySkills);
            $("#dob").html(result.BirthDate);
            $("#gender").html(result.Gender);
            $("#nationality").html(result.Country);
            $("#langKnown").html(result.LanguagesKnown);
            $("#maritalStatus").html(result.MaritalStatus);
            var info = (result.CTC == undefined || result.CTC == '' || parseFloat(result.CTC) == 0) ? 'N/A' : result.CTC + " (" + result.CTCCurrencyType + ")";
            $("#ctc").html(info);
            info = (result.ECTC == undefined || result.ECTC == '' || parseFloat(result.ECTC) == 0) ? 'N/A' : result.ECTC + " (" + result.CTCCurrencyType + ")";
            $("#ectc").html(info);
            $("#passport").html(result.PassportStatus);
            $("#visa").html(result.VisaStatus);

            /* Graduation: */
            var edu = 0;
            result = profileInfo.Graduation;
            if (result != undefined || result != null) {
                info = (result.Degree == undefined || result.Degree == '' || result.Degree == null) ? '' : "Completed <strong>" + result.Degree + " (" + result.Course + ")</strong>";

                if (info == '')
                    $("#liUgCourse").hide();
                else {
                    $("#ugCourse").html(info);

                    info = (result.PassedYear == undefined || result.PassedYear == '' || result.PassedYear == null) ? '' : ", in " + result.PassedYear;
                    $("#ugPeriod").html(info);

                    info = (result.InstituteName == undefined || result.InstituteName == '' || result.InstituteName == null) ? '' : " from " + result.InstituteName + ", affiliated to " + result.University + ".";
                    $("#ugInstitute").html(info);
                    edu++;
                }
            }

            /* Under graduation */
            result = profileInfo.UnderGraduation;
            if (result != undefined && result[0] != undefined) {
                info = (result[0].Degree == undefined || result[0].Degree == '' || result[0].Degree == null) ? '' : "Passed <strong>10+2 (" + result[0].Course + ")</strong> in " + result[0].PassedYear + " from " + result[0].InstituteName + ", affilicated to " + result[0].University + " board.";
                if (info == '')
                    $("#liCourse12").hide();
                else {
                    $("#ugCourse12").html(info);
                    edu++;
                }
            }
            else
                $("#liCourse12").hide();

            if (result != undefined && result[1] != undefined) {
                info = (result[1].Degree == undefined || result[1].Degree == '' || result[1].Degree == null) ? '' : "Passed <strong>10th</strong> in " + result[1].PassedYear + " from " + result[1].InstituteName + ", affilicated to " + result[0].University + " board.";
                if (info == '')
                    $("#liCourse10").hide();
                else {
                    $("#ugCourse10").html(info);
                    edu++;
                }
            }
            else
                $("#liCourse10").hide();

            /* Post Graduation:postGraduation */
            result = profileInfo.PostGraduation;
            if (result != undefined && result[0] != undefined) {
                var pgCourses = '';
                $.each(result, function (obj) {
                    pgCourses = pgCourses + "<li>Completed <strong>" + result[obj].Degree + " (" + result[obj].Course + ")</strong> in " + result[obj].PassedYear + " from " + result[obj].InstituteName + ", affiliated to " + result[obj].University + ".</li>";
                    edu++;
                });

                $("#ulPG").html(pgCourses);
            }
            else {
                $("#ulPG").hide();
            }

            if (edu == 0)
                $("#divEducation").html("<p style='padding: 5px 20px'>Educational detail not provided.</p>");
        },
        error: function () {
        }
    });

    $("#divProfileLoader").hide();
    $("#divProfileComplete").show();
}


/* View Profile: Load organizational detail: */
function LaodOrganizationalDetail() {
    $.ajax(
    {
        url: '../../Talent/Profile/LaodOrganizationalInfo',
        type: 'POST',
        data: {},
        datatype: "json",
        processdata: false,
        success: function (result) {
            if (result.Status) {
                var list = '', i = 0;
                $.each(result.Data, function (obj) {
                    list = list + "<ul><li><strong>" + result.Data[obj].Designation + "</strong>, " + result.Data[obj].DateFrom + " - " + result.Data[obj].ToDate + ", " + result.Data[obj].OrganizationName + ", " + result.Data[obj].City + " (" + result.Data[obj].CountryName + ")</li></ul>";
                    i++;
                });

                if (i > 0) {
                    $("#divOrgDetail").html(list);
                }
                else
                    $("#divOrgDetail").html("<p style='padding: 5px 20px'>Organizational detail not provided.</p>");
            } else
                window.location = document.location.origin + "/" + result.Url;
        },
        error: function () { }
    });
}

/* View Profile: Load project detail: */
function LoadProjectDetail() {
    $.ajax({
        url: '../../Talent/Profile/LoadProjects',
        type: 'POST',
        data: {},
        datatype: "json",
        contenttype: "application/json; charset=utf-8",
        processdata: false,
        success: function (result) {
            if (result.Status) {
                var proj = '', i = 0;
                $.each(result.Data, function (obj) {
                    proj = proj + "<div style='margin: 15px; position:relative'><div style='width:100%; font-weight:bold; font-size:16px'>" + result.Data[obj].Title + "</div><div style='width:100%;'><div style='width:15%; float:left'>Synopsis:</div><div style='width:85%; float:left'>" + result.Data[obj].Synopsis + "</div></div><div style='width:100%;'><div style='width:15%; float:left'>Tools:</div><div style='width:85%; float:left'>" + result.Data[obj].ToolsUsed + "</div></div><div class='clearfix'>&nbsp;</div></div>";
                    i++;
                });
                if (i > 0) {
                    $("#divProjectDone").html(proj);
                }
                else
                    $("#divProjectDone").html("<p style='padding: 5px 20px'>Project detail not provided.</p>");

                $("#msgLoader").html('');
                $("#msgLoader").hide();
            } else
                window.location = document.location.origin + "/" + result.Url;
        },
        error: function () {
        }
    });
}


/* Load JS summary profile details: */
function LoadSummaryProfile() {
    $.ajax({
        url: '../../Talent/Profile/GetSummaryProfile',
        type: 'POST',
        data: {},
        datatype: "json",
        contenttype: "application/json; charset=utf-8",
        processdata: false,
        success: function (result) {
            if (result != null) {
                $("#txtPriSkills").html(result.PrimarySkills);
                $("#txtSecSkills").html(result.SecondarySkills);
                $("#txtYears").val(parseInt(result.TotalExperienceYear));
                $("#txtMonths").val(parseInt(result.TotalExperienceMonth));
                $("#txtSummaryProfile").html(result.ProfileSummary);
                $("#noticePeriod option[value=" + result.NoticePeriod + "]").attr('selected', 'selected');
            }
        }
    });
}

/* Load project detail: */
function LoadProjects(_id, _action) {
    $("#txtMsg").html('');
    $("#txtMsg").hide('');

    $.ajax({
        url: '../../Talent/Profile/LoadProjects',
        type: 'POST',
        data: { id: _id, action: _action },
        datatype: "json",
        contenttype: "application/json; charset=utf-8",
        processdata: false,
        success: function (result) {
            if (_action == "EDIT") {
                $.each(result.Data, function (obj) {
                    $("#txtTitle").val(result.Data[obj].Title);
                    $("#txtTools").val(result.Data[obj].ToolsUsed);
                    $("#txtRoles").val(result.Data[obj].RolePlayed);
                    $("#txtSynopsis").val(result.Data[obj].Synopsis);
                    $("#btnProjects").attr('btnProjVal', result.Data[obj].ProjId);
                    $("#btnProjects").attr('btnProjAction', "UPDATE");
                });
            }
            else {
                var proj = '';
                $.each(result.Data, function (obj) {
                    //proj = proj + "<tr><td style='vertical-align: text-top'>" + (obj + 1) + "</td><td><a href='javascript:void(0)' name='projName' projVal=" + result.Data[obj].ProjId + "><span class='glyphicon glyphicon-edit'></span> " + result.Data[obj].Title + "</a><br /><span>" + result.Data[obj].Duration + "</span><a><span class='glyphicon glyphicon-trash'></a></td><td>" + result.Data[obj].Synopsis + "<br /><span>" + result.Data[obj].ToolsUsed + "</span></td></tr>";
                    proj = proj + "<div style='margin: 15px; position:relative'><div style='width: 100%; font-weight: bold; font-size: 16px'><a href='javascript:void(0)' name='projName' projval=" + result.Data[obj].ProjId + "><span class='glyphicon glyphicon-edit'></span> " + result.Data[obj].Title + "</a></div><div style='width: 100%;'><div style='width: 15%; float: left'>Synopsis:</div><div style='width: 85%; float: left'>" + result.Data[obj].Synopsis + "</div></div><div style='width: 100%;'><div style='width: 15%; float: left'>Tools:</div><div style='width: 85%; float: left'>" + result.Data[obj].ToolsUsed + "</div></div><div class='clearfix'>&nbsp;</div><div style='width: 100%; padding-bottom:15px; border-bottom:dashed 1px #ccc'><a href='javascript:void(0)' name='projName' projval=" + result.Data[obj].ProjId + "><span class='glyphicon glyphicon-edit'></span></a>&nbsp;&nbsp;<a href='javascript:void(0)' name='trashProj' projval=" + result.Data[obj].ProjId + "><span class='glyphicon glyphicon-trash'></span></a></div></div>";
                });

                $("#projects").html(proj);
            }
        },
        error: function () {
        }
    });
}


/* Graduation detail update: */
$("#btnSaveUG").click(function () {
    $.ajax({
        url: '../../Talent/Profile/ManageGraduateInfo',
        type: 'POST',
        data: { degree: $("#txtDegree").val(), course: $("#txtSpecialization").val(), passedYear: $("#txtYear").val(), institute: $("#txtIntitute").val(), university: $("#txtUniversity").val() },

        datatype: "json",
        contenttype: "application/json; charset=utf-8",

        processdata: false,
        success: function (result) {
            if (result.Status) {
                $("#txtMsgUg").attr("class", "alert alert-success alert-dismissible");

                if (result != undefined && result.Url != '')
                    window.location.href = '../../talent/' + result.Url + '?user=profile';
            }
            else {
                $("#txtMsgUg").attr("class", "alert alert-danger alert-dismissible");
            }

            $("#txtMsgUg").show();
            $("#txtMsgUg").html(result.Message);
        },
        error: function () {
        }
    });
});


/* Schooling detail update: */
$("#btnSaveSchooling").click(function () {
    $.ajax({
        url: '../../Talent/Profile/ManageSchoolingInfo',
        type: 'POST',
        data: {
            board: $("#txt12Board").val(), school: $("#txt12School").val(), passedYear: $("#txt12PassYear").val(),
            course: $("#txt12Specialization").val(), board10: $("#txt10Board").val(), school10: $("#txt10School").val(),
            passedYear10: $("#txt10PassYear").val(), course10: $("#txt10Specialization").val()
        },

        datatype: "json",
        contenttype: "application/json; charset=utf-8",

        processdata: false,
        success: function (result) {
            if (result.Status) {
                $("#txtMsgUg").attr("class", "alert alert-success alert-dismissible");

                if (result != undefined && result.Url != '')
                    window.location.href = '../../talent/' + result.Url + '?user=profile';
            }
            else {
                $("#txtMsgUg").attr("class", "alert alert-danger alert-dismissible");
            }

            $("#txtMsgUg").show();
            $("#txtMsgUg").html(result.Message);
        },
        error: function () {
        }
    });
});


/* Graduation detail update: */
$("#btnSavePG").click(function () {
    $.ajax({
        url: '../../Talent/Profile/ManagePostGraduateInfo',
        type: 'POST',
        data: { degree: $("#txtPGDegree").val(), course: $("#txtPGSpecialization").val(), passedYear: $("#txtPGYear").val(), institute: $("#txtPGIntitute").val(), university: $("#txtPGUniversity").val(), pgId: $("#degreeVal").val() },

        datatype: "json",
        contenttype: "application/json; charset=utf-8",

        processdata: false,
        success: function (result) {
            if (result.Status) {
                var pgDtl = result.Data.PostGraduation;
                var courses = "";
                $.each(pgDtl, function (obj) {
                    courses = courses + "<tr style='border-bottom:dashed 1px #ccc;'><td style='vertical-align: text-top; width:5%; padding:15px'>" + (obj + 1) + "</td><td style='width:85%; padding:15px'><a href='javascript:void(0)' name='pgDtl' pgVal=" + pgDtl[obj].SrNo + "><span class='glyphicon glyphicon-edit'></span>  " + pgDtl[obj].Degree + "</a><br />" + pgDtl[obj].Course + ", " + pgDtl[obj].PassedYear + ", " + pgDtl[obj].InstituteName + ", " + pgDtl[obj].University + "<div style='width: 100%; padding-top:10px'><a href='javascript:void(0)' name='pgDtl' pgVal=" + pgDtl[obj].SrNo + "><span class='glyphicon glyphicon-edit'></span></a>&nbsp;&nbsp;<a href='javascript:void(0)' name='trashpgDtl' pgVal=" + pgDtl[obj].SrNo + "><span class='glyphicon glyphicon-trash'></span></a></div></td><td style='text-align:center; width:10%; padding:15px'></td></tr>";
                });

                $("#post_Graduation").html(courses);
                $("#editDtl").hide();
            }
        },
        error: function () {
        }
    });
});

/* Load qualification detail: */
function LoadQualification(_action, _eduVal) {
    $.ajax({
        url: '../../Talent/Profile/LoadQualification',
        type: 'POST',
        data: { eduAction: _action, eduVal: _eduVal },

        datatype: "json",
        contenttype: "application/json; charset=utf-8",

        processdata: false,
        success: function (result) {
            if (_action == "EDIT") {
                $("#degreeVal").val(_eduVal);
                $("#txtPGDegree").val(result.Graduation.Degree);
                $("#txtPGSpecialization").val(result.Graduation.Course);
                $("#txtPGYear").val(result.Graduation.PassedYear);
                $("#txtPGIntitute").val(result.Graduation.InstituteName);
                $("#txtPGUniversity").val(result.Graduation.University);
            } else {
                $("#txtDegree").val(result.Graduation.Degree);
                $("#txtSpecialization").val(result.Graduation.Course);
                $("#txtYear").val(result.Graduation.PassedYear);
                $("#txtIntitute").val(result.Graduation.InstituteName);
                $("#txtUniversity").val(result.Graduation.University);

                /* schooling */
                $("#txt12Specialization").val(result.UnderGraduation[0].Course);
                $("#txt12PassYear").val(result.UnderGraduation[0].PassedYear);
                $("#txt12School").val(result.UnderGraduation[0].InstituteName);
                $("#txt12Board").val(result.UnderGraduation[0].University);

                $("#txt10Specialization").val(result.UnderGraduation[1].Course);
                $("#txt10PassYear").val(result.UnderGraduation[1].PassedYear);
                $("#txt10School").val(result.UnderGraduation[1].InstituteName);
                $("#txt10Board").val(result.UnderGraduation[1].University);

                var pgDtl = result.PostGraduation;
                var courses = '';
                if (pgDtl != null || pgDtl != undefined) {
                    $.each(pgDtl, function (obj) {
                        courses = courses + "<tr style='border-bottom:dashed 1px #ccc;'><td style='vertical-align: text-top; width:5%; padding:15px'>" + (obj + 1) + "</td><td style='width:85%; padding:15px'><a href='javascript:void(0)' name='pgDtl' pgVal=" + pgDtl[obj].SrNo + "><span class='glyphicon glyphicon-edit'></span>  " + pgDtl[obj].Degree + "</a><br />" + pgDtl[obj].Course + ", " + pgDtl[obj].PassedYear + ", " + pgDtl[obj].InstituteName + ", " + pgDtl[obj].University + "<div style='width: 100%; padding-top:10px'><a href='javascript:void(0)' name='pgDtl' pgVal=" + pgDtl[obj].SrNo + "><span class='glyphicon glyphicon-edit'></span></a>&nbsp;&nbsp;<a href='javascript:void(0)' name='trashpgDtl' pgVal=" + pgDtl[obj].SrNo + "><span class='glyphicon glyphicon-trash'></span></a></div></td><td style='text-align:center; width:10%; padding:15px'></td></tr>";
                    });
                } else
                    $("#lblPgInfo").html('You have not added any post-graduation program.');

                $("#post_Graduation").html(courses);
            }
        },
        error: function () {
        }
    });
}


function ResetControl() {
    $("#btnCurOrg").attr('btnOrgAction', "ADD");
    $("#btnCurOrg").attr('btnOrgVal', '');
    $("#txtOrganization").val('');
    $("#txtFromDate").val('');
    $("#txtToDate").val('');
    $("#txtCity").val('');
    $("#Country").val('0');
    $("#txtDesignation").val('');
}


/* Save organizational detail of job-seeker. */
$("#btnCurOrg").click(function () {
    var _compType = null;
    var _noticePeriod = -1;

    if ($("#chkCurOrganization").prop('checked')) {
        _compType = 'CO';

        if ($("#noticePeriod").val() == "-1") {
            $("#txtMsg").attr("class", "alert alert-danger alert-dismissible");
            $("#txtMsg").html("Please select notice period of current organzation.");
            $("#txtMsg").show();
            $("#noticePeriod").focus();
            return false;
        }
        else {
            _noticePeriod = $("#noticePeriod").val();
        }
    }


    $.ajax({
        url: '../../Talent/Profile/ManageOrganizationalDetail',
        type: 'POST',
        data: {
            compName: $("#txtOrganization").val(), fromDate: $("#txtFromDate").val(), toDate: $("#txtToDate").val(), city: $("#txtCity").val(),
            country: $("#Country").val(), designation: $("#txtDesignation").val(), noticePeriod: _noticePeriod, compType: _compType,
            id: $("#btnCurOrg").attr('btnOrgVal'), action: $("#btnCurOrg").attr('btnOrgAction')
        },

        datatype: "json",
        contenttype: "application/json; charset=utf-8",

        processdata: false,
        success: function (result) {
            if (result.Status) {
                LaodOrganizationalInfo(null, '');
                $("#txtMsg").attr("class", "alert alert-success alert-dismissible");
                ResetControl();
                $("#editDtl").hide();
            }
            else {
                $("#txtMsg").attr("class", "alert alert-danger alert-dismissible");
            }

            $("#txtMsg").html(result.Message);
            $("#txtMsg").show();
        },
        error: function () {
        }
    });
});

/* Load organizational detail: */
function LaodOrganizationalInfo(orgVal, _action) {
    $("#txtMsg").html('');
    $("#txtMsg").hide('');

    $.ajax(
    {
        url: '../../Talent/Profile/LaodOrganizationalInfo',
        type: 'POST',
        data: { val: orgVal, action: _action },
        datatype: "json",
        processdata: false,
        success: function (result) {
            if (_action == "EDIT") {
                $.each(result.Data, function (obj) {
                    $("#txtOrganization").val(result.Data[obj].OrganizationName);
                    $("#txtDesignation").val(result.Data[obj].Designation);
                    $("#txtFromDate").val(result.Data[obj].DateFrom);
                    $("#txtToDate").val(result.Data[obj].ToDate);
                    $("#txtCity").val(result.Data[obj].City);
                    $("#chkCurOrganization").prop('checked', result.Data[obj].CompanyType);

                    if (result.Data[obj].CompanyType)
                        $("#divNoticePeriod").show();
                    else
                        $("#divNoticePeriod").hide();

                    $("#Country").val(result.Data[obj].Country);
                    $("#btnCurOrg").attr('btnOrgVal', result.Data[obj].SrNo);
                    $("#btnCurOrg").attr('btnOrgAction', "UPDATE");
                    $('#noticePeriod').val(result.Data[obj].NoticePeriod);
                });
            }
            else {
                var list = '';
                $.each(result.Data, function (obj) {
                    var img = (result.Data[obj].CompanyType == true) ? "<img src=../../assets/images/tick.png title='Current organization' />" : "";
                    list = list + "<tr style='border-bottom:dashed 1px #ccc;'><td style='vertical-align: text-top; width:5%; padding:15px'>" + (obj + 1) + "</td><td style='width:85%; padding:15px'><a href='javascript:void(0)' name='orgName' orgval=" + result.Data[obj].SrNo + "><span class='glyphicon glyphicon-edit'></span>  " + result.Data[obj].OrganizationName + "</a><br />" + result.Data[obj].Designation + ", from " + result.Data[obj].DateFrom + " - " + result.Data[obj].ToDate + "<br />" + result.Data[obj].City + ", " + result.Data[obj].CountryName + "<div style='width: 100%; padding-top:10px'><a href='javascript:void(0)' name='orgName' orgval=" + result.Data[obj].SrNo + "><span class='glyphicon glyphicon-edit'></span></a>&nbsp;&nbsp;<a href='javascript:void(0)' name='trashOrg' orgval=" + result.Data[obj].SrNo + "><span class='glyphicon glyphicon-trash'></span></a></div></td><td style='text-align:center; width:10%; padding:15px'>" + img + "</td></tr>";
                });

                $("#orgList").html(list);
            }
        },
        error: function () { }
    });
}


$('#lnkAttachResume').click(function () {
    BootstrapDialog.show({
        title: 'Say-hello dialog',
        message: 'Hi Apple!'
    });
});

/* Print page*/
//function PrintPage() {
//    var printContents = document.getElementById('divProfile').innerHTML;
//    var originalContents = document.body.innerHTML;
//    document.title = '';
//    document.URL = '';
//    document.body.innerHTML = printContents;
//    window.print();
//    document.body.innerHTML = originalContents;
//}