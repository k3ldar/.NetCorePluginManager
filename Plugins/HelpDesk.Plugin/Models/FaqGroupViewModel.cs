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
 *  Product:  Helpdesk Plugin
 *  
 *  File: FaqGroupViewModel.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  27/04/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace HelpdeskPlugin.Models
{
	public class FaqGroupViewModel : BaseModel
	{
		#region Constructors

		public FaqGroupViewModel(in BaseModelData modelData,
			in List<FaqGroup> groups, in FaqGroup activeGroup)
			: base(modelData)
		{
			Groups = groups ?? throw new ArgumentNullException(nameof(groups));
			ActiveGroup = activeGroup;
		}

		#endregion Constructors

		#region Properties

		public FaqGroup ActiveGroup { get; private set; }

		public List<FaqGroup> Groups { get; private set; }

		#endregion Properties
	}
}

#pragma warning restore CS1591