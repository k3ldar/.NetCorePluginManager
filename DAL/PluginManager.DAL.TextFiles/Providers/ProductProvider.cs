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
 *  File: ProductProvider.cs
 *
 *  Purpose:  IProductProvider for text based storage
 *
 *  Date        Name                Reason
 *  25/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Runtime.CompilerServices;

using Middleware;
using Middleware.Products;

using PluginManager.DAL.TextFiles.Tables;
using SimpleDB;

using SharedPluginFeatures;

namespace PluginManager.DAL.TextFiles.Providers
{
	internal class ProductProvider : IProductProvider
	{
		#region Private Members

		private readonly ISimpleDBOperations<ProductGroupDataRow> _productGroupsData;
		private readonly ISimpleDBOperations<ProductDataRow> _productData;

		#endregion Private Members

		#region Constructors

		public ProductProvider(ISimpleDBOperations<ProductDataRow> productData,
			ISimpleDBOperations<ProductGroupDataRow> productGroupData)
		{
			_productData = productData ?? throw new ArgumentNullException(nameof(productData));
			_productGroupsData = productGroupData ?? throw new ArgumentNullException(nameof(productGroupData));
		}

		#endregion Constructors

		#region IProductProvider Members

		#region Product Groups

		public ProductGroup ProductGroupGet(in int id)
		{
			int groupId = id;

			return ConvertProductGroupDataRowToProductGroup(_productGroupsData.Select(groupId));
		}

		public int ProductCountForGroup(ProductGroup productGroup)
		{
			return _productData.Select(r => r.ProductGroupId.Equals(productGroup.Id)).Count;
		}

		public List<ProductGroup> ProductGroupsGet()
		{
			IReadOnlyList<ProductGroupDataRow> allGroups = _productGroupsData.Select();

			List<ProductGroup> Result = [];

			foreach (ProductGroupDataRow group in allGroups)
			{
				Result.Add(ConvertProductGroupDataRowToProductGroup(group));
			}

			return [.. Result.OrderBy(pg => pg.SortOrder)];
		}

		public bool ProductGroupDelete(in int id, out string errorMessage)
		{
			if (_productGroupsData.RecordCount == 1)
			{
				errorMessage = Languages.LanguageStrings.ProductGroupMustHaveOneGroup;
				return false;
			}

			int productGroupId = id;

			if (_productData.Select().Any(p => p.ProductGroupId.Equals(productGroupId)))
			{
				errorMessage = Languages.LanguageStrings.ProductGroupContainsProducts;
				return false;
			}

			ProductGroupDataRow group = _productGroupsData.Select(id);

			if (group == null)
			{
				errorMessage = Languages.LanguageStrings.ProductGroupNotFound;
				return false;
			}

			_productGroupsData.Delete(group);
			errorMessage = String.Empty;
			return true;
		}

		public bool ProductGroupSave(in int id, in string description, in bool showOnWebsite, in int sortOrder, in string tagLine, in string url, out string errorMessage)
		{
			try
			{
				ProductGroupDataRow productGroupDataRow = _productGroupsData.Select(id);

				productGroupDataRow ??= new ProductGroupDataRow();

				productGroupDataRow.Description = description;
				productGroupDataRow.ShowOnWebsite = showOnWebsite;
				productGroupDataRow.SortOrder = sortOrder;
				productGroupDataRow.TagLine = tagLine;
				productGroupDataRow.Url = url;

				_productGroupsData.InsertOrUpdate(productGroupDataRow);

				errorMessage = String.Empty;

				return true;
			}
			catch (InvalidDataRowException err)
			{
				errorMessage = err.Message;
				return false;
			}
			catch (UniqueIndexException err)
			{
				errorMessage = err.Message;
				return false;
			}
		}

		#endregion Product Groups

		#region Products

