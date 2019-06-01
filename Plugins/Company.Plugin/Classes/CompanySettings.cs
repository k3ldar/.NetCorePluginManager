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
 *  Product:  Company.Plugin
 *  
 *  File: CompanySettings.cs
 *
 *  Purpose:  Settings
 *
 *  Date        Name                Reason
 *  07/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using AppSettings;

#pragma warning disable CS1591

namespace Company.Plugin.Classes
{
    public sealed class CompanySettings
    {
        #region Properties

        [SettingDefault(true)]
        public bool ShowAbout { get; set; }

        [SettingDefault(false)]
        public bool ShowCareers { get; set; }

        [SettingDefault(true)]
        public bool ShowContact { get; set; }

        [SettingDefault(false)]
        public bool ShowCookies { get; set; }

        [SettingDefault(false)]
        public bool ShowDelivery { get; set; }

        [SettingDefault(false)]
        public bool ShowNewsletter { get; set; }

        [SettingDefault(false)]
        public bool ShowPrivacy { get; set; }

        [SettingDefault(false)]
        public bool ShowReturns { get; set; }

        [SettingDefault(false)]
        public bool ShowTerms { get; set; }

        [SettingDefault(false)]
        public bool ShowAffiliates { get; set; }

        #endregion Properties
    }
}

#pragma warning restore CS1591