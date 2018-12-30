var userAccount = (function () {
    var _settings = {
        updateMsg: '',
        deliveryAddressUrl: '',
        deleteAddressUrl: '',
        deleteAddressCreateUrl: '',
        createDeliveryAddressBtn: '',
    };

    var root = {
        init: function (settings) {
            _settings = settings;

            $(document).ready(function () {
                if (_settings.updateMsg !== '')
                    $.growl.notice({ title: 'Account Updated', message: _settings.updateMsg });
            });

            if (_settings.createDeliveryAddressBtn !== '') {
                $('#' + _settings.createDeliveryAddressBtn).click(function () {
                    location.replace(_settings.deleteAddressCreateUrl);
                })
            }
        },

        deleteAddress: function (addressId) {
            if (window.confirm("Are you sure you want to delete the address?")) {
                var xhr = new XMLHttpRequest();
                xhr.open("POST", _settings.deleteAddressUrl + '/' + addressId, true);
                xhr.setRequestHeader('Content-Type', 'application/json');
                xhr.send();
                location.reload();
            }
        },
    };

    return root;
})();