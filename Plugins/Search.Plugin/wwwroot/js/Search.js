var searchPlugin = (function () {
    var _controls = {
        btnSubmit: '',
    };

    var root = {
        init: function (controls) {
            _controls = controls;

            $(document).ready(function () {
                $('#btnQuickSearch').click(function () {
                    $('#dlgQuickSearch').load('/Search/QuickSearch/', function () {
                        $('#dlgQuickSearch').modal('show');
                    });
                });
            });
        },

        initQuickSearch: function () {
            $('#idSearchText').on('change keydown paste input', function () {
                performQuickSearch();
            });
        },

        performQuickSearch: function () {
            debugger;
            let searchText = $('#idSearchText').text;

            $.ajax({
                url: '/Search/QuickKeywordSearch/' + searchText + '/',
                type: 'post',
                dataType: 'json',
                contentType: 'application/json',
                success: function (data) {
                    debugger;
                    $('#target').html(data.msg);
                },
                data: JSON.stringify(person)
            });
        },
    };

    return root;
})();