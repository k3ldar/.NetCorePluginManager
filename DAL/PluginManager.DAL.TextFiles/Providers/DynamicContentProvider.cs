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
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: DynamicContentProvider.cs
 *
 *  Purpose:  IDynamicContentProvider for text based storage
 *
 *  Date        Name                Reason
 *  25/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Reflection;

using DynamicContent.Plugin.Templates;

using Middleware;
using Middleware.DynamicContent;

using PluginManager.Abstractions;
using PluginManager.DAL.TextFiles.Tables;
using PluginManager.SimpleDB;

using SharedPluginFeatures;
using SharedPluginFeatures.DynamicContent;

namespace PluginManager.DAL.TextFiles.Providers
{
    internal class DynamicContentProvider : IDynamicContentProvider
    {
        #region Private Members

        private static List<DynamicContentTemplate> _templates;
        
        private readonly IPluginClassesService _pluginClassesService;
        private readonly IMemoryCache _memoryCache;
        private readonly ITextTableOperations<ContentPageDataRow> _pageData;
        private readonly ITextTableOperations<ContentPageItemDataRow> _pageItemsData;

        #endregion Private Members

        #region Constructors

        public DynamicContentProvider(IPluginClassesService pluginClassesService, IMemoryCache memoryCache, 
            ITextTableOperations<ContentPageDataRow> pageData, ITextTableOperations<ContentPageItemDataRow> pageItemsData)
        {
            _pluginClassesService = pluginClassesService ?? throw new ArgumentNullException(nameof(pluginClassesService));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _pageData = pageData ?? throw new ArgumentNullException(nameof(pageData));
            _pageItemsData = pageItemsData ?? throw new ArgumentNullException(nameof(pageItemsData));
        }

        #endregion Constructors

        #region IDynamicContentProvider Members

        public long CreateCustomPage()
        {
            ContentPageDataRow newPage = new ContentPageDataRow();
            _pageData.Insert(newPage);
            return newPage.Id;
        }

        public List<LookupListItem> GetCustomPageList()
        {
            List<LookupListItem> Result = new List<LookupListItem>();

            IReadOnlyList<ContentPageDataRow> pages = _pageData.Select();

            foreach (ContentPageDataRow page in pages)
            {
                Result.Add(new LookupListItem((int)page.Id, page.Name));
            }

            return Result;
        }

        public List<IDynamicContentPage> GetCustomPages()
        {
            List<IDynamicContentPage> Result = new List<IDynamicContentPage>();

            IReadOnlyList<ContentPageDataRow> pages = _pageData.Select();

            foreach (ContentPageDataRow page in pages)
            {
                Result.Add(InternalGetCustomPage(page));
            }

            return Result;
        }

        public IDynamicContentPage GetCustomPage(long id)
        {
            return InternalGetCustomPage(id);
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
            return _pageData.Select().Where(p => p.Id != id && p.Name.Equals(pageName, StringComparison.InvariantCultureIgnoreCase)).Any();
        }

        public bool RouteNameExists(long id, string routeName)
        {
            return _pageData.Select().Where(p => p.Id != id && p.RouteName.Equals(routeName, StringComparison.InvariantCultureIgnoreCase)).Any();
        }

        public bool Save(IDynamicContentPage dynamicContentPage)
        {
            ContentPageDataRow pageDataRow = _pageData.Select(dynamicContentPage.Id);

            if (pageDataRow == null)
                return false;

            pageDataRow.Name = dynamicContentPage.Name;
            pageDataRow.RouteName = dynamicContentPage.RouteName;
            pageDataRow.BackgroundColor = dynamicContentPage.BackgroundColor;
            pageDataRow.BackgroundImage = dynamicContentPage.BackgroundImage;
            pageDataRow.ActiveFromTicks = dynamicContentPage.ActiveFrom.Ticks;
            pageDataRow.ActiveToticks = dynamicContentPage.ActiveTo.Ticks;
            _pageData.Update(pageDataRow);

            foreach (DynamicContentTemplate item in dynamicContentPage.Content)
            {
                ContentPageItemDataRow contentPageItem = new ContentPageItemDataRow()
                {
                    UniqueId = item.UniqueId,
                    AssemblyQualifiedName = item.AssemblyQualifiedName,
                    ActiveFromTicks = item.ActiveFrom.Ticks,
                    ActiveToTicks = item.ActiveTo.Ticks,
                    Data = item.Data,
                    Height = item.Height,
                    HeightType = (byte)item.HeightType,
                    Width = item.Width,
                    WidthType = (byte)item.WidthType,
                    SortOrder = item.SortOrder,
                    PageId = pageDataRow.Id,
                };

                _pageItemsData.InsertOrUpdate(contentPageItem);

                item.Id = contentPageItem.Id;
            }

            return true;
        }

