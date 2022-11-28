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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: MockLoadData.cs
 *
 *  Purpose:  Mock ILoadData class
 *
 *  Date        Name                Reason
 *  29/11/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using DynamicContent.Plugin.Templates;

using Middleware;
using Middleware.DynamicContent;

using PluginManager.Abstractions;

using SharedPluginFeatures.DynamicContent;

namespace AspNetCore.PluginManager.DemoWebsite.Classes.Mocks
{
    [ExcludeFromCodeCoverage]
    public class MockDynamicContentProvider : IDynamicContentProvider
    {
        #region Private Members

        private readonly IPluginClassesService _pluginClassesService;
        private static List<DynamicContentTemplate> _templates;
        private readonly List<IDynamicContentPage> _dynamicContent;
        private IDynamicContentPage _dynamicContentPage1;
        private IDynamicContentPage _dynamicContentPage2;
        private IDynamicContentPage _dynamicContentPage3;
        private IDynamicContentPage _dynamicContentPage10;

        #endregion Private Members

        #region Constructors

        public MockDynamicContentProvider(IPluginClassesService pluginClassesService, bool useDefaultContent)
        {
            _pluginClassesService = pluginClassesService ?? throw new ArgumentNullException(nameof(pluginClassesService));
            _dynamicContent = new List<IDynamicContentPage>();
            UseDefaultContent = useDefaultContent;
            AllowSavePage = true;
        }

        public MockDynamicContentProvider(IPluginClassesService pluginClassesService)
            : this(pluginClassesService, true)
        {

        }

        #endregion Constructors

        public bool UseDefaultContent { get; set; }

        public bool AllowSavePage { get; set; }

        public bool HasSaveUserInput { get; set; }

        public long CreateCustomPage()
        {
            return 50;
        }

        public void AddPage(IDynamicContentPage dynamicContentPage)
        {
            _dynamicContent.Add(dynamicContentPage ?? throw new ArgumentNullException(nameof(dynamicContentPage)));
        }

        public void ClearAllPages()
        {
            _dynamicContent.Clear();
        }

        #region IDynamicContentProvider Members

        public List<LookupListItem> GetCustomPageList()
        {
            List<LookupListItem> Result = new List<LookupListItem>();

            if (UseDefaultContent)
            {
                Result.Add(new LookupListItem((int)GetPage1().Id, GetPage1().Name));
                Result.Add(new LookupListItem((int)GetPage2().Id, GetPage2().Name));
                Result.Add(new LookupListItem((int)GetPage3().Id, GetPage3().Name));
                Result.Add(new LookupListItem((int)GetPage10().Id, GetPage10().Name));
            }


            _dynamicContent.ForEach(dc => Result.Add(new LookupListItem((int)dc.Id, dc.Name)));

            return Result;
        }

        public List<IDynamicContentPage> GetCustomPages()
        {
            List<IDynamicContentPage> Result = new List<IDynamicContentPage>();

            if (UseDefaultContent)
            {
                Result.Add(GetPage1());
                Result.Add(GetPage2());
                Result.Add(GetPage3());
                Result.Add(GetPage10());
            }

            _dynamicContent.ForEach(dc => Result.Add(dc));

            return Result;
        }

        public IDynamicContentPage GetCustomPage(long id)
        {
            if (UseDefaultContent && id == 1)
            {
                return GetPage1();
            }

            if (UseDefaultContent && id == 2)
            {
                return GetPage2();
            }

            if (UseDefaultContent && id == 3)
            {
                return GetPage3();
            }

            if (UseDefaultContent && id == 10)
            {
                return GetPage10();
            }

            return _dynamicContent.FirstOrDefault(dc => dc.Id.Equals(id));
        }

        public List<DynamicContentTemplate> Templates()
        {
            if (_templates != null)
                return _templates;

            _templates = _pluginClassesService.GetPluginClasses<DynamicContentTemplate>();

            return _templates;
        }

        public bool PageNameExists(long id, string pageName)
        {
            return GetCustomPages().Where(p => p.Id != id && p.Name.Equals(pageName, StringComparison.InvariantCultureIgnoreCase)).Any();
        }

        public bool RouteNameExists(long id, string routeName)
        {
            return GetCustomPages().Where(p => p.Id != id && p.RouteName.Equals(routeName, StringComparison.InvariantCultureIgnoreCase)).Any();
        }

        public bool Save(IDynamicContentPage dynamicContentPage)
        {
            return AllowSavePage;
        }

