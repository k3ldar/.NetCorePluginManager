let systemAdmin = function () {
let _settings = {
seoPage: '',
seoButton: '',
seoModal: '',
partialViewContent: '',
controllerRoot: '',
};
let that = {
init: function (settings) {
_settings = settings;
$(document).ready(function () {
});
},
submitData: function (form, route, values, method, encoding) {
$.ajax({
type: method,
url: route,
data: values,
contentType: encoding,
cache: false,
success: function (response) {
$(_settings.partialViewContent).html(response);
$(form).removeData("validator");
$(form).removeData("unobtrusiveValidation");
$.validator.unobtrusive.parse("form");
that.hookForms();
$(window).scrollTop(0);
},
})
},
submitHook: function (e) {
e.preventDefault();
let form = e.currentTarget;
let route = form.action;
let data = $(form).serializeArray();
let method = form.method;
let encoding = form.encoding;
that.submitData(form, route, data, method, encoding);
},
hookForms: function () {
let formsCollection = document.forms;
for (let i = 0; i < formsCollection.length; i++) {
let form = formsCollection[i];
if (form.action.includes(_settings.controllerRoot)) {
if (form.attachEvent) {
form.attachEvent("submit", systemAdmin.submitHook);
} else {
form.addEventListener("submit", systemAdmin.submitHook);
}
}
}
}
};
return that;
}();