        public bool SaveUserInput(string data)
        {
            throw new NotImplementedException();
        }

        #endregion IDynamicContentProvider Members

        #region Private Methods

        private IDynamicContentPage InternalGetCustomPage(long id)
        {
            if (!_pageData.IdExists(id))
                return null;

            ContentPageDataRow pageData = _pageData.Select(id);

            if (pageData == null)
                return null;

            return InternalGetCustomPage(pageData);
        }

        private IDynamicContentPage InternalGetCustomPage(ContentPageDataRow pageData)
        { 
            DynamicContentPage Result = new DynamicContentPage()
            {
                Id = pageData.Id,
                ActiveFrom = new DateTime(pageData.ActiveFromTicks, DateTimeKind.Utc),
                ActiveTo = new DateTime(pageData.ActiveFromTicks, DateTimeKind.Utc),
                BackgroundColor = pageData.BackgroundColor,
                BackgroundImage = pageData.BackgroundImage,
                Name = pageData.Name,
                RouteName = pageData.RouteName,
            };

            IEnumerable<ContentPageItemDataRow> pageItems = _pageItemsData.Select().Where(pi => pi.PageId.Equals(pageData.Id));

            foreach (ContentPageItemDataRow page in pageItems)
            {
                string[] classParts = page.AssemblyQualifiedName.Split(",");

                if (classParts.Length < 2)
                    throw new InvalidOperationException();

                DynamicContentTemplate pageItem = CreateTemplateItem(classParts[1].Trim(), classParts[0].Trim(), page.UniqueId, out bool templateClassFound);
                pageItem.Id = page.Id;
                pageItem.UniqueId = page.UniqueId;
                pageItem.ActiveFrom = new DateTime(page.ActiveFromTicks);
                pageItem.ActiveTo = new DateTime(page.ActiveToTicks);
                string data = page.Data;

                if (templateClassFound)
                    pageItem.Data = data;
                else
                    pageItem.Data = "<p>Content template not found</p>";
                
                pageItem.HeightType = (DynamicContentHeightType)page.HeightType;
                pageItem.WidthType = (DynamicContentWidthType)page.WidthType;
                pageItem.Height = page.Height;
                pageItem.SortOrder = page.SortOrder;
                pageItem.Width = page.Width;
                Result.Content.Add(pageItem);
            }

            return Result;
        }

        private static DynamicContentTemplate CreateTemplateItem(string assemblyName, string className, string uniqueId, out bool templateClassFound)
        {
            DynamicContentTemplate baseInstance;

            try
            {
                Assembly classAssembly = Assembly.Load(assemblyName);
                Type t = classAssembly.GetType(className);

                if (t == null)
                {
                    baseInstance = new HtmlTextTemplate();
                    templateClassFound = false;
                }
                else
                {
                    baseInstance = (DynamicContentTemplate)Activator.CreateInstance(t);
                    templateClassFound = true;
                }
            }
            catch
            {
                baseInstance = new HtmlTextTemplate();
                templateClassFound = false;
            }

            return baseInstance.Clone(uniqueId);
        }

        //private IDynamicContentPage GetPage1()
        //{
        //    if (_dynamicContentPage1 == null)
        //    {
        //        _dynamicContentPage1 = new DynamicContentPage()
        //        {
        //            Id = 1,
        //            Name = "Custom Page 1",
        //            RouteName = "page-1"
        //        };

        //        HtmlTextTemplate htmlLayout1 = new HtmlTextTemplate()
        //        {
        //            UniqueId = "1",
        //            SortOrder = 0,
        //            WidthType = SharedPluginFeatures.DynamicContentWidthType.Columns,
        //            Width = 12,
        //            HeightType = SharedPluginFeatures.DynamicContentHeightType.Automatic,
        //            Data = "<p>This is <br />html over<br />three lines</p>"
        //        };

        //        _dynamicContentPage1.Content.Add(htmlLayout1);
        //    }

        //    return _dynamicContentPage1;
        //}

        #endregion Private Methods
    }
}
