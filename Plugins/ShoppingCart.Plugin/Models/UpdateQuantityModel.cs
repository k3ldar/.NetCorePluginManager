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
 *  Copyright (c) 2019 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Shopping Cart Plugin
 *  
 *  File: UpdateQuantityModel.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  17/03/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

#pragma warning disable CS1591

namespace ShoppingCartPlugin.Models
{
    public sealed class UpdateQuantityModel
    {
        #region Constructors

        public UpdateQuantityModel()
        {

        }

        public UpdateQuantityModel(in int productId, in int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }

        #endregion Constructors

        #region Properties

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        #endregion Properties
    }
}

#pragma warning restore CS1591