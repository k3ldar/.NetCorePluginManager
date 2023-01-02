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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginMiddleware
 *  
 *  File: Licence.cs
 *
 *  Purpose:  Licence Types
 *
 *  Date        Name                Reason
 *  05/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace Middleware.Accounts.Licences
{
    /// <summary>
    /// User licence.  Provides details on an individual user licence.
    /// </summary>
    public sealed class Licence
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Unique id of licence.</param>
        /// <param name="userId">Unique user id.</param>
        /// <param name="licenceType">Type of licence.</param>
        /// <param name="startDate">Date/Time licence valid from.</param>
        /// <param name="expireDate">Date/Time licence expires.</param>
        /// <param name="isValid">Indicates whether the licence is valid or not.</param>
        /// <param name="isTrial">Indicates whether the licence is a trial licence.</param>
        /// <param name="updateCount">Number of time the domain has been updated.</param>
        /// <param name="invoiceId">Unique invoice id associated with the licence.</param>
        /// <param name="domainName">Domain or Ip address where the licence is linked to.</param>
        /// <param name="encryptedLicence">Encrypted licence details.</param>
        public Licence(in long id, in long userId, in LicenceType licenceType, in DateTime startDate,
            in DateTime expireDate, in bool isValid, in bool isTrial, in byte updateCount,
            in long invoiceId, in string domainName, in string encryptedLicence)
        {
            Id = id;
            UserId = userId;
            LicenceType = licenceType ?? throw new ArgumentNullException(nameof(licenceType));
            StartDate = startDate;
            ExpireDate = expireDate;
            IsValid = isValid;
            IsTrial = isTrial;
            UpdateCount = updateCount;
            InvoiceId = invoiceId;
            DomainName = domainName;
            EncryptedLicence = encryptedLicence ?? throw new ArgumentNullException(nameof(encryptedLicence));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Unique user id.
        /// </summary>
        /// <value>int</value>
        public long Id { get; private set; }

        /// <summary>
        /// Unique user id.
        /// </summary>
        /// <value>long</value>
        public long UserId { get; private set; }

        /// <summary>
        /// Type of licence.
        /// </summary>
        /// <value>LicenceType</value>
        public LicenceType LicenceType { get; private set; }

        /// <summary>
        /// Date/Time licence valid from.
        /// </summary>
        /// <value>DateTime</value>
        public DateTime StartDate { get; private set; }

        /// <summary>
        /// Date/Time licence expires.
        /// </summary>
        /// <value>DateTime</value>
        public DateTime ExpireDate { get; private set; }

        /// <summary>
        /// Indicates whether the licence is valid or not.
        /// </summary>
        /// <value>bool</value>
        public bool IsValid { get; private set; }

        /// <summary>
        /// Indicates whether the licence is a trial licence.
        /// </summary>
        /// <value>bool</value>
        public bool IsTrial { get; private set; }

        /// <summary>
        /// Number of time the domain has been updated.
        /// </summary>
        /// <value>byte</value>
        public byte UpdateCount { get; private set; }

        /// <summary>
        /// Unique invoice id associated with the licence.
        /// </summary>
        /// <value>int</value>
        public long InvoiceId { get; private set; }

        /// <summary>
        /// Domain or Ip address where the licence is linked to.
        /// </summary>
        /// <value>string</value>
        public string DomainName { get; set; }

        /// <summary>
        /// Encrypted licence details.
        /// </summary>
        /// <value>string</value>
        public string EncryptedLicence { get; private set; }

        #endregion Properties
    }
}
