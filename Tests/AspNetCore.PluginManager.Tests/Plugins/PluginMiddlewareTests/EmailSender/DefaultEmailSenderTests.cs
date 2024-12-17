/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: DynamicContentPageTests.cs
 *
 *  Purpose:  Tests for DynamicContentPage
 *
 *  Date        Name                Reason
 *  08/04/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware.Classes;

using PluginManager.Tests.Mocks;

namespace AspNetCore.PluginManager.Tests.Plugins.PluginMiddlewareTests.EmailSender
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DefaultEmailSenderTests
    {
        private const string TestCategoryName = "Email Sender";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_SettingsProviderNull_Throws_ArgumentNullException()
        {
            new DefaultEmailSender(null, new MockLogger());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_LoggerNull_Throws_ArgumentNullException()
        {
            new DefaultEmailSender(new MockSettingsProvider("{}"), null);
        }

        [Ignore("Ignored as relies on environment variables which may not always be present")]
        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_Success()
        {
            DefaultEmailSender sut = new DefaultEmailSender(new MockSettingsProvider("{}"), new MockLogger());
            Assert.IsNotNull(sut);
            Assert.IsTrue(sut.IsValid);
        }

        [Ignore("Ignored as relies on environment variables which may not always be present")]
        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SendEmail_EmailIsQueued_Success()
        {
            DefaultEmailSender sut = new DefaultEmailSender(new MockSettingsProvider("{}"), new MockLogger());
            Assert.IsNotNull(sut);

            sut.SendEmail("Joe Bloggs", "joe@bloggs.com", "Unit Test", "This is from a unit test", false, null);
            Assert.AreEqual(1, sut.QueueLength);
        }
    }
}
