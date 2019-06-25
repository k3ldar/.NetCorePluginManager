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
 *  The Original Code was created by unknown and modified by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2012 - 2018 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SharedPluginFeatues
 *  
 *  File: Constants.cs
 *
 *  Purpose:  Shared constant values.
 *
 *  Date        Name                Reason
 *  09/12/2018  Simon Carter        Initially Created
 *  12/05/2019  Simon Carter        Add seo data constants
 *  
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */


namespace SharedPluginFeatures
{
    /// <summary>
    /// Constant values shared between all plugin modules and the AspNetCore.PluginManager
    /// </summary>
    public sealed class Constants
    {
        /// <summary>
        /// Minimum password length, default to 8 characters
        /// </summary>
        public const int MinimumPasswordLength = 8;

        /// <summary>
        /// Maximum password length, default to 40 characters
        /// </summary>
        public const int MaximumPasswordLength = 40;

        /// <summary>
        /// Name of UserSession that is injected into the request pipeline
        /// </summary>
        public const string UserSession = "UserSession";

        /// <summary>
        /// Name of culture used by the user and injected into the request pipeline by Localization.Plugin module.
        /// </summary>
        public const string UserCulture = "UserCulture";

        /// <summary>
        /// Name of the breadcrumbs injected into the request pipeline by UserSessionMiddleware.Plugin module.
        /// </summary>
        public const string Breadcrumbs = "Breadcrumbs";

        /// <summary>
        /// Name of the ShoppingCart item injected into the request pipeline by ShoppingCart.Plugin module.
        /// </summary>
        public const string ShoppingCart = "ShoppingCart";

        /// <summary>
        /// Name of the ShoppingCartSummary item injected into the request pipeline by ShoppingCart.Plugin module.
        /// </summary>
        public const string BasketSummary = "BasketSummary";

        /// <summary>
        /// Default tax rate used by ShoppingCart.Plugin module.
        /// </summary>
        public const string DefaultTaxRate = "DefaultTaxRate";

        /// <summary>
        /// Name of the title of the document injected into the request pipline by SeoPlugin module.
        /// </summary>
        public const string SeoTitle = "SeoTitle";

        /// <summary>
        /// Name of the meta description of the document injected into the request pipline by SeoPlugin module.
        /// </summary>
        public const string SeoMetaDescription = "SeoMetaDescription";

        /// <summary>
        /// Name of the meta keywords of the document injected into the request pipline by SeoPlugin module.
        /// </summary>
        public const string SeoMetaKeywords = "SeoTags";

        /// <summary>
        /// Name of the author of the document injected into the request pipline by SeoPlugin module.
        /// </summary>
        public const string SeoMetaAuthor = "SeoAuthor";

        /// <summary>
        /// Default name of session cookie if no value is specified when the application is initialised.
        /// </summary>
        public const string DefaultSessionCookie = "user_session";

        /// <summary>
        /// Name of the user session configuration setting.
        /// </summary>
        public const string UserSessionConfiguration = "UserSessionConfiguration";

        /// <summary>
        /// Default static file extensions
        /// </summary>
        public const string StaticFileExtensions = ".less;.ico;.css;.js;.svg;.jpg;.jpeg;.gif;.png;.eot;.map;";

        /// <summary>
        /// Page referrer constant
        /// </summary>
        public const string PageReferer = "Referer";

        /// <summary>
        /// Forward slash (/) constant
        /// </summary>
        public const string ForwardSlash = "/";

        /// <summary>
        /// Forward slash (/) char constant.
        /// </summary>
        public const char ForwardSlashChar = '/';

        /// <summary>
        /// Dash (-) char constant.
        /// </summary>
        public const char Dash = '-';

        /// <summary>
        /// Name of the Breadcrumb.Plugin module.
        /// </summary>
        public const string PluginSettingBreadcrumb = "Breadcrumb.Plugin";

        /// <summary>
        /// Name of the UserSessionMiddleware.Plugin module.
        /// </summary>
        public const string PluginNameUserSession = "UserSessionMiddleware.Plugin.dll";

