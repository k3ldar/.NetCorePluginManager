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
 *  File: TestDynamicTemplate.cs
 *
 *  Purpose:  Designed to test abstract class
 *
 *  Date        Name                Reason
 *  25/03/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using SharedPluginFeatures;
using SharedPluginFeatures.DynamicContent;

namespace AspNetCore.PluginManager.Tests.Plugins.DynamicContentTests
{
    [ExcludeFromCodeCoverage]
    internal class TestDynamicTemplate : DynamicContentTemplate
    {
        public override string AssemblyQualifiedName => "no assembly";

        public override string EditorAction => "not applicable";

        public override string Name => "test only template";

        public override int SortOrder { get; set; }
        public override DynamicContentWidthType WidthType { get; set; }
        public override int Width { get; set; }
        public override DynamicContentHeightType HeightType { get; set; }
        public override int Height { get; set; }
        public override string Data { get; set; }
        public override DateTime ActiveFrom { get; set; }
        public override DateTime ActiveTo { get; set; }

        public override string Content()
        {
            return Data;
        }

        public override DynamicContentTemplate Clone(string uniqueId)
        {
            if (String.IsNullOrEmpty(uniqueId))
                uniqueId = Guid.NewGuid().ToString();

            return new TestDynamicTemplate()
            {
                UniqueId = uniqueId
            };
        }

        public void TestHtmlStart(StringBuilder stringBuilder)
        {
            HtmlStart(stringBuilder);
        }

        public void TestHtmlEnd(StringBuilder stringBuilder)
        {
            HtmlEnd(stringBuilder);
        }
    }
}
