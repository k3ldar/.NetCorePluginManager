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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Breadcrumb.Plugin
 *  
 *  File: BreadcrumbService.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  20/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using PluginManager.Abstractions;

using SharedPluginFeatures;

namespace Breadcrumb.Plugin
{
    internal sealed class BreadcrumbService : IBreadcrumbService
    {
        #region Private Members

        private readonly string _homeName;
        private readonly string _homeController;
        private readonly string _defaultMethod;

        #endregion Private Members

        #region Constructors

        public BreadcrumbService(ISettingsProvider settingsProvider)
        {
            if (settingsProvider == null)
                throw new ArgumentNullException(nameof(settingsProvider));

            BreadcrumbSettings settings = settingsProvider.GetSettings<BreadcrumbSettings>(Constants.PluginSettingBreadcrumb);
            _homeName = settings.HomeName;
            _homeController = settings.HomeController;
            _defaultMethod = settings.DefaultAction;
        }

        #endregion Constructors

        #region IBreadcrumbService Methods

        public void AddBreadcrumb(in string name, in string route, in bool hasParameters)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (String.IsNullOrEmpty(route))
                throw new ArgumentNullException(nameof(route));

            AddBreadcrumb(name, route, String.Empty, hasParameters);
        }

        public void AddBreadcrumb(in string name, in string route, in string parentRoute, in bool hasParameters)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (String.IsNullOrEmpty(route))
                throw new ArgumentNullException(nameof(route));

            if (BreadcrumbMiddleware.Routes.ContainsKey(route.ToLower()))
                return;

            BreadcrumbRoute newRoute = new BreadcrumbRoute(route, hasParameters);

            if (!String.IsNullOrEmpty(parentRoute) &&
                BreadcrumbMiddleware.Routes.ContainsKey(parentRoute.ToLower()))
            {
                BreadcrumbRoute breadcrumbRoute = BreadcrumbMiddleware.Routes[parentRoute.ToLower()];

                foreach (BreadcrumbItem item in breadcrumbRoute.Breadcrumbs)
                    newRoute.Breadcrumbs.Add(new BreadcrumbItem(item.Name, item.Route, item.HasParameters));
            }

            if (newRoute.Breadcrumbs.Count == 0)
                newRoute.Breadcrumbs.Add(new BreadcrumbItem(_homeName, $"/{_homeController}/{_defaultMethod}", false));

            newRoute.Breadcrumbs.Add(new BreadcrumbItem(name, route.ToLower(), hasParameters));
            BreadcrumbMiddleware.Routes.Add(route.ToLower(), newRoute);
        }

        #endregion IBreadcrumbService Methods
    }
}
