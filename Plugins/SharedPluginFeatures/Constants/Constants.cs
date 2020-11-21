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
 *  
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */


namespace SharedPluginFeatures
{
    public sealed class Constants
    {
        public const int MinimumPasswordLength = 8;
        public const int MaximumPasswordLength = 40;


        public const string UserSession = "UserSession";
        public const string UserCulture = "UserCulture";
        public const string Breadcrumbs = "Breadcrumbs";

        public const string DefaultSessionCookie = "user_session";

        public const string UserSessionConfiguration = "UserSessionConfiguration";

        public const string StaticFileExtensions = ".less;.ico;.css;.js;.svg;.jpg;.jpeg;.gif;.png;.eot;";

        public const string PageReferer = "Referer";


        public const string ForwardSlash = "/";


        public const char ForwardSlashChar = '/';


        public const string PluginNameLocalization = "Localization.Plugin.dll";
        public const string PluginNameBreadcrumb = "Breadcrumb.Plugin";
    }
}
