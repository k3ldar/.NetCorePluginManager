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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
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
using System;
using System.Runtime.CompilerServices;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

#pragma warning disable CS1591

namespace SharedPluginFeatures
{
	[AttributeUsage(AttributeTargets.Method)]
	public class AjaxOnlyAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (context == null)
				throw new ArgumentNullException(nameof(context));

			if (!IsAjaxRequest(context.HttpContext.Request))
			{
				context.HttpContext.Response.StatusCode = 404;
				context.Result = new StatusCodeResult(404);
			}
			else
			{
				base.OnActionExecuting(context);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsAjaxRequest(HttpRequest request)
		{
			if (request.Headers != null && request.Headers.ContainsKey("X-Requested-With"))
			{
				string requestedWith = request.Headers["X-Requested-With"];
				return requestedWith.Equals("XmlHttpRequest", StringComparison.OrdinalIgnoreCase);
			}

			return false;
		}
	}
}

#pragma warning restore CS1591