        public bool SaveUserInput(string data)
        {
            HasSaveUserInput = true;

            return HasSaveUserInput;
        }

        #endregion IDynamicContentProvider Members

        #region Private Methods

        private IDynamicContentPage GetPage1()
        {
            if (_dynamicContentPage1 == null)
            {
                _dynamicContentPage1 = new DynamicContentPage()
                {
                    Id = 1,
                    Name = "Custom Page 1",
                    RouteName = "page-1"
                };

                HtmlTextTemplate htmlLayout1 = new HtmlTextTemplate()
                {
                    UniqueId = "1",
                    SortOrder = 0,
                    WidthType = SharedPluginFeatures.DynamicContentWidthType.Columns,
                    Width = 12,
                    HeightType = SharedPluginFeatures.DynamicContentHeightType.Automatic,
                    Data = "<p>This is <br />html over<br />three lines</p>"
                };

                _dynamicContentPage1.Content.Add(htmlLayout1);
            }

            return _dynamicContentPage1;
        }

        private IDynamicContentPage GetPage2()
        {
            if (_dynamicContentPage2 == null)
            {
                _dynamicContentPage2 = new DynamicContentPage()
                {
                    Id = 2,
                    Name = "Custom Page 2",
                    RouteName = "page-2",
                };

                HtmlTextTemplate htmlLayout1 = new HtmlTextTemplate()
                {
                    UniqueId = "control-1",
                    SortOrder = 0,
                    WidthType = SharedPluginFeatures.DynamicContentWidthType.Columns,
                    Width = 12,
                    HeightType = SharedPluginFeatures.DynamicContentHeightType.Automatic,
                    Data = "<p>This is <br />html over<br />three lines</p>"
                };

                _dynamicContentPage2.Content.Add(htmlLayout1);

                HtmlTextTemplate htmlLayout2 = new HtmlTextTemplate()
                {
                    UniqueId = "control-2",
                    SortOrder = 2,
                    WidthType = SharedPluginFeatures.DynamicContentWidthType.Columns,
                    Width = 4,
                    HeightType = SharedPluginFeatures.DynamicContentHeightType.Automatic,
                    Data = "<p>This is html<br />over two lines</p>"
                };

                _dynamicContentPage2.Content.Add(htmlLayout2);
            }

            return _dynamicContentPage2;
        }

        private IDynamicContentPage GetPage3()
        {
            if (_dynamicContentPage3 == null)
            {
                _dynamicContentPage3 = new DynamicContentPage()
                {
                    Id = 3,
                    Name = "Custom Page 3",
                    RouteName = "page-3",
                };

                HtmlTextTemplate htmlLayout1 = new HtmlTextTemplate()
                {
                    UniqueId = "control-1",
                    SortOrder = 0,
                    WidthType = SharedPluginFeatures.DynamicContentWidthType.Columns,
                    Width = 12,
                    HeightType = SharedPluginFeatures.DynamicContentHeightType.Automatic,
                    Data = "<p>This is <br />html over<br />three lines</p>"
                };

                _dynamicContentPage3.Content.Add(htmlLayout1);

                SpacerTemplate spacerTemplate1 = new SpacerTemplate()
                {
                    SortOrder = 1,
                    Width = 8
                };

                _dynamicContentPage3.Content.Add(spacerTemplate1);

                HtmlTextTemplate htmlLayout2 = new HtmlTextTemplate()
                {
                    UniqueId = "control-2",
                    SortOrder = 2,
                    WidthType = SharedPluginFeatures.DynamicContentWidthType.Columns,
                    Width = 4,
                    HeightType = SharedPluginFeatures.DynamicContentHeightType.Automatic,
                    Data = "<p>This is html<br />over two lines</p>"
                };

                _dynamicContentPage3.Content.Add(htmlLayout2);
            }

            return _dynamicContentPage3;
        }

