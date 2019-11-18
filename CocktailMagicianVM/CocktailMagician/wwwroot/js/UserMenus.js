const userProfile = function () {
    $('#user-view').load('/account/userprofilepartial');
    removeActive();
    $("#user-profile").attr("class", "active");

}

const userDashboard = function () {
    $('#user-view').load('/account/userdashboard');
    removeActive();
    $("#user-dashboard").attr("class", "active");
}
const userBars = function () {
    $('#user-view').load('/account/userprofile');
    removeActive();
    $("#user-bars").attr("class", "active");
}

const userCocktails = function () {
    $('#user-view').load('/account/userprofile');
    removeActive();
    $("#user-cocktails").attr("class", "active");
}
const userPassword = function () {
    $('#user-view').load('/account/userpasswordupdate');
    removeActive();
    $("#user-password").attr("class", "active");
}
const removeActive = function () {
    $('#user-buttons li').each(function () {
        $(this).attr("class", "");
    });
}