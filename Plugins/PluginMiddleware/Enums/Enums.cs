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
    /// Login results
    /// </summary>
    public enum LoginResult
    {
        Success = 0,

        InvalidCredentials =  1,

        AccountLocked = 2,

        PasswordChangeRequired = 3,

        Remembered = 4
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
        ShowEmail = 1,

        ShowTelephone = 2,

        ShowSMS = 4,

        ShowPostal = 8,
    }

    /// <summary>
    /// Invoice and Order Process Status
    /// </summary>
    public enum ProcessStatus
    {
        OrderReceived = 0,

        FraudCheck = 1,

        PaymentCheck = 2,

        Processing = 3,

        Dispatched = 4,

        IssueRefund = 5,

        PaymentPending = 6,

        OnHold = 7,

        Cancelled = 10
    }

    /// <summary>
    /// Order / Invoice Payment Status
    /// </summary>
    public enum PaymentStatus
    {
        Unpaid = 0,

        Paid = 1,

        PaidCash = 2,

        PaidCheque = 3,

        PaidCard= 4,

        PaidVoucher = 5,

        PaidMixed = 6
    }

    /// <summary>
    /// Invoice / Order Item Status
    /// </summary>
    public enum ItemStatus
    {
        Received = 0,

        Processing = 1,

        Dispatched = 2,

        OnHold = 3,

        BackOrder = 4,

        Cancelled = 10,
    }

    public enum DiscountType
    {
        None = 0,

        PercentageTotal = 1,

        PercentageSubTotal= 2,

        Value = 3
    }
}
