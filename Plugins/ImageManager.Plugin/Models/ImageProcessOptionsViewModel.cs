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
 *  Product:  Image Manager Plugin
 *  
 *  File: ImageProcessOptionsViewModel.cs
 *
 *  Date        Name                Reason
 *  17/05/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SharedPluginFeatures;

namespace ImageManager.Plugin.Models
{
	/// <summary>
	/// Options for processing image options
	/// </summary>
	public sealed class ImageProcessOptionsViewModel : IImageProcessOptions
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public ImageProcessOptionsViewModel()
		{
			ShowSubgroup = true;
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// Name of group to which the options apply
		/// </summary>
		/// <value>string</value>
		public string GroupName { get; set; }

		/// <summary>
		/// Name of subgroup to which the options apply
		/// </summary>
		/// <value>string</value>
		public string SubgroupName { get; set; }

		/// <summary>
		/// Indicates whether the subgroup will be shown or not
		/// </summary>
		/// <value>bool</value>
		public bool ShowSubgroup { get; set; }

		/// <summary>
		/// Name of additional data
		/// </summary>
		/// <value>string</value>
		public string AdditionalDataName { get; set; }

		/// <summary>
		/// Additional data supplied by user
		/// </summary>
		/// <value>string</value>
		public string AdditionalData { get; set; }

		/// <summary>
		/// Indicates that additional data is mandatory
		/// </summary>
		/// <value>bool</value>
		public bool AdditionalDataMandatory { get; set; }

		#endregion Properties
	}
}
