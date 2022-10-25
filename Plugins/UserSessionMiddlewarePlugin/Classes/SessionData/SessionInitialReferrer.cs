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
 *  Product:  UserSessionMiddleware.Plugin
 *  
 *  File: SessionPageView.cs
 *
 *  Purpose:  Page view statistics
 *
 *  Date        Name                Reason
 *  03/09/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;

namespace UserSessionMiddleware.Plugin.Classes.SessionData
{
    /// <summary>
    /// Initial referrer details
    /// </summary>
    public sealed class SessionInitialReferrer
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public SessionInitialReferrer()
        {

        }

        internal SessionInitialReferrer(in string hash, in string referrerUrl)
        {
            if (string.IsNullOrEmpty(hash))
                throw new ArgumentNullException(nameof(hash));

            if (string.IsNullOrEmpty(referrerUrl))
                throw new ArgumentNullException(nameof(referrerUrl));

            Hash = hash;
            ReferrerUrl = referrerUrl;
            Usage = 0;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Hash of Url
        /// </summary>
        /// <value>string</value>
        public string Hash { get; set; }

        /// <summary>
        /// Url being monitored
        /// </summary>
        /// <value>string</value>
        public string ReferrerUrl { get; set; }

        /// <summary>
        /// Number of times the Url has been used to refer to the page
        /// </summary>
        /// <value>uint</value>
        public uint Usage { get; set; }

        #endregion Properties
    }
}
