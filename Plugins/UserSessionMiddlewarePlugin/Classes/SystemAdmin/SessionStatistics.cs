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
 *  Product:  SharedPluginFeatures
 *  
 *  File: SessionStatistics.cs
 *
 *  Purpose:  Class for containing session statistics
 *
 *  Date        Name                Reason
 *  29/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace UserSessionMiddleware.Plugin.Classes.SystemAdmin
{
	internal sealed class SessionStatistics
	{
		#region Constructors

		internal SessionStatistics()
		{
			Count = 0;
			IsBot = false;
		}

		internal SessionStatistics(in string countryCode)
			: this()
		{
			if (String.IsNullOrEmpty(countryCode))
				throw new ArgumentNullException(nameof(countryCode));

			CountryCode = countryCode;
		}

		#endregion Constructors

		#region Static Methods



		#endregion Static Methods

		#region Properties

		internal int Count { get; set; }

		internal bool IsBot { get; set; }

		internal string CountryCode { get; set; }

		internal decimal Value { get; set; }

		#endregion Properties
	}
}
