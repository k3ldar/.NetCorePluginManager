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
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: UrlHashProvider.cs
 *
 *  Purpose:  Url hash provider
 *
 *  Date        Name                Reason
 *  11/08/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Security.Cryptography;
using System.Text;

using Middleware;

namespace PluginManager.DAL.TextFiles.Providers
{
	internal class UrlHashProvider : IUrlHashProvider
	{

		#region IUrlHashProvider

		/// <summary>
		/// Retreives a hash of a url
		/// </summary>
		/// <param name="url"></param>
		/// <returns>string</returns>
		public string GetUrlHash(in string url)
		{
			using (SHA256 sha256Hash = SHA256.Create())
			{
				byte[] data = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(url.ToLower()));

				// Create a new Stringbuilder to collect the bytes
				// and create a string.
				StringBuilder sBuilder = new();

				// Loop through each byte of the hashed data
				// and format each one as a hexadecimal string.
				for (int i = 0; i < data.Length; i++)
				{
					sBuilder.Append(data[i].ToString("x2"));
				}

				// Return the hexadecimal string.
				return sBuilder.ToString();
			}
		}

		#endregion IUrlHashProvider
	}
}
