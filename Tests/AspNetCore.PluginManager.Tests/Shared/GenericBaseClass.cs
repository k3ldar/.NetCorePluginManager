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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: HtmlTextTemplateTests.cs
 *
 *  Purpose:  Tests for html text template
 *
 *  Date        Name                Reason
 *  27/03/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

using System.Text.Json.Serialization;
using System.Threading;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Shared
{
    [ExcludeFromCodeCoverage]
    public class GenericBaseClass
    {
        protected bool MethodHasAttribute<T>(Type classType, string methodName) where T : Attribute
        {
            if (classType == null)
                throw new ArgumentNullException(nameof(classType));

            if (String.IsNullOrEmpty(methodName))
                throw new ArgumentNullException(nameof(methodName));

            MethodInfo methodInfo = classType.GetMethod(methodName);

            if (methodInfo == null)
                throw new InvalidOperationException($"Method {methodName} does not exist");

            return methodInfo.IsDefined(typeof(T));
        }

        protected bool PropertyHasJsonAttribute(Type classType, string propertyName, string value)
        {
            if (classType == null)
                throw new ArgumentNullException(nameof(classType));

            if (String.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            var propertyInfo = classType.GetProperty(propertyName);

            if (propertyInfo == null)
                throw new InvalidOperationException($"Method {propertyName} does not exist");

            if (!propertyInfo.IsDefined(typeof(JsonPropertyNameAttribute)))
                throw new InvalidOperationException($"Method {propertyName} does not chave JsonPropertyName attribute");

            JsonPropertyNameAttribute attribute = propertyInfo.GetCustomAttributes<JsonPropertyNameAttribute>().FirstOrDefault();

            return attribute.Name.Equals(value);
        }

        protected BaseModelData GenerateTestBaseModelData()
        {
            BaseModelData Result = new BaseModelData(new List<BreadcrumbItem>(),
                new ShoppingCartSummary(1, 0, 0, 0, 0, 20, Thread.CurrentThread.CurrentUICulture, "GBP"),
                "The Title", "The Author", "The Description", "The Tags", false);


            return Result;
        }
    }
}
