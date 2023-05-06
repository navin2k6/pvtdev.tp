function ValidateForm() {
    if ($("#ddlCurrency").val() == 'selected') {
        alert('Please select salary range');
        return false;
    }
    else if ($("#ddlCountry").val() == 'selected') {
        return false;
    }
    else if ($("#publishType").prop('checked')) {
        if ($("#txtPostDate").val() == '')
            return false;
    }
}


$("#formPostJob").submit(function () {
    //    if (ValidateForm()) {

    //    }
    //    else
    //        return false;
});



$("[name=publishType]").change(function () {
    if ($("[name=publishType]:radio:checked").val() == 2) {
        $("#divPublishDate").show();
    } else { $("#divPublishDate").hide(); }
});

$("[name=experience]").change(function () {
    if ($("[name=experience]:radio:checked").val() == 2) {
        $("#divExperience").show();
    } else { $("#divExperience").hide(); }
});

function LoadPostedWalkins() {
    LoadJobs(true);
}

function LoadPostedJobs() {
    LoadJobs(false);
}

function LoadJobs(jobType) {
    $.ajax({
        url: '/employer/dashboard/LoadPostedJobs',
        type: 'POST',
        data: { jobType: jobType },
        datatype: "json",
        contenttype: "application/json; charset=utf-8",
        processdata: false,
        success: function (result) {
            if (result.Status) {
                var html = '';
                var i = 0;
                var toUrl = (jobType) ? 'walkins' : 'postjob';

                $.each(result.Jobs, function (obj) {
                    html = html + '<tr><td>' + (i+1) + '</td><td>' + result.Jobs[obj].PublishOn + '</td><td><a name="jobTitle" href="../employer/' + toUrl + '?edit=' + result.Jobs[obj].JobId + '">' + result.Jobs[obj].JobTitle + '</a></td><td>' + result.Jobs[obj].Experience + '</td><td>' + result.Jobs[obj].JobRefCode + '</td><td>' + result.Jobs[obj].SkillsSet + '</td><td>' + result.Jobs[obj].JobStatus + '</td></tr>';
                    i++;
                });

                if(i==0)
                {
                    html = '<tr><td></td><td colspan="6">No jobs available</td></tr>';
                }

                $('#postedJobs').html(html);
                $('#loader').html('');
            }
        },
        error: function () {
            $('#loader').html('');
        }
    });
}