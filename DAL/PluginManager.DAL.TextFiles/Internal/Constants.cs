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
 *  File: Constants.cs
 *
 *  Purpose:  Internal constants
 *
 *  Date        Name                Reason
 *  05/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace PluginManager.DAL.TextFiles
{
    internal class Constants
    {
        public const string TableNameSettings = "Settings";
        public const string TableNameUsers = "Users";
        public const string TableNameBlogs = "Blogs";
        public const string TableNameBlogComments = "BlogComments";
        public const string TableNameCountries = "Countries";
        public const string TableNameExternalUsers = "ExternalUsers";
        public const string TableNameUserClaims = "UserClaims";
        public const string TableNameUserApi = "UserApi";
        public const string TableNameAddresses = "Addresses";
        public const string TableNameOrders = "Orders";
        public const string TableNameOrderItems = "OrderItems";
        public const string TableNameInvoices = "Invoices";
        public const string TableNameInvoiceItems = "InvoiceItems";
        public const string TableNameSeo = "Seo";
        public const string TableNameProducts = "Products";
        public const string TableNameProductGroups = "ProductGroups";
        public const string TableNameShoppingCart = "ShoppingCart";
        public const string TableNameShoppingCartItems = "ShoppingCartItems";
        public const string TableNameShoppingCartVoucher = "ShoppingCartVoucher";
        public const string TableNameStock = "Stock";
        public const string TableNameLicenseTypes = "LicenseTypes";
        public const string TableNameLicenses = "Licenses";
        public const string TableNameFeedback = "Feedback";

        public const string DomainHelpdesk = "Helpdesk";
        public const string TableNameTicket = "HelpdeskTicket";
        public const string TableNameTicketMessages = "HelpdeskTicketMessage";
        public const string TableNameTicketDepartments = "HelpdeskTicketDepartments";
        public const string TableNameTicketPriorities = "HelpdeskTicketPriorities";
        public const string TableNameTicketStatus = "HelpdeskTicketStatus";
        public const string TableNameFAQ = "FAQGroups";
        public const string TableNameFAQItem = "FAQItems";

        public const string DomainDownloads = "Downloads";
        public const string TableNameDownloadCategories = "DownloadCategories";
        public const string TableNameDownloadItems = "DownloadItems";

        public const string DomainDynamicContent = "DynamicContent";
        public const string TableNameContentPage = "Page";
        public const string TableNameContentItem = "PageItems";

		public const string DomainSessions = "Sessions";
		public const string TableNameInitialReferrals = "InitialReferrals";
		public const string TableNamePageViews = "PageViews";
		public const string TableNameSession = "Sessions";
		public const string TableNameSessionPages = "SessionPages";
		public const string TableNameSessionStats = "TableNameSessionStats";
		public const string TableNameSessionStatsBot = "TableNameSessionStatsBot";
		public const string TableNameSessionStatsHourly = "TableNameSessionStatsHourly";
		public const string TableNameSessionStatsDaily = "TableNameSessionStatsDaily";
		public const string TableNameSessionStatsWeekly = "TableNameSessionStatsWeekly";
		public const string TableNameSessionStatsMonthly = "TableNameSessionStatsMonthly";
		public const string TableNameSessionStatsYearly = "TableNameSessionStatsYearly";

		public const string TableDomainResources = "Resources";
		public const string TableNameResourceCateogories = "ResourceCategories";
		public const string TableNameResourceItems = "ResourceItems";
		public const string TableNameResourceResponse = "ResourceResponse";
		public const string TableNameResourceBookmarks = "ResourceBookmarks";
	}
}
