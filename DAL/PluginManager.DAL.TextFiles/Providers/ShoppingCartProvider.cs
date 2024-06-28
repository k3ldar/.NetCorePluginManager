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
 *  File: ShoppingCartProvider.cs
 *
 *  Purpose:  IShoppingCartProvider and IShoppingCartService for text based storage
 *
 *  Date        Name                Reason
 *  25/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Middleware;
using Middleware.Accounts;
using Middleware.Accounts.Orders;
using Middleware.Products;
using Middleware.ShoppingCart;

using PluginManager.DAL.TextFiles.Tables;
using SimpleDB;

using Shared.Classes;

using SharedPluginFeatures;

using System.Globalization;

#pragma warning disable IDE1006

namespace PluginManager.DAL.TextFiles.Providers
{
	internal sealed class ShoppingCartProvider : IShoppingCartProvider, IShoppingCartService, IDisposable
	{
		#region Private Members

		private static readonly CacheManager _cartCacheManager = new("Shopping Carts", new TimeSpan(0, 20, 0), true);
		private bool _cartHookedUp;
		private readonly IAccountProvider _accountProvider;
		private readonly string _encryptionKey;
		private readonly decimal _defaultTaxRate;
		private readonly string _defaultCurrency;
		private readonly ISimpleDBOperations<ShoppingCartDataRow> _shoppingCartData;
		private readonly ISimpleDBOperations<ShoppingCartItemDataRow> _shoppingCartItemData;
		private readonly ISimpleDBOperations<OrderDataRow> _orderData;
		private readonly ISimpleDBOperations<OrderItemDataRow> _orderItemsData;
		private readonly ISimpleDBOperations<VoucherDataRow> _voucherData;
		private readonly object _lockObject = new();


		#endregion Private Members

		#region Constructors

		public ShoppingCartProvider(ISimpleDBOperations<ShoppingCartDataRow> shoppingCartData,
			ISimpleDBOperations<ShoppingCartItemDataRow> shoppingCartItemData,
			ISimpleDBOperations<OrderDataRow> orderData, ISimpleDBOperations<OrderItemDataRow> orderItemsData,
			ISimpleDBOperations<VoucherDataRow> voucherData, IAccountProvider accountProvider,
						IApplicationSettingsProvider settingsProvider)
		{
			_shoppingCartData = shoppingCartData ?? throw new ArgumentNullException(nameof(shoppingCartData));
			_shoppingCartItemData = shoppingCartItemData ?? throw new ArgumentNullException(nameof(shoppingCartItemData));
			_accountProvider = accountProvider ?? throw new ArgumentNullException(nameof(accountProvider));
			_orderData = orderData ?? throw new ArgumentNullException(nameof(orderData));
			_orderItemsData = orderItemsData ?? throw new ArgumentNullException(nameof(orderItemsData));
			_voucherData = voucherData ?? throw new ArgumentNullException(nameof(voucherData));

			if (settingsProvider == null)
				throw new ArgumentNullException(nameof(settingsProvider));

			_encryptionKey = settingsProvider.RetrieveSetting("ShoppingCartEncryption");
			_defaultTaxRate = settingsProvider.RetrieveSetting<decimal>("DefaultTaxRate");
			_defaultCurrency = settingsProvider.RetrieveSetting("DefaultCurrency");

			if (String.IsNullOrEmpty(_encryptionKey))
				throw new InvalidOperationException("Encryption key for shopping cart can not be null or empty");

			lock (_lockObject)
			{
				if (!_cartHookedUp)
				{
					_cartCacheManager.ItemNotFound += cartCacheManager_ItemNotFound;
					_cartHookedUp = true;
				}
			}
		}

		#endregion Constructors

		#region IShoppingCartProvider Methds

