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
 *  Product:  SystemAdmin.Plugin
 *  
 *  File: ChartViewModel.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  26/09/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Newtonsoft.Json;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace SystemAdmin.Plugin.Models
{
    public sealed class ChartViewModel : BaseModel
    {
        #region Constructors

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "Validating property of param so ok")]
        public ChartViewModel(in BaseModelData modelData, SystemAdminSubMenu subMenu)
            : base(modelData)
        {
            if (subMenu == null)
                throw new ArgumentNullException(nameof(subMenu));

            Title = subMenu.Name();

            ChartModel chartModel = JsonConvert.DeserializeObject<ChartModel>(subMenu.Data());

            ChartTitle = chartModel.ChartTitle;
            DataNames = chartModel.DataNames;
            DataValues = chartModel.DataValues;
        }

        #endregion Constructors

        #region Public Properties

        public string Title { get; private set; }

        public string ChartTitle { get; private set; }

        public List<KeyValuePair<ChartDataType, string>> DataNames { get; private set; }

        public Dictionary<string, List<decimal>> DataValues { get; private set; }

        #endregion Public Properties
    }
}

#pragma warning restore CS1591
