//var jobkeywords = null;
//var city = null;
var jobType = null;
var qry = null;

$("#btnSearchAftrLogin").click(function () {
    //SelectedKeywords();
    SearchJobs();
});


/* FOR HOME-SEARCH */
$("#btnSearch").click(function () {
    var _keys = ($("#keywords").val().split(',')).join('-');
    var _loc = ($("#city").val().split(',')).join('-');

    if (_keys.length > 0)
        qry = "key=" + _keys;
    if (_loc.length > 0)
        qry = (qry == null) ? ("loc=" + _loc) : (qry + "&loc=" + _loc);

    SearchFor();
});


/* Manage keywords for searching/selecting user's options */
function SearchFor() {
    if (qry == null || qry == undefined)
        window.location.href = '/search/jobs';
    else
        window.location.href = '/search/jobs?' + qry;
}


function ReadKeywords() {
    /* When user directly got to search page, without any search keywords */
    if (window.location.href.indexOf('?') == -1) {
    }
    else {
        var _qryStr = window.location.href.split('?')[1].split('&');

        if (_qryStr != undefined) {
            var keys = _qryStr[0].replace('key=', '').replace(/-/g, ',').replace(/%20/g, ' ');

            if (_qryStr.length > 0) {
                if (keys == 'walkins')
                    $('#chkWalkins').prop('checked', 'checked');
                if (keys == 'skills' || keys == 'loc' || keys == 'edu')
                    $("#keywords").val('');
                else
                    $("#keywords").val(keys);
            }

            if (_qryStr.length > 1)
                $("#city").val(_qryStr[1].replace('loc=', '').replace(/-/g, ',').replace(/%20/g, ' '));
        }
    }

    SearchJobs();
}


function SearchJobs() {
    $('#resultCount').html('No job found for selected key words.');
    $('#progressbarValue').css('width', '9%');

    if ($("#chkCompany").is(":checked") == true)
        jobType = "company";

    if ($("#chkConsultant").is(":checked") == true) {
        if (jobType.length > 1)
            jobType = "all";
        else
            jobType = "consultant";
    }

    var postedBy = '';
    if ($("#chkWalkins").prop('checked'))
        postedBy = $("#chkWalkins").val();
    if ($("#chkCompany").prop('checked'))
        postedBy = postedBy + ',' + $("#chkCompany").val();
    if ($("#chkConsultant").prop('checked'))
        postedBy = postedBy + ',' + $("#chkConsultant").val();

    $.ajax(
 {
     url: '/search/jobs/SearchJob',
     type: 'POST',
     data: { "keyword": $("#keywords").val(), "location": $("#city").val(), "jobType": jobType, "minExpr": $("#minExpr").val(), "maxExpr": $("#maxExpr").val(), "jobType": $("#JobType").val(), "postedBy": postedBy, "postedOn": $("#postedOn").val() },
     datatype: "json",
     contenttype: "application/json; charset=utf-8",
     success: function (result) {
         var html = '';
         var i = 0;

         if (result.Jobs != null && result.Status) {
             $.each(result.Jobs, function (obj) {
                 //html = html + '<div class="item-block"><header><img src="/ui/images/company.png" alt="Alternate Text" /><span>' + result.Jobs[obj].PostedDate + '</span><a target="_blank" href="/search/jobs/desc/' + result.Jobs[obj].JobId + '?job=' + result.Jobs[obj].JobTitle.toLowerCase() + '"><h4>' + result.Jobs[obj].JobTitle + '<span class="label label-warning">' + result.Jobs[obj].JobType + '</span></h4></a><span>Experience: ' + result.Jobs[obj].Experience + ', Designation: ' + result.Jobs[obj].Designation + '</span></br><span>' + result.Jobs[obj].Organization + ', ' + result.Jobs[obj].Location + '</span></header><div class="item-body"><div class="skills">Skills Set: ' + result.Jobs[obj].SkillsSet + '<p>' + result.Jobs[obj].Description + '</p></div></div><footer><input type="submit" class="btn btn-primary btn-sm" id="btnViewJob" value="View" job="' + result.Jobs[obj].JobId + '" key="' + result.Jobs[obj].JobTitle.toLowerCase() + '" />&nbsp;&nbsp;<input type="submit" class="btn btn-primary btn-sm" id="btnApplyJob" value="Apply"/></footer></div>';
                 html = html + '<div class="item-block"><header><img src="/ui/images/company.png" alt="Alternate Text" /><span>' + result.Jobs[obj].PostedDate + '</span><a target="_blank" href="/search/jobs/desc/' + result.Jobs[obj].JobId + '?job=' + result.Jobs[obj].JobTitle.toLowerCase() + '"><h4>' + result.Jobs[obj].JobTitle + '<span class="label label-warning">' + result.Jobs[obj].JobType + '</span></h4></a><span>Experience: ' + result.Jobs[obj].Experience + ', Designation: ' + result.Jobs[obj].Designation + '</span></br><span>' + result.Jobs[obj].Organization + ', ' + result.Jobs[obj].Location + '</span></header><div class="item-body"><div class="skills">Skills Set: ' + result.Jobs[obj].SkillsSet + '<p>' + result.Jobs[obj].Description + '</p></div></div><footer><div class="btn btn-primary btn-sm"><a href="/search/jobs/desc/' + result.Jobs[obj].JobId + '?job=' + result.Jobs[obj].JobTitle.toLowerCase() + '" target="_blank" style="color:#fff">View</a></div></footer></div>';

                 i++;
             });

             $('#resultSet').html(html);
         }

         if (i == 1)
             $('#resultCount').html(i + ' job found.');
         else if (i > 1)
             $('#resultCount').html(i + ' jobs found.');

         if (i > 0)
             $('#filterAction').show();
     },
     error: function () {
     }
 });

    ManageLoader();

    $('#resultCount').show();

}

