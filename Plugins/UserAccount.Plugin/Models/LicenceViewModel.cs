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
 *  Product:  UserAccount.Plugin
 *  
 *  File: LicenceViewModel.cs
 *
 *  Purpose:  Licence view model
 *
 *  Date        Name                Reason
 *  06/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using SharedPluginFeatures;

namespace UserAccount.Plugin.Models
{
#pragma warning disable CS1591

	public class LicenceViewModel : BaseModel
	{
		#region Constructors

		public LicenceViewModel()
		{
			Licences = [];
			GrowlMessage = String.Empty;
		}

		public LicenceViewModel(in BaseModelData baseModelData, in List<ViewLicenceViewModel> licences)
			: base(baseModelData)
		{
			Licences = licences ?? throw new ArgumentNullException(nameof(licences));
		}

		public LicenceViewModel(in BaseModelData baseModelData,
			in List<ViewLicenceViewModel> licences, in string growlMessage)
			: base(baseModelData)
		{
			Licences = licences ?? throw new ArgumentNullException(nameof(licences));
			GrowlMessage = growlMessage ?? throw new ArgumentNullException(nameof(growlMessage));
		}

		#endregion Constructors

		#region Properties

		public List<ViewLicenceViewModel> Licences { get; private set; }

		public string GrowlMessage { get; private set; }

		#endregion Properties
	}

#pragma warning restore CS1591
}
