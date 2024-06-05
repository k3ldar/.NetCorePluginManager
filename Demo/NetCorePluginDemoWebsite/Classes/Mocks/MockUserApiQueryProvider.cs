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
 *  Product:  Demo Website
 *  
 *  File: MockApiAuthorizationService.cs
 *
 *  Purpose:  Mock IApiAuthorizationService for tesing purpose
 *
 *  Date        Name                Reason
 *  23/11/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using Middleware;

namespace AspNetCore.PluginManager.DemoWebsite.Classes.Mocks
{
	[ExcludeFromCodeCoverage(Justification = "Code coverage not required for mock classes")]
	public class MockUserApiQueryProvider : IUserApiQueryProvider
    {
        private const string MockMerchantId = "mer-9djn5r49fdljnfkjed89dfljhsaf9";
        private const string MockApiKey = "GH9asdflnler08dsfowlaenfrlasdkfnpo8u";
        private const string MockSecret = "iOfdafasdfcDSAF48scdjkfnasdfSAAf";

        public bool ApiSecret(string merchantId, string apiKey, out string secret)
        {
            if (merchantId.Equals(MockMerchantId) && apiKey.Equals(MockApiKey))
            {
                secret = MockSecret;
                return true;
            }

            secret = String.Empty;
            return false;
        }
    }
}
