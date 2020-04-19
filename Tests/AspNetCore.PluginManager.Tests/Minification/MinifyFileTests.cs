/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: MinifyFileTests.cs
 *
 *  Purpose:  Minification Tests
 *
 *  Date        Name                Reason
 *  23/01/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Minify
{
    [TestClass]
    public sealed class MinifyFileTests : MinifyTestBase
    {
        [TestMethod]
        public void MinifyRazorWithIfBlock()
        {
            const string csRazorInput = "@model SystemAdmin.Plugin.Models.AvailableIconViewModel\r\n@inject Microsoft.AspNetCore.Mvc.Localization.IViewLocalizer Localizer\r\n" +
                "@{\r\n                ViewData[\"Title\"] = \"Home Page\";\r\n            }\r\n            @using SharedPluginFeatures\r\n\r\n            <link " +
                "rel=\"stylesheet\" href=\"~/css/SystemAdmin.css\" asp-append-version=\"true\" />\r\n\r\n\r\n              <h1> @Model.Title </h1>\r\n              <div class=\"row\">\r\n" +
                "    <div class=\"col\">\r\n        @if(Model.HomeIcons != null)\r\n        {\r\n            foreach (SystemAdminMainMenu menu in Model.HomeIcons)\r\n" +
                "            {\r\n                <div class=\"systemIcon\" style=\"border: solid 1px @menu.BackColor()\">\r\n" +
                "                    <a asp-area=\"@menu.Area()\" asp-controller=\"@menu.Controller()\" asp-action=\"@menu.Action()\" asp-route-id=\"@menu.UniqueId\">\r\n" +
                "                        <h3 style=\"background-color: @menu.BackColor(); color: @menu.ForeColor()\"> @Localizer[menu.Name] </h3>\r\n" +
                "                        <img src=\"@Model.ProcessImage(menu.Image)\" />\r\n                    </a>\r\n                </div>\r\n            }\r\n" +
                "}\r\n        else\r\n        {\r\n            foreach (SystemAdminSubMenu menu in Model.MenuItems)\r\n            {\r\n" +
                "                <div class=\"systemIcon\" style=\"border: solid 1px @menu.BackColor()\">\r\n                    <a href=\"@Model.GetMenuLink(menu)\" >\r\n" +
                "                        <h3 style=\"background-color: @menu.BackColor(); color: @menu.ForeColor()\">@Localizer[menu.Name()]</h3>\r\n" +
                "                        <img src=\"@Model.ProcessImage(menu.Image())\" />\r\n                    </a>\r\n                </div>\r\n" +
                "\r\n            }\r\n        }\r\n    </div>\r\n</div>";
            const string csRazorOutput = "@model SystemAdmin.Plugin.Models.AvailableIconViewModel\n@inject Microsoft.AspNetCore.Mvc.Localization.IViewLocalizer Localizer\n" +
                "@{\nViewData[\"Title\"] = \"Home Page\";\n}\n@using SharedPluginFeatures\n<link rel=\"stylesheet\" href=\"~/css/SystemAdmin.css\" asp-append-version=\"true\" />\n" +
                "<h1> @Model.Title </h1>\n<div class=\"row\">\n<div class=\"col\">\n@if(Model.HomeIcons != null)\n" +
                "{\nforeach (SystemAdminMainMenu menu in Model.HomeIcons)\n{\n<div class=\"systemIcon\" style=\"border: solid 1px @menu.BackColor()\">\n" +
                "<a asp-area=\"@menu.Area()\" asp-controller=\"@menu.Controller()\" asp-action=\"@menu.Action()\" asp-route-id=\"@menu.UniqueId\">\n" +
                "<h3 style=\"background-color: @menu.BackColor(); color: @menu.ForeColor()\"> @Localizer[menu.Name] </h3>\n" +
                "<img src=\"@Model.ProcessImage(menu.Image)\" />\n</a>\n</div>\n}\n}\nelse\n{\nforeach (SystemAdminSubMenu menu in Model.MenuItems)\n" +
                "{\n<div class=\"systemIcon\" style=\"border: solid 1px @menu.BackColor()\">\n<a href=\"@Model.GetMenuLink(menu)\" >\n" +
                "<h3 style=\"background-color: @menu.BackColor(); color: @menu.ForeColor()\">@Localizer[menu.Name()]</h3>\n" +
                "<img src=\"@Model.ProcessImage(menu.Image())\" />\n</a>\n</div>\n}\n}\n</div>\n</div>";

            List<IMinifyResult> minifyResult = _minifyFileContents.MinifyData(MinificationFileType.Razor, csRazorInput, out string result);

            Assert.IsTrue(result.Equals(csRazorOutput));
        }

        [TestMethod]
        public void MinifyRazorHtmlFile()
        {
            const string csRazorInput = "@model SharedPluginFeatures.BaseModel\r\n@inject Microsoft.AspNetCore.Mvc.Localization.IViewLocalizer Localizer\r\n\r\n@{" +
                "\r\n    ViewData[\"Title\"] = \"Access Denied\";\r\n}\r\n\r\n<div class=\"row\">\r\n    <div class=\"col\">\r\n\r\n        <h1>@Localizer[nameof(" +
                "Languages.LanguageStrings.AccessDenied)]</h1>\r\n    </div>\r\n</div>\r\n<div class=\"row\">\r\n    <div class=\"col\">\r\n\r\n        <div class=\"errorMessage\">" +
                "\r\n            <p>@Localizer[nameof(Languages.LanguageStrings.AccessDeniedDescription)]</p>\r\n        </div>\r\n    </div>\r\n</div>\r\n";
            const string csRazorOutput = "@model SharedPluginFeatures.BaseModel\n@inject Microsoft.AspNetCore.Mvc.Localization.IViewLocalizer Localizer\n@{\n" +
                "ViewData[\"Title\"] = \"Access Denied\";\n}\n<div class=\"row\">\n<div class=\"col\">\n<h1>@Localizer[nameof(Languages.LanguageStrings.AccessDenied)]" +
                "</h1>\n</div>\n</div>\n<div class=\"row\">\n<div class=\"col\">\n<div class=\"errorMessage\">\n<p>@Localizer[nameof(" +
                "Languages.LanguageStrings.AccessDeniedDescription)]</p>\n</div>\n</div>\n</div>";

            List<IMinifyResult> minifyResult = _minifyFileContents.MinifyData(MinificationFileType.Razor, csRazorInput, out string result);

            Assert.IsTrue(result.Equals(csRazorOutput));
        }

        [TestMethod]
        public void MinifyJSFile()
        {
            const string jsFile = @"var systemAdmin = function () {
    let _settings = {
        seoPage: '',
        seoButton: '',
        seoModal: '',
    };

    let that = {

        init: function (settings) {
            debugger;
            _settings = settings;

            $(document).ready(function () {
                debugger;
            });
        }
    };

    return that;
}();

";
            List<IMinifyResult> minifyResult = _minifyFileContents.MinifyData(MinificationFileType.Js, jsFile, out string result);

            Assert.IsTrue(result.Equals("var systemAdmin = function () {\nlet _settings = {\nseoPage: '',\nseoButton: '',\nseoModal: '',\n};\nlet that = {\ninit: function (settings) {\ndebugger;\n_settings = settings;\n$(document).ready(function () {\ndebugger;\n});\n}\n};\nreturn that;\n}();"));
            Assert.AreEqual(minifyResult[minifyResult.Count - 1].EndLength, 236);
        }

        [TestMethod]
        public void MinifyRazorPreservingRazorCode()
        {
            const string tempDataWithRazor = "@model SharedPluginFeatures.BaseModel\r\n\r\n\r\n@inject Microsoft.AspNetCore.Mvc.Localization.IViewLocalizer Localizer\r\n\r\n" +
                "@{\r\n                ViewData[\"Title\"] = \"HighVolume\";\r\n  }\r\n\r\n<div class=\"row\">\r\n\t<div class=\"col\">\r\n\r\n<h1>@Localizer[nameof(" +
                "Languages.LanguageStrings.UnableToProcessRequest)]</h1>\r\n\t</div>  \r\n</div>\r\n";
            const string tempDataResult = "@model SharedPluginFeatures.BaseModel\n@inject Microsoft.AspNetCore.Mvc.Localization.IViewLocalizer Localizer\n@{\nViewData" +
                "[\"Title\"] = \"HighVolume\";\n}\n<div class=\"row\">\n<div class=\"col\">\n<h1>@Localizer[nameof(Languages.LanguageStrings.UnableToProcessRequest)]</h1>\n</div>\n</div>";

            List<IMinifyResult> minifyResult = _minifyFileContents.MinifyData(MinificationFileType.Razor, tempDataWithRazor, out string result);

            Assert.IsTrue(result.Equals(tempDataResult));
            Assert.AreEqual(minifyResult[minifyResult.Count - 1].EndLength, tempDataResult.Length);
        }

        [TestMethod]
        public void MinifyTestIgnorePre()
        {
            const string tempDataWithPreBlock = "<html><body><h1>Test</h1><pre><!-- ignored --></pre></body></html>";
            List<IMinifyResult> minifyResult = _minifyFileContents.MinifyData(MinificationFileType.Htm, tempDataWithPreBlock, out string result);

            Assert.IsTrue(result.Equals(tempDataWithPreBlock));
            Assert.AreEqual(66, minifyResult[4].EndLength);
        }

        [TestMethod]
        public void MinifyTestIgnoreLinesAndTabsInPreBlock()
        {
            const string tempDataWithPreBlock = "<html><body><h1>Test</h1>\r\n<pre>\t\r\n\t<!-- ignored -->\r\n\t\t\r\n</pre></body></html>";

            List<IMinifyResult> minifyResult = _minifyFileContents.MinifyData(MinificationFileType.Htm, tempDataWithPreBlock, out string result);

            Assert.IsTrue(result.Equals("<html><body><h1>Test</h1>\n<pre>\t\n\t<!-- ignored -->\n\t\t\n</pre></body></html>"));
            Assert.AreEqual(74, minifyResult[4].EndLength);
        }
    }
}
