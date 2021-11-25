﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SharedPluginFeatures
 *  
 *  File: AjaxOnlyAttribute.cs
 *
 *  Purpose:  Attribute for MVC methods only accepting ajax calls
 *
 *  Date        Name                Reason
 *  15/11/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SharedPluginFeatures
{
    public sealed class ApiAuthorizationAttribute : ActionFilterAttribute
    {
        private readonly string _policyName;

        public ApiAuthorizationAttribute()
            : this(string.Empty)
        {

        }

        public ApiAuthorizationAttribute(string policyName)
        {
            _policyName = policyName;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            IApiAuthorizationService apiAuthorizationService = context.HttpContext.RequestServices.GetService(typeof(IApiAuthorizationService)) as IApiAuthorizationService;

            if (apiAuthorizationService == null)
            {
                context.HttpContext.Response.StatusCode = Constants.HtmlResponseMethodNotAllowed;
                context.Result = new JsonResult(new { response = "Not allowed" });
                return;
            }

            if (!apiAuthorizationService.ValidateApiRequest(context.HttpContext.Request, _policyName, out int responseCode))
            {
                context.HttpContext.Response.StatusCode = responseCode;
                context.Result = new JsonResult(new { response = "Invalid Request" });
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
