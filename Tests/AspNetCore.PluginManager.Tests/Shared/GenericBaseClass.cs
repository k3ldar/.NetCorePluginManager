/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
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
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;

using System.Text.Json.Serialization;
using System.Threading;

using DynamicContent.Plugin.Templates;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;
using Middleware.DynamicContent;

using SharedPluginFeatures;

using sc = Shared.Classes;

namespace AspNetCore.PluginManager.Tests.Shared
{
    [ExcludeFromCodeCoverage]
    public class GenericBaseClass
    {
        protected readonly static DateTime DefaultActiveFrom = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        protected readonly static DateTime DefaultActiveTo = new DateTime(2050, 12, 31, 23, 59, 59, DateTimeKind.Utc);

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

        protected bool PropertyHasDisplayAttribute(Type classType, string propertyName, string nameValue)
        {
            if (classType == null)
                throw new ArgumentNullException(nameof(classType));

            if (String.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            var propertyInfo = classType.GetProperty(propertyName);

            if (propertyInfo == null)
                throw new InvalidOperationException($"Property {propertyName} does not exist");

            if (!propertyInfo.IsDefined(typeof(DisplayAttribute)))
                throw new InvalidOperationException($"Property {propertyName} does not have DisplayAttribute attribute");

            DisplayAttribute attribute = propertyInfo.GetCustomAttributes<DisplayAttribute>().FirstOrDefault();

            return attribute.Name.Equals(nameValue);
        }

        protected bool PropertyHasRequiredAttribute(Type classType, string propertyName, string errorValue)
        {
            if (classType == null)
                throw new ArgumentNullException(nameof(classType));

            if (String.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            var propertyInfo = classType.GetProperty(propertyName);

            if (propertyInfo == null)
                throw new InvalidOperationException($"Property {propertyName} does not exist");

            if (!propertyInfo.IsDefined(typeof(RequiredAttribute)))
                throw new InvalidOperationException($"Property {propertyName} does not have RequiredAttribute attribute");

            RequiredAttribute attribute = propertyInfo.GetCustomAttributes<RequiredAttribute>().FirstOrDefault();

            return attribute.ErrorMessage.Equals(errorValue);
        }

        protected bool PropertyHasJsonAttribute(Type classType, string propertyName, string value)
        {
            if (classType == null)
                throw new ArgumentNullException(nameof(classType));

            if (String.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            var propertyInfo = classType.GetProperty(propertyName);

            if (propertyInfo == null)
                throw new InvalidOperationException($"Property {propertyName} does not exist");

            if (!propertyInfo.IsDefined(typeof(JsonPropertyNameAttribute)))
                throw new InvalidOperationException($"Property {propertyName} does not have JsonPropertyName attribute");

            JsonPropertyNameAttribute attribute = propertyInfo.GetCustomAttributes<JsonPropertyNameAttribute>().FirstOrDefault();

            return attribute.Name.Equals(value);
        }

        protected IBaseModelData GenerateTestBaseModelData()
        {
            return new BaseModelData(new List<BreadcrumbItem>(),
                new ShoppingCartSummary(1, 0, 0, 0, 0, 20, Thread.CurrentThread.CurrentUICulture, "GBP"),
                "The Title", "The Author", "The Description", "The Tags", false, false, true);
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

        protected string GetLiveFilePath(in string directory, in string resourceName)
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

        protected void ValidateSystemAdminColumnCounts(string data)
        {
            bool first = true;
            int colCount = 0;
            int rowColCount = 0;
            int line = 0;

            foreach (char c in data)
            {
                if (first)
                {
                    if (c == '|')
                    {
                        colCount++;
                    }

                    if (c == '\r')
                    {
                        first = false;
                        line++;
                    }
                }
                else
                {
                    if (c == '|')
                    {
                        rowColCount++;
                    }

                    if (c == '\r')
                    {
                        Assert.AreEqual(rowColCount, colCount, $"Column counts do not match on line {line}");

                        line++;
                        rowColCount = 0;
                    }
                }
            }

            if (line > 0)
                Assert.AreEqual(rowColCount, colCount, "Column counts do not match on last line");
        }

        protected IDynamicContentPage GetPage1()
        {
            IDynamicContentPage Result = new DynamicContentPage()
            {
                Id = 1,
                Name = "Custom Page 1",
                RouteName = "Page-1",
            };

            HtmlTextTemplate htmlLayout1 = new HtmlTextTemplate()
            {
                UniqueId = "1",
                SortOrder = 0,
                WidthType = DynamicContentWidthType.Columns,
                Width = 12,
                HeightType = DynamicContentHeightType.Automatic,
                Data = "<p>This is <br />html over<br />three lines</p>"
            };

            Result.Content.Add(htmlLayout1);

            return Result;
        }

        protected IDynamicContentPage GetPage2()
        {
            IDynamicContentPage Result = new DynamicContentPage()
            {
                Id = 2,
                Name = "Custom Page 2",
                RouteName = "Page-2",
            };

            HtmlTextTemplate htmlLayout1 = new HtmlTextTemplate()
            {
                UniqueId = "control-1",
                SortOrder = 0,
                WidthType = DynamicContentWidthType.Columns,
                Width = 12,
                HeightType = DynamicContentHeightType.Automatic,
                Data = "<p>This is <br />html over<br />three lines</p>"
            };

            Result.Content.Add(htmlLayout1);

            HtmlTextTemplate htmlLayout2 = new HtmlTextTemplate()
            {
                UniqueId = "control-2",
                SortOrder = 2,
                WidthType = DynamicContentWidthType.Columns,
                Width = 4,
                HeightType = DynamicContentHeightType.Automatic,
                Data = "<p>This is html<br />over two lines</p>"
            };

            Result.Content.Add(htmlLayout2);

            return Result;
        }

        protected IDynamicContentPage GetPage3()
        {
            IDynamicContentPage Result = new DynamicContentPage()
            {
                Id = 3,
                Name = "Custom Page 3",
                RouteName = "Page-3",
            };

            HtmlTextTemplate htmlLayout1 = new HtmlTextTemplate()
            {
                UniqueId = "control-1",
                SortOrder = 0,
                WidthType = DynamicContentWidthType.Columns,
                Width = 12,
                HeightType = DynamicContentHeightType.Automatic,
                Data = "<p>This is <br />html over<br />three lines</p>"
            };

            Result.Content.Add(htmlLayout1);

            SpacerTemplate spacerTemplate1 = new SpacerTemplate()
            {
                SortOrder = 1,
                Width = 8
            };

            Result.Content.Add(spacerTemplate1);

            HtmlTextTemplate htmlLayout2 = new HtmlTextTemplate()
            {
                UniqueId = "control-2",
                SortOrder = 2,
                WidthType = DynamicContentWidthType.Columns,
                Width = 4,
                HeightType = DynamicContentHeightType.Automatic,
                Data = "<p>This is html<br />over two lines</p>"
            };

            Result.Content.Add(htmlLayout2);

            return Result;
        }

        protected IDynamicContentPage GetPage10()
        {
            IDynamicContentPage Result = new DynamicContentPage()
            {
                Id = 10,
                Name = "Custom Page 10",
                RouteName = "Page-10"
            };

            HtmlTextTemplate htmlLayout1 = new HtmlTextTemplate()
            {
                UniqueId = "control-1",
                SortOrder = 0,
                WidthType = DynamicContentWidthType.Columns,
                Width = 12,
                HeightType = DynamicContentHeightType.Automatic,
                Data = "<p>This is <br />html over<br />three lines</p>"
            };

            Result.Content.Add(htmlLayout1);

            HtmlTextTemplate htmlLayout2 = new HtmlTextTemplate()
            {
                UniqueId = "control-2",
                SortOrder = 2,
                WidthType = DynamicContentWidthType.Columns,
                Width = 4,
                HeightType = DynamicContentHeightType.Automatic,
                Data = "<p>This is html<br />Content 2</p>"
            };

            Result.Content.Add(htmlLayout2);

            HtmlTextTemplate htmlLayout3 = new HtmlTextTemplate()
            {
                UniqueId = "control-3",
                SortOrder = 9,
                WidthType = DynamicContentWidthType.Columns,
                Width = 4,
                HeightType = DynamicContentHeightType.Automatic,
                Data = "<p>This is html<br />Content 3</p>"
            };

            Result.Content.Add(htmlLayout3);

            HtmlTextTemplate htmlLayout4 = new HtmlTextTemplate()
            {
                UniqueId = "control-4",
                SortOrder = 8,
                WidthType = DynamicContentWidthType.Columns,
                Width = 4,
                HeightType = DynamicContentHeightType.Automatic,
                Data = "<p>This is html<br />Content 4</p>"
            };

            Result.Content.Add(htmlLayout4);

            HtmlTextTemplate htmlLayout5 = new HtmlTextTemplate()
            {
                UniqueId = "control-5",
                SortOrder = 7,
                WidthType = DynamicContentWidthType.Columns,
                Width = 4,
                HeightType = DynamicContentHeightType.Automatic,
                Data = "<p>This is html<br />Content 5</p>"
            };

            Result.Content.Add(htmlLayout5);

            HtmlTextTemplate htmlLayout6 = new HtmlTextTemplate()
            {
                UniqueId = "control-6",
                SortOrder = 6,
                WidthType = DynamicContentWidthType.Columns,
                Width = 4,
                HeightType = DynamicContentHeightType.Automatic,
                Data = "<p>This is html<br />Content 6</p>"
            };

            Result.Content.Add(htmlLayout6);

            HtmlTextTemplate htmlLayout7 = new HtmlTextTemplate()
            {
                UniqueId = "control-7",
                SortOrder = 5,
                WidthType = DynamicContentWidthType.Columns,
                Width = 4,
                HeightType = DynamicContentHeightType.Automatic,
                Data = "<p>This is html<br />Content 7</p>"
            };

            Result.Content.Add(htmlLayout7);

            HtmlTextTemplate htmlLayout8 = new HtmlTextTemplate()
            {
                UniqueId = "control-8",
                SortOrder = 4,
                WidthType = DynamicContentWidthType.Columns,
                Width = 4,
                HeightType = DynamicContentHeightType.Automatic,
                Data = "<p>This is html<br />Content 8</p>"
            };

            Result.Content.Add(htmlLayout8);

            HtmlTextTemplate htmlLayout9 = new HtmlTextTemplate()
            {
                UniqueId = "control-9",
                SortOrder = 3,
                WidthType = DynamicContentWidthType.Columns,
                Width = 4,
                HeightType = DynamicContentHeightType.Automatic,
                Data = "<p>This is html<br />Content 9</p>"
            };

            Result.Content.Add(htmlLayout9);

            HtmlTextTemplate htmlLayout10 = new HtmlTextTemplate()
            {
                UniqueId = "control-10",
                SortOrder = 20,
                WidthType = DynamicContentWidthType.Columns,
                Width = 4,
                HeightType = DynamicContentHeightType.Automatic,
                Data = "<p>This is html<br />Content 10</p>"
            };

            Result.Content.Add(htmlLayout10);

            return Result;
        }

    }
}
