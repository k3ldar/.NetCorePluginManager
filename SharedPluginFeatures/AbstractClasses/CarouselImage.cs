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
 *  Product:  SharedPluginFeatures
 *  
 *  File: CarouselImage.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  29/09/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace SharedPluginFeatures
{
    /// <summary>
    /// Carousel Image, abstract class that plugin modules can implement to carousel images.
    /// </summary>
    public abstract class CarouselImage
    {
        /// <summary>
        /// Image url obtains the image to be shown within the carousel.
        /// </summary>
        /// <returns>url for the image.</returns>
        public abstract string ImageUrl();

        /// <summary>
        /// Url to be used when the carousel item is clicked
        /// </summary>
        /// <returns>url of the route to be redirected to.</returns>
        public abstract string Url();

        /// <summary>
        /// Effects the order in which the carousel item is displayed.
        /// 
        /// Carousel items will be sorted by SortOrder ascending and then by name.
        /// </summary>
        /// <returns>int.  Order in which the item will be sorted.</returns>
        public abstract int SortOrder();
    }
}
