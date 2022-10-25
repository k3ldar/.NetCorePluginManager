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
 *  Product:  UserAccount.Plugin
 *  
 *  File: AccountViewModel.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  10/12/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using SharedPluginFeatures;

namespace UserAccount.Plugin.Models
{
#pragma warning disable CS1591

    public class AccountViewModel : BaseModel
    {
        #region Constructors

        public AccountViewModel()
        {
            GrowlMessage = String.Empty;
        }

        public AccountViewModel(in BaseModelData baseModelData,
            AccountSettings accountSettings, string growl)
            : this(baseModelData, accountSettings)
        {
            GrowlMessage = growl;
        }

        public AccountViewModel(in BaseModelData baseModelData,
            AccountSettings accountSettings)
            : base(baseModelData)
        {
            Settings = accountSettings ?? throw new ArgumentNullException(nameof(accountSettings));
        }

        #endregion Constructors

        #region Properties

        public AccountSettings Settings { get; private set; }

        public string GrowlMessage { get; set; }

        #endregion Properties
    }

#pragma warning restore CS1591
}
