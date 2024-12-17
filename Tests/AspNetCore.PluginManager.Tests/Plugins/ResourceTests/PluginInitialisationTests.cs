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
 *  File: PluginInitialisationTests.cs
 *
 *  Purpose:  Tests for Resources plugin initialisation
 *
 *  Date        Name                Reason
 *  11/09/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.DemoWebsite.Classes.Mocks;
using AspNetCore.PluginManager.Tests.Controllers;
using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Resources.Plugin;
using Resources.Plugin.Controllers;
using Resources.Plugin.Models;

using SharedPluginFeatures;

using Microsoft.Extensions.DependencyInjection;

using Middleware.Interfaces;

using PluginManager.Abstractions;
using PluginManager.Internal;
using PluginManager.Tests.Mocks;

namespace AspNetCore.PluginManager.Tests.Plugins.ResourceTests
{
	[TestClass]
	[ExcludeFromCodeCoverage]
	public class PluginInitialisationTests : BaseControllerTests
	{
		private const string TestCategoryName = "Resources";

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void Construct_ValidInstance_Success()
		{
			PluginInitialisation sut = new PluginInitialisation();
			Assert.IsNotNull(sut);
			Assert.AreEqual(1, sut.GetVersion());
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AfterConfigureServices_InvalidServiceCollectionNull_Throws_ArgumentNullException()
		{
			PluginInitialisation sut = new PluginInitialisation();
			sut.AfterConfigureServices(null);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void AfterConfigureServices_AddsCorrectPolicies_Success()
		{
			ServiceDescriptor[] serviceDescriptors = new ServiceDescriptor[]
			{
				new ServiceDescriptor(typeof(INotificationService), new NotificationService()),
				new ServiceDescriptor(typeof(IImageProvider), new MockImageProvider()),
				new ServiceDescriptor(typeof(ISettingsProvider), new MockSettingsProvider()),
			};
			PluginInitialisation sut = new PluginInitialisation();

			MockServiceCollection services = new MockServiceCollection(serviceDescriptors);

			sut.AfterConfigureServices(services);

			string[] claims = { "AddResources", "UserId", "Email" };
			Assert.IsTrue(services.HasPolicyConfigured("AddResources", claims));

			claims = new string[] { "ManageResources", "UserId", "Email", "Name", "StaffMember" };
			Assert.IsTrue(services.HasPolicyConfigured("ManageResources", claims));
		}
	}
}
