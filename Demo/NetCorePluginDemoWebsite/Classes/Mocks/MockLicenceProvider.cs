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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Demo Website
 *  
 *  File: MockLicenceProvider.cs
 *
 *  Purpose:  Mock ILicenceProvider for tesing purpose
 *
 *  Date        Name                Reason
 *  05/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Middleware;
using Middleware.Accounts.Licences;

namespace AspNetCore.PluginManager.DemoWebsite.Classes
{
    public class MockLicenceProvider : ILicenceProvider
    {
        #region Private Members

        private List<Licence> _licences;

        #endregion Private Members

        #region Public Interface Methods

        public List<LicenceType> LicenceTypesGet()
        {
            return new List<LicenceType>()
                {
                    new LicenceType(1, "Product 1"),
                    new LicenceType(2, "Product 2")
                };
        }

        public List<Licence> LicencesGet(in Int64 userId)
        {
            if (_licences == null)
            {
                _licences = new List<Licence>()
                {
                    new Licence(1, 1, LicenceTypesGet()[0], DateTime.Now.AddMonths(-9), DateTime.Now.AddMonths(3),
                        true, false, 0, 1, "65.45.76.124", String.Empty),
                    new Licence(2, 1, LicenceTypesGet()[1], DateTime.Now.AddMonths(-9), DateTime.Now.AddMonths(3),
                        true, false, 0, 1, "124.76.45.65", "Encrypted String value")
                };
            }

            return _licences;
        }

        public bool LicenceUpdateDomain(in long userId, in Licence licence, in string domain)
        {
            if (licence == null || String.IsNullOrEmpty(domain))
                return false;

            _licences[licence.Id - 1].DomainName = domain;

            return true;
        }

        public bool LicenceSendEmail(in long userId, in int licenceId)
        {
            return true;
        }

        public LicenceCreate LicenceTrialCreate(in Int64 userId, in LicenceType licenceType)
        {
            if (licenceType == null)
                throw new ArgumentNullException(nameof(licenceType));

            if (licenceType.Id == 1)
            {
                _licences.Add(new Licence(_licences.Count + 1, userId, licenceType, DateTime.Now,
                    DateTime.Now.AddMonths(1), true, true, 0, 1, String.Empty, String.Empty));

                return LicenceCreate.Success;
            }

            return LicenceCreate.Failed;
        }

        #endregion Public Interface Methods
    }
}
