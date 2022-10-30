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
 *  Product:  PluginMiddleware
 *  
 *  File: Enums.cs
 *
 *  Purpose:  Shared Enum Values
 *
 *  Date        Name                Reason
 *  16/12/2018  Simon Carter        Moved to PluginMiddleware library
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace Middleware
{
	/// <summary>
	/// Resource type
	/// </summary>
	public enum ResourceType
	{
		/// <summary>
		/// Text resource type
		/// </summary>
		Text = 0,

		/// <summary>
		/// Image resource type
		/// </summary>
		Image = 1,

		/// <summary>
		/// Uri resource type
		/// </summary>
		Uri = 2,

		/// <summary>
		/// Youtube resource type
		/// </summary>
		YouTube = 3,

		/// <summary>
		/// TikTok resource type
		/// </summary>
		TikTok = 4,
	}

    /// <summary>
    /// Login results
    /// </summary>
    public enum LoginResult
    {
        /// <summary>
        /// Login was successfull.
        /// </summary>
        Success = 0,

        /// <summary>
        /// Invalid credentials provided.
        /// </summary>
        InvalidCredentials = 1,

        /// <summary>
        /// Account is locked.
        /// </summary>
        AccountLocked = 2,

        /// <summary>
        /// Password change is required.
        /// </summary>
        PasswordChangeRequired = 3,

        /// <summary>
        /// Login details were remembered, Login was successfull.
        /// </summary>
        Remembered = 4
    }

    /// <summary>
    /// Address Option
    /// </summary>
    public enum AddressOption
    {
        /// <summary>
        /// Delivery Address Option
        /// </summary>
        Delivery,

        /// <summary>
        /// Billing Address Option
        /// </summary>
        Billing,
    }

    /// <summary>
    /// Address options
    /// </summary>
    [Flags]
    public enum AddressOptions
    {
        /// <summary>
        /// Determines wether to show the business name or not
        /// </summary>
        BusinessNameShow = 1,

        /// <summary>
        /// Determines wether Business Name is mandatory
        /// </summary>
        BusinessNameMandatory = 2,

        /// <summary>
        /// Determines wether AddressLine 1 is shown or not
        /// </summary>
        AddressLine1Show = 4,

        /// <summary>
        /// Determines wether AddressLine1 is mandatory or not
        /// </summary>
        AddressLine1Mandatory = 8,

        /// <summary>
        /// Determines wether AddressLine 2 is shown or not
        /// </summary>
        AddressLine2Show = 16,

        /// <summary>
        /// Determines wether AddressLine2 is mandatory or not
        /// </summary>
        AddressLine2Mandatory = 32,

        /// <summary>
        /// Determines wether AddressLine 3 is shown or not
        /// </summary>
        AddressLine3Show = 64,

        /// <summary>
        /// Determines wether AddressLine1 is mandatory or not
        /// </summary>
        AddressLine3Mandatory = 128,

        /// <summary>
        /// Determines wether to show city data or not
        /// </summary>
        CityShow = 256,

        /// <summary>
        /// Determines wether City is mandatory or not
        /// </summary>
        CityMandatory = 512,

        /// <summary>
        /// Determines wether to show county or not
        /// </summary>
        CountyShow = 1024,

        /// <summary>
        /// Determines wether County is mandatory or not
        /// </summary>
        CountyMandatory = 2048,

        /// <summary>
        /// Determines wether to show the postcode or not
        /// </summary>
        PostCodeShow = 4096,

        /// <summary>
        /// Determines wether post code is mandatory or not
        /// </summary>
        PostCodeMandatory = 8192,

        /// <summary>
        /// Determines wether telephone number is shown
        /// </summary>
        TelephoneShow = 16384,

        /// <summary>
        /// Determines wether telephone is a mandatory field
        /// </summary>
        TelephoneMandatory = 32768
    }

    /// <summary>
    /// Marketing Optons
    /// </summary>
    [Flags]
    public enum MarketingOptions
    {
        /// <summary>
        /// Show email marketing check box.
        /// </summary>
        ShowEmail = 1,

        /// <summary>
        /// Show telephone marketing check box.
        /// </summary>
        ShowTelephone = 2,

        /// <summary>
        /// Show sms marketing check box.
        /// </summary>
        ShowSMS = 4,

        /// <summary>
        /// Show postal marketing check box.
        /// </summary>
        ShowPostal = 8,
    }

    /// <summary>
    /// Invoice and Order Process Status
    /// </summary>
    public enum ProcessStatus
    {
        /// <summary>
        /// Order is in received status.
        /// </summary>
        OrderReceived = 0,

        /// <summary>
        /// Order is being processed for fraud.
        /// </summary>
        FraudCheck = 1,

        /// <summary>
        /// Payment is being verified.
        /// </summary>
        PaymentCheck = 2,

        /// <summary>
        /// Order is being processed for dispatch.
        /// </summary>
        Processing = 3,

        /// <summary>
        /// Order has been dispatched.
        /// </summary>
        Dispatched = 4,

        /// <summary>
        /// Order is having a refund processed.
        /// </summary>
        IssueRefund = 5,

        /// <summary>
        /// Payment is pending.
        /// </summary>
        PaymentPending = 6,

        /// <summary>
        /// Order is on hold.
        /// </summary>
        OnHold = 7,

        /// <summary>
        /// Order has been cancelled.
        /// </summary>
        Cancelled = 10
    }

    /// <summary>
    /// Order / Invoice Payment Status
    /// </summary>
    public enum PaymentStatus
    {
        /// <summary>
        /// Invoice/order has not been paid.
        /// </summary>
        Unpaid = 0,

        /// <summary>
        /// Invoice/order has been paid.
        /// </summary>
        Paid = 1,

        /// <summary>
        /// Invoice/order has been paid using cash.
        /// </summary>
        PaidCash = 2,

        /// <summary>
        /// Invoice/order has been paid using checkque.
        /// </summary>
        PaidCheque = 3,

        /// <summary>
        /// Invoice/order has been paid using a credit/debit card.
        /// </summary>
        PaidCard = 4,

        /// <summary>
        /// Invoice/order has been paid using a voucher.
        /// </summary>
        PaidVoucher = 5,

        /// <summary>
        /// Invoice/order has been paid using mixed payment types.
        /// </summary>
        PaidMixed = 6
    }

    /// <summary>
    /// Invoice / Order Item Status
    /// </summary>
    public enum ItemStatus
    {
        /// <summary>
        /// Invoice/order item has been received.
        /// </summary>
        Received = 0,

        /// <summary>
        /// Invoice/order item is being processed.
        /// </summary>
        Processing = 1,

        /// <summary>
        /// Invoice/order item has been dispatched.
        /// </summary>
        Dispatched = 2,

        /// <summary>
        /// Invoice/order item is on hold.
        /// </summary>
        OnHold = 3,

        /// <summary>
        /// Invoice/order item is on back order.
        /// </summary>
        BackOrder = 4,

        /// <summary>
        /// Invoice/order item has been cancelled.
        /// </summary>
        Cancelled = 10,
    }

    /// <summary>
    /// Licence creation results.
    /// </summary>
    public enum LicenceCreate
    {
        /// <summary>
        /// Licence was successfully created.
        /// </summary>
        Success = 0,

        /// <summary>
        /// Licence already exists.
        /// </summary>
        Existing = 1,

        /// <summary>
        /// Failed to create a licence.
        /// </summary>
        Failed = 2
    }

	/// <summary>
	/// Result of adding/removing a bookmark
	/// </summary>
	public enum BookmarkActionResult
	{
		/// <summary>
		/// Unknown response or error
		/// </summary>
		Unknown,

		/// <summary>
		/// Bookmark has been added
		/// </summary>
		Added,

		/// <summary>
		/// Bookmark has been removed
		/// </summary>
		Removed,

		/// <summary>
		/// The max number of allowed bookmarks has been reached
		/// </summary>
		QuotaExceeded,
	}
}
