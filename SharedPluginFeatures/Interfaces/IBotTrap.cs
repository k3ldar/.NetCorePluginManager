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
 *  Copyright (c) 2012 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SharedPluginFeatues
 *  
 *  File: IBotTrap.cs
 *
 *  Purpose:  Notifies of bot which entered the trap
 *
 *  Date        Name                Reason
 *  15/10/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace SharedPluginFeatures
{
    /// <summary>
    /// IBotTrap interface is called when a bot enters a trap within Spider Middleware
    /// </summary>
    public interface IBotTrap
    {
        /// <summary>
        /// Method called when a bot enters a trap route
        /// </summary>
        /// <param name="ipAddress">Ip Address which triggered the bot.</param>
        /// <param name="userAgent">User agent which triggered the bot.</param>
        void OnTrapEntered(in string ipAddress, in string userAgent);
    }
}
