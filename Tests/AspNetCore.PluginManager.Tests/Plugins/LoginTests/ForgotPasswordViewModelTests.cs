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
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: ForgotPasswordViewModelTests.cs
 *
 *  Purpose:  Test units ForgotPasswordViewModel
 *
 *  Date        Name                Reason
 *  09/08/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using LoginPlugin.Classes;
using LoginPlugin.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AspNetCore.PluginManager.Tests.Plugins.LoginTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ForgotPasswordViewModelTests : GenericBaseClass
    {
        private const string TestCategoryName = "Login";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidatePropertyAttributes()
        {
            Assert.IsTrue(PropertyHasDisplayAttribute(typeof(ForgotPasswordViewModel), "Username", "Username"));
            Assert.IsTrue(PropertyHasRequiredAttribute(typeof(ForgotPasswordViewModel), "Username", "PleaseEnterUserNameOrEmail"));

            Assert.IsTrue(PropertyHasDisplayAttribute(typeof(ForgotPasswordViewModel), "CaptchaText", "Code"));
            Assert.IsTrue(PropertyHasRequiredAttribute(typeof(ForgotPasswordViewModel), "CaptchaText", "CodeNotValid"));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_DefaultConstructor_Success()
        {
            ForgotPasswordViewModel sut = new ForgotPasswordViewModel();
            Assert.IsNotNull(sut);
            Assert.IsNull(sut.Username);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_ModelData_Null_Throws_ArgumentNullException()
        {
            ForgotPasswordViewModel sut = new ForgotPasswordViewModel(null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_Success()
        {
            ForgotPasswordViewModel sut = new ForgotPasswordViewModel(GenerateTestBaseModelData());
            Assert.IsNotNull(sut);
            Assert.IsNull(sut.Username);
            Assert.IsNull(sut.CaptchaText);

            sut.CaptchaText = "123";
            sut.Username = "user";

            Assert.AreEqual("123", sut.CaptchaText);
            Assert.AreEqual("user", sut.Username);
        }
    }
}
