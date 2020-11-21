/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  Plugin Manager is distributed under the GNU General Public License version 3 and  
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
 *  Product:  PluginManager
 *  
 *  File: IPluginVersion.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  22/09/2018  Simon Carter        Initially Created
 *  14/10/2018  Simon Carter        Move to SharedPluginFeatures
 *  28/12/2019  Simon Carter        Converted to generic plugin that can be used by any 
 *                                  application type.  Originally part of .Net 
 *                                  Core Plugin Manager
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace PluginManager.Abstractions
{
    /// <summary>
    /// This interface should be implemented by all plugin modules which need to specify which 
    /// internal version of the AspNetCore.PluginManager is used.  The default is one (1).
    /// </summary>
    public interface IPluginVersion
    {
        /// <summary>
        /// Retrieves the currently supported version.  If the value is outside of the valid range
        /// of internal versions that are in use, then the default one (1) is used.
        /// </summary>
        /// <returns>ushort</returns>
        ushort GetVersion();
    }
}