function ManageLoader() {
    /* Manage progress status*/
    for (p = 10; p <= 100; p++) {
        setTimeout(function () {
            $('#progressbarValue').css('width', p + '%');
        }, 500);
    }

    setTimeout(function () {
        $('#progressContainer').hide();
    }, 3000);
}

$(function () {
    var availableTags = [
			".Net",
            "ActionScript",
			"AppleScript",
			"Asp",
            "ASP.Net",
			"BASIC",
			"C",
			"C++",
            "C#",
			"Clojure",
			"COBOL",
			"ColdFusion",
			"Erlang",
			"Fortran",
			"Groovy",
			"Haskell",
			"Java",
			"JavaScript",
			"Lisp",
			"Perl",
			"PHP",
			"Python",
			"Ruby",
			"Scala",
			"Scheme",
            "Angular",
            "TypeScript",
            "AWS",
            "WPF",
            "WCF",
            "WWF",
            "Azure",
            ".Net Core"
    ];
    $("#jobkeyword").autocomplete({
        source: availableTags
    });


    // Hover states on the static widgets
    $("#dialog-link, #icons li").hover(
			function () {
			    $(this).addClass("ui-state-hover");
			},
			function () {
			    $(this).removeClass("ui-state-hover");
			}
		);
});


$(function () {
    var availableTags = [
            "Delhi",
            "Bangalore",
            "Hyderabad",
            "Kolkata",
            "Chandigarh",
            "Mumbai",
            "Pune",
            "Hyderabad",
            "Secunderabad",
            "Nagpur",
            "Gurgaon",
            "Noida",
            "Chennai",
            "Jaipur",
            "Kanpur",
            "Allahabad",
            "Patna",
            "Mysore",
            "Coimbatore",
            "Bhuwneshwar",
            "Ahmadabad",
            "Thane",
            "Silliguri",
            "Guwahati",
            "Ranchi",
            "Jamshedpur",
            "Vishakhapatnam"
    ];
    $("#jobbylocation").autocomplete({
        source: availableTags
    });


    // Hover states on the static widgets
    $("#dialog-link, #icons li").hover(
            function () {
                $(this).addClass("ui-state-hover");
            },
            function () {
                $(this).removeClass("ui-state-hover");
            }
        );
});


//$("#advSearchBtn").click(function () {
//    if ($("#advSearchBtn").html() == 'Reset') {
//        $("#advSearchBtn").html('Customize');
//        //$("#divAdvSearch").hide();

//        $("#minExpr").val('0');
//        $("#maxExpr").val('0');
//        $("#minSalary").val('0');
//        $("#postedOn").val('30');
//        $("#chkCompany").prop('checked', false);
//        $("#chkConsultant").prop('checked', false);
//    }
//    else {
//        $("#advSearchBtn").html('Reset');
//        //$("#divAdvSearch").show();
//    }
//});


/* Add list of keyword in search job page */
/* Removing this feature currently */

