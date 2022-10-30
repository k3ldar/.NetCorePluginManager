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
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: AccountLockedViewModelTests.cs
 *
 *  Purpose:  Unit tests for AccountLockedViewModel
 *
 *  Date        Name                Reason
 *  09/08/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using LoginPlugin.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AspNetCore.PluginManager.Tests.Plugins.LoginTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class AccountLockedViewModelTests : GenericBaseClass
    {
        private const string TestCategoryName = "Login";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidatePropertyAttributes()
        {
            Assert.IsTrue(PropertyHasDisplayAttribute(typeof(AccountLockedViewModel), "Username", "Username"));
            Assert.IsTrue(PropertyHasRequiredAttribute(typeof(AccountLockedViewModel), "Username", "PleaseEnterUserNameOrEmail"));

            Assert.IsTrue(PropertyHasDisplayAttribute(typeof(AccountLockedViewModel), "UnlockCode", "Code"));
            Assert.IsTrue(PropertyHasRequiredAttribute(typeof(AccountLockedViewModel), "UnlockCode", "EnterUnlockCode"));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_DefaultConstructor_Success()
        {
            AccountLockedViewModel sut = new AccountLockedViewModel();
            Assert.IsNotNull(sut);
            Assert.AreEqual("", sut.Username);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_ModelData_Null_Throws_ArgumentNullException()
        {
            AccountLockedViewModel sut = new AccountLockedViewModel(null, "user");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_Username_Null_Throws_ArgumentNullException()
        {
            AccountLockedViewModel sut = new AccountLockedViewModel(GenerateTestBaseModelData(), null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_Username_EmptyString_Throws_ArgumentNullException()
        {
            AccountLockedViewModel sut = new AccountLockedViewModel(GenerateTestBaseModelData(), "");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_Success()
        {
            AccountLockedViewModel sut = new AccountLockedViewModel(GenerateTestBaseModelData(), "user");
            Assert.IsNotNull(sut);
            Assert.AreEqual("user", sut.Username);
            Assert.IsNull(sut.UnlockCode);

            sut.UnlockCode = "123";

            Assert.AreEqual("123", sut.UnlockCode);
        }
    }
}
