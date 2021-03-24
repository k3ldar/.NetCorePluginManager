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
 *  Copyright (c) 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: BaseMiddlewareTests.cs
 *
 *  Purpose:  Base class for MVC Middleware class
 *
 *  Date        Name                Reason
 *  20/02/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace AspNetCore.PluginManager.Tests.MiddlewareTests
{
    [ExcludeFromCodeCoverage]
    public class BaseMiddlewareTests : TestBasePlugin
    {
        protected const string TestCategoryMiddleware = "MiddlewareTests";

        public bool ClassHasAttribute<T>(Type classType) where T : Attribute
        {
            if (classType == null)
                throw new ArgumentNullException(nameof(classType));

            return classType.IsDefined(typeof(T));
        }

        public bool ClassHasAttributeUsageFlag(Type classType, AttributeTargets targets)
        {
            if (classType == null)
                throw new ArgumentNullException(nameof(classType));

            if (!classType.IsDefined(typeof(AttributeUsageAttribute)))
                return false;

            object[] attributes = classType.GetCustomAttributes(typeof(AttributeUsageAttribute), false);

            if (attributes.Length > 1)
                throw new InvalidOperationException("Can only test attribute usage when single instance applied");

            AttributeUsageAttribute attribute = attributes[0] as AttributeUsageAttribute;

            if (attribute == null)
                throw new InvalidOperationException("AttributeUsageAttribute not found");

            return attribute.ValidOn == targets;
        }

        public bool ClassAttributeUsageAllowsMultiple(Type classType)
        {
            if (classType == null)
                throw new ArgumentNullException(nameof(classType));

            if (!classType.IsDefined(typeof(AttributeUsageAttribute)))
                return false;

            object[] attributes = classType.GetCustomAttributes(typeof(AttributeUsageAttribute), false);

            if (attributes.Length > 1)
                throw new InvalidOperationException("Can only test attribute usage when single instance applied");

            AttributeUsageAttribute attribute = attributes[0] as AttributeUsageAttribute;

            if (attribute == null)
                throw new InvalidOperationException("AttributeUsageAttribute not found");

            return attribute.AllowMultiple;
        }

    }
}
