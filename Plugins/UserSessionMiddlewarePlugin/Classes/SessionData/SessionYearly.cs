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
 *  Product:  UserSessionMiddleware.Plugin
 *  
 *  File: SessionMonthly.cs
 *
 *  Purpose:  Monthly session data
 *
 *  Date        Name                Reason
 *  12/09/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace UserSessionMiddleware.Plugin.Classes.SessionData
{
    /// <summary>
    /// Yearly session data
    /// </summary>
    public class SessionYearly : SessionBaseData
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public SessionYearly()
            : base()
        {

        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Year of visit
        /// </summary>
        public int Year { get; set; }

        #endregion Properties
    }
}
