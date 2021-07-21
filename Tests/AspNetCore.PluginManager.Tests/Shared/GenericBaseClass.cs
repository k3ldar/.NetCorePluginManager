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
using System.IO;
using System.Linq;
using System.Reflection;

using System.Text.Json.Serialization;
using System.Threading;

using SharedPluginFeatures;

using sc = Shared.Classes;

namespace AspNetCore.PluginManager.Tests.Shared
{
    [ExcludeFromCodeCoverage]
    public class GenericBaseClass
    {
        protected readonly static DateTime DefaultActiveFrom = new DateTime(2020, 1, 1, 0, 0, 0);
        protected readonly static DateTime DefaultActiveTo = new DateTime(2050, 12, 31, 23, 59, 59);

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

        protected void ExtractImageResources(in string directory)
        {
            Assembly testAssembly = Assembly.GetExecutingAssembly();
            foreach (string resource in testAssembly.GetManifestResourceNames())
            {
                if (String.IsNullOrEmpty(resource))
                    continue;

                using (Stream stream = testAssembly.GetManifestResourceStream(resource))
                {
                    string resourceFileName = GetLiveFilePath(directory, resource);

                    if (!Directory.Exists(directory))
                        Directory.CreateDirectory(directory);

                    if (File.Exists(resourceFileName))
                        File.Delete(resourceFileName);

                    using (Stream fileStream = File.OpenWrite(resourceFileName))
                    {
                        byte[] buffer = new byte[stream.Length];

                        stream.Read(buffer, 0, buffer.Length);
                        fileStream.Write(buffer, 0, buffer.Length);
                    }
                }
            }
        }
        
        protected bool CacheManagerExists(string cacheName)
        {
            for (int i = 0; i < sc.CacheManager.GetCount(); i++)
            {
                if (sc.CacheManager.GetCacheName(i).Equals(cacheName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
        private string GetLiveFilePath(in string directory, in string resourceName)
        {
            // remove the first part of the name which is the library
            string Result = resourceName;

            int lastIndex = Result.LastIndexOf('\\');

            while (lastIndex > 0)
            {
                Result = Result.Substring(lastIndex);
                lastIndex = Result.LastIndexOf('\\');
            }

            Result = Path.Combine(directory, Result).Replace("AspNetCore.PluginManager.Tests.Properties.Images.", "");

            return Result;
        }

    }
}
