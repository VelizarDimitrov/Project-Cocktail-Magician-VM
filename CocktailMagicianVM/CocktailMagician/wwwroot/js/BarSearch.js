
    function NextPage() {
        var Keyword = $('#current-keyword').val();
        var Criteria = $('#current-criteria').val();
        var Order = $('#current-order').val();
        var Page = parseInt($('#current-page').val()) + 1;
        $('#search-results').load('/catalog/barsearchresults', { keyword: Keyword, criteria: Criteria, order: Order, page:Page });
    }
        function SearchEventHandler() {
            var Keyword = $('#keyword').val();
            var Criteria = $('#criteria').val();
            var Order = $('#order').val();
            var Page = 1;
            $('#search-results').load('/catalog/barsearchresults', { keyword: Keyword, criteria: Criteria, order: Order, page:Page });
    }
    
    function PrevPage() {
        var Keyword = $('#current-keyword').val();
        var Criteria = $('#current-criteria').val();
        var Order = $('#current-order').val();
        var Page = parseInt($('#current-page').val()) - 1;
        $('#search-results').load('/catalog/barsearchresults', { keyword: Keyword, criteria: Criteria, order: Order, page:Page });
    }
