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
    /// <summary>
    /// This attribute is used in conjunction with the RestricIp.Plugin module to restrict specific rotues
    /// to specific Ip Addresses within the system.
    /// 
    /// See RestrictIp.Plugin.RestrictIp.Plugin for further details on configuring Ip restrictions by routes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class RestrictedIpRouteAttribute : Attribute
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// 
        /// This constructor takes the name of the profile which is used to configure whether to allow or deny an Ip address for a specific route.
        /// </summary>
        /// <param name="profileName">Name of profile within the settings.</param>
        public RestrictedIpRouteAttribute(string profileName)
        {
            if (String.IsNullOrEmpty(profileName))
                throw new ArgumentNullException(nameof(profileName));

            ProfileName = profileName;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Name of the profile which is used to allow or deny an Ip address within a specific route.
        /// </summary>
        public string ProfileName { get; private set; }

        #endregion Properties
    }
}
