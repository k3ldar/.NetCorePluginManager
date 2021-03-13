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
 *  File: IMarketingCampaign.cs
 *
 *  Purpose:  Interface for individual marketing campaigns
 *
 *  Date        Name                Reason
 *  13/03/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.Interfaces
{
    /// <summary>
    /// Interface for marketing campaign item
    /// </summary>
    public interface IMarketingCampaign
    {
        #region Properties

        /// <summary>
        /// Id of campaign
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Start date/time of campaign
        /// </summary>
        DateTime StartDateTime { get; }

        /// <summary>
        /// Finish date/time of campaign
        /// </summary>
        DateTime FinishDateTime { get; }

        /// <summary>
        /// Title for Campaign
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Name of campaign
        /// </summary>
        string CampaignName { get; }

        /// <summary>
        /// Image to be placed on main index page
        /// </summary>
        string ImageMainPage { get; }

        /// <summary>
        /// Image to be placed on left menu for each page
        /// </summary>
        string ImageLeftMenu { get; }

        /// <summary>
        /// Image to be placed on offers page
        /// </summary>
        string ImageOffersPage { get; }

        /// <summary>
        /// Text to be displayed on offers page
        /// </summary>
        string OffersPageText { get; }

        /// <summary>
        /// Override's the link that jumps straight to the Offers Page
        /// </summary>
        string LinkOverride { get; }

        #endregion Properties
    }
}
