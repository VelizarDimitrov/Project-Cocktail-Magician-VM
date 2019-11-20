function unFreezeUser(userId) {
    $.ajax({
        url: '/administration/admin/unfreezeuser',
        type: "POST",
        data: { userId: userId },
        success: function () {
            changePage(0);
        }
    });
}
function freezeUser(userId) {
    $.ajax({
        url: '/administration/admin/freezeuser',
        type: "POST",
        data: { userId: userId },
        success: function () {
            changePage(0);
        }
    });
}
function promoteUser(userId) {
    $.ajax({
        url: '/administration/admin/promoteuser',
        type: "POST",
        data: { userId: userId },
        success: function () {
            changePage(0);
        }
    });
}
function demoteUser(userId) {
    $.ajax({
        url: '/administration/admin/demoteuser',
        type: "POST",
        data: { userId: userId },
        success: function () {
            changePage(0);
        }
    });
}

const transparency = function () {
    $('body').attr('class', 'not-transparent-header');
}
window.onload = transparency;

function changePage(number) {
    let keyword = $('#current-keyword').val();
    let page = parseInt($('#current-page').val()) + number;
    let pageSize = 15;
    $('#search-results').load('/administration/admin/usersearchresults', { keyword: keyword, page: page, pageSize: pageSize });
}
function searchEventHandler() {
    let keyword = $('#keyword').val();
    let page = 1;
    let pageSize = 15;
    $('#search-results').load('/administration/admin/usersearchresults', { keyword: keyword, page: page, pageSize: pageSize });
}
searchEventHandler();