		public long AddToCart(in UserSession userSession, in ShoppingCartSummary shoppingCart,
			in Product product, in int count)
		{
			if (userSession == null)
				throw new ArgumentNullException(nameof(userSession));

			if (shoppingCart == null)
				throw new ArgumentNullException(nameof(shoppingCart));

			if (product == null)
				throw new ArgumentNullException(nameof(product));

			ArgumentOutOfRangeException.ThrowIfLessThan(count, 1);

			decimal cost = product.RetailPrice;

			if (cost < 0)
				throw new InvalidOperationException($"{nameof(cost)} value must be greater than zero");

			ShoppingCartDetail cartDetail = null;

			string cacheName = $"Cart {shoppingCart.Id}";

			CacheItem basket = _cartCacheManager.Get(cacheName);

			if (basket == null)
			{
				if (shoppingCart.Id == 0)
				{
					ShoppingCartDataRow cartDataRow = CreateNewShoppingCart(-1);

					// create a new cart
					shoppingCart.ResetShoppingCartId(cartDataRow.Id);

					cartDetail = new ShoppingCartDetail(shoppingCart.Id,
						0, 0, _defaultTaxRate, 0, 0, shoppingCart.Culture, String.Empty,
						new List<ShoppingCartItem>(), false, _defaultCurrency);
				}

				basket = new CacheItem(cacheName, cartDetail);
				_cartCacheManager.Add(cacheName, basket, true);
			}

			if (userSession.UserBasketId != shoppingCart.Id)
				userSession.UserBasketId = shoppingCart.Id;

			if (shoppingCart.Id == 0 || userSession.UserBasketId != shoppingCart.Id)
				shoppingCart.ResetShoppingCartId(userSession.UserBasketId);

			ShoppingCartDetail cart = basket.Value as ShoppingCartDetail;

			_shoppingCartItemData.Insert(new ShoppingCartItemDataRow()
			{
				ProductId = product.Id,
				ShoppingCartId = shoppingCart.Id,
				ItemCost = product.RetailPrice,
				Description = product.Description,
				CanBackOrder = product.AllowBackorder,
				IsDownload = product.IsDownload,
				ItemCount = count,
				SKU = product.Sku,
				Name = product.Name,
			});

			cart.Add(product, count);

			ShoppingCartDataRow shoppingCartData = _shoppingCartData.Select(userSession.UserBasketId);
			shoppingCartData.CouponCode = cart.CouponCode;
			shoppingCartData.Culture = cart.Culture.Name;
			shoppingCartData.Discount = cart.Discount;
			shoppingCartData.DiscountRate = cart.DiscountRate;
			shoppingCartData.TaxRate = cart.TaxRate;
			shoppingCartData.Tax = cart.Tax;
			shoppingCartData.RequiresShipping = cart.RequiresShipping;
			shoppingCartData.Shipping = cart.Shipping;
			shoppingCartData.SubTotal = cart.SubTotal;
			shoppingCartData.Total = cart.Total;
			shoppingCartData.TotalItems = cart.TotalItems;

			_shoppingCartData.Update(shoppingCartData);

			return userSession.UserBasketId;
		}

		public ShoppingCartDetail GetDetail(in long shoppingCartId)
		{
			ArgumentOutOfRangeException.ThrowIfZero(shoppingCartId);

			string basketCache = $"Cart {shoppingCartId}";

			CacheItem cacheItem = _cartCacheManager.Get(basketCache);

			if (cacheItem == null)
			{
				_ = CreateNewShoppingCart(shoppingCartId);

				ShoppingCartDetail cartDetail = new(shoppingCartId, 0,
					0, _defaultTaxRate, 0, 0, Thread.CurrentThread.CurrentUICulture,
					"", new List<ShoppingCartItem>(), false, _defaultCurrency);
				cacheItem = new CacheItem(basketCache, cartDetail);
				_cartCacheManager.Add(basketCache, cacheItem, true);
			}

			return (ShoppingCartDetail)cacheItem.Value;
		}

