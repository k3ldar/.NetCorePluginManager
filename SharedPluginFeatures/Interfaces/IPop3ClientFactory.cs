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
 *  Product:  SharedPluginFeatues
 *  
 *  File: IPop3ClientFactory.cs
 *
 *  Purpose:  IPop3ClientFactory interface for creating pop 3 clients
 *
 *  Date        Name                Reason
 *  16/12/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using Shared.Communication;

namespace SharedPluginFeatures
{
#if NET6_0_OR_GREATER

	/// <summary>
	/// Factory class for creating pop 3 clients
	/// </summary>
	public interface IPop3ClientFactory
	{
		/// <summary>
		/// Create an IPop3Client instance
		/// </summary>
		/// <returns></returns>
		IPop3Client Create();
	}

#endif
}
