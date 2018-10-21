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
 *  Copyright (c) 2018 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SharedPluginFeatures
 *  
 *  File: RestrictedIpRouteAttribute.cs
 *
 *  Purpose:  Restricted Ip Route attribute
 *
 *  Date        Name                Reason
 *  29/09/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace SharedPluginFeatures
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class RestrictedIpRouteAttribute  :  Attribute
    {
        #region Constructors

        public RestrictedIpRouteAttribute(string profileName)
        {
            if (String.IsNullOrEmpty(profileName))
                throw new ArgumentNullException(nameof(profileName));

            ProfileName = profileName;
        }

        #endregion Constructors

        #region Properties

        public string ProfileName { get; private set; }

        #endregion Properties
    }
}