		public bool ValidateVoucher(in ShoppingCartSummary cartSummary, in string voucher, in long userId)
		{
			if (cartSummary == null)
				throw new ArgumentNullException(nameof(cartSummary));

			string voucherName = voucher;
			long user = userId;
			VoucherDataRow voucherDataRow = _voucherData.Select()
				.FirstOrDefault(v => v.Name.Equals(voucherName, StringComparison.InvariantCultureIgnoreCase) &&
					(v.UserId.Equals(0) || v.UserId.Equals(user)));

			if (voucherDataRow == null)
				return false;

			if (!voucherDataRow.IsValid(userId))
				return false;

			ShoppingCartDetail cartDetail = GetDetail(cartSummary.Id);
			cartDetail.ClearVoucherData();

			if (voucherDataRow.ProductId == 0)
			{
				// voucher applied to entire shopping cart
				cartDetail.UpdateDiscount(voucherDataRow.Name, (DiscountType)voucherDataRow.DiscountType, voucherDataRow.DiscountRate);

				ShoppingCartDataRow cartData = _shoppingCartData.Select(cartDetail.Id);
				cartData.DiscountRate = voucherDataRow.DiscountRate;
				cartData.DiscountType = voucherDataRow.DiscountType;
				cartData.Discount = cartDetail.Discount;
				_shoppingCartData.Update(cartData);
			}
			else
			{
				// voucher applied to individual products
				List<ShoppingCartItem> cartItems = cartDetail.Items.Where(i => i.Id.Equals(voucherDataRow.ProductId)).ToList();

				if (cartItems.Count == 0)
					return false;

				foreach (ShoppingCartItem item in cartItems)
				{
					item.UpdateDiscountCode(voucherDataRow.Name, (DiscountType)voucherDataRow.DiscountType,
						voucherDataRow.DiscountRate, voucherDataRow.MaxProductsToDiscount);

					ShoppingCartItemDataRow cartItem = _shoppingCartItemData.Select(item.Id);
					cartItem.DiscountRate = voucherDataRow.DiscountRate;
					cartItem.DiscountType = (int)item.DiscountType;
					_shoppingCartItemData.Update(cartItem);
				}
			}

			return true;
		}

		public bool ConvertToOrder(in ShoppingCartSummary cartSummary, in long userId, out Order order)
		{
			if (cartSummary == null)
				throw new ArgumentNullException(nameof(cartSummary));

			ArgumentOutOfRangeException.ThrowIfNegative(userId);

			order = null;

			ShoppingCartDetail cartDetail = GetDetail(cartSummary.Id);

			DeliveryAddress shippingAddress = _accountProvider.GetDeliveryAddress(userId, cartDetail.DeliveryAddressId);

			if (shippingAddress == null)
				return false;

			cartDetail.SetDeliveryAddress(shippingAddress);

			List<OrderItem> items = new();

			OrderDataRow orderData = new()
			{
				UserId = userId,
				Culture = cartSummary.Culture.Name,
				Postage = cartSummary.Shipping,
				ProcessStatus = (int)ProcessStatus.PaymentPending,
				DeliveryAddress = shippingAddress.Id
			};
			_orderData.Insert(orderData);

			foreach (ShoppingCartItem item in cartDetail.Items)
			{
				_orderItemsData.Insert(new OrderItemDataRow()
				{
					OrderId = orderData.Id,
					Description = item.Description,
					TaxRate = item.TaxRate,
					Price = item.ItemCost,
					Quantity = item.ItemCount,
					Discount = item.DiscountRate,
					DiscountType = (int)item.DiscountType,
					ItemStatus = (int)ItemStatus.Received,
				});

				items.Add(new OrderItem(item.Id, item.Name, item.ItemCost, cartSummary.TaxRate, item.ItemCount,
					ItemStatus.Received, item.DiscountType, item.DiscountRate));
			}

			order = new Order(orderData.Id, orderData.Created, orderData.Postage, new CultureInfo(orderData.Culture),
				(ProcessStatus)orderData.ProcessStatus, shippingAddress, items);

			_shoppingCartItemData.Delete(_shoppingCartItemData.Select().Where(sci => sci.ShoppingCartId.Equals(cartDetail.Id)).ToList());
			cartDetail.Clear();

			return true;
		}