		public List<Product> GetProducts(in int page, in int pageSize)
		{
			if (page < 1)
				throw new ArgumentOutOfRangeException(nameof(page));

			if (pageSize < 1)
				throw new ArgumentOutOfRangeException(nameof(pageSize));

			List<ProductDataRow> allProducts = [.. _productData.Select().OrderBy(p => p.Name)];

			List<Product> Result = [];

			(int start, int end, bool isEmpty) = GetPageStartAndEnd(page, pageSize, allProducts.Count);

			if (isEmpty)
				return Result;

			for (int i = start; i < end; i++)
			{
				Result.Add(ConvertProductDataRowToProduct(allProducts[i]));
			}

			return Result;
		}

		public List<Product> GetProducts(in ProductGroup productGroup, in int page, in int pageSize)
		{
			if (page < 1)
				throw new ArgumentOutOfRangeException(nameof(page));

			if (pageSize < 1)
				throw new ArgumentOutOfRangeException(nameof(pageSize));

			int prodGroup = productGroup.Id;
			List<ProductDataRow> allProducts = [.. _productData.Select().Where(p => p.ProductGroupId.Equals(prodGroup)).OrderBy(p => p.Name)];

			List<Product> Result = [];

			(int start, int end, bool isEmpty) = GetPageStartAndEnd(page, pageSize, allProducts.Count);

			if (isEmpty)
				return Result;

			for (int i = start; i < end; i++)
			{
				Result.Add(ConvertProductDataRowToProduct(allProducts[i]));
			}

			return Result;
		}

		public Product GetProduct(in int id)
		{
			return ConvertProductDataRowToProduct(_productData.Select(id));
		}

		public bool ProductSave(in int id, in int productGroupId, in string name, in string description, in string features,
			in string videoLink, in bool newProduct, in bool bestSeller, in decimal retailPrice, in string sku,
			in bool isDownload, in bool allowBackOrder, in bool isVisible, out string errorMessage)
		{
			try
			{
				ProductDataRow productDataRow = _productData.Select(id);

				productDataRow ??= new ProductDataRow();

				productDataRow.ProductGroupId = productGroupId;
				productDataRow.Name = name;
				productDataRow.Description = description;
				productDataRow.Features = features;
				productDataRow.VideoLink = videoLink;
				productDataRow.NewProduct = newProduct;
				productDataRow.BestSeller = bestSeller;
				productDataRow.Sku = sku;
				productDataRow.IsDownload = isDownload;
				productDataRow.AllowBackorder = allowBackOrder;
				productDataRow.RetailPrice = retailPrice;
				productDataRow.IsVisible = isVisible;

				_productData.InsertOrUpdate(productDataRow);

				errorMessage = String.Empty;
				return true;
			}
			catch (InvalidDataRowException err)
			{
				errorMessage = err.Message;
				return false;
			}
			catch (UniqueIndexException err)
			{
				errorMessage = err.Message;
				return false;
			}
		}

		public bool ProductDelete(in int id, out string errorMessage)
		{
			errorMessage = "Unable to delete";
			return false;
		}

		public int ProductCount => 9;

		#endregion Products

		#endregion IProductProvider Members

		#region Private Members

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static (int, int, bool) GetPageStartAndEnd(int page, int pageSize, int productCount)
		{
			int start = (page * pageSize) - pageSize;
			int end = (start + pageSize);

			decimal pageCount = (decimal)productCount / pageSize;

			int pages = (int)Math.Truncate(pageCount);

			if (pageCount - pages > 0)
				pages++;

			if (page > pages)
				return (0, 0, true);

			if (end > productCount)
				end = productCount;

			return (start, end, false);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ProductGroup ConvertProductGroupDataRowToProductGroup(ProductGroupDataRow group)
		{
			if (group == null)
				return null;

			return new ProductGroup((int)group.Id, group.Description, group.ShowOnWebsite, group.SortOrder, group.TagLine, group.Url);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Product ConvertProductDataRowToProduct(ProductDataRow product)
		{
			if (product == null)
				return null;

			return new Product((int)product.Id, product.ProductGroupId, product.Name, product.Description, product.Features, product.VideoLink,
				["NoImage"], product.RetailPrice, product.Sku, product.IsDownload, product.AllowBackorder, product.IsVisible);
		}

		#endregion Private Members
	}
}
