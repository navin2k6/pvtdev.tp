$("#btnSearchTalents").click(function () {
    $.ajax(
    {
        url: '/Employer/SearchProfiles',
        type: 'POST',
        data: { "keyword": $("#keywords").val(), "location": $("#city").val(), "minExpr": $("#minExpr").val(), "maxExpr": $("#maxExpr").val(), "edu": $("#education").val(), "lastUpdated": $("#lastUpdated").val() },
        datatype: "json",
        contenttype: "application/json; charset=utf-8",
        success: function (result) {
            var html = '';
            var i = 0;

            if (result.Status && result.Profiles != null) {
                $.each(result.Profiles, function (obj) {
                    html = html + '<div class="resultSeparator"><a target="_blank" href="../talents/profile/' + result.Profiles[obj].ProfileId + '"><span class="resultTitle">' + result.Profiles[obj].Name + '</span></a><div>Experience: ' + result.Profiles[obj].TotExpr + ' years, ' + result.Profiles[obj].Education + ', City: ' + result.Profiles[obj].City + '</div><div>Skills Set: ' + result.Profiles[obj].PrimarySkills + ', ' + result.Profiles[obj].SecondarySkills + '</div><div><div class="postedDate">Last updated on ' + result.Profiles[obj].LastUpdatedOn + '</div></div></div>';
                    i++;
                });

                $('#resultSet').html(html);
            }

            $('#resultCount').html(i + ' results found');
        },
        error: function () {
        }
    });
});