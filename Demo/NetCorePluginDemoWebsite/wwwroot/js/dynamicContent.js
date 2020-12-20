let dynamicContent = (function () {
let _settings = {
dynamicContainer: '',
dynamicContainerId: '',
cacheId: '',
getDynamicUrlContent: '',
updatePositionUrl: '',
editDialogue: '',
editDialogueUrl: '',
errorList: '',
previewUrl: '',
deleteDialogue: '',
};
let root = {
init: function (settings) {
_settings = settings;
$(document).ready(function () {
$(_settings.dynamicContainer).sortable({
receive: root.updated});
$(_settings.dynamicContainer).disableSelection();
$(_settings.dynamicContainer).sortable({
stop: root.updated
});
root.updatePage(_settings.cacheId);
});
},
updatePage: function (cacheId) {
$.ajax({
type: 'GET',
url: _settings.getDynamicUrlContent + _settings.cacheId,
cache: false,
success: function (response) {
if (response.success) {
$(_settings.dynamicContainer).html(response.data);
var editButtons = document.getElementsByClassName("editBtn");
for (var i = 0; i < editButtons.length; i++) {
editButtons[i].addEventListener('click', root.editClicked, false);
}
var deleteButtons = document.getElementsByClassName("deleteBtn");
for (var i = 0; i < deleteButtons.length; i++) {
deleteButtons[i].addEventListener('click', root.deleteClicked, false);
}
}
else {
$(_settings.dynamicContainer).html("Error retrieving data");
}
},
});
},
updated: function (event, ui) {
let left = ui.position.left;
let top = ui.position.top;
let ctl = ui.item[0].id;
let items = document.getElementById(_settings.dynamicContainerId).getElementsByTagName("li");
var controls = [items[0].id];
for (var i = 1; i < items.length; i++) {
controls.push(items[i].id);
}
let updateJson = {
cacheId: _settings.cacheId,
controlId: ctl,
controls: controls,
top: top,
left: left
};
$.ajax({
type: 'POST',
url: _settings.updatePositionUrl,
data: updateJson,
cache: false,
success: function (response) {
if (response.success) {
root.updatePage(_settings.cacheId);
}
else {
$(_settings.dynamicContainer).html("Error updating custom page");
}
},
error: function (xhr, ajaxOptions, thrownError) {
root.updatePage(_settings.cacheId);
}
});
},
editClicked: function (e) {
let control = e.currentTarget.id;
let customEditor = e.currentTarget.dataset.cs;
let url = _settings.editDialogueUrl + _settings.cacheId + '/' + control;
$(_settings.editDialogue).load(url, function () {
$(_settings.editDialogue).modal('show');
});
},
deleteClicked: function (e) {
let control = e.currentTarget.id;
let customEditor = e.currentTarget.dataset.cs;
let url = _settings.deleteDialogueUrl + _settings.cacheId + '/' + control;
$(_settings.deleteDialogue).load(url, function () {
$(_settings.deleteDialogue).modal('show');
});
},
submitData: function (form, route, values, method, encoding, dialog) {
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
if (response.success) {
$(dialog).modal('hide');
root.updatePage(_settings.cacheId);
}
else {
let errMessage = response.data;
$(_settings.errorList).html('<li>' + errMessage + '</li>');
}
},
error: function (xhr, ajaxOptions, thrownError) {
let errMessage = xhr.responseJSON.data;
$(_settings.errorList).html('<li>' + errMessage + '</li>');
}
})
},
submitTemplate: function (e) {
$(_settings.errorList).html('<li style="display:none"></li>');
let form = e.form;
let route = form.action;
let data = $(form).serializeArray();
let method = form.method;
let encoding = form.encoding;
root.submitData(form, route, data, method, encoding, _settings.editDialogue);
},
deleteItem: function (e) {
$(_settings.errorList).html('<li style="display:none"></li>');
let form = e.form;
let route = form.action;
let data = $(form).serializeArray();
let method = form.method;
let encoding = form.encoding;
root.submitData(form, route, data, method, encoding, _settings.deleteDialogue);
},
preview: function () {
let url = _settings.previewUrl + _settings.cacheId;
window.open(url, '_blank');
}
};
return root;
})();