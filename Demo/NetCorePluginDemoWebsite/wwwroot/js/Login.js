let loginPlugin = (function () {
let _controls = {
btnForgotPassword: '',
btnSubmit: '',
};
let root = {
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