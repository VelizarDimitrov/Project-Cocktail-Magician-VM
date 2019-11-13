
function nextPage() {
    let rating;
    let mainIngredient;
    var sortSpan = $('#sort_order_span');
    let sort = sortSpan.text();
    if (document.getElementById('refine-result').className === "collapse") {
        rating = "0;5";
        mainIngredient = "";
    }
    else {
        rating = $('#price_range').val();
        mainIngredient = $('#main-ingredient').val();
    }
    let keyword = $('#current-keyword').val();
    let criteria = $('#current-criteria').val();
    let order = $('#current-order').val();
    let page = parseInt($('#current-page').val()) + 1;
    $('#search-results').load('/catalog/cocktailsearchresults', { keyword: keyword, criteria: criteria, order: order, page: page, rating: rating, sortOrder: sort, mainIngredient: mainIngredient });
}
function searchEventHandler() {
    let rating;
    var sortSpan = $('#sort_order_span');
    let sort = sortSpan.text();
    if (document.getElementById('refine-result').className === "collapse") {
        rating = "0;5";
        mainIngredient = "";
    }
    else {
        rating = $('#price_range').val();
        mainIngredient = $('#main-ingredient').val();
    }
    let keyword = $('#keyword').val();
    let criteria = $('#criteria').val();
    let order = $('#order').val();
    let page = 1;
    $('#search-results').load('/catalog/cocktailsearchresults', { keyword: keyword, criteria: criteria, order: order, page: page, rating: rating, sortOrder: sort, mainIngredient: mainIngredient });
}

function prevPage() {
    let rating;
    var sortSpan = $('#sort_order_span');
    let sort = sortSpan.text();
    if (document.getElementById('refine-result').className === "collapse") {
        rating = "0;5";
        mainIngredient = "";
    }
    else {
        rating = $('#price_range').val();
        mainIngredient = $('#main-ingredient').val();
    }
    let keyword = $('#current-keyword').val();
    let criteria = $('#current-criteria').val();
    let order = $('#current-order').val();
    let page = parseInt($('#current-page').val()) - 1;
    $('#search-results').load('/catalog/cocktailsearchresults', { keyword: keyword, criteria: criteria, order: order, page: page, rating: rating, sortOrder: sort, mainIngredient: mainIngredient });
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

const searchWithoutExtraFilters = function () {
    var sortSpan = $('#sort_order_span');
    let sort = sortSpan.text();
    let rating = "0;5";
    let mainIngredient = "";
    let keyword = $('#keyword').val();
    let criteria = $('#criteria').val();
    let order = $('#order').val();
    let page = 1;
    $('#search-results').load('/catalog/cocktailsearchresults', { keyword: keyword, criteria: criteria, order: order, page: page, rating: rating, sortOrder: sort, mainIngredient: mainIngredient });
}

const searchWithExtraFilters = function () {
    var sortSpan = $('#sort_order_span');
    let sort = sortSpan.text();
    let rating = $('#price_range').val();
    let mainIngredient = $('#main-ingredient').val();
    let keyword = $('#current-keyword').val();
    let criteria = $('#current-criteria').val();
    let order = $('#current-order').val();
    let page = 1;
    $('#search-results').load('/catalog/cocktailsearchresults', { keyword: keyword, criteria: criteria, order: order, page: page, rating: rating, sortOrder: sort, mainIngredient: mainIngredient });
}