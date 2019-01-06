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
 *  Copyright (c) 2012 - 2018 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginMiddleware
 *  
 *  File: ILicenceProvider.cs
 *
 *  Purpose:  Licence provider
 *
 *  Date        Name                Reason
 *  05/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Text;

using Middleware.Accounts.Licences;

namespace Middleware
{
    public interface ILicenceProvider
    {
        #region Public Interface Methods

        List<LicenceType> LicenceTypesGet();

        List<Licence> LicencesGet(in Int64 userId);

        bool LicenceUpdateDomain(in Int64 userId, in Licence licence, in string domain);

        bool LicenceSendEmail(in Int64 userId, in int licenceId);

        LicenceCreate LicenceTrialCreate(in Int64 userId, in LicenceType licenceType);

        #endregion Public Interface Methods
    }
}
