var searchPlugin = (function () {
var _controls = {
btnSubmit: '',
};
var _options = {
searchControl: '',
form: '',
searchButton: '',
urlControl: '',
validation: '',
minSearchLength: 3,
searchId: '',
};
var _currentFocus;
var _keywords = null;
var root = {
init: function (controls) {
_controls = controls;
$(document).ready(function () {
$('#btnQuickSearch').click(function () {
$('#dlgQuickSearch').load('/Search/QuickSearch/', function () {
$('#dlgQuickSearch').modal('show');
});
});
});
},
initQuickSearch: function (options) {
_options = options;
$(_options.searchControl).on("keyup", function () {
let value = $(this).val();
root.performQuickSearch(value, this);
});
$(_options.validation).hide();
$(_options.searchControl).keydown(root.quickSearchKeyDown);
$(_options.searchControl).on("input", root.quickSearchInput);
$(_options.searchButton).click(root.quickSearch);
},
quickSearch: function (e) {
var text = $(_options.searchControl)[0].value;
if (text === undefined || text.length < _options.minSearchLength) {
$(_options.validation).show();
e.preventDefault();
return false;
}
$(_options.validation).hide();
document.getElementById(_options.form).submit();
},
performQuickSearch: function (s, ctrl) {
kwords = new String(s);
if (kwords.length < _options.minSearchLength) {
_keywords = null;
root.closeAllLists();
return;
}
content = new Object();
content.keywords = kwords;
content.searchId = "123456";
$.ajax({
url: '/Search/QuickKeywordSearch/',
type: 'POST',
data: JSON.stringify(content),
contentType: "application/json; charset=utf-8",
dataType: "json",
success: function (data) {
_keywords = data;
root.quickSearchBuildLists(ctrl);
}
});
},
quickSearchInput: function (e) {
var val = this.value;
if (!val) {
return false;
}
root.quickSearchBuildLists(this);
},
quickSearchBuildLists: function(ctrl) {
root.closeAllLists();
if (_keywords === null || _keywords === undefined) {
return false;
}
var prntDiv = document.createElement("div");
prntDiv.setAttribute("id", this.id + "autocomplete-list");
prntDiv.setAttribute("class", "autocomplete-items");
ctrl.parentNode.appendChild(prntDiv);
for (var i = 0; i < _keywords.length; i++) {
var itemDiv = document.createElement("div");
itemDiv.innerHTML += _keywords[i].response;
itemDiv.innerHTML += "<input type='hidden' value='" + _keywords[i].url + "'>";
itemDiv.addEventListener("click", function (e) {
root.closeAllLists();
window.location.replace(this.getElementsByTagName("input")[0].value);
});
prntDiv.appendChild(itemDiv);
}
},
quickSearchKeyDown: function (e) {
var items = document.getElementById(this.id + "autocomplete-list");
if (items)
items = items.getElementsByTagName("div");
if (e.keyCode == 40) {
_currentFocus++;
root.activeAdd(items);
} else if (e.keyCode == 38) {
_currentFocus--;
root.activeAdd(items);
} else if (e.keyCode == 13) {
if (_currentFocus > -1) {
e.preventDefault();
if (items) {
items[_currentFocus].click();
}
}
}
},
closeAllLists: function (elmnt) {
let itms = document.getElementsByClassName("autocomplete-items");
for (var i = 0; i < itms.length; i++) {
if (elmnt != itms[i] && elmnt != _options.searchControl) {
itms[i].parentNode.removeChild(itms[i]);
}
}
},
activeRemove: function (itm) {
for (var i = 0; i < itm.length; i++) {
itm[i].classList.remove("autocomplete-active");
}
},
activeAdd: function (itm) {
if (!itm)
return false;
root.activeRemove(itm);
if (_currentFocus >= itm.length)
_currentFocus = 0;
if (_currentFocus < 0)
_currentFocus = (itm.length - 1);
itm[_currentFocus].classList.add("autocomplete-active");
}
};
return root;
})();