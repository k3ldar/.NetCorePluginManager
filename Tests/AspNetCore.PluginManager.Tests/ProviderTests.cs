using System;
using System.Collections.Generic;
using System.Text;
using AspNetCore.PluginManager.DemoWebsite.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Middleware;

namespace AspNetCore.PluginManager.Tests.Provider
{
    [TestClass]
    public class ProviderTests
    {
        [TestMethod]
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
