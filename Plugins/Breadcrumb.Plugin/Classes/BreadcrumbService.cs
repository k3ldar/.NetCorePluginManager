using System;
using System.Collections.Generic;
using System.Text;

using SharedPluginFeatures;

namespace Breadcrumb.Plugin
{
    public sealed class BreadcrumbService : IBreadcrumbService
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

            BreadcrumbSettings settings = settingsProvider.GetSettings<BreadcrumbSettings>(Constants.PluginNameBreadcrumb);
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
