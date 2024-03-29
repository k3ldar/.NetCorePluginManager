﻿let dynamicContent = (function () {
let _settings = {
dynamicContainer: '',
dynamicContainerId: '',
templateId: '',
templateUrl: '',
cacheId: '',
getDynamicUrlContent: '',
updatePositionUrl: '',
editDialogue: '',
editDialogueUrl: '',
errorList: '',
previewUrl: '',
deleteDialogue: '',
addTemplateUrl: '',
navLayout: '',
navSettings: '',
navControls: '',
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
root.updateTemplates();
$('#' + _settings.navLayout).click(function (e) {
root.layoutTabSelected();
});
$('#' + _settings.navSettings).click(function (e) {
root.settingsTabSelected();
});
});
},
layoutTabSelected: function () {
$('#' + _settings.navControls)[0].style.display = "block";
},
settingsTabSelected: function () {
$('#' + _settings.navControls)[0].style.display = "none";
},
updatePage: function (cacheId) {
$.ajax({
type: 'GET',
url: _settings.getDynamicUrlContent + _settings.cacheId,
cache: false,
success: function (response) {
if (response.success) {
$(_settings.dynamicContainer).html(response.responseData);
let editButtons = document.getElementsByClassName("editBtn");
for (let i = 0; i < editButtons.length; i++) {
editButtons[i].addEventListener('click', root.editClicked, false);
}
let deleteButtons = document.getElementsByClassName("deleteBtn");
for (let i = 0; i < deleteButtons.length; i++) {
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
let controls = [items[0].id];
for (let i = 1; i < items.length; i++) {
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
let url = _settings.editDialogueUrl + _settings.cacheId + '/' + control;
$(_settings.editDialogue).load(url, function () {
$(_settings.editDialogue).modal('show');
});
},
deleteClicked: function (e) {
let control = e.currentTarget.id;
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
let errMessage = response.responseData;
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
},
updateTemplates: function () {
$.ajax({
type: 'POST',
url: _settings.templateUrl,
cache: false,
success: function (response) {
$(_settings.templateId).html(response);
},
});
},
dragStart: function (event) {
event.dataTransfer.setData('text', event.target.id);
},
dragOver: function (event) {
event.preventDefault();
},
dropTemplate: function (event) {
let nextControl = "";
let nearliId = ""
let nearest = document.elementFromPoint(event.pageX, event.pageY);
if (nearest != null && nearest != undefined) {
let nearli = nearest.closest("li");
if (nearli != null && nearli != undefined) {
let nearul = nearli.closest("ul");
if (nearul != null && nearul != undefined) {
nearliId = nearli.id;
for (let i = 0; i < nearul.childNodes.length; i++) {
if (nearul.childNodes[i].id === nearliId) {
nextControl = nearul.childNodes[i].id;
break;
}
}
}
}
}
let addControlJson = {
cacheId: _settings.cacheId,
templateId: event.dataTransfer.getData('text'),
nextControl: nextControl
};
$.ajax({
type: 'POST',
url: _settings.addTemplateUrl,
data: addControlJson,
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
}
};
return root;
})();