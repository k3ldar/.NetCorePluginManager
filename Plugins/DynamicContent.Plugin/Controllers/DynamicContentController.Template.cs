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
 *  Copyright (c) 2018 - 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  DynamicContent.Plugin
 *  
 *  File: DynamicContentController.cs
 *
 *  Purpose:  Dynamic Content Controller
 *
 *  Date        Name                Reason
 *  12/06/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Web;

using DynamicContent.Plugin.Model;

using Microsoft.AspNetCore.Mvc;

using Middleware.Images;

using SharedPluginFeatures;

using static SharedPluginFeatures.Constants;

namespace DynamicContent.Plugin.Controllers
{
    public partial class DynamicContentController
    {
        [HttpGet]
        [AjaxOnly]
        public IActionResult TextTemplateEditor(string data)
        {
            return PartialView("/Views/DynamicContent/_TextTemplateEditor.cshtml", new TextTemplateEditorModel(HttpUtility.HtmlDecode(data)));
        }

        [HttpGet]
        [AjaxOnly]
        public IActionResult YouTubeTemplateEditor(string data)
        {
            return PartialView("/Views/DynamicContent/_YouTubeTemplateEditor.cshtml", new YouTubeTemplateEditorModel(HttpUtility.HtmlDecode(data)));
        }
    }
}
