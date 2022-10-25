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
 *  File: DefaultUserSessionService.cs
 *
 *  Purpose:  Default user session service
 *
 *  Date        Name                Reason
 *  01/09/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;

using Middleware;

namespace UserSessionMiddleware.Plugin.Classes.SessionData
{
    /// <summary>
    /// Contains a list of page views for all user sessions
    /// </summary>
    public sealed class SessionPageViews : IUrlHash
	{
		#region Private Members

		private IUrlHashProvider _urlHashProvider;

		#endregion Private Members

		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public SessionPageViews()
		{
			PageViews = new List<SessionPageView>();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// List of page views for all sessions
        /// </summary>
        public List<SessionPageView> PageViews { get; set; }

        /// <summary>
        /// Indicates whether the list has been updated or not
        /// </summary>
        internal bool IsDirty { get; set; }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Adds a new page view to the collection
        /// </summary>
        /// <param name="url">Url being visited</param>
        /// <param name="timeStamp">Date/time the visit was made</param>
        /// <param name="totalTime">Total time spent on the page</param>
        /// <param name="isBot">Was the visit from a bot</param>
        /// <param name="bounced">Did the visit end at this page (human visits only)</param>
        public void Add(in string url, DateTime timeStamp, in double totalTime, in bool isBot, in bool bounced)
        {
            if (String.IsNullOrEmpty(url))
                return;

            string hash = _urlHashProvider.GetUrlHash(url);
            SessionPageView currentSession = PageViews
                .Where(pv => pv.Hash == hash && pv.Year == timeStamp.Year && pv.Month == timeStamp.Month)
                .FirstOrDefault();

            if (currentSession == null)
            {
                currentSession = new SessionPageView();
                currentSession.Hash = hash;
                currentSession.Url = url;
                currentSession.Year = timeStamp.Year;
                currentSession.Month = (byte)timeStamp.Month;

                PageViews.Add(currentSession);
            }

            if (isBot)
            {
                currentSession.BotCount++;
            }
            else
            {
                currentSession.HumanCount++;
                currentSession.TotalTime += totalTime;

                if (bounced)
                    currentSession.BounceCount++;
            }

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
