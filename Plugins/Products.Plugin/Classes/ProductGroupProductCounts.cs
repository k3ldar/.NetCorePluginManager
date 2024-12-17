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
 *  Product:  Products.Plugin
 *  
 *  File: ProductGroupProductCounts.cs
 *
 *  Purpose:  Product group names updates product counts
 *
 *  Date        Name                Reason
 *  04/04/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;

using Middleware;
using Middleware.Products;

using Shared.Classes;

namespace ProductPlugin.Classes
{
	internal class ProductGroupProductCounts : ThreadManager
	{
		#region Private Members

		internal const int MaximumProducts = 50000000;
		private readonly IProductProvider _productProvider;

		#endregion Private Members

		#region Constructors

		public ProductGroupProductCounts(in IProductProvider productProvider, in List<string> productGroups)
			: base(productGroups, new TimeSpan())
		{
			if (productGroups == null)
				throw new ArgumentNullException(nameof(productGroups));

			_productProvider = productProvider ?? throw new ArgumentNullException(nameof(productProvider));
		}

		#endregion Constructors

		#region Overridden Methods

		protected override Boolean Run(object parameters)
		{
			List<string> productGroups = (List<string>)parameters;

			for (int i = 0; i < productGroups.Count; i++)
			{
				ProductGroup group = _productProvider.ProductGroupsGet().Find(p => p.Description.Equals(productGroups[i]));

				if (group != null)
				{
					int groupProductCount = _productProvider.GetProducts(1, MaximumProducts)
						.Count(p => p.ProductGroupId == group.Id);
					productGroups[i] += $" ({groupProductCount})";
				}
			}

			return false;
		}

		#endregion Overridden Methods
	}
}
