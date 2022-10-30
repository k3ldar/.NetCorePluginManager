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
 *  File: LoginViewModelTests.cs
 *
 *  Purpose:  Test units for LoginViewModel
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
    public class LoginViewModelTests : GenericBaseClass
    {
        private const string TestCategoryName = "Login";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidatePropertyAttributes()
        {
            Assert.IsTrue(PropertyHasDisplayAttribute(typeof(LoginViewModel), "Username", "Username"));
            Assert.IsTrue(PropertyHasRequiredAttribute(typeof(LoginViewModel), "Username", "PleaseEnterUserNameOrEmail"));

            Assert.IsTrue(PropertyHasDisplayAttribute(typeof(LoginViewModel), "Password", "Password"));
            Assert.IsTrue(PropertyHasRequiredAttribute(typeof(LoginViewModel), "Password", "PleaseEnterPassword"));

            Assert.IsTrue(PropertyHasDisplayAttribute(typeof(LoginViewModel), "CaptchaText", "Code"));

            Assert.IsTrue(PropertyHasDisplayAttribute(typeof(LoginViewModel), "RememberMe", "RememberMe"));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_DefaultConstructor_Success()
        {
            LoginViewModel sut = new LoginViewModel();
            Assert.IsNotNull(sut);
            Assert.IsNull(sut.Username);
            Assert.IsNull(sut.ReturnUrl);
            Assert.IsNull(sut.Password);
            Assert.IsNull(sut.CaptchaText);
            Assert.IsFalse(sut.ShowCaptchaImage);
            Assert.IsFalse(sut.RememberMe);
            Assert.IsFalse(sut.ShowRememberMe);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_ModelData_Null_Throws_ArgumentNullException()
        {
            LoginViewModel sut = new LoginViewModel(null, "returnUrl", true, true, true);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_ReturnUrl_Null_Throws_ArgumentNullException()
        {
            LoginViewModel sut = new LoginViewModel(GenerateTestBaseModelData(), null, true, true, true);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidParam_ReturnUrl_EmptyString_DoesNotThrow_ArgumentNullException()
        {
            LoginViewModel sut = new LoginViewModel(GenerateTestBaseModelData(), "", true, true, true);
            Assert.IsNotNull(sut);
            Assert.AreEqual("", sut.ReturnUrl);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_Success()
        {
            LoginViewModel sut = new LoginViewModel(GenerateTestBaseModelData(), "returnUrl", true, true, true);
            Assert.IsNotNull(sut);

            Assert.IsNotNull(sut);
            Assert.IsNull(sut.Username);
            Assert.AreEqual("returnUrl", sut.ReturnUrl);
            Assert.IsNull(sut.Password);
            Assert.IsNull(sut.CaptchaText);
            Assert.IsFalse(sut.ShowCaptchaImage);
            Assert.IsFalse(sut.RememberMe);
            Assert.IsTrue(sut.ShowRememberMe);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AssignProperties_ValuesAreRemembered_Success()
        {
            LoginViewModel sut = new LoginViewModel(GenerateTestBaseModelData(), "returnUrl", true, true, true)
            {
                Username = "joe bloggs",
                Password = "not a real password",
                CaptchaText = "Art123",
                ShowCaptchaImage = true,
                RememberMe = true
            };

            Assert.IsNotNull(sut);

            Assert.IsNotNull(sut);
            Assert.AreEqual("joe bloggs", sut.Username);
            Assert.AreEqual("returnUrl", sut.ReturnUrl);
            Assert.AreEqual("not a real password", sut.Password);
            Assert.AreEqual("Art123", sut.CaptchaText);
            Assert.IsTrue(sut.ShowCaptchaImage);
            Assert.IsTrue(sut.RememberMe);
            Assert.IsTrue(sut.ShowRememberMe);
        }
    }
}
