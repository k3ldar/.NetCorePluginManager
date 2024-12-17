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
 *  File: ResourcesModelTests.cs
 *
 *  Purpose:  Tests for ResourcesModel class
 *
 *  Date        Name                Reason
 *  28/08/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Resources.Plugin.Models;


namespace AspNetCore.PluginManager.Tests.Plugins.ResourceTests
{
	[TestClass]
	[ExcludeFromCodeCoverage]
	public class ResourcesCategoryModelTests : GenericBaseClass
	{
		[TestMethod]
		public void Construct_ValidInstance_DefaultConstructor_Success()
		{
			ResourceCategoryModel sut = new ResourceCategoryModel();
			Assert.IsNotNull(sut);
			Assert.IsNull(sut.Name);
			Assert.IsNull(sut.Description);
			Assert.AreEqual("#000", sut.ForeColor);
		}

		[TestMethod]
		public void Construct_ValidInstance_WithParameters_Success()
		{
			ResourceCategoryModel sut = new ResourceCategoryModel(1, "test", "this is a test", "#F0F0F0", "#0F0F0F", "img", "test", true, 23);
			Assert.IsNotNull(sut);
			Assert.AreEqual("test", sut.Name);
			Assert.AreEqual("this is a test", sut.Description);
			Assert.AreEqual("#F0F0F0", sut.ForeColor);
			Assert.AreEqual("img", sut.Image);
			Assert.AreEqual("test", sut.RouteName);
			Assert.AreEqual(23, sut.ParentId);
		}
	}
}
