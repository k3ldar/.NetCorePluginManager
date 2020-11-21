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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Products.Plugin
 *  
 *  File: ProductPriceInfo.cs
 *
 *  Purpose:  Product price information for searching
 *
 *  Date        Name                Reason
 *  29/03/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace ProductPlugin.Classes
{
    /// <summary>
    /// Contains price information for displaying on the search page, a min and max value for the price range.
    /// </summary>
    public sealed class ProductPriceInfo
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="text">Display text for price bracket</param>
        /// <param name="minValue">Minimum price value</param>
        /// <param name="maxValue">Maximum price value</param>
        public ProductPriceInfo(in string text, in decimal minValue, in decimal maxValue)
        {
            if (String.IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text));

            if (minValue > maxValue)
                throw new ArgumentOutOfRangeException(nameof(minValue));

            if (minValue < 0)
                throw new ArgumentOutOfRangeException(nameof(minValue));

            Text = text;
            MinValue = minValue;
            MaxValue = maxValue;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Display text, e.g. Under $10
        /// </summary>
        public string Text { get; internal set; }

        /// <summary>
        /// Minimum product value
        /// </summary>
        public decimal MinValue { get; }

        /// <summary>
        /// Maximum product value
        /// </summary>
        public decimal MaxValue { get; }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Determines whether a price matches the price value
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public bool PriceMatch(in decimal price)
        {
            return price >= MinValue && price <= MaxValue;
        }

        #endregion Public Methods
    }
}
