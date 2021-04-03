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
 *  Product:  DynamicContent.Plugin
 *  
 *  File: EditPageModel.cs
 *
 *  Purpose:  Edit dynamic page
 *
 *  Date        Name                Reason
 *  13/08/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using SharedPluginFeatures;
using SharedPluginFeatures.DynamicContent;

namespace DynamicContent.Plugin.Model
{
    public sealed class EditPageModel : BaseModel
    {
        #region Constructors

        public EditPageModel()
        {
            DynamicContents = new List<DynamicContentTemplate>();
        }

        public EditPageModel(in BaseModelData modelData, string cacheId, int id, 
            string name, List<DynamicContentTemplate> dynamicContents)
            : base(modelData)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (String.IsNullOrEmpty(cacheId))
                throw new ArgumentNullException(nameof(cacheId));

            DynamicContents = dynamicContents ?? throw new ArgumentNullException(nameof(dynamicContents));
            CacheId = cacheId;
            Id = id;
            Name = name;
        }

        #endregion Constructors

        #region Properties

        public string CacheId { get; }

        public int Id { get; }

        public string Name { get; }

        public List<DynamicContentTemplate> DynamicContents { get; }

        #endregion Properties
    }
}
