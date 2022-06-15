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
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: TableUserRowDefinition.cs
 *
 *  Purpose:  Table definition for user details
 *
 *  Date        Name                Reason
 *  31/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using PluginManager.DAL.TextFiles.Internal;

namespace PluginManager.DAL.TextFiles.Tables
{
    /// <summary>
    /// User table 
    /// </summary>
    [Table(Constants.TableNameUsers, CompressionType.Brotli, CachingStrategy.Memory)]
    internal sealed class TableUser : TableRowDefinition
    {
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string Password { get; set; }

        public DateTime PasswordExpire { get; set; }

        public string Telephone { get; set; }

        public string BusinessName { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }

        public string City { get; set; }

        public string County { get; set; }

        public string Postcode { get; set; }

        public string CountryCode { get; set; }

        public bool Locked { get; set; }

        public bool EmailConfirmed { get; set; }

        public string EmailConfirmCode { get; set; }

        public bool TelephoneConfirmed { get; set; }

        public string TelephoneConfirmCode { get; set; }

        public bool MarketingEmail { get; set; }

        public bool MarketingPostal { get; set; }

        public bool MarketingSms { get; set; }

        public bool MarketingTelephone { get; set; }

        public string FullName => $"{FirstName} {Surname}";
    }
}
