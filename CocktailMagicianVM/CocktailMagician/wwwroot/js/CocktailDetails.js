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
        url: '/account/ratecocktail',
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

    $("#comments-partial").load('/cocktail/loadcocktailcomments', { barId: barId, commentCount: commentCount });
    commentCount = commentCount + 6;
}
loadRating($("#initial-rating").val());
loadComments($("#cocktail-id").val());

function searchEventHandler() {
    let rating = "0;5";
    let keyword = $('#cocktail-name').val();
    let criteria = "Cocktail";
    let page = 1;
    let pageSize = 4;
    $('#search-results').load('/bar/barsearchresults', { keyword: keyword, criteria: criteria, page: page, rating: rating, pageSize: pageSize });
}
function changePage(number) {
    let rating = "0;5";
    let keyword = $('#cocktail-name').val();
    let criteria = $('Cocktail').val();
    let page = parseInt($('#current-page').val()) + number;
    let pageSize = 4;
    $('#search-results').load('/bar/barsearchresults', { keyword: keyword, criteria: criteria, page: page, rating: rating, pageSize: pageSize });
}
const favoriteThisCocktail = function () {
    let cocktailId = $("#cocktail-id").val();
    $.ajax({
        url: '/account/favoritecocktail',
        type: "POST",
        data: { cocktailId: cocktailId },
        success: function () {
            $("#favorite-button").toggle();
            $("#unfavorite-button").toggle();
        }

    });
}
const unFavoriteThisCocktail = function () {
    let cocktailId = $("#cocktail-id").val();
    $.ajax({
        url: '/cocktail/unfavoritecocktail',
        type: "POST",
        data: { cocktailId: cocktailId },
        success: function () {
            $("#unfavorite-button").toggle();
            $("#favorite-button").toggle();
        }

    });
}
const buttonONOrOff = function () {
    let cocktailId = $("#cocktail-id").val();
    $.ajax({
        url: '/account/checkforfavoritecocktail',
        type: "GET",
        data: { cocktailId: cocktailId },
        success: function (result) {
            if (result == "exist") {
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