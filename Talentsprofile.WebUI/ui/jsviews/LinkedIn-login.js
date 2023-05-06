function liAuth() {
    IN.User.authorize(function () {
        callback();
    });
}

// Setup an event listener to make an API call once auth is complete
function onLinkedInLoad() {
    IN.Event.on(IN, "auth", getProfileData);
}

// Use the API call wrapper to request the member's profile data
function getProfileData() {
    IN.API.Profile("me").fields("id", "first-name", "last-name", "headline", "location", "picture-url", "public-profile-url", "email-address", "summary", "specialties", "industry", "positions", "api-standard-profile-request").result(displayProfileData).error(onError);
}

// Handle the successful return from the API call
function displayProfileData(data) {
    var user = data.values[0];

    $.ajax({
        url: '/Login',
        type: 'POST',
        data: {
            email: JSON.stringify(user),
            pwd: 'linkedin',
            rememberMe: false,
        },
        success: function (result) {
            if (result.Status) {
                sessionStorage.setItem("talentsprofileUser", JSON.stringify(result.Token));
                window.location = document.location.origin + "/" + result.Url;
            }
            else {
                $('#msgAlert').html(result.Message);
                $('#msgAlert').show();
                $('#password').val('');
            }
        },
        error: function () {
        }
    });
}

// Handle an error response from the API call
function onError(error) {
    console.log(error);
}

// Destroy the session of linkedin
function logout() {
    IN.User.logout(removeProfileData);
}

// Remove profile data from page
function removeProfileData() {
    document.getElementById('profileData').remove();
}

//function GetHtmlPageData() {
//    $.ajax({
//        url: 'https://www.linkedin.com/in/navinpandit/',
//        type: 'GET',
//        //contentType: 'application/json',
//        dataType: 'jsonp',
//        //data: { q: idiom },
//        async: true,
//        crossDomain: true,
//        //responseType: 'application/json',
//        //xhrFields: {
//        //    withCredentials: false
//        //},
//        headers: {
//            'Access-Control-Allow-Origin': '*',
//            'Access-Control-Allow-Credentials': true,
//            'Access-Control-Allow-Methods': 'GET',
//            //'Access-Control-Allow-Headers': 'application/json',
//            'Access-Control-Allow-Headers': 'text/html',
//        },
//        //success: function (result) {
//        //    var html = JSON.stringify(result);
//        //},
//        success: function(data, status, xhr) {
//            var html = JSON.stringify(xhr);
//        },
//        error: function (xhtml, status, xhr) {
//            var html = xhtml;
//        }
//    });
//}