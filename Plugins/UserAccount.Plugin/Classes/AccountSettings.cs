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
 *  Copyright (c) 2018 Simon Carter.  All Rights Reserved.
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

namespace UserAccount.Plugin
{
    public sealed class AccountSettings
    {
        public bool ShowOrders { get; set; }

        public bool ShowInvoices { get; set; }

        public bool ShowCreditCards { get; set; }

        public bool ShowBillingAddress { get; set; }

        public bool ShowDeliveryAddress { get; set; }

        public bool ShowSpecialOffers { get; set; }

        public bool ShowLicences { get; set; }

        public bool ShowSupportTickets { get; set; }

        public bool ShowAppointments { get; set; }

        public bool ShowDownloads { get; set; }
    }
}
