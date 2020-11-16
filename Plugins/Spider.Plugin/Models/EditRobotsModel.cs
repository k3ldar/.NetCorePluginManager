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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Spider.Plugin
 *  
 *  File: EditRobotsModel.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  17/10/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SharedPluginFeatures;

#pragma warning disable CS1591

namespace Spider.Plugin.Models
{
    public sealed class EditRobotsModel : BaseModel
    {
        #region Constructors

        public EditRobotsModel()
        {

        }

        public EditRobotsModel(BaseModelData modelData, List<string> agents, List<CustomAgentModel> routes)
            : base(modelData)
        {
            Agents = agents ?? throw new ArgumentNullException(nameof(agents));
            Routes = routes ?? throw new ArgumentNullException(nameof(routes));
        }

        #endregion Constructors

        #region Properties

        public List<string> Agents { get; private set; }

        public List<CustomAgentModel> Routes { get; private set; }

        public bool Allowed { get; set; }

        [Required]
        public string Route { get; set; }

        [Required]
        public string AgentName { get; set; }

        #endregion Properties
    }
}

#pragma warning restore CS1591