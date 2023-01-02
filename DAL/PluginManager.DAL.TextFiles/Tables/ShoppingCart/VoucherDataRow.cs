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
 *  File: VoucherDataRow.cs
 *
 *  Purpose:  Table definition for shopping cart vouchers
 *
 *  Date        Name                Reason
 *  09/07/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
    [Table(Constants.TableNameShoppingCartVoucher)]
    internal sealed class VoucherDataRow : TableRowDefinition
    {
        #region Private Members

        private string _name;
        private long _validFromTicks;
        private long _validToTicks;
        private decimal _discountRate;
        private int _discountType;
        private long _userId;
        private long _productId;
        private ushort _maxProductsToDiscount;

        #endregion Private Members

        #region Properties

        [UniqueIndex]
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                if (value.Equals(_name))
                    return;

                _name = value;
                Update();
            }
        }

        public long ValidFromTicks
        {
            get
            {
                return _validFromTicks;
            }

            set
            {
                if (value == _validFromTicks)
                    return;

                _validFromTicks = value;
                Update();
            }
        }

        public long ValidToTicks
        {
            get
            {
                return _validToTicks;
            }

            set
            {
                if (value == _validToTicks)
                    return;

                _validToTicks = value;
                Update();
            }
        }

        public int DiscountType
        {
            get
            {
                return _discountType;
            }

            set
            {
                if (value == _discountType)
                    return;

                _discountType = value;
                Update();
            }
        }

        public decimal DiscountRate
        {
            get
            {
                return _discountRate;
            }

            set
            {
                if (value == _discountRate)
                    return;

                _discountRate = value;
                Update();
            }
        }

        [ForeignKey(Constants.TableNameUsers, true)]
        public long UserId
        {
            get
            {
                return _userId;
            }

            set
            {
                if (value == _userId)
                    return;

                _userId = value;
                Update();
            }
        }

        [ForeignKey(Constants.TableNameProducts, true)]
        public long ProductId
        {
            get
            {
                return _productId;
            }

            set
            {
                if (value == _productId)
                    return;

                _productId = value;
                Update();
            }
        }

        public ushort MaxProductsToDiscount
        {
            get
            {
                return _maxProductsToDiscount;
            }

            set
            {
                if (value == _maxProductsToDiscount)
                    return;

                _maxProductsToDiscount = value;
                Update();
            }
        }

        public DateTime ValidFrom => new DateTime(ValidFromTicks, DateTimeKind.Utc);

        public DateTime ValidTo => new DateTime(ValidToTicks, DateTimeKind.Utc);

        #endregion Properties

        #region Public Methods

        public bool IsValid(long userId)
        {
            return ValidFrom <= DateTime.UtcNow && 
                ValidTo >= DateTime.UtcNow &&
                (_userId == 0 || _userId.Equals(userId));
        }

        #endregion Public Methods
    }
}
