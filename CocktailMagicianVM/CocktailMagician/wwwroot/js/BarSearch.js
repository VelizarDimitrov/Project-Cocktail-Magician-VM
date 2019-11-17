
function changePage(number) {
    let rating;
    var sortSpan = $('#sort_order_span');
    let sort = sortSpan.text();
    if (document.getElementById('refine-result').className === "collapse") {
        rating = "0;5";
    }
    else {
        rating = $('#price_range').val();
    }
    let keyword = $('#current-keyword').val();
    let criteria = $('#current-criteria').val();
    let order = $('#current-order').val();
    let page = parseInt($('#current-page').val()) + number;
    let pageSize = 10;
    $('#search-results').load('/bar/barsearchresults', { keyword: keyword, criteria: criteria, order: order, page: page, rating: rating, sortOrder: sort, pageSize: pageSize});
}
function searchEventHandler() {
    let rating;
    var sortSpan = $('#sort_order_span');
    let sort = sortSpan.text();
    if (document.getElementById('refine-result').className === "collapse") {
        rating = "0;5";
    }
    else {
        rating = $('#price_range').val();
    }
    let keyword = $('#keyword').val();
    let criteria = $('#criteria').val();
    let order = $('#order').val();
    let page = 1;
    let pageSize = 10;
    $('#search-results').load('/bar/barsearchresults', { keyword: keyword, criteria: criteria, order: order, page: page, rating: rating, sortOrder: sort, pageSize: pageSize });
}

const changeSortMethod = function (id) {
    if (id === 'sort_checkbox-1') {
        $("#sort_checkbox-2").prop("checked", false);
        $("#sort_checkbox-3").prop("checked", false);
    }
    else if (id === 'sort_checkbox-2') {
        $("#sort_checkbox-1").prop("checked", false);
        $("#sort_checkbox-3").prop("checked", false);
    }
    else if (id === 'sort_checkbox-3') {
        $("#sort_checkbox-2").prop("checked", false);
        $("#sort_checkbox-1").prop("checked", false);
    }

    if (document.getElementById(id).checked === true) {
        $("#order").val(document.getElementById(id).value);
    }
    else {
        $("#order").val(null);
    }
    changeSorting();
}
const changeSortOrder = function () {
    var sortSpan = $('#sort_order_span');
    if (sortSpan.text() === "Ascending") {
        sortSpan.text("Descending");
    }
    else {
        sortSpan.text("Ascending")
    }
    changeSorting();
}

var moreFilters = 0;

const ExtraFilters = function () {
    var sortSpan = $('#sort_order_span');
    let sort = sortSpan.text();
    let rating;
    if (moreFilters === 1) {
        rating = "0;5";
        moreFilters = 0;
    }
    else {
        rating = $('#price_range').val();
        moreFilters = 1;
    }
    let keyword = $('#keyword').val();
    let criteria = $('#criteria').val();
    let order = $('#order').val();
    let page = 1;
    let pageSize = 10;
    $('#search-results').load('/bar/barsearchresults', { keyword: keyword, criteria: criteria, order: order, page: page, rating: rating, sortOrder: sort, pageSize: pageSize });
}

function changeRatingFilter() {
    var sortSpan = $('#sort_order_span');
    let sort = sortSpan.text();
    let rating = $('#price_range').val();
    let keyword = $('#current-keyword').val();
    let criteria = $('#current-criteria').val();
    let order = $('#current-order').val();
    let page = 1;
    let pageSize = 10;
    $('#search-results').load('/bar/barsearchresults', { keyword: keyword, criteria: criteria, order: order, page: page, rating: rating, sortOrder: sort, pageSize: pageSize });
}

function changeSorting() {
    var sortSpan = $('#sort_order_span');
    let sort = sortSpan.text();
    let rating;
    if (document.getElementById('refine-result').className === "collapse") {
        rating = "0;5";
    }
    else {
        rating = $('#price_range').val();
    }
    let keyword = $('#current-keyword').val();
    let criteria = $('#current-criteria').val();
    let order = $('#order').val();
    let page = 1;
    let pageSize = 10;
    $('#search-results').load('/bar/barsearchresults', { keyword: keyword, criteria: criteria, order: order, page: page, rating: rating, sortOrder: sort, pageSize: pageSize });
}