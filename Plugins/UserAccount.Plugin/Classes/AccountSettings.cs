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
 *  Product:  UserAccount.Plugin
 *  
 *  File: AccountSettings.cs
 *
 *  Purpose:  User Account Settings
 *
 *  Date        Name                Reason
 *  09/12/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using AppSettings;

namespace UserAccount.Plugin
{
    /// <summary>
    /// Settings that affect how the UserAccount.Plugin module is used and configured.
    /// </summary>
    public sealed class AccountSettings
    {
        /// <summary>
        /// Determines whether orders are shown within the website.
        /// </summary>
        public bool ShowOrders { get; set; }

        /// <summary>
        /// Detemines whether invoices are shown within the website.
        /// </summary>
        /// <value>bool</value>
        public bool ShowInvoices { get; set; }

        /// <summary>
        /// Show credit cards.
        /// 
        /// If you show credit cards you  must never store the credit card number or other details that could be used by hackers.  
        /// 
        /// As well as breaking a number of privacy rules it will leave you open to potential law suits.  Instead store tokenized card details that are encrypted.
        /// </summary>
        /// <value>bool</value>
        public bool ShowCreditCards { get; set; }

        /// <summary>
        /// Show billing address, if you take payments.
        /// </summary>
        /// <value>bool</value>
        public bool ShowBillingAddress { get; set; }

        /// <summary>
        /// Show delivery addresses if you deliver physical products.
        /// </summary>
        /// <value>bool</value>
        public bool ShowDeliveryAddress { get; set; }

        /// <summary>
        /// Show marketing preferences for the user.
        /// </summary>
        /// <value>bool</value>
        public bool ShowMarketingPreferences { get; set; }

        /// <summary>
        /// Shows the licences that are owned by the user.
        /// </summary>
        /// <value>bool</value>
        public bool ShowLicences { get; set; }

        /// <summary>
        /// Show support tickets that the user has created.
        /// </summary>
        /// <value>bool</value>
        public bool ShowSupportTickets { get; set; }

        /// <summary>
        /// Show appointments made by the user, if your website supports appointments.
        /// </summary>
        /// <value>bool</value>
        public bool ShowAppointments { get; set; }

        /// <summary>
        /// Show the downloads section, contains items the user has downloaded.
        /// </summary>
        /// <value>bool</value>
        public bool ShowDownloads { get; set; }

        /// <summary>
        /// If Blog.Plugin module is loaded, this will show an icon for the user to view their own blogs.
        /// </summary>
        /// <value>bool</value>
        public bool ShowBlog { get; set; }

        /// <summary>
        /// Minimum number of upper case characters that have to be in a password.
        /// 
        /// Default: 1
        /// Minimum: 0
        /// Maximum: 3
        /// </summary>
        /// <value>int</value>
        [SettingDefault(1)]
        [SettingRange(0, 3)]
        public int PasswordUppercaseCharCount { get; set; }

        /// <summary>
        /// Minimum number of lower case characters that have to be in a password.
        /// 
        /// Default: 1
        /// Minimum: 0
        /// Maximum: 3
        /// </summary>
        /// <value>int</value>
        [SettingDefault(1)]
        [SettingRange(0, 3)]
        public int PasswordLowercaseCharCount { get; set; }

        /// <summary>
        /// Minimum number of numbers that must appear within a password.
        /// 
        /// Default: 1
        /// Minimum: 0
        /// Maximum: 3
        /// </summary>
        /// <value>int</value>
        [SettingDefault(1)]
        [SettingRange(0, 3)]
        public int PasswordNumberCharCount { get; set; }

        /// <summary>
        /// Minimum number of special characters that must appear within a password.
        /// 
        /// Default: 1
        /// Minimum: 0
        /// Maximum: 3
        /// </summary>
        /// <value>int</value>
        [SettingDefault(1)]
        [SettingRange(0, 3)]
        public int PasswordSpecialCharCount { get; set; }

        /// <summary>
        /// Special password characters that can be used for login password.
        /// 
        /// Default: £$^*()#,.&lt;&gt;?:;!@
        /// </summary>
        /// <value>string</value>
        [SettingDefault("£$^*()#,.<>?:;!@")]
        public string PasswordSpecialCharacters { get; set; }
    }
}
