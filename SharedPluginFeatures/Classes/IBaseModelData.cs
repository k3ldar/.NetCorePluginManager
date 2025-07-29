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
 *  File: BaseModelData.cs
 *
 *  Purpose:  Contains basic data to be loaded into BaseModel for general use.
 *
 *  Date        Name                Reason
 *  08/06/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;

namespace SharedPluginFeatures
{
	/// <summary>
	/// Interface for base model data that can be used across various pages in a web application.
	/// </summary>
	public interface IBaseModelData
	{
		/// <summary>
		/// Gets or sets the collection of breadcrumb items representing the navigation path.
		/// </summary>
		List<BreadcrumbItem> Breadcrumbs { get; set; }

		/// <summary>
		/// Indicates whether the user can manage SEO data.
		/// </summary>
		bool CanManageSeoData { get; }

		/// <summary>
		/// Shopping cart summary containing details about the user's shopping cart, if used.
		/// </summary>
		ShoppingCartSummary CartSummary { get; set; }

		/// <summary>
		/// Author of the SEO data for the page, typically used for metadata.
		/// </summary>
		string SeoAuthor { get; }

		/// <summary>
		/// Description for the SEO data of the page, typically used for metadata.
		/// </summary>
		string SeoDescription { get; }

		/// <summary>
		/// Tags for the SEO data of the page, typically used for metadata.
		/// </summary>
		string SeoTags { get; }

		/// <summary>
		/// Title for the SEO data of the page, typically used for metadata.
		/// </summary>
		string SeoTitle { get; }

		/// <summary>
		/// Gets a value indicating whether the user has provided consent via a cookie.
		/// </summary>
		bool UserHasConsentCookie { get; }

		/// <summary>
		/// Gets a value indicating whether the user is currently logged in.
		/// </summary>
		bool UserIsLoggedIn { get; }

		/// <summary>
		/// Replaces the current breadcrumb trail with the specified list of breadcrumb items.
		/// </summary>
		/// <remarks>The provided list of breadcrumbs will completely overwrite the existing breadcrumb trail. Ensure
		/// the list accurately represents the desired navigation path. Passing an empty list will clear the breadcrumb
		/// trail.</remarks>
		/// <param name="breadcrumbs">A list of <see cref="BreadcrumbItem"/> objects representing the new breadcrumb trail. Each item in the list
		/// defines a step in the navigation hierarchy.</param>
		void ReplaceBreadcrumbs(List<BreadcrumbItem> breadcrumbs);

		/// <summary>
		/// Replaces the current shopping cart summary with the specified summary.
		/// </summary>
		/// <remarks>This method updates the shopping cart summary to reflect the provided details. Ensure that
		/// <paramref name="shoppingCartSummary"/> contains valid data before calling this method.</remarks>
		/// <param name="shoppingCartSummary">The new shopping cart summary to replace the existing one. Cannot be <see langword="null"/>.</param>
		void ReplaceCartSummary(ShoppingCartSummary shoppingCartSummary);
	}
}