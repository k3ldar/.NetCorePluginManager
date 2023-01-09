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
 *  File: DatabaseTimingTests.cs
 *
 *  Purpose:  Tests for database timings admin menu
 *
 *  Date        Name                Reason
 *  08/01/2023  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Shared.Classes;

using SharedPluginFeatures;

using SystemAdmin.Plugin.Classes;
using SystemAdmin.Plugin.Classes.MenuItems;


namespace AspNetCore.PluginManager.Tests.Plugins.SystemAdminTests
{
	[TestClass]
	[ExcludeFromCodeCoverage]
	public class DatabaseTimingTests
	{
		[TestMethod]
		public void Construct_WithoutIDatabaseTimingsInstance_EnabledIsFalse()
		{
			DatabaseTimings sut = new DatabaseTimings();
			Assert.IsFalse(sut.Enabled());
		}

		[TestMethod]
		public void Action_ReturnsEmptyString()
		{
			DatabaseTimings sut = new DatabaseTimings();
			Assert.AreEqual("", sut.Action());
		}

		[TestMethod]
		public void Area_ReturnsEmptyString()
		{
			DatabaseTimings sut = new DatabaseTimings();
			Assert.AreEqual("", sut.Area());
		}

		[TestMethod]
		public void Controller_ReturnsEmptyString()
		{
			DatabaseTimings sut = new DatabaseTimings();
			Assert.AreEqual("", sut.Controller());
		}

		[TestMethod]
		public void MenuType_ReturnsGrid()
		{
			DatabaseTimings sut = new DatabaseTimings();
			Assert.AreEqual(Enums.SystemAdminMenuType.Grid, sut.MenuType());
		}

		[TestMethod]
		public void Name_ReturnsDatabaseTimings()
		{
			DatabaseTimings sut = new DatabaseTimings();
			Assert.AreEqual("DatabaseTimings", sut.Name());
		}

		[TestMethod]
		public void ParentMenuName_ReturnsDatabase()
		{
			DatabaseTimings sut = new DatabaseTimings();
			Assert.AreEqual("Database", sut.ParentMenuName());
		}

		[TestMethod]
		public void Image_ReturnsStopwatch()
		{
			DatabaseTimings sut = new DatabaseTimings();
			Assert.AreEqual("stopwatch", sut.Image());
		}

		[TestMethod]
		public void SortOrder_ReturnsIntMinimum()
		{
			DatabaseTimings sut = new DatabaseTimings();
			Assert.AreEqual(Int32.MinValue, sut.SortOrder());
		}

		[TestMethod]
		public void Data_WithoutIDatabaseTimingsInstance_ReturnsEmptyString()
		{
			DatabaseTimings sut = new DatabaseTimings();

			Assert.AreEqual("", sut.Data());
		}

		[TestMethod]
		public void Data_RetrievesDataInCorrectFormat_Success()
		{
			const string ExpectedData = "Operation|Total Requests|Fastest|Slowest|Average|Trimmed Avg ms|Total ms\rTable 1||||||\rOp1|0|0|0|0|0|0\rOp2|0|0|0|0|0|0\r" + 
				"Op3|0|0|0|0|0|0\rOp4|0|0|0|0|0|0\r||||||\rTable 2||||||\rOp1|0|0|0|0|0|0\rOp2|0|0|0|0|0|0\rOp3|0|0|0|0|0|0\rOp4|0|0|0|0|0|0";
			DatabaseTimings sut = new DatabaseTimings(new MockDatabaseTimings());

			string tableData = sut.Data();
			Assert.AreEqual(ExpectedData, tableData);
		}
	}
}
