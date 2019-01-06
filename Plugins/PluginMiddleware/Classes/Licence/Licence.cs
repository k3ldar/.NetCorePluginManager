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
    public sealed class Licence
    {
        #region Constructors

        public Licence(in int id, in Int64 userId, in LicenceType licenceType, in DateTime startDate, 
            in DateTime expireDate, in bool isValid, in bool isTrial, in byte updateCount, 
            in int invoiceId, in string domainName, in string encryptedLicence)
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

        public int Id { get; private set; }

        public Int64 UserId { get; private set; }

        public LicenceType LicenceType { get; private set; }

        public DateTime StartDate { get; private set; }

        public DateTime ExpireDate { get; private set; }

        public bool IsValid { get; private set; }

        public bool IsTrial { get; private set; }

        public byte UpdateCount { get; private set; }

        public int InvoiceId { get; private set; }

        public string DomainName { get; set; }

        public string EncryptedLicence { get; private set; }

        #endregion Properties
    }
}
