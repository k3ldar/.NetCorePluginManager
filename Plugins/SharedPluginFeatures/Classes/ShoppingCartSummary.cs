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
 *  Copyright (c) 2012 - 2018 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginMiddleware
 *  
 *  File: ShoppingCartSummary.cs
 *
 *  Purpose:  Provide summary for display on any page
 *
 *  Date        Name                Reason
 *  07/03/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Globalization;

namespace SharedPluginFeatures
{
    public class ShoppingCartSummary
    {
        #region Constructors

        public ShoppingCartSummary(in long id, in int totalItems, in decimal totalCost, 
            in CultureInfo culture)
        {
            Id = id;

            if (totalItems < 0)
                throw new ArgumentOutOfRangeException(nameof(totalItems));

            if (totalCost < 0)
                throw new ArgumentOutOfRangeException(nameof(totalCost));

            TotalItems = totalItems;
            TotalCost = totalCost;
            Currency = culture ?? throw new ArgumentNullException(nameof(culture));
        }

        #endregion Constructors

        #region Public Methods

        public void ResetBasketId(in long id)
        {
            if (Id != 0)
                throw new InvalidOperationException();

            Id = id;
        }

        public void ResetTotalItems(in int totalItems)
        {
            if (totalItems < 0)
                throw new InvalidOperationException();

            TotalItems = totalItems;
        }

        public void ResetTotalCost(in decimal cost, in CultureInfo cultureInfo)
        {
            if (cost < 0)
                throw new InvalidOperationException();

            TotalCost = cost;
            Currency = cultureInfo ?? throw new ArgumentNullException(nameof(cultureInfo));
        }

        #endregion Public Methods

        #region Properties

        public long Id { get; private set; }

        public int TotalItems { get; private set; }

        public decimal TotalCost { get; private set; }

        public CultureInfo Currency { get; private set; }

        #endregion Properties
    }
}
