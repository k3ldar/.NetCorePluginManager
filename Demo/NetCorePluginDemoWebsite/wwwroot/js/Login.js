var loginPlugin = (function () {
var _controls = {
btnForgotPassword: '',
btnSubmit: '',
};
var root = {
init: function (controls) {
_controls = controls;
$(document).ready(function () {
if (_controls.btnForgotPassword !== undefined) {
$(_controls.btnForgotPassword).on("click", function () {
window.location.replace("/Login/ForgotPassword");
});
}
});
},
};
return root;
})();