		#endregion IShoppingCartProvider Methods

		#region IDisposable Methods

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "uses flag")]
		public void Dispose()
		{
			if (_cartHookedUp)
			{
				_cartCacheManager.ItemNotFound -= cartCacheManager_ItemNotFound;
				_cartHookedUp = false;
			}

			GC.SuppressFinalize(this);
		}

		#endregion IDisposable Methods

		#region IShoppingCartService Methods

		public ShoppingCartSummary GetSummary(in long shoppingCartId)
		{
			return GetDetail(shoppingCartId);
		}

		public string GetEncryptionKey()
		{
			return _encryptionKey;
		}

		#endregion IShoppingCartService Methods

		#region Internal Methods

		internal static void ClearCache()
		{
			_cartCacheManager.Clear();
		}

		#endregion Internal Methods

		#region Private Methods

		private ShoppingCartDataRow CreateNewShoppingCart(long id)
		{
			ShoppingCartDataRow cartDataRow = new()
			{
				Id = id,
				Culture = Thread.CurrentThread.CurrentUICulture.Name,
				CouponCode = "",
				CurrencyCode = _defaultCurrency,
				RequiresShipping = false,
				Shipping = 0,
				SubTotal = 0,
				Tax = 0,
				TaxRate = _defaultTaxRate,
				Total = 0,
				TotalItems = 0,
			};

			_shoppingCartData.Insert(cartDataRow, new InsertOptions(id < 0));
			return cartDataRow;
		}

		private void cartCacheManager_ItemNotFound(object sender, Shared.CacheItemNotFoundArgs e)
		{
			if (!Int64.TryParse(e.Name.AsSpan(5), out long cartId))
				return;

			ShoppingCartDataRow cart = _shoppingCartData.Select(cartId);

			if (cart == null)
				return;

			ShoppingCartDetail cartDetail = ConvertShoppingCartDataRowToShoppingCartDetail(cart);

			e.CachedItem = new CacheItem(e.Name, cartDetail);
		}

		private ShoppingCartDetail ConvertShoppingCartDataRowToShoppingCartDetail(ShoppingCartDataRow shoppingCartData)
		{
			if (shoppingCartData == null)
				return null;

			List<ShoppingCartItem> shoppingCartItems = ConvertShoppingCartItemsDataRowToShoppingCartItems(
				_shoppingCartItemData.Select().Where(item => item.ShoppingCartId.Equals(shoppingCartData.Id)).ToList());

			return new ShoppingCartDetail(shoppingCartData.Id, shoppingCartData.TotalItems, shoppingCartData.Total,
				shoppingCartData.TaxRate, shoppingCartData.Shipping, shoppingCartData.Discount, new CultureInfo(shoppingCartData.Culture),
				shoppingCartData.CouponCode, shoppingCartItems, shoppingCartData.RequiresShipping, shoppingCartData.CurrencyCode);
		}

		private static List<ShoppingCartItem> ConvertShoppingCartItemsDataRowToShoppingCartItems(List<ShoppingCartItemDataRow> shoppingCartItems)
		{
			List<ShoppingCartItem> Result = new();

			if (shoppingCartItems == null)
				return Result;

			shoppingCartItems.ForEach(item => Result.Add(new ShoppingCartItem((int)item.Id, item.ItemCount, item.ProductId, item.ItemCost, item.Name,
				item.Description, item.SKU, new string[] { "NoImage" }, item.IsDownload, item.CanBackOrder, item.Size,
				(DiscountType)item.DiscountType, item.DiscountRate)));

			return Result;
		}

		#endregion Private Methods
	}
}
