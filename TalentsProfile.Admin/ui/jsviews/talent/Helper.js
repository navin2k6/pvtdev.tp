/* Loads related jobs */
function LoadProfileJobs() {
    $.ajax({
        url: '/Talent/Profile/GetProfileJobs',
        type: 'POST',
        data: {},
        datatype: "json",
        contenttype: "application/json; charset=utf-8",
        processdata: false,
        success: function (profileJobs) {
            var html = "";
            var _for = "";
            if (profileJobs[0].Desc == "" || profileJobs[0].Desc == undefined) {
                html = html + '<a href="#" class="list-group-item"><span class="badge"></span>You have not provided required details.</br>Complete your profile for job notifications related to your profile.</a>';
            } else {
                $.each(profileJobs, function (obj) {
                    html = html + '<a href="#" class="list-group-item"><span class="badge">' + profileJobs[obj].JobsCount + '</span><span name="jobKeywords" val=' + profileJobs[obj].Keywords+'>' + profileJobs[obj].Desc + '</span></a>';
                    $("#btnViewDtlJobNotify").show();
                });
            }
            $("#profileJobs").html(html);
        },
        error: function () {
        }
    });
}
