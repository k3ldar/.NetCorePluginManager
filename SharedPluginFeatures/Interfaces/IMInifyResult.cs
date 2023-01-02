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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SharedPluginFeatures
 *  
 *  File: IMinifyResult.cs
 *
 *  Purpose:  Provides interface for results of minification
 *
 *  Date        Name                Reason
 *  23/01/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace SharedPluginFeatures
{
    /// <summary>
    /// Interface for minification result
    /// </summary>
    public interface IMinifyResult
    {
        /// <summary>
        /// Name of minification process.
        /// </summary>
        /// <value>string</value>
        string ProcessName { get; }

        /// <summary>
        /// The length of the data at the start of the minification process.
        /// </summary>
        int StartLength { get; }

        /// <summary>
        /// The length of the data at the end of the minification process.
        /// </summary>
        int EndLength { get; }

        /// <summary>
        /// Time taken to minify the data.
        /// </summary>
        decimal TimeTaken { get; }
    }
}
