const systemAdmin = (function () {
    let _settings = {
        seoPage: '',
        seoButton: '',
        seoModal: '',
    };

    const root = {
        init: function (settings) {
            _settings = settings;

            $(document).ready(function () {

            });
        },

        openSeoDialog: function (e) {
            $(_settings.seoModal).modal('show');
        },
    };

    return root;
})();

