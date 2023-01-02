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
 *  File: ViewBookmarksModelTests.cs
 *
 *  Purpose:  Tests for ViewBookmarksModel class
 *
 *  Date        Name                Reason
 *  29/10/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Resources.Plugin.Models;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Plugins.ResourceTests
{
	[TestClass]
	public class ViewBookmarksModelTests : GenericBaseClass
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ViewBookmarksModel_Construct_NullList_Throws_ArgumentNullException()
		{
			new ViewBookmarksModel(GenerateTestBaseModelData(), null);
		}

		[TestMethod]
		public void ResourcesModel_ConstructWithResourceList_Success()
		{
			List<NameIdModel> resourceModels = new List<NameIdModel>()
			{
				new NameIdModel(1, "test 1"),
				new NameIdModel(2, "test 2"),
			};

			ViewBookmarksModel sut = new ViewBookmarksModel(GenerateTestBaseModelData(), resourceModels);

			Assert.IsNotNull(sut);
			Assert.IsInstanceOfType(sut, typeof(BaseModel));
			Assert.AreEqual(2, sut.Bookmarks.Count);
		}
	}
}
