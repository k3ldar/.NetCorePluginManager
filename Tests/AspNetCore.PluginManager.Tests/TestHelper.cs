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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: TestHelper.cs
 *
 *  Purpose:  Generic test helper methods
 *
 *  Date        Name                Reason
 *  20/08/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.IO;
using System.Reflection;

namespace AspNetCore.PluginManager.Tests
{
	public static class TestHelper
	{
		private static string _rootPath;

		static TestHelper()
		{
			string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

			_rootPath = Path.Combine(Path.GetFullPath(assemblyPath), "..\\..\\..\\..\\..\\", "Output", "TestFolders");

			if (!Directory.Exists(_rootPath))
				Directory.CreateDirectory(_rootPath);
		}

		public static string GetTestPath()
		{
			return Path.Combine(_rootPath, Guid.NewGuid().ToString());
		}

		public static string CreateTestPath()
		{
			string path = GetTestPath();
			Directory.CreateDirectory(path);
			return path;
		}

		public static string GetTestPath(string pathPart)
		{
			return Path.Combine(_rootPath, pathPart, Guid.NewGuid().ToString());
		}
	}
}
