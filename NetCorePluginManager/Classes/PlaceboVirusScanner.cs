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
 *  Product:  AspNetCore.PluginManager
 *  
 *  File: PlaceboVirusScanner.cs
 *
 *  Purpose:  Provides a placebo virus scanner for registering if no custom virus scanner has
 *            been registered and the OS is running on other than Windows 
 *
 *  Date        Name                Reason
 *  02/06/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Internal
{
	internal sealed class PlaceboVirusScanner : IVirusScanner
	{
		public void ScanDirectory(in string directory)
		{
			// required by interface not used in demo
		}

		public void ScanFile(in string fileName)
		{
			// required by interface not used in demo
		}

		public void ScanFile(in string[] fileNames)
		{
			// required by interface not used in demo
		}
	}
}
