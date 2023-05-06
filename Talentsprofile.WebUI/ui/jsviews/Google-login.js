var OAUTHURL = 'https://accounts.google.com/o/oauth2/auth?';
var VALIDURL = 'https://www.googleapis.com/oauth2/v1/tokeninfo?access_token=';
var SCOPE = 'https://www.googleapis.com/auth/userinfo.profile https://www.googleapis.com/auth/userinfo.email';
var CLIENTID = '554526791771-jqjraa543grpohsq561f1607fssi8g4d.apps.googleusercontent.com';
var REDIRECT = 'http://talentsprofile.com';
var LOGOUT = 'http://talentsprofile.com';
var TYPE = 'token';
var _url = OAUTHURL + 'scope=' + SCOPE + '&client_id=' + CLIENTID + '&redirect_uri=' + REDIRECT + '&response_type=' + TYPE;
var acToken;
var tokenType;
var expiresIn;
var user;
var loggedIn = false;

function login() {
    var win = window.open(_url, "Talentsprofile.com", 'width=800, height=600');
    var pollTimer = window.setInterval(function () {
        try {
            console.log(win.document.URL);
            if (win.document.URL.indexOf(REDIRECT) != -1) {
                window.clearInterval(pollTimer);
                var url = win.document.URL;
                acToken = gup(url, 'access_token');
                tokenType = gup(url, 'token_type');
                expiresIn = gup(url, 'expires_in');
                win.close();
                debugger;
                validateToken(acToken);
            }
        }
        catch (e) {
        }
    }, 500);
}

function gup(url, name) {
    namename = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\#&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(url);
    if (results == null)
        return "";
    else
        return results[1];
}

function validateToken(token) {
    getUserInfo();
    $.ajax(
        {
            url: VALIDURL + token,
            data: null,
            success: function (responseText) {

            },
        });
}

function getUserInfo() {
    $.ajax({
        url: 'https://www.googleapis.com/oauth2/v1/userinfo?access_token=' + acToken,
        data: null,
        success: function (resp) {
            user = resp;

            $.ajax({
                url: '/Login',
                type: 'POST',
                data: {
                    email: JSON.stringify(user),
                    pwd: 'google',
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
        },
    });
}
