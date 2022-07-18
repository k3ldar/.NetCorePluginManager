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
            List<Licence> Result = new List<Licence>();

            UserDataRow user =  _users.Select(userId);

            if (user == null)
                return Result;

            List<LicenseTypeDataRow> licenseTypes = _licenseTypes.Select().ToList();
            List<LicenseDataRow> userLicenses = _licenses.Select().Where(l => l.UserId.Equals(user.Id)).ToList();

            userLicenses.ForEach(ul =>
            {
                LicenseTypeDataRow licenseTypeDataRow = licenseTypes.Where(lt => lt.Id.Equals(ul.LicenseType)).First();
                Result.Add(new Licence(ul.Id, ul.UserId, new LicenceType(licenseTypeDataRow.Id, licenseTypeDataRow.Description), ul.StartDate, 
                    ul.ExpireDate, ul.IsValid, ul.IsTrial, ul.UpdateCount, ul.InvoiceId, ul.DomainName, ul.EncryptedLicense));
            });

            return Result;
        }

        public bool LicenceUpdateDomain(in long userId, in Licence licence, in string domain)
        {
            if (licence == null || String.IsNullOrEmpty(domain))
                return false;

            UserDataRow user = _users.Select(userId);

            if (user == null)
                return false;

            LicenseDataRow licenseDataRow = _licenses.Select(licence.Id);

            if (licenseDataRow == null)
                return false;

            licenseDataRow.DomainName = domain;
            licenseDataRow.UpdateCount++;


            bool Result = licenseDataRow.HasChanged;

            _licenses.Update(licenseDataRow);

            return Result;
        }

        public bool LicenceSendEmail(in long userId, in int licenceId)
        {

            throw new NotImplementedException();
        }

        public LicenceCreate LicenceTrialCreate(in Int64 userId, in LicenceType licenceType)
        {
            if (licenceType == null)
                throw new ArgumentNullException(nameof(licenceType));

            UserDataRow user = _users.Select(userId);

            if (user == null)
               return LicenceCreate.Failed;

            long licenseTypeId = licenceType.Id;

            if (_licenses.Select().Where(l => l.UserId.Equals(user.Id) && l.LicenseType.Equals(licenseTypeId) && l.IsTrial).Any())
                return LicenceCreate.Existing;

            LicenseDataRow licenseDataRow = new LicenseDataRow()
            {
                InvoiceId = 0,
                UserId = userId,
                LicenseType = licenceType.Id,
                IsTrial = true,
                IsValid = true,
                StartDateTicks = DateTime.UtcNow.Ticks,
                ExpireDateTicks = DateTime.UtcNow.AddDays(30).Ticks,                
            };

            _licenses.Insert(licenseDataRow);

            return LicenceCreate.Success;
        }

        #endregion Public Interface Methods
    }
}
