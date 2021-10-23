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
 *  Product:  DynamicContent.Plugin
 *  
 *  File: CustomPagesModel.cs
 *
 *  Purpose:  Custom pages model
 *
 *  Date        Name                Reason
 *  29/11/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Middleware;

using SharedPluginFeatures;

namespace DynamicContent.Plugin.Model
{
    public class CustomPagesModel : BaseModel
    {
        #region Constructors

        public CustomPagesModel()
        {

        }

        public CustomPagesModel(BaseModelData modelData, List<LookupListItem> customPages)
            : base(modelData)
        {
            if (customPages == null)
                throw new ArgumentNullException(nameof(customPages));

            CustomPages = new List<NameIdModel>();

            customPages.ForEach(cp => CustomPages.Add(new NameIdModel(cp.Id, cp.Description)));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// List of custom pages
        /// </summary>
        public List<NameIdModel> CustomPages { get; }

        #endregion Properties
    }
}
