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
 *  Product:  Middleware.Plugin
 *  
 *  File: SessionMonthly.cs
 *
 *  Purpose:  Monthly session data
 *
 *  Date        Name                Reason
 *  12/09/2020  Simon Carter        Initially Created
 *  02/08/2022	Simon Carter		Moved to middleware
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace Middleware.SessionData
{
    /// <summary>
    /// Monthly session data
    /// </summary>
    public sealed class SessionMonthly : SessionYearly
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public SessionMonthly()
            : base()
        {

        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Month of Visit
        /// </summary>
        public int Month { get; set; }

        #endregion Properties
    }
}
