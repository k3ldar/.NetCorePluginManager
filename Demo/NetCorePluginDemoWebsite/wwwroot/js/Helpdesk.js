let helpdesk = (function () {
let _settings = {
growlTitle: '',
updateMsg: '',
};
let root = {
init: function (settings) {
_settings = settings;
$(document).ready(function () {
if (_settings.updateMsg !== '')
$.growl.notice({ title: _settings.growlTitle, message: _settings.updateMsg });
});
if (_settings.createDeliveryAddressBtn !== '') {
$('#' + _settings.createDeliveryAddressBtn).click(function () {
location.replace(_settings.deleteAddressCreateUrl);
})
}
if (_settings.createTrialLicenceBtn !== '') {
$('#' + _settings.createTrialLicenceBtn).click(function () {
location.replace(_settings.trialLicenceUrl);
})
}
if (_settings.sendEmailLicenceBtn !== '') {
$('#' + _settings.sendEmailLicenceBtn).click(function () {
location.replace(_settings.licenceSendEmailUrl);
})
}
},
};
return root;
})();