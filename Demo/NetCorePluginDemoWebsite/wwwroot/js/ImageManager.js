var imageManager = (function () {
var _options = {
submitButton: '',
showDeleteButton: '',
submitForm: '',
confirmDeleteId: '',
confirmMessage: '',
deleteImageDialog: '',
errorId: '',
group: '',
subgroup: '',
extraData: '',
};
var root = {
init: function (options) {
_options = options;
$(document).ready(function () {
$(_options.errorId).hide();
$(_options.confirmMessage).hide();
$(_options.submitButton).click(root.validateAndSubmit);
$(_options.showDeleteButton).click(function () {
$(_options.confirmMessage).hide();
$(_options.deleteImageDialog).modal({
backdrop: 'static'
});
});
});
},
validateAndSubmit: function (e) {
var confirmed = $(_options.confirmDeleteId)[0].checked;
if (confirmed === undefined || confirmed === false) {
$(_options.confirmMessage).show();
e.preventDefault();
return false;
}
$(_options.confirmMessage).hide();
$('#ConfirmDelete').val(true);
var form = $(_options.submitForm);
if (form == null) {
return false;
}
var content = new Object();
content.ImageName = $('#ImageName').val();
content.GroupName = $('#GroupName').val();
content.SubgroupName = $('#SubgroupName').val();
content.ConfirmDelete = $('#ConfirmDelete').val() === "true";
var json = JSON.stringify(content);
$.ajax({
url: '/ImageManager/DeleteImage/',
type: 'POST',
data: json,
contentType: "application/json; charset=utf-8",
dataType: "json",
success: function (data) {
if (content.SubgroupName === "") {
window.location.href = "/ImageManager/ViewGroup/" + content.GroupName + "/";
}
else {
window.location.href = "/ImageManager/ViewSubgroup/" + content.GroupName + "/" + content.SubgroupName + "/";
}
},
error: function (jqXHR, textStatus, errorThrown) {
$(_options.errorId).show();
}
});
},
};
return root;
})();