        /// <summary>
        /// Name of the Localization.Plugin module which controlls all localization requests.
        /// </summary>
        public const string PluginNameLocalization = "Localization.Plugin.dll";

        /// <summary>
        /// Name of ShoppingCart.Plugin, this can be used in conjunction with IPluginHelperService when determining whether the shopping cart plugin is loaded.
        /// </summary>
        public const string PluginNameShoppingCart = "ShoppingCartPlugin.dll";

        /// <summary>
        /// Exception text used to raise an exception if the user session service is not found or configured.
        /// </summary>
        public const string UserSessionServiceNotFound = "UserSessionService not found";

        /// <summary>
        /// Exception text used to raise an exception should the breadcrumb route match its parent route.
        /// </summary>
        public const string BreadcrumbRoutEqualsParentRoute = "Parent route matches route; Route: {0}; Parent Route {1}";

        /// <summary>
        /// Exception text used to raise an exception if their is a problem recursively obtaining breadcrumb results for a route.
        /// </summary>
        public const string TooManyBreadcrumbs = "Breadcrumb recursion, check parent route values";

        /// <summary>
        /// Exception message used when registering a INotificationListener with the INotificationService message notifications, when the listener does not provide any event names.
        /// </summary>
        public const string InvalidListener = "Listener must provide at least one event";

        /// <summary>
        /// Exception message used when registering a INotificationListener with the INotificationService message notifications, when the event name has not been recognised.
        /// </summary>
        public const string InvalidEventName = "Invalid event name in listener";

        /// <summary>
        /// Exception message used when registering a INotificationListener with the INotificationService message notifications, when the event name has not been registered.
        /// </summary>
        public const string EventNameNotRegistered = "Event name has not been registered";

        /// <summary>
        /// Exception message used when attempting to view an invalid class type
        /// </summary>
        public const string InvalidTypeName = "Invalid type name";

        /// <summary>
        /// Default currency code used when no currency code is supplied
        /// </summary>
        public const string CurrencyCodeDefault = "GBP";

        /// <summary>
        /// Name of thread used for INotificationService message notifications
        /// </summary>
        public const string ThreadNotificationService = "Notification Service";

        /// <summary>
        /// Name of the documentation file cache
        /// </summary>
        public const string DocumentationFileCache = "DocumentationFileCache";

        /// <summary>
        /// Name of the documentation list cache
        /// </summary>
        public const string DocumentationListCache = "DocumentationListCache";

        /// <summary>
        /// Notification event name for obtaining GeoIp load times.
        /// </summary>
        public const string NotificationEventGeoIpLoadTime = "GeoIpLoadTime";

        /// <summary>
        /// Notification event name for obtaining GeoIp record count.
        /// </summary>
        public const string NotificationEventGeoIpRecordCount = "GeoIpRecordCount";

        /// <summary>
        /// Name of the default cache manager.
        /// </summary>
        public const string CacheNameDefault = "Internal Default Cache Manager";

        /// <summary>
        /// Name of the short cache manager.
        /// </summary>
        public const string CacheNameShort = "Internal Short Cache Manager";

        /// <summary>
        /// Name of the extending cache manager.
        /// </summary>
        public const string CacheNameExtending = "Extending Cache Manager";

        /// <summary>
        /// Name of the permanent cache manager.
        /// </summary>
        public const string CacheNamePermanent = "Permanent Cache Manager";

        /// <summary>
        /// Name of the url to return to in the event of a 403 error.
        /// </summary>
        public const string ReturnUrl = "ReturnUrl";

        /// <summary>
        /// Name of the thread that loads default documentation
        /// </summary>
        public const string DocumentationLoadThread = "Documentation load thread";

        /// <summary>
        /// Defines the names of the forward for header, should the request be received via a proxy.
        /// </summary>
        public static readonly string[] ForwardForHeader = new string[] { "HTTP_X_FORWARDED_FOR", "X-Forwarded-For", "http-X-Forwarded-For", "X-Real-IP" };
    }
}