//function SelectedKeywords() {
//    var html = '';
//    var lstKeywords = $("#keywords").val().split(',');
//    $.each(lstKeywords, function (index, keyword) {
//        if (keyword != undefined && keyword != '')
//            html = html + '<span>' + keyword + '<i class="close-tag">x</i></span>';
//    });

//    if (lstKeywords.length > 0) {
//        html = html + '<div class="action-tags"><a href="#" title=""><i class="fa fa-trash"></i>Clean</a></div>';
//        $('#selectedKeywords').html(html);
//        $('#selectedKeywords').show();
//    }
//}

function LoadJobDetail() {
    $('#progressbarValue').css('width', '9%');

    $.ajax(
 {
     url: '/search/jobs/LoadJobDetail',
     type: 'POST',
     data: {},
     datatype: "json",
     contenttype: "application/json; charset=utf-8",
     success: function (result) {
         if (result.Status) {
             $('#pgtitle').html(result.Jobs.JobTitle);
             $('#skillSet').html(result.Jobs.SkillsSet);
             $('#Experience').html('Experience: ' + result.Jobs.Experience);
             $('#Designation').html(result.Jobs.Designation);
             $('#Qualification').html(result.Jobs.Education);
             $('#NotificationDtlUri').attr('href', result.Jobs.Url);
             $('#JobCode').html('Code: ' + result.Jobs.JobCode);
             $('#JobType').html('Position: ' + result.Jobs.JobType);

             $('[name=Location]').html(result.Jobs.Location + ' (' + result.Jobs.Country + ')');
             $('#Organization').html(result.Jobs.Organization);
             $('#PostedDate').html('Posted on: ' + result.Jobs.PostedDate);
             $('#JobExpireDate').html('Posted on: ' + result.Jobs.CloseDate);
             $('#Compensation').html('Compensation: ' + result.Jobs.Salary);

             //$('#ContactPerson').html('Contact Person: ' + result.Jobs.ContactPerson);
             $('#HrEmail').html('E-mail: ' + result.Jobs.ContactEmail);
             $('#HrPhone').html('Phone: ' + result.Jobs.ContactPhone);
             $('#CompUri').html('Website: ' + '<a href="' + result.Jobs.CompUri + '" target="_blank">' + result.Jobs.CompUri + '</a>');

             //$('#Qualification').val('Qualification: ' + result.Jobs.Qualification);
             $('#jobDesc').html('<h4>Job Description: </h4>' + result.Jobs.Description);
             $('#jobRoles').html('<h4>Responsiblities: </h4>' + result.Jobs.Responsibity);
             $('#aboutOrg').html('<h4>About Organization: </h4>' + result.Jobs.CompanyProfile);
             //CompanyLogo  
             $('#compLogoUri').attr('src', result.Jobs.CompUri + result.Jobs.CompanyLogo);
             $('#compLogoUri').attr('alt', result.Jobs.Organization);
             $("meta[name='description']").attr("content", (result.Jobs.Description).substr(0, 200));
             $("meta[name='og:description']").attr("content", result.Jobs.SkillsSet + ',' + result.Jobs.JobTitle + ', ' + result.Jobs.Designation + ', ' + result.Jobs.Education);
             $("meta[name='og:title']").attr("content", result.Jobs.JobTitle + ', ' + result.Jobs.Organization);
             $('#divProfile').show();
         }
         else {
             window.location.href = result.Redirect;
         }
     },
     error: function () {
     }
 });

    ManageLoader();
}


function UpdateJobHistory() {
    $.ajax(
 {
     url: '/search/jobs/UserJobSearchHistory',
     type: 'POST',
     data: { "id": 2, },
     datatype: "json",
     contenttype: "application/json; charset=utf-8",
     success: function (result) {
         //$(document).prop('title', 'Hello');
     },
     error: function () {
     }
 });
}

function PostJobApplication() {
    $.ajax({
        url: '/Talent/Profile/ApplyToJob',
        type: 'POST',
        data: { id: 0 },
        datatype: 'json',
        contenttype: "application/json; charset=utf-8",
        processdata: false,
        success: function (result) {
            if (result.Status) {
                $("#msgMsgAlert").attr("class", "alert alert-success alert-dismissible");
            }
            else {
                $("#msgMsgAlert").attr("class", "alert alert-danger alert-dismissible");
            }

            $("#msgMsgAlert").html(result.Message);
            $("#msgMsgAlert").show();
        },

        error: function () { }
    });
}

