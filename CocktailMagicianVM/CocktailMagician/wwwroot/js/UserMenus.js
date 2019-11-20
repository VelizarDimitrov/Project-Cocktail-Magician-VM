const userProfile = function () {
    $('#user-view').load('/account/userprofilepartial');
    removeActive();
    $("#user-profile").attr("class", "active");

}

const userProfileEdit = function () {
    $('#user-view').load('/account/userprofileupdate');
    removeActive();
}

const userDashboard = function () {
    let page = 1;
    let pageSize = 6;
    $('#user-view').load('/account/userdashboard', { page: page, pageSize: pageSize });

    removeActive();
    $("#user-dashboard").attr("class", "active");
}
const userBars = function () {
    $('#user-view').load('/account/favoritebarspartial');
    removeActive();
    $("#user-bars").attr("class", "active");
}

const userCocktails = function () {
    $('#user-view').load('/account/favoritecocktailspartial');
    removeActive();
    $("#user-cocktails").attr("class", "active");
}
const changePassword = function () {
    $('#user-view').load('/account/userpasswordupdate');
    removeActive();
    $("#user-password").attr("class", "active");
}
const removeActive = function () {
    $('#user-buttons li').each(function () {
        $(this).attr("class", "");
    });
}

function changePage(number) {
    let page = parseInt($('#current-page').val()) + number;
    let pageSize = 6;
    $('#user-view').load('/account/userdashboard', { page: page, pageSize: pageSize });
}
function checkIfFree(username) {
    $.ajax({
        url: '/auth/checkifusernameavailable',
        type: "GET",
        data: { username: username },
        success: function (result) {
            const userSpan = $('#check-username');
            if (result === 'unavailable' && $("#hidden-username").val() != username) {
                userSpan.removeAttr('hidden');
                document.getElementById("register-button").disabled = true;
            }
            else {
                userSpan.attr('hidden', 'hidden');
                document.getElementById("register-button").disabled = false;
            }

        }
    });
}
const checkIfPassConfirmed = function () {
    let password = document.getElementById("password").value;
    let confirmed = document.getElementById('password-confirm').value;
    const passValidSpan = $('#password-validation');
    const passLengthSpan = $('#password-length');
    if (password === confirmed) {
        passValidSpan.attr('hidden', 'hidden');
        document.getElementById("register-button").disabled = false;
    }
    else {
        passValidSpan.removeAttr('hidden', 'hidden');
        document.getElementById("register-button").disabled = true;
    }
    if (password.length < 6) {
        passLengthSpan.removeAttr('hidden', 'hidden');
        document.getElementById("register-button").disabled = true;
    }
    else {
        passLengthSpan.attr('hidden', 'hidden');
        document.getElementById("register-button").disabled = false;
    }
}
const checkIfPasswordCorrect = function (password) {
    //let password = document.getElementById("existing-password").val()
    const passCorrectSpan = $('#password-check');
    $.ajax({
        url: '/auth/checkifpasswordiscorrect',
        type: "GET",
        data: { password: password },
        success: function (result) {
            if (result === 'incorrect') {
                passCorrectSpan.removeAttr('hidden');
                document.getElementById("register-button").disabled = true;
            }
            else {
                passCorrectSpan.attr('hidden', 'hidden');
                document.getElementById("register-button").disabled = false;
            }
        }
    });
}
const checkPartial = function () {
    const result = $("#initial").val();
    if (result=="userPage") {
        userProfile();
    }
    if (result=="favoriteBars") {
        userBars();
    }
    if (result=="favoriteCocktails") {
        userCocktails();
    }
}


