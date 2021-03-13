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
 *  Copyright (c) 2018 - 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginMiddleware
 *  
 *  File: MarketingCampaign.cs
 *
 *  Purpose:  User login details
 *
 *  Date        Name                Reason
 *  21/02/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using Middleware.Interfaces;

namespace Middleware
{
    /// <summary>
    /// Represents a marketing campaign that is used with Marketing plugin
    /// </summary>
    public sealed class MarketingCampaign : IMarketingCampaign
    {
        #region Properties

        /// <summary>
        /// Id of campaign
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Start date/time of campaign
        /// </summary>
        public DateTime StartDateTime { get; private set; }

        /// <summary>
        /// Finish date/time of campaign
        /// </summary>
        public DateTime FinishDateTime { get; private set; }

        /// <summary>
        /// Title for Campaign
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Name of campaign
        /// </summary>
        public string CampaignName { get; private set; }

        /// <summary>
        /// Image to be placed on main index page
        /// </summary>
        public string ImageMainPage { get; private set; }

        /// <summary>
        /// Image to be placed on left menu for each page
        /// </summary>
        public string ImageLeftMenu { get; private set; }

        /// <summary>
        /// Image to be placed on offers page
        /// </summary>
        public string ImageOffersPage { get; private set; }

        /// <summary>
        /// Text to be displayed on offers page
        /// </summary>
        public string OffersPageText { get; private set; }

        /// <summary>
        /// Override's the link that jumps straight to the Offers Page
        /// </summary>
        public string LinkOverride { get; private set; }

        #endregion Properties
    }
}
