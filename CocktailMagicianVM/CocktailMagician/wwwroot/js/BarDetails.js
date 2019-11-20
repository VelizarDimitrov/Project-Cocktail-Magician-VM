const transparency = function () {
    $('body').attr('class', 'not-transparent-header');
}
window.onload = transparency;
let commentCount = 6;
let userRating;
var hoverStarOn = function (number) {
    for (var i = 1; i <= number; i++) {
        $(`#star-${i}`).attr("style", "color:#ED1C3B;");
    }
}
var hoverStarOff = function () {
    for (var i = 1; i <= 5; i++) {
        if (i > userRating) {
            $(`#star-${i}`).attr("style", "color:#CCCCCC;");
        }
    }
}
var starClick = function (number, id) {
    userRating = number;
    $.ajax({
        url: '/account/ratebar',
        type: "POST",
        data: { userRating: userRating, id: id }
    });
}

let loadRating = function (rating) {
    userRating = rating;
    for (var i = 1; i <= rating; i++) {
        $(`#star-${i}`).attr("style", "color:#ED1C3B;");
    }
}
let loadComments = function (barId) {

    $("#comments-partial").load('/bar/loadbarcomments', { barId: barId, commentCount: commentCount });
    commentCount = commentCount + 6;
}
loadRating($("#initial-rating").val());
loadComments($("#bar-id").val());

function searchEventHandler() {
    let rating = "0;5";
    let keyword = $('#bar-name').val();
    let criteria = "Bar";
    let page = 1;
    let rowSize = 3;
    $('#search-results').load('/cocktail/cocktailsearchresults', { keyword: keyword, criteria: criteria, page: page, rowSize: rowSize, rating: rating });
}
function changePage(number) {
    let rating = "0;5";
    let keyword = $('#bar-name').val();
    let criteria = "Bar";
    let page = parseInt($('#current-page').val()) + number;
    let rowSize = 3;
    $('#search-results').load('/cocktail/cocktailsearchresults', { keyword: keyword, criteria: criteria, page: page, rowSize: rowSize, rating: rating });
}
const favoriteThisBar = function () {
    let barId = $("#bar-id").val();
    $.ajax({
        url: '/account/favoritebar',
        type: "POST",
        data: { barId: barId },
        success: function () {
            $("#favorite-button").toggle();
            $("#unfavorite-button").toggle();
        }
        
    });
}
const unFavoriteThisBar = function () {
    let barId = $("#bar-id").val();
    $.ajax({
        url: '/bar/unfavoritebar',
        type: "POST",
        data: { barId: barId },
        success: function () {
            $("#unfavorite-button").toggle();
            $("#favorite-button").toggle();
        }

    });
}
const buttonONOrOff = function () {
    let barId = $("#bar-id").val();
    $.ajax({
        url: '/account/checkforfavoritebar',
        type: "GET",
        data: { barId: barId },
        success: function (result) {
            if (result=="exist") {
                $("#favorite-button").toggle();
            }
            else {
                $("#unfavorite-button").toggle();
            }
        }
    })


}
searchEventHandler();
buttonONOrOff();