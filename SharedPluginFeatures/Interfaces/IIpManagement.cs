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
 *  Product:  SharedPluginFeatures
 *  
 *  File: IIpManagement.cs
 *
 *  Purpose:  Provides interface for updating Ip Management
 *
 *  Date        Name                Reason
 *  11/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace SharedPluginFeatures
{
    /// <summary>
    /// This interface is implemented by the BadEgg.Plugin module and is used to manage 
    /// black and white listed Ip addresses.
    /// 
    /// BadEgg Plugin module does not store any of these Ip addresses but requires that
    /// the host application informs it which Ip addresses are black or white listed.
    /// </summary>
    public interface IIpManagement
    {
        /// <summary>
        /// Adds an address to the black listed address list.  Any request from an Ip
        /// address in this list will be rejected whilst the pipeline is iterated when 
        /// a request is made.
        /// </summary>
        /// <param name="ipAddress">Ip address</param>
        void AddBlackListedIp(in string ipAddress);

        /// <summary>
        /// Adds an address to the whilte listed address list.  Any request from an Ip
        /// address in this list will never be rejected when making a request to the
        /// application.
        /// </summary>
        /// <param name="ipAddress">Ip address</param>
        void AddWhiteListedIp(in string ipAddress);

        /// <summary>
        /// Removes an Ip address from both the black and whilte address lists.  
        /// </summary>
        /// <param name="ipAddress">Ip address</param>
        void RemoveIpAddress(in string ipAddress);

        /// <summary>
        /// Removes all Ip addresses from both black and white lists.
        /// </summary>
        void ClearAllIpAddresses();
    }
}
