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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: TestConnectionInfo.cs
 *
 *  Purpose:  Mock ConnectionInfo class
 *
 *  Date        Name                Reason
 *  20/04/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace AspNetCore.PluginManager.Tests
{
    public class TestConnectionInfo : ConnectionInfo
    {
        #region ConnectionInfo Methods

        public override X509Certificate2 ClientCertificate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override String Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override IPAddress LocalIpAddress { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override Int32 LocalPort { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override IPAddress RemoteIpAddress { get; set; }

        public override Int32 RemotePort { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override Task<X509Certificate2> GetClientCertificateAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        #endregion ConnectionInfo Methods
    }
}
