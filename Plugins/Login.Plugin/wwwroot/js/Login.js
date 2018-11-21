var loginPlugin = (function () {
    var _controls = {
        forgotPassword: ''
    };

    var root = {
        init: function (controls) {
            _controls = controls;

            $(document).ready(function () {
                $(_controls.forgotPassword).on("click", function () {
                    window.location.replace("/Login/ForgotPassword");
                });
            });
        }
    };

    return root;
})();