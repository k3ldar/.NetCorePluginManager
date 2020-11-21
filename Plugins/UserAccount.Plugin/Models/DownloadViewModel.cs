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
 *  Product:  UserAccount.Plugin
 *  
 *  File: DownloadViewModel.cs
 *
 *  Purpose:  Download view model
 *
 *  Date        Name                Reason
 *  05/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Middleware.Downloads;

using SharedPluginFeatures;

namespace UserAccount.Plugin.Models
{
#pragma warning disable CS1591

    public class DownloadViewModel : BaseModel
    {
        #region Constructors

        public DownloadViewModel()
        {
        }

        public DownloadViewModel(in BaseModelData baseModelData,
            in List<DownloadCategory> categories, in string activeCategory,
            in List<ViewDownloadViewItem> downloads)
            : base(baseModelData)
        {
            if (categories == null)
                throw new ArgumentNullException(nameof(categories));

            if (categories.Count == 0)
                throw new ArgumentOutOfRangeException(nameof(categories));

            if (String.IsNullOrEmpty(activeCategory))
                throw new ArgumentNullException(nameof(activeCategory));

            Categories = categories;
            ActiveCategory = activeCategory;
            Downloads = downloads ?? throw new ArgumentNullException(nameof(downloads));
        }

        #endregion Constructors

        #region Properties

        public List<DownloadCategory> Categories { get; private set; }

        public string ActiveCategory { get; private set; }

        public List<ViewDownloadViewItem> Downloads { get; private set; }

        #endregion Properties
    }

#pragma warning restore CS1591
}
