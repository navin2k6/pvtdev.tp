//$('#viewOrgList').click(function () {
//    $.get('../adm/dashboard/GetPartial', function (data) {
//        $('#contentArea').html(data);

//        LoadOrganization(null);
//    })
//});

function LoadOrganization(id) {
    $.ajax({
        url: '../adm/dashboard/GetOrganization',
        type: 'POST',
        data: {
            'id': id
        },
        datatype: "json",
        contenttype: "application/json; charset=utf-8",
        processdata: false,
        success: function (result) {
            if (result.Code == 200) {
                var html = '';
                $.each(result.Data, function (row) {
                    html = html + '<tr><th scope="row"><a href="#" name="addNewJob" data-toggle="modal" data-target="#manageJobProfile"><i class="fa fa-plus-circle" title="Add Job" aria-hidden="true" name="orgValue" value=' + result.Data[row].Id + '></i></a></th><td>' + result.Data[row].OrganizationName + '</td><td>' + result.Data[row].City + '</td><td>' + result.Data[row].Category + '</td><td>' + result.Data[row].Sector + '</td></tr>';
                });

                $('#orgRows').html(html);
            }
        },
        error: function () {
        }
    });
}

//txtOrganizationId
$(document).on('click', '[name=addNewJob]', function (thisEvnt) {
    var child = $(this).children(thisEvnt)[0];
    $('#txtOrganizationId').val(child.attributes["value"].value);
});