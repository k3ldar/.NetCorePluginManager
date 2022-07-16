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
 *  File: LicenceProvider.cs
 *
 *  Purpose:  ILicenceProvider for text based storage
 *
 *  Date        Name                Reason
 *  25/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Middleware;
using Middleware.Accounts.Licences;

using PluginManager.DAL.TextFiles.Tables;

namespace PluginManager.DAL.TextFiles.Providers
{
    internal class LicenceProvider : ILicenceProvider
    {
        #region Private Members

        private readonly ITextTableOperations<UserDataRow> _users;
        private readonly ITextTableOperations<InvoiceDataRow> _invoices;
        private readonly ITextTableOperations<LicenseDataRow> _licenses;
        private readonly ITextTableOperations<LicenseTypeDataRow> _licenseTypes;

        #endregion Private Members

        #region Constructors

        public LicenceProvider(ITextTableOperations<UserDataRow> users,
            ITextTableOperations<InvoiceDataRow> invoices,
            ITextTableOperations<LicenseDataRow> addresses,
            ITextTableOperations<LicenseTypeDataRow> orders)
        {
            _users = users ?? throw new ArgumentNullException(nameof(users));
            _invoices = invoices ?? throw new ArgumentNullException(nameof(invoices));
            _licenses = addresses ?? throw new ArgumentNullException(nameof(addresses));
            _licenseTypes = orders ?? throw new ArgumentNullException(nameof(orders));

        }

        #endregion Constructors

        #region Public Interface Methods

        public List<LicenceType> LicenceTypesGet()
        {
            List<LicenceType> Result = new List<LicenceType>();
            IReadOnlyList<LicenseTypeDataRow> licenseTypes = _licenseTypes.Select();

            foreach (LicenseTypeDataRow row in licenseTypes)
            {
                Result.Add(new LicenceType(row.Id, row.Description));
            }

            return Result;
        }

        public List<Licence> LicencesGet(in Int64 userId)
        {
            //if (_licences == null)
            //{
            //    _licences = new List<Licence>()
            //    {
            //        new Licence(1, 1, LicenceTypesGet()[0], DateTime.Now.AddMonths(-9), DateTime.Now.AddMonths(3),
            //            true, false, 0, 1, "65.45.76.124", String.Empty),
            //        new Licence(2, 1, LicenceTypesGet()[1], DateTime.Now.AddMonths(-9), DateTime.Now.AddMonths(3),
            //            true, false, 0, 1, "124.76.45.65", "Encrypted String value")
            //    };
            //}

            //return _licences;

            throw new NotImplementedException();
        }

        public bool LicenceUpdateDomain(in long userId, in Licence licence, in string domain)
        {
            //if (licence == null || String.IsNullOrEmpty(domain))
            //    return false;

            //_licences[licence.Id - 1].DomainName = domain;

            //return true;

            throw new NotImplementedException();
        }

        public bool LicenceSendEmail(in long userId, in int licenceId)
        {

            throw new NotImplementedException();
        }

        public LicenceCreate LicenceTrialCreate(in Int64 userId, in LicenceType licenceType)
        {
            if (licenceType == null)
                throw new ArgumentNullException(nameof(licenceType));

            //if (licenceType.Id == 1)
            //{
            //    _licences.Add(new Licence(_licences.Count + 1, userId, licenceType, DateTime.Now,
            //        DateTime.Now.AddMonths(1), true, true, 0, 1, String.Empty, String.Empty));

            //    return LicenceCreate.Success;
            //}

            //return LicenceCreate.Failed;

            throw new NotImplementedException();
        }

        #endregion Public Interface Methods
    }
}
