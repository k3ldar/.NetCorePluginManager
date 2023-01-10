let userAccount = (function () {
let _settings = {
updateMsg: '',
deliveryAddressUrl: '',
deleteAddressUrl: '',
deleteAddressCreateUrl: '',
trialLicenceUrl: '',
createDeliveryAddressBtn: '',
createTrialLicenceBtn: '',
licenceSendEmailUrl: '',
sendEmailLicenceBtn: ''
};
let root = {
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
deleteAddress: function (addressId) {
if (window.confirm("Are you sure you want to delete the address?")) {
let xhr = new XMLHttpRequest();
xhr.open("POST", _settings.deleteAddressUrl + '/' + addressId, true);
xhr.setRequestHeader('Content-Type', 'application/json');
xhr.send();
location.reload();
}
},
};
return root;
})();