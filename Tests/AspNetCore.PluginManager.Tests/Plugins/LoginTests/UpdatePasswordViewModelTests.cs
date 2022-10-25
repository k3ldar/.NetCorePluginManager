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
    public class UpdatePasswordViewModelTests : GenericBaseClass
    {
        private const string TestCategoryName = "Login";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidatePropertyAttributes()
        {
            Assert.IsTrue(PropertyHasDisplayAttribute(typeof(UpdatePasswordViewModel), "Username", "Username"));
            Assert.IsTrue(PropertyHasRequiredAttribute(typeof(UpdatePasswordViewModel), "Username", "PleaseEnterUserNameOrEmail"));

            Assert.IsTrue(PropertyHasDisplayAttribute(typeof(UpdatePasswordViewModel), "CurrentPassword", "CurrentPassword"));
            Assert.IsTrue(PropertyHasRequiredAttribute(typeof(UpdatePasswordViewModel), "CurrentPassword", "PleaseEnterPassword"));

            Assert.IsTrue(PropertyHasDisplayAttribute(typeof(UpdatePasswordViewModel), "NewPassword", "NewPassword"));
            Assert.IsTrue(PropertyHasRequiredAttribute(typeof(UpdatePasswordViewModel), "NewPassword", "PleaseEnterPassword"));

            Assert.IsTrue(PropertyHasDisplayAttribute(typeof(UpdatePasswordViewModel), "ConfirmNewPassword", "ConfirmPassword"));
            Assert.IsTrue(PropertyHasRequiredAttribute(typeof(UpdatePasswordViewModel), "ConfirmNewPassword", "PasswordDoesNotMatch"));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_DefaultConstructor_Success()
        {
            UpdatePasswordViewModel sut = new UpdatePasswordViewModel();
            Assert.IsNotNull(sut);
            Assert.AreEqual("", sut.Username);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_ModelData_Null_Throws_ArgumentNullException()
        {
            UpdatePasswordViewModel sut = new UpdatePasswordViewModel(null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_Success()
        {
            UpdatePasswordViewModel sut = new UpdatePasswordViewModel(GenerateTestBaseModelData());
            Assert.IsNotNull(sut);
            Assert.AreEqual("", sut.Username);
            Assert.IsNull(sut.CurrentPassword);
            Assert.IsNull(sut.NewPassword);
            Assert.IsNull(sut.ConfirmNewPassword);

            sut.Username = "Joe Blogs";
            sut.CurrentPassword = "old password";
            sut.NewPassword = "new password";
            sut.ConfirmNewPassword = "new password confirm";

            
            Assert.AreEqual("Joe Blogs", sut.Username);
            Assert.AreEqual("old password", sut.CurrentPassword);
            Assert.AreEqual("new password", sut.NewPassword);
            Assert.AreEqual("new password confirm", sut.ConfirmNewPassword);
        }
    }
}