        private IDynamicContentPage GetPage10()
        {
            if (_dynamicContentPage10 == null)
            {
                _dynamicContentPage10 = new DynamicContentPage()
                {
                    Id = 10,
                    Name = "Custom Page 10",
                    RouteName = "page-10",
                };

                HtmlTextTemplate htmlLayout1 = new HtmlTextTemplate()
                {
                    UniqueId = "control-1",
                    SortOrder = 0,
                    WidthType = SharedPluginFeatures.DynamicContentWidthType.Columns,
                    Width = 12,
                    HeightType = SharedPluginFeatures.DynamicContentHeightType.Automatic,
                    Data = "<p>This is <br />html over<br />three lines</p>"
                };

                _dynamicContentPage10.Content.Add(htmlLayout1);

                HtmlTextTemplate htmlLayout2 = new HtmlTextTemplate()
                {
                    UniqueId = "control-2",
                    SortOrder = 2,
                    WidthType = SharedPluginFeatures.DynamicContentWidthType.Columns,
                    Width = 4,
                    HeightType = SharedPluginFeatures.DynamicContentHeightType.Automatic,
                    Data = "<p>This is html<br />Content 2</p>"
                };

                _dynamicContentPage10.Content.Add(htmlLayout2);

                HtmlTextTemplate htmlLayout3 = new HtmlTextTemplate()
                {
                    UniqueId = "control-3",
                    SortOrder = 9,
                    WidthType = SharedPluginFeatures.DynamicContentWidthType.Columns,
                    Width = 4,
                    HeightType = SharedPluginFeatures.DynamicContentHeightType.Automatic,
                    Data = "<p>This is html<br />Content 3</p>"
                };

                _dynamicContentPage10.Content.Add(htmlLayout3);

                HtmlTextTemplate htmlLayout4 = new HtmlTextTemplate()
                {
                    UniqueId = "control-4",
                    SortOrder = 8,
                    WidthType = SharedPluginFeatures.DynamicContentWidthType.Columns,
                    Width = 4,
                    HeightType = SharedPluginFeatures.DynamicContentHeightType.Automatic,
                    Data = "<p>This is html<br />Content 4</p>"
                };

                _dynamicContentPage10.Content.Add(htmlLayout4);

                HtmlTextTemplate htmlLayout5 = new HtmlTextTemplate()
                {
                    UniqueId = "control-5",
                    SortOrder = 7,
                    WidthType = SharedPluginFeatures.DynamicContentWidthType.Columns,
                    Width = 4,
                    HeightType = SharedPluginFeatures.DynamicContentHeightType.Automatic,
                    Data = "<p>This is html<br />Content 5</p>"
                };

                _dynamicContentPage10.Content.Add(htmlLayout5);

                HtmlTextTemplate htmlLayout6 = new HtmlTextTemplate()
                {
                    UniqueId = "control-6",
                    SortOrder = 6,
                    WidthType = SharedPluginFeatures.DynamicContentWidthType.Columns,
                    Width = 4,
                    HeightType = SharedPluginFeatures.DynamicContentHeightType.Automatic,
                    Data = "<p>This is html<br />Content 6</p>"
                };

                _dynamicContentPage10.Content.Add(htmlLayout6);

                HtmlTextTemplate htmlLayout7 = new HtmlTextTemplate()
                {
                    UniqueId = "control-7",
                    SortOrder = 5,
                    WidthType = SharedPluginFeatures.DynamicContentWidthType.Columns,
                    Width = 4,
                    HeightType = SharedPluginFeatures.DynamicContentHeightType.Automatic,
                    Data = "<p>This is html<br />Content 7</p>"
                };

                _dynamicContentPage10.Content.Add(htmlLayout7);

                HtmlTextTemplate htmlLayout8 = new HtmlTextTemplate()
                {
                    UniqueId = "control-8",
                    SortOrder = 4,
                    WidthType = SharedPluginFeatures.DynamicContentWidthType.Columns,
                    Width = 4,
                    HeightType = SharedPluginFeatures.DynamicContentHeightType.Automatic,
                    Data = "<p>This is html<br />Content 8</p>"
                };

                _dynamicContentPage10.Content.Add(htmlLayout8);

                HtmlTextTemplate htmlLayout9 = new HtmlTextTemplate()
                {
                    UniqueId = "control-9",
                    SortOrder = 3,
                    WidthType = SharedPluginFeatures.DynamicContentWidthType.Columns,
                    Width = 4,
                    HeightType = SharedPluginFeatures.DynamicContentHeightType.Automatic,
                    Data = "<p>This is html<br />Content 9</p>"
                };

                _dynamicContentPage10.Content.Add(htmlLayout9);

                HtmlTextTemplate htmlLayout10 = new HtmlTextTemplate()
                {
                    UniqueId = "control-10",
                    SortOrder = 20,
                    WidthType = SharedPluginFeatures.DynamicContentWidthType.Columns,
                    Width = 4,
                    HeightType = SharedPluginFeatures.DynamicContentHeightType.Automatic,
                    Data = "<p>This is html<br />Content 10</p>"
                };

                _dynamicContentPage10.Content.Add(htmlLayout10);
            }

            return _dynamicContentPage10;
        }

        #endregion Private Methods
    }
}
