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
