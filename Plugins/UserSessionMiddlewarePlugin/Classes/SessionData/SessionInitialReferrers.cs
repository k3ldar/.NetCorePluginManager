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
 *  File: SessionPageView.cs
 *
 *  Purpose:  Page view statistics
 *
 *  Date        Name                Reason
 *  03/09/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;

using Middleware;

namespace UserSessionMiddleware.Plugin.Classes.SessionData
{
    /// <summary>
    /// Contains a collection of initial referrers
    /// </summary>
    public sealed class SessionInitialReferrers : IUrlHash
	{
		#region Private Members

		private IUrlHashProvider _urlHashProvider;

		#endregion Private Members

		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public SessionInitialReferrers()
        {
            InitialReferrers = new List<SessionInitialReferrer>();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// List of initial referrers
        /// </summary>
        public List<SessionInitialReferrer> InitialReferrers { get; set; }

        /// <summary>
        /// Indicates whether the list has been updated or not
        /// </summary>
        internal bool IsDirty { get; set; }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Adds or updates the count for an initial referrer
        /// </summary>
        /// <param name="referalUrl"></param>
        public void Add(in string referalUrl)
        {
            if (string.IsNullOrEmpty(referalUrl))
                return;

            string hash = _urlHashProvider.GetUrlHash(referalUrl);

            SessionInitialReferrer referrer = InitialReferrers
                .Where(ir => ir.Hash.Equals(hash))
                .FirstOrDefault();

            if (referrer == null)
            {
                referrer = new SessionInitialReferrer(hash, referalUrl);
                InitialReferrers.Add(referrer);
            }

            referrer.Usage++;
            IsDirty = true;
        }

		/// <summary>
		/// Sets the url hash provider interface
		/// </summary>
		/// <param name="urlHashProvider"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public void SetUrlHash(IUrlHashProvider urlHashProvider)
		{
			_urlHashProvider = urlHashProvider ?? throw new ArgumentNullException(nameof(urlHashProvider));
		}

		#endregion Public Methods
	}
}
