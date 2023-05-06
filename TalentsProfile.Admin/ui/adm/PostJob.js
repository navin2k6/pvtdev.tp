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
        url: '/adm/jobs/LoadPostedJob',
        type: 'POST',
        data: { 'iswalkins': jobType },
        datatype: "json",
        contenttype: "application/json; charset=utf-8",
        processdata: false,
        success: function (result) {
            if (result.Status == 200) {
                var html = '';
                var i = 0;
                var toUrl = (jobType) ? 'walkins' : 'postjob';

                $.each(result.Data, function (obj) {
                    html = html + '<tr><td>' + (i + 1) + '</td><td>' + result.Data[obj].PublishOn + '</td><td><a name="jobTitle" href="../employer/' + toUrl + '?edit=' + result.Data[obj].JobId + '">' + result.Data[obj].JobTitle + '</a></td><td>' + result.Data[obj].Experience + '</td><td>' + result.Data[obj].JobRefCode + '</td><td>' + result.Data[obj].SkillsSet + '</td><td>' + result.Data[obj].JobStatus + '</td></tr>';
                    i++;
                });

                if (i == 0) {
                    html = '<tr><td></td><td colspan="6">No jobs available</td></tr>';
                }

                $('#postedJobs').html(html);
                $('#loader').html('');
            }
        },
        error: function (xhr, status, error) {
            $('#loader').html('');
        }
    });
}