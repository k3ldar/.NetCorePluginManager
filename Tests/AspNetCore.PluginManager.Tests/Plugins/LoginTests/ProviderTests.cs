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
 *  File: ProviderTests.cs
 *
 *  Purpose:  Tests for Provider
 *
 *  Date        Name                Reason
 *  31/07/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.DemoWebsite.Classes;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;

namespace AspNetCore.PluginManager.Tests.Plugins.LoginTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ProviderTests
    {
        private const string TestCategoryName = "Login";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void LoginProvider_ValidLogon_PasswordChangeRequired()
        {
            ILoginProvider loginProvider = new MockLoginProvider();

            UserLoginDetails userLoginDetails = new UserLoginDetails();
            LoginResult loginResult = loginProvider.Login("admin", "changepassword", "::1", 0, ref userLoginDetails);

            Assert.AreEqual(LoginResult.PasswordChangeRequired, loginResult);
            Assert.AreEqual(124, userLoginDetails.UserId);
        }
    }
}
