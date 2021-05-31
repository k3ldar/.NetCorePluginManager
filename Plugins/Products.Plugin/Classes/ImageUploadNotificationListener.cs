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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Products.Plugin
 *  
 *  File: ImageUploadNotificationListener.cs
 *
 *  Purpose:  Product image upload listener
 *
 *  Date        Name                Reason
 *  30/05/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;

using PluginManager.Abstractions;

using SharedPluginFeatures;

namespace ProductPlugin.Classes
{
    public sealed class ImageUploadNotificationListener : INotificationListener
    {
        #region INotificationListener Methods

        public bool EventRaised(in string eventId, in object param1, in object param2, ref object result)
        {
            throw new System.NotImplementedException();
        }

        public void EventRaised(in string eventId, in object param1, in object param2)
        {

        }

        public List<string> GetEvents()
        {
            return new List<string>()
            {
                Constants.NotificationEventImageUploaded,
                Constants.NotificationEventImageUploadOptions
            };
        }

        #endregion INotificationListener Methods
